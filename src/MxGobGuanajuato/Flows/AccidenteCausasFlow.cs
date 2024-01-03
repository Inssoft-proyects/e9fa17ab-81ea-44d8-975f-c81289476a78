using System.Text;
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Dtos;
using Newtonsoft.Json;

namespace MxGobGuanajuato.Flows
{
    public sealed class AccidenteCausasFlow : IFlowData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AccidenteCausasFlow));

        private IReaderData<String>? crr;

        private IReaderData<String>? cwr;

        private IReaderData<AccidenteCausas>? accr;

        private IWriterData<AccidenteCausas>? accw;

        public IReaderData<String> CRR {set {this.crr = value;}}

        public IReaderData<String> CWR {set {this.cwr = value;}}

        public IReaderData<AccidenteCausas> ACCR {set {this.accr = value;}}

        public IWriterData<AccidenteCausas> ACCW {set {this.accw = value;}}

        public void Inside(IDictionary<string, object> p)
        {
            log.Info("Vamos a comenzar el flujo de migración para AccidenteCausas.");
            
            StringBuilder sql = new();

            log.Debug("Recuperando los parametros de inicio (valor minímo y máximo del campo ACAID en la tabla ACCIDENTESCAUSAS)");

            sql.Append("SELECT '{' ||\n");
            sql.Append("       '\"idMin\": ' || MIN(acaid) || ', ' ||\n");
            sql.Append("       '\"idMax\": ' || MAX(acaid) ||\n");
            sql.Append("       '}' AS json\n");
            sql.Append("FROM sitteg.accidentescausas");

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
                sql.Append("       '\"idMax\": ', COALESCE(MAX(idAccidenteCausa), 0),\n");
                sql.Append("       '}') AS json\n");
                sql.Append("FROM [dbo].[accidenteCausas]");

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

            sql.Append("SELECT ACAID AS \"idAccidenteCausa\",\n");
            sql.Append("       ACAIDACCIDENTE AS \"idAccidente\",\n");
            sql.Append("       ACAIDCAUSA AS \"idCausaAccidente\",\n");
            sql.Append("       ACAORDEN AS \"indice\"\n");
            sql.Append("FROM sitteg.ACCIDENTESCAUSAS\n");
            sql.Append("WHERE ACAID BETWEEN :ini AND :fin\n");
            sql.Append("ORDER BY ACAID");

            pams.Add("sql", sql.ToString());
            
            sql.Clear();

            log.Debug("Recuperando los datos para la tabla AccidenteCausas, realizando paginación de 100 elementos.");

            List<AccidenteCausas>? accs = null;

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

                if((accs = accr?.Get(pams)) == null) {
                    log.Error("No se recupero ningún registro de SITTEG.");
                    log.Info("Marca inicio -> " + mrkIni);
                    log.Info("Marca fin ->" + mrkFin);

                    break;
                }
                
                log.Debug("Se recuperaron " + accs.Count + " registros.");

                if(accw != null)
                    ei = accw.Set(accs);

                if(ei != accs.Count) {
                    log.Error("No se realizo la inserción de todos los registros en SREGINA.");
                    log.Info("Marca inicio de la pagina -> " + mrkIni);
                    log.Info("Marca fin de la pagina ->" + mrkFin);
                }

                ec += ei;
                
                mrkIni = mrkFin + 1;
            }

            log.Debug("Se migraron " + ec + " registros.");

            log.Info("Se concluye el flujo de migración para AccidenteCausas.");
        }
    }
}