using System.Text;
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Dtos;
using Newtonsoft.Json;

namespace MxGobGuanajuato.Flows
{
    public sealed class InvolucradosAccidenteFlow : IFlowData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(InvolucradosAccidenteFlow));

        private IReaderData<String>? crr;

        private IReaderData<String>? cwr;

        private IReaderData<InvolucradosAccidente>? iaccr;

        private IWriterData<InvolucradosAccidente>? iaccw;

        public IReaderData<String> CRR {set {this.crr = value;}}

        public IReaderData<String> CWR {set {this.cwr = value;}}

        public IReaderData<InvolucradosAccidente> IACCR {set {this.iaccr = value;}}

        public IWriterData<InvolucradosAccidente> IACCW {set {this.iaccw = value;}}

        public void Inside(IDictionary<string, object> p)
        {
            log.Info("Vamos a comenzar el flujo de migración para InvolucradosAccidente.");
            
            StringBuilder sql = new();

            log.Debug("Recuperando los parametros de inicio (valor minímo y máximo del campo AINIDACCIDENTE en la tabla ACCIDENTEINVOLUCRADOS)");

            sql.Append("SELECT '{' ||\n");
            sql.Append("       '\"idMin\": ' || MIN(ainidaccidente) || ', ' ||\n");
            sql.Append("       '\"idMax\": ' || MAX(ainidaccidente) ||\n");
            sql.Append("       '}' AS json\n");
            sql.Append("FROM sitteg.accidenteinvolucrados");

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
                sql.Append("       '\"idMax\": ', COALESCE(MAX(idAccidente), 0),\n");
                sql.Append("       '}') AS json\n");
                sql.Append("FROM [dbo].[involucradosAccidente]");

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

            sql.Append("SELECT accinv.AINIDACCIDENTE AS \"idAccidente\",\n");
            sql.Append("       accinv.AINIDPERSONA AS \"idPersona\",\n");
            sql.Append("       accinv.AINIDTIPOVICTIMA AS \"idTipoInvolucrado\",\n");
            sql.Append("       accinv.AINIDESTATUSVICTIMA AS \"idEstadoVictima\",\n");
            sql.Append("       aveh.AVIDVEHICULO AS \"idVehiculo\",\n");
            sql.Append("       accinv.AINNUMEROVEHICULO AS \"numero\",\n");
            sql.Append("       accinv.AINIDHOSPITAL AS \"idHospital\",\n");
            sql.Append("       accinv.AINIDTRASLADO AS \"idInstitucionTraslado\",\n");
            sql.Append("       accinv.AINASIENTO AS \"idAsiento\",\n");
            sql.Append("       accinv.AINIDESTATUSCINTURON AS \"idCinturon\",\n");
            sql.Append("       TO_DATE(accinv.AINFECHAINGRESO, 'yyyymmdd') AS \"fechaIngreso\",\n");
            sql.Append("       TO_CHAR(TO_TIMESTAMP(accinv.AINHORAINGRESO, 'hh24mi'), 'hh24:mi') AS \"horaIngreso\",\n");
            sql.Append("       CASE\n");
            sql.Append("        WHEN AINBAJA = 0\n");
            sql.Append("            THEN 1\n");
            sql.Append("        ELSE 0\n");
            sql.Append("       END AS \"estatus\"\n");
            sql.Append("FROM sitteg.ACCIDENTEINVOLUCRADOS accinv\n");
            sql.Append("LEFT JOIN sitteg.ACCIDENTEVEHICULOS aveh ON accinv.AINIDACCIDENTE = aveh.AVIDACCIDENTE AND accinv.AINNUMEROVEHICULO = aveh.AVNUMEROVEHICULO\n");
            sql.Append("WHERE accinv.AINIDACCIDENTE BETWEEN :ini AND :fin\n");
            sql.Append("ORDER BY accinv.AINIDACCIDENTE, accinv.AINIDPERSONA, aveh.AVIDVEHICULO");

            pams.Add("sql", sql.ToString());
            
            sql.Clear();

            log.Debug("Recuperando los datos para la tabla Accidentes, realizando paginación de 100 accidentes.");

            List<InvolucradosAccidente>? iaccs = null;

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

                if((iaccs = iaccr?.Get(pams)) == null) {
                    log.Error("No se recupero ningún registro de SITTEG.");
                    log.Info("Marca inicio -> " + mrkIni);
                    log.Info("Marca fin ->" + mrkFin);

                    break;
                }
                
                log.Debug("Se recuperaron " + iaccs.Count + " registros.");

                if(iaccw != null)
                    ei = iaccw.Set(iaccs);

                if(ei != iaccs.Count) {
                    log.Error("No se realizo la inserción de todos los registros en SREGINA.");
                    log.Info("Marca inicio de la pagina -> " + mrkIni);
                    log.Info("Marca fin de la pagina ->" + mrkFin);
                }

                ec += ei;
                
                mrkIni = mrkFin + 1;
            }

            log.Debug("Se migraron " + ec + " registros.");

            log.Info("Se concluye el flujo de migración para Accidentes.");
        }
    }
}