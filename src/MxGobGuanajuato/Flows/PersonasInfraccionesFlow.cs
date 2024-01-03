
using System.Text;
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Dtos;
using Newtonsoft.Json;

namespace MxGobGuanajuato.Flows
{
    public sealed class PersonasInfraccionesFlow : IFlowData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PersonasInfraccionesFlow));

        private IReaderData<String>? crr;

        private IReaderData<String>? cwr;

        private IReaderData<PersonasInfracciones>? pir;

        private IWriterData<PersonasInfracciones>? piw;

        public IReaderData<String> CRR {set {this.crr = value;}}

        public IReaderData<String> CWR {set {this.cwr = value;}}

        public IReaderData<PersonasInfracciones> PIR {set {this.pir = value;}}

        public IWriterData<PersonasInfracciones> PIW {set {this.piw = value;}}

        public void Inside(IDictionary<string, object> p)
        {
            log.Info("Vamos a comenzar el flujo de migración para PersonasInfracciones.");
            
            StringBuilder sql = new();

            log.Debug("Recuperando los parametros de inicio (valor minímo y máximo del campo PERID en la tabla PERSONAS)");

            sql.Append("SELECT '{' ||\n");
            sql.Append("       '\"idMin\": ' || MIN(perid) || ', ' ||\n");
            sql.Append("       '\"idMax\": ' || MAX(perid) ||\n");
            sql.Append("       '}' AS json\n");
            sql.Append("FROM sitteg.personas");

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
                sql.Append("       '\"idMax\": ', COALESCE(MAX(idPersonaInfraccion), 0),\n");
                sql.Append("       '}') AS json\n");
                sql.Append("FROM [dbo].[personasInfracciones]");

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

            sql.Append("SELECT per.PERID AS \"idPersonaInfraccion\",\n");
            sql.Append("       inf.INFID AS \"idInfraccion\",\n");
            sql.Append("       inf.INFLICENCIA AS \"numeroLicencia\",\n");
            sql.Append("       per.PERCURP AS \"CURP\",\n");
            sql.Append("       per.PERRFC AS \"RFC\",\n");
            sql.Append("       per.PERNOMBRE AS \"nombre\",\n");
            sql.Append("       per.PERPATERNO AS \"apellidoPaterno\",\n");
            sql.Append("       per.PERMATERNO AS \"apellidoMaterno\",\n");
            sql.Append("       1 AS \"idCatTipoPersona\",\n");
            sql.Append("       CURRENT_DATE AS \"fechaActualizacion\",\n");
            sql.Append("       0 AS \"actualizadoPor\",\n");
            sql.Append("       CASE\n");
            sql.Append("        WHEN per.PERBAJA = 0\n");
            sql.Append("            THEN 1\n");
            sql.Append("        ELSE 0\n");
            sql.Append("       END AS \"estatus\"\n");
            sql.Append("FROM sitteg.INFRACCIONES inf\n");
            sql.Append("LEFT JOIN sitteg.PERSONAS per ON inf.INFIDCONDUCTOR = per.PERID\n");
            sql.Append("WHERE inf.INFID BETWEEN :ini AND :fin\n");
            sql.Append("ORDER BY inf.INFID");

            pams.Add("sql", sql.ToString());
            
            sql.Clear();

            log.Debug("Recuperando los datos para la tabla PersonasInfracciones, realizando paginación de 100 elementos.");

            List<PersonasInfracciones>? pis = null;

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

                if((pis = pir?.Get(pams)) == null) {
                    log.Error("No se recupero ningún registro de SITTEG.");
                    log.Info("Marca inicio -> " + mrkIni);
                    log.Info("Marca fin ->" + mrkFin);

                    break;
                }
                
                log.Debug("Se recuperaron " + pis.Count + " registros.");

                if(piw != null)
                    ei = piw.Set(pis);

                if(ei != pis.Count) {
                    log.Error("No se realizo la inserción de todos los registros en SREGINA.");
                    log.Info("Marca inicio de la pagina -> " + mrkIni);
                    log.Info("Marca fin de la pagina ->" + mrkFin);
                }

                ec += ei;
                
                mrkIni = mrkFin + 1;
            }

            log.Debug("Se migraron " + ec + " registros.");

            log.Info("Se concluye el flujo de migración para PersonasInfracciones.");
        }
    }
}