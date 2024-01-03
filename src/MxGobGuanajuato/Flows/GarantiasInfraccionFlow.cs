using System.Text;
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Dtos;
using Newtonsoft.Json;

namespace MxGobGuanajuato.Flows
{
    public sealed class GarantiasInfraccionFlow : IFlowData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GarantiasInfraccionFlow));

        private IReaderData<String>? crr;

        private IReaderData<String>? cwr;

        private IReaderData<GarantiasInfraccion>? gir;

        private IWriterData<GarantiasInfraccion>? giw;

        public IReaderData<String> CRR {set {this.crr = value;}}

        public IReaderData<String> CWR {set {this.cwr = value;}}

        public IReaderData<GarantiasInfraccion> GIR {set {this.gir = value;}}

        public IWriterData<GarantiasInfraccion> GIW {set {this.giw = value;}}

        public void Inside(IDictionary<string, object> p)
        {
            log.Info("Vamos a comenzar el flujo de migración para GarantiasInfraccion.");
            
            StringBuilder sql = new();

            log.Debug("Recuperando los parametros de inicio (valor minímo y máximo del campo INFID en la tabla INFRACCIONES)");

            sql.Append("SELECT '{' ||\n");
            sql.Append("       '\"idMin\": ' || MIN(infid) || ', ' ||\n");
            sql.Append("       '\"idMax\": ' || MAX(infid) ||\n");
            sql.Append("       '}' AS json\n");
            sql.Append("FROM sitteg.infracciones");

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
            else {
                log.Debug("No fue posible recuperar los parametros de inicio.");

                return;
            }

            int mrkIni = Convert.ToInt32(pi["idMin"]), mrkFin = Convert.ToInt32(pi["idMin"]), fin = Convert.ToInt32(pi["idMax"]);

            string mod = (string)p["modalidad"];

            if(mod.Equals("INCREMENTAL")) {
                log.Info("La migración es incremental.");

                log.Debug("Se van a recuperar los parametros incrementales.");

                sql.Append("SELECT CONCAT('{',\n");
                sql.Append("       '\"idMax\": ', COALESCE(MAX(idInfraccion), 0),\n");
                sql.Append("       '}') AS json\n");
                sql.Append("FROM [dbo].[garantiasInfraccion]");

                pams.Add("sql" , sql.ToString());

                strs = cwr?.Get(pams);

                sql.Clear();

                pams.Clear();

                if(strs != null) {
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
                else {
                    log.Debug("No fue posible recuperar los parametros incrementales.");

                    return;
                }
            }

            sql.Append("SELECT INFID AS \"idInfraccion\",\n");
            sql.Append("       INFIDTIPOGARANTIA AS \"idGarantia\",\n");
            sql.Append("       0 AS \"idCatGarantia\",\n");
            sql.Append("       INFIDTIPOPLACA AS \"idTipoPlaca\",\n");
            sql.Append("       INFIDTIPOLICENCIA AS \"idTipoLicencia\",\n");
            sql.Append("       INFNOPLACA AS \"numPlaca\",\n");
            sql.Append("       INFLICENCIA AS \"numLicencia\",\n");
            sql.Append("       NULL AS \"vehiculoDocumento\",\n");
            sql.Append("       CURRENT_DATE AS \"fechaActualizacion\",\n");
            sql.Append("       0 AS \"actualizadoPor\",\n");
            sql.Append("       CASE\n");
            sql.Append("        WHEN INFBAJA = 0\n");
            sql.Append("            THEN 1\n");
            sql.Append("        ELSE 0\n");
            sql.Append("       END AS \"estatus\"\n");
            sql.Append("FROM sitteg.INFRACCIONES\n");
            sql.Append("WHERE INFID BETWEEN :ini AND :fin\n");
            sql.Append("ORDER BY INFID");
            
            pams.Add("sql", sql.ToString());
            
            sql.Clear();

            log.Debug("Recuperando los datos para la tabla GarantiasInfraccion, realizando paginación de 100 elementos.");

            List<GarantiasInfraccion>? gis = null;

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

                if((gis = gir?.Get(pams)) == null) {
                    log.Error("No se recupero ningún registro de SITTEG.");
                    log.Info("Marca inicio -> " + mrkIni);
                    log.Info("Marca fin ->" + mrkFin);

                    break;
                }

                log.Debug("Se recuperaron " + gis.Count + " registros.");

                if(giw != null)
                    ei = giw.Set(gis);

                if(ei != gis.Count) {
                    log.Error("No se realizo la inserción de todos los registros en SREGINA.");
                    log.Info("Marca inicio de la pagina -> " + mrkIni);
                    log.Info("Marca fin de la pagina ->" + mrkFin);
                }

                ec += ei;
                
                mrkIni = mrkFin + 1;
            }

            log.Debug("Se migraron " + ec + " registros.");

            log.Info("Se concluye el flujo de migración para GarantiasInfraccion.");
        }
    }
}