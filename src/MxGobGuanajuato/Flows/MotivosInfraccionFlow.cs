
using System.Text;
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Dtos;
using Newtonsoft.Json;

namespace MxGobGuanajuato.Flows
{
    public sealed class MotivosInfraccionFlow : IFlowData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MotivosInfraccionFlow));

        private IReaderData<String>? crr;

        private IReaderData<String>? cwr;

        private IReaderData<MotivosInfraccion>? mir;

        private IWriterData<MotivosInfraccion>? miw;

        public IReaderData<String> CRR {set {this.crr = value;}}

        public IReaderData<String> CWR {set {this.cwr = value;}}

        public IReaderData<MotivosInfraccion> MIR {set {this.mir = value;}}

        public IWriterData<MotivosInfraccion> MIW {set {this.miw = value;}}
        
        public void Inside(IDictionary<string, object> p)
        {
            log.Info("Vamos a comenzar el flujo de migración para MotivosInfraccion.");
            
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
                sql.Append("       '\"idMax\": ', OOALESCE(MAX(idMotivoInfraccion), 0),\n");
                sql.Append("       '}') AS json\n");
                sql.Append("FROM [dbo].[motivosInfraccion]");

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

            sql.Append("SELECT infmo.IMID as \"idMotivoInfraccion\",\n");
            sql.Append("	   catmot.MICALIFICACIONMINIMA as \"calificacionMinima\",\n");
            sql.Append("	   catmot.MICALIFICACIONMINIMA as \"calificacionMaxima\",\n");
            sql.Append("	   infmo.IMCALIFICACION as \"calificacion\",\n");
            sql.Append("	   TO_DATE('2023-08-01','YYYY-MM-DD') as \"fechaActualizacion\",\n");
            sql.Append("	   0 as \"actualizadoPor\",\n");
            sql.Append("	   1 as \"estatus\",\n");
            sql.Append("	   infmo.IMIDMOTIVO as \"idCatMotivosInfraccion\",\n");
            sql.Append("	   infmo.IMIDINFRACCION as \"idInfraccion\",\n");
            sql.Append("	   1 as \"IdConcepto\",\n");
            sql.Append("	   1 as \"IdSubConcepto\"\n");
            sql.Append("FROM SITTEG.INFRACCIONES inf\n");
            sql.Append("LEFT JOIN SITTEG.INFRACCIONESENVIADAS infenv on infenv.IEIDINFRACCION=inf.INFID\n");
            sql.Append("LEFT JOIN SITTEG.ENVIOINFRACCIONES einf on einf.EINID=infenv.IEIDENVIO\n");
            sql.Append("LEFT JOIN SITTEG.OFICINASRENTAS ofre on inf.INFIDLUGARPAGO=ofre.ORID\n");
            sql.Append("LEFT JOIN SITTEG.INFRACCIONESMOTIVOS infmo on infmo.IMIDINFRACCION=inf.INFID\n");
            sql.Append("LEFT JOIN SITTEG.MOTIVOSINFRACCION catmot on infmo.IMIDMOTIVO=catmot.MIID\n");
            sql.Append("WHERE inf.INFID BETWEEN :ini AND :fin\n");
            sql.Append("ORDER BY inf.INFID");

            pams.Add("sql", sql.ToString());
            
            sql.Clear();

            log.Debug("Recuperando los datos para la tabla MotivosInfraccion, realizando paginación de 100 elementos.");

            List<MotivosInfraccion>? mis = null;

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

                if((mis = mir?.Get(pams)) == null) {
                    log.Error("No se recupero ningún registro de SITTEG.");
                    log.Info("Marca inicio -> " + mrkIni);
                    log.Info("Marca fin ->" + mrkFin);

                    break;
                }

                log.Debug("Se recuperaron " + mis.Count + " registros.");

                if(miw != null)
                    ei = miw.Set(mis);

                if(ei != mis.Count) {
                    log.Error("No se realizo la inserción de todos los registros en SREGINA.");
                    log.Info("Marca inicio de la pagina -> " + mrkIni);
                    log.Info("Marca fin de la pagina ->" + mrkFin);
                }

                ec += ei;
                
                mrkIni = mrkFin + 1;
            }

            sql.Append("SELECT infmo.IMID AS \"idMotivoInfraccion\",\n");
            sql.Append("	   catmot.MICALIFICACIONMINIMA as \"calificacionMinima\",\n");
            sql.Append("	   catmot.MICALIFICACIONMINIMA as \"calificacionMaxima\",\n");
            sql.Append("	   infmo.IMCALIFICACION as \"calificacion\",\n");
            sql.Append("	   TO_DATE('2023-08-01','YYYY-MM-DD') as \"fechaActualizacion\",\n");
            sql.Append("	   0 as \"actualizadoPor\",\n");
            sql.Append("	   1 as \"estatus\",\n");
            sql.Append("	   infmo.IMIDMOTIVO as \"idCatMotivosInfraccion\",\n");
            sql.Append("	   infmo.IMIDINFRACCION as \"idInfraccion\",\n");
            sql.Append("	   1 as \"IdConcepto\",\n");
            sql.Append("	   1 as \"IdSubConcepto\"\n");
            sql.Append("FROM SITTEG.INFRACCIONESMOTIVOS infmo\n");
            sql.Append("LEFT JOIN SITTEG.MOTIVOSINFRACCION catmot on infmo.IMIDMOTIVO=catmot.MIID\n");
            sql.Append("WHERE infmo.IMIDINFRACCION = 402");
            
            pams.Clear();
            pams.Add("sql", sql.ToString());
            
            sql.Clear();

            mis = mir?.Get(pams);

            if(mis != null && miw != null)
            {
                ei = miw.Set(mis);

                if(ei != mis.Count)
                    log.Error("No se realizo la inserción de todos los registros.");
                
                ec += ei;
            }

            log.Debug("Se migraron " + ec + " registros.");

            log.Info("Se concluye el flujo de migración para MotivosInfraccion.");
       }
    }
}