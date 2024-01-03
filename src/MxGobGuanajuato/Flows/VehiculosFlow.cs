using System.Text;
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Dtos;
using Newtonsoft.Json;

namespace MxGobGuanajuato.Flows
{
    public sealed class VehiculosFlow : IFlowData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(VehiculosFlow));

        private IReaderData<String>? crr;

        private IReaderData<String>? cwr;

        private IReaderData<Vehiculos>? vr;

        private IWriterData<Vehiculos>? vw;

        public IReaderData<String> CRR {set {this.crr = value;}}

        public IReaderData<String> CWR {set {this.cwr = value;}}

        public IReaderData<Vehiculos> VR {set {this.vr = value;}}

        public IWriterData<Vehiculos> VW {set {this.vw = value;}}

        public void Inside(IDictionary<string, object> p)
        {
            log.Info("Vamos a comenzar el flujo de migración para Vehiculos.");
            
            StringBuilder sql = new();

            log.Debug("Recuperando los parametros de inicio (valor minímo y máximo del campo VEHID en la tabla VEHICULOS)");

            sql.Append("SELECT '{' ||\n");
            sql.Append("       '\"idMin\": ' || MIN(vehid) || ', ' ||\n");
            sql.Append("       '\"idMax\": ' || MAX(vehid) ||\n");
            sql.Append("       '}' AS json\n");
            sql.Append("FROM sitteg.vehiculos");

            IDictionary<string, object> pams = new Dictionary<string, object>()
                {
                    { "sql", sql.ToString() }
                };
            
            List<string>? strs = crr?.Get(pams);

            sql.Clear();

            pams.Clear();

            IDictionary<string, object>? pi = null;

            if(strs != null) {
                try {
                    pi = JsonConvert.DeserializeObject<Dictionary<string, object>>(strs[0]);

                    if(pi == null) {
                        log.Debug("No fue posible recuperar los parametros de inicio.");

                        return;
                    }
                } catch(JsonSerializationException jse) {
                    log.Error(jse);
                    
                    return;
                } finally {
                    strs.Clear();
                }
            }
            else 
            {
                log.Debug("No fue posible recuperar los parametros de inicio.");

                return;
            }

            int mrkIni = Convert.ToInt32(pi["idMin"]), mrkFin = Convert.ToInt32(pi["idMin"]), fin = Convert.ToInt32(pi["idMax"]);

            string mod = (string)p["modalidad"];

            if(mod.Equals("INCREMENTAL"))
            {
                log.Info("La migración es incremental.");

                log.Debug("Se van a recuperar los parametros incrementales.");

                sql.Append("SELECT CONCAT('{',\n");
                sql.Append("       '\"idMax\": ', COALESCE(MAX(idVehiculo), 0),\n");
                sql.Append("       '}') AS json\n");
                sql.Append("FROM [dbo].[vehiculos]");

                pams.Add("sql" , sql.ToString());

                strs = cwr?.Get(pams);

                sql.Clear();

                pams.Clear();

                if(strs != null)
                {
                    try {
                        pi = JsonConvert.DeserializeObject<Dictionary<string, object>>(strs[0]);
                    } catch(JsonSerializationException jse) {
                        log.Error(jse);

                        return;
                    } finally {
                        strs.Clear();
                    }

                    if(pi == null) {
                        log.Debug("No fue posible recuperar los parametros incrementales.");

                        return;
                    }

                    mrkIni = Convert.ToInt32(pi["idMax"]) + 1;

                    if(mrkIni < mrkFin)
                        mrkIni = mrkFin;
                    else
                        mrkFin = mrkIni;
                }
                else 
                {
                    log.Debug("No fue posible recuperar los parametros incrementales.");

                    return;
                }
            }

            sql.Append("SELECT VEHID AS \"idVehiculo\",\n");
            sql.Append("       VEHPLACAS AS \"placas\",\n");
            sql.Append("       VEHSERIE AS \"serie\",\n");
            sql.Append("       VEHTARJETA AS \"tarjeta\",\n");
            sql.Append("       CASE WHEN LENGTH(TRIM(VEHVIGENCIATARJETA)) > 0");
            sql.Append("                THEN TO_DATE(VEHVIGENCIATARJETA, 'yyyymmdd')");
            sql.Append("            ELSE NULL");
            sql.Append("       END AS \"vigenciaTrajeta\",\n");
            sql.Append("       VEHIDMARCA AS \"idMarcaVehiculo\",\n");
            sql.Append("       VEHIDSUBMARCA AS \"idSubmarca\",\n");
            sql.Append("       VEHIDTIPO AS \"idTipoVehiculo\",\n");
            sql.Append("       VEHMODELO AS \"modelo\",\n");
            sql.Append("       VEHIDCOLOR AS \"idColor\",\n");
            sql.Append("       VEHIDIDENTIDAD AS \"idEntidad\",\n");
            sql.Append("       VEHIDTIPOSERVICIO AS \"idCatTipoServicio\",\n");
            sql.Append("       VEHIDSUBTIPOSERVICIO AS \"idSubtipoServicio\",\n");
            sql.Append("       VEHIDPROPIETARIO AS \"propietario\",\n");
            sql.Append("       VEHNUMEROECONOMICO AS \"numeroEconomico\",\n");
            sql.Append("       NULL AS \"paisManufactura\",\n");
            sql.Append("       VEHIDPROPIETARIO AS \"idPersona\",\n");
            sql.Append("       1 AS \"actualizadoPor\",\n");
            sql.Append("       CURRENT_TIMESTAMP AS \"fechaActualizacion\",\n");
            sql.Append("       CASE WHEN VEHBAJA = 0\n");
            sql.Append("                THEN 1\n");
            sql.Append("            ELSE 0\n");
            sql.Append("       END AS \"estatus\",\n");
            sql.Append("       VEHMOTOR AS \"motor\",\n");
            sql.Append("       VEHCAPACIDAD AS \"capacidad\",\n");
            sql.Append("       VEHPOLIZA AS \"poliza\",\n");
            sql.Append("       VEHCARGA AS \"carga\",\n");
            sql.Append("       VEHOTROS AS \"otros\"\n");
            sql.Append("FROM sitteg.VEHICULOS\n");
            sql.Append("WHERE VEHID BETWEEN :ini AND :fin\n");
            sql.Append("ORDER BY VEHID");
            

            pams.Add("sql", sql.ToString());
            
            sql.Clear();

            log.Debug("Recuperando los datos para la tabla vehiculos, realizando paginación de 100 elementos.");

            List<Vehiculos>? vs = null;

            int ec = 0, ei = 0;

            while(mrkFin < fin)
            {
                pams.Remove("ini");
                pams.Remove("fin");

                mrkFin += 100;

                if(mrkFin > fin)
                    mrkFin = fin;
                
                pams.Add("ini", mrkIni);
                pams.Add("fin", mrkFin);

                if((vs = vr?.Get(pams)) == null) {
                    log.Error("No se recupero ningún registro de SITTEG.");
                    log.Info("Marca inicio -> " + mrkIni);
                    log.Info("Marca fin ->" + mrkFin);

                    break;
                }
                
                log.Debug("Se recuperaron " + vs.Count + " registros.");

                if(vw != null)
                    ei = vw.Set(vs);

                if(ei != vs.Count) {
                    log.Error("No se realizo la inserción de todos los registros en SREGINA.");
                    log.Info("Marca inicio de la pagina -> " + mrkIni);
                    log.Info("Marca fin de la pagina ->" + mrkFin);
                }

                ec += ei;
                
                mrkIni = mrkFin + 1;
            }

            log.Debug("Se migraron " + ec + " registros.");

            log.Info("Se concluye el flujo de migración para Vehiculos.");
        }
    }
}