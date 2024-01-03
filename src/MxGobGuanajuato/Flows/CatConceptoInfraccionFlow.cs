using System.Text;
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Dtos;
using Newtonsoft.Json;

namespace MxGobGuanajuato.Flows
{
    public sealed class CatConceptoInfraccionFlow : IFlowData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CatConceptoInfraccionFlow));

        private IReaderData<String>? crr;

        private IReaderData<String>? cwr;

        private IReaderData<CatConceptoInfraccion>? cmir;

        private IWriterData<CatConceptoInfraccion>? cmiw;

        public IReaderData<String> CRR {set {this.crr = value;}}

        public IReaderData<String> CWR {set {this.cwr = value;}}

        public IReaderData<CatConceptoInfraccion> CMIR {set {this.cmir = value;}}

        public IWriterData<CatConceptoInfraccion> CMIW {set {this.cmiw = value;}}
        
        public void Inside(IDictionary<string, object> p)
        {
            log.Info("Vamos a comenzar el flujo de migración para CatConceptoInfraccion.");
            
            StringBuilder sql = new();
            string query = string.Empty;

            log.Debug("Recuperando los parametros de inicio (valor minímo y máximo del campo MIID en la tabla TIPOMOTIVOINFRACCION)");

            sql.Append("SELECT '{' ||\n");
            sql.Append("       '\"idMin\": ' || MIN(TMIID) || ', ' ||\n");
            sql.Append("       '\"idMax\": ' || MAX(TMIID) ||\n");
            sql.Append("       '}' AS json\n");
            sql.Append("FROM sitteg.TIPOMOTIVOINFRACCION WHERE TMINIVEL =1");

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
                } catch(JsonSerializationException jse) {
                    log.Error(jse);
                    
                    return;
                } finally {
                    strs.Clear();
                }

                if(pi == null) {
                    log.Debug("No fue posible recuperar los parametros de inicio.");

                    return;
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
                sql.Append("       '\"idMax\": ', COALESCE(MAX(idConcepto), 0),\n");
                sql.Append("       '}') AS json\n");
                sql.Append("FROM [dbo].[catConceptoInfraccion]");

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


            query = @" SELECT [TMIID] as 'idConcepto'
                      ,[TMIDDESCRIPCION] as 'concepto'
	                  ,CURRENT_TIMESTAMP as 'fechaActualizacion'
	                  ,0 as 'actualizadoPor'
                      ,
	                  CASE
                    WHEN [TMIBAJA] = 0
                       THEN 1
                       ELSE 0
	                END as 'estatus'
                  FROM [sregina].[sitteg].[TIPOMOTIVOINFRACCION]
                  WHERE TMINIVEL =1 AND TMIID BETWEEN :ini AND :fin 
                  ORDER BY TMIID";

            pams.Add("sql", query);
            
            sql.Clear();

            log.Debug("Recuperando los datos para la tabla CatConceptoInfraccion, realizando paginación de 100 elementos.");

            List<CatConceptoInfraccion>? cmis = null;

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

                ;

                if((cmis = cmir?.Get(pams)) == null) {
                    log.Error("No se recupero ningún registro de SITTEG.");
                    log.Info("Marca inicio -> " + mrkIni);
                    log.Info("Marca fin ->" + mrkFin);

                    break;
                }

                log.Debug("Se recuperaron " + cmis.Count + " registros.");
                
                if(cmiw != null)
                    ei = cmiw.Set(cmis);

                if(ei != cmis.Count) {
                    log.Error("No se realizo la inserción de todos los registros en SREGINA.");
                    log.Info("Marca de inicio de la pagina -> " + mrkIni);
                    log.Info("Marca fin de la pagina ->" + mrkFin);
                }

                ec += ei;
                
                mrkIni = mrkFin + 1;
            }

            log.Debug("Se migraron " + ec + " registros.");

            log.Info("Se concluye el flujo de migración para CatConceptoInfraccion.");
        }
    }
}