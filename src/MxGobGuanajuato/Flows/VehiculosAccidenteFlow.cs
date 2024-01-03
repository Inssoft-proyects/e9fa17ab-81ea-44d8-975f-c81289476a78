using System.Text;
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Dtos;
using Newtonsoft.Json;

namespace MxGobGuanajuato.Flows
{
    public sealed class VehiculosAccidenteFlow : IFlowData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(VehiculosAccidenteFlow));

        private IReaderData<String>? crr;

        private IReaderData<String>? cwr;

        private IReaderData<VehiculosAccidente>? vaccr;

        private IWriterData<VehiculosAccidente>? vaccw;

        public IReaderData<String> CRR {set {this.crr = value;}}

        public IReaderData<String> CWR {set {this.cwr = value;}}

        public IReaderData<VehiculosAccidente> VACCR {set {this.vaccr = value;}}

        public IWriterData<VehiculosAccidente> VACCW {set {this.vaccw = value;}}

        public void Inside(IDictionary<string, object> p)
        {
            log.Info("Vamos a comenzar el flujo de migración para VehiculosAccidente.");
            
            StringBuilder sql = new();

            log.Debug("Recuperando los parametros de inicio (valor minímo y máximo del campo AVID en la tabla ACCIDENTEVEHICULOS)");

            sql.Append("SELECT '{' ||\n");
            sql.Append("       '\"idMin\": ' || MIN(avid) || ', ' ||\n");
            sql.Append("       '\"idMax\": ' || MAX(avid) ||\n");
            sql.Append("       '}' AS json\n");
            sql.Append("FROM sitteg.accidentevehiculos");

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
                sql.Append("       '\"idMax\": ', COALESCE(MAX(idVehiculoAccidente), 0),\n");
                sql.Append("       '}') AS json\n");
                sql.Append("FROM [dbo].[vehiculosAccidente]");

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

            sql.Append("SELECT AVID AS \"idVehiculoAccidente\",\n");
            sql.Append("       accveh.AVIDACCIDENTE AS \"idAccidente\",\n");
            sql.Append("       accveh.AVIDCONDUCTOR AS \"idPersona\",\n");
            sql.Append("       accveh.AVIDVEHICULO AS \"idVehiculo\",\n");
            sql.Append("       accveh.AVMONTODAÑOS AS \"montoVehiculo\",\n");
            sql.Append("       accveh.AVPLACASVEHICULO AS \"placa\",\n");
            sql.Append("       veh.VEHSERIE AS \"serie\",\n");
            sql.Append("       CASE\n");
            sql.Append("        WHEN accveh.AVBAJA = 0\n");
            sql.Append("            THEN 1\n");
            sql.Append("        ELSE 0\n");
            sql.Append("       END AS \"estatus\"\n");
            sql.Append("FROM sitteg.ACCIDENTEVEHICULOS accveh\n");
            sql.Append("LEFT JOIN sitteg.VEHICULOS veh ON accveh.AVIDVEHICULO = veh.VEHID\n");
            sql.Append("WHERE AVID BETWEEN :ini AND :fin\n");
            sql.Append("ORDER BY AVID");

            pams.Add("sql", sql.ToString());
            
            sql.Clear();

            log.Debug("Recuperando los datos para la tabla vehiculos, realizando paginación de 100 elementos.");

            List<VehiculosAccidente>? vaccs = null;

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

                if((vaccs = vaccr?.Get(pams)) == null) {
                    log.Error("No se recupero ningún registro de SITTEG.");
                    log.Info("Marca inicio -> " + mrkIni);
                    log.Info("Marca fin ->" + mrkFin);

                    break;
                }
                
                log.Debug("Se recuperaron " + vaccs.Count + " registros.");

                if(vaccw != null)
                    ei = vaccw.Set(vaccs);

                if(ei != vaccs.Count) {
                    log.Error("No se realizo la inserción de todos los registros en SREGINA.");
                    log.Info("Marca inicio de la pagina -> " + mrkIni);
                    log.Info("Marca fin de la pagina ->" + mrkFin);
                }

                ec += ei;
                
                mrkIni = mrkFin + 1;
            }

            log.Debug("Se migraron " + ec + " registros.");

            log.Info("Se concluye el flujo de migración para VehiculosAccidente.");
        }
    }
}