
using System.Text;
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Dtos;
using Newtonsoft.Json;

namespace MxGobGuanajuato.Flows
{
    public sealed class PersonasFlow : IFlowData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PersonasFlow));

        private IReaderData<String>? crr;

        private IReaderData<String>? cwr;

        private IReaderData<Personas>? per;

        private IWriterData<Personas>? pew;

        public IReaderData<String> CRR {set {this.crr = value;}}

        public IReaderData<String> CWR {set {this.cwr = value;}}

        public IReaderData<Personas> PER {set {this.per = value;}}

        public IWriterData<Personas> PEW {set {this.pew = value;}}

        public void Inside(IDictionary<string, object> p)
        {
            log.Info("Vamos a comenzar el flujo de migración para Personas.");
            
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
                sql.Append("       '\"idMax\": ', COALESCE(MAX(idPersona), 0),\n");
                sql.Append("       '}') AS json\n");
                sql.Append("FROM [dbo].[personas]");

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

            sql.Append("SELECT PERID AS \"idPersona\",\n");
            sql.Append("	   PERLICENCIA AS \"numeroLicencia\",\n");
            sql.Append("	   PERRFC AS \"RFC\",\n");
            sql.Append("       PERCURP AS \"CURP\",\n");
            sql.Append("       PERNOMBRE AS \"nombre\",\n");
            sql.Append("       PERPATERNO AS \"apellidoPaterno\",\n");
            sql.Append("       PERMATERNO AS \"apellidoMaterno\",\n");
            sql.Append("	   CURRENT_TIMESTAMP AS \"fechaActualizacion\",\n");
            sql.Append("       0 AS \"actualizadoPor\",\n");
            sql.Append("       CASE WHEN PERBAJA = 0");
            sql.Append("                THEN 1");
            sql.Append("            ELSE 0");
            sql.Append("       END AS \"estatus\",\n");
            sql.Append("       1 AS \"idCatTipoPersona\",\n");
            sql.Append("       CASE WHEN PERSEXO = 'H'");
            sql.Append("                THEN 1");
            sql.Append("            ELSE 2");
            sql.Append("       END AS \"idGenero\",\n");
            sql.Append("       TO_DATE(PERFECHANACIMIENTO, 'yyyymmdd') AS \"fechaNacimiento\",\n");
            sql.Append("       PERIDTIPOLICENCIA AS \"idTipoLicencia\",\n");
            sql.Append("       TO_DATE(PERVENCIMIENTO, 'yyyymmdd') AS \"vigenciaLicencia\"\n");
            sql.Append("FROM SITTEG.PERSONAS\n");
            sql.Append("WHERE PERID BETWEEN :ini AND :fin\n");
            sql.Append("ORDER BY PERID");

            pams.Add("sql", sql.ToString());
            
            sql.Clear();

            log.Debug("Recuperando los datos para la tabla Personas, realizando paginación de 100 elementos.");

            List<Personas>? pes = null;

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

                if((pes = per?.Get(pams)) == null) {
                    log.Error("No se recupero ningún registro de SITTEG.");
                    log.Info("Marca inicio -> " + mrkIni);
                    log.Info("Marca fin ->" + mrkFin);

                    break;
                }
                
                log.Debug("Se recuperaron " + pes.Count + " registros.");

                if(pew != null)
                    ei = pew.Set(pes);

                if(ei != pes.Count) {
                    log.Error("No se realizo la inserción de todos los registros en SREGINA.");
                    log.Info("Marca inicio de la pagina -> " + mrkIni);
                    log.Info("Marca fin de la pagina ->" + mrkFin);
                }

                ec += ei;
                
                mrkIni = mrkFin + 1;
            }

            log.Debug("Se migraron " + ec + " registros.");

            log.Info("Se concluye el flujo de migración para Personas.");
        }
    }
}