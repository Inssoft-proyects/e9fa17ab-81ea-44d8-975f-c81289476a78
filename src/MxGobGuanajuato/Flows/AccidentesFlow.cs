using System.Text;
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Dtos;
using Newtonsoft.Json;

namespace MxGobGuanajuato.Flows
{
    public sealed class AccidentesFlow : IFlowData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AccidentesFlow));

        private IReaderData<String>? crr;

        private IReaderData<String>? cwr;

        private IReaderData<Accidentes>? accr;

        private IWriterData<Accidentes>? accw;

        public IReaderData<String> CRR {set {this.crr = value;}}

        public IReaderData<String> CWR {set {this.cwr = value;}}

        public IReaderData<Accidentes> ACCR {set {this.accr = value;}}

        public IWriterData<Accidentes> ACCW {set {this.accw = value;}}

        public void Inside(IDictionary<string, object> p)
        {
            log.Info("Vamos a comenzar el flujo de migración para Accidentes.");
            
            StringBuilder sql = new();

            log.Debug("Recuperando los parametros de inicio (valor minímo y máximo del campo ACCID en la tabla ACCIDENTES)");

            sql.Append("SELECT '{' ||\n");
            sql.Append("       '\"idMin\": ' || MIN(accid) || ', ' ||\n");
            sql.Append("       '\"idMax\": ' || MAX(accid) ||\n");
            sql.Append("       '}' AS json\n");
            sql.Append("FROM sitteg.accidentes");

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
                sql.Append("       '\"idMax\": ', COALESCE(MAX(accid), 0),\n");
                sql.Append("       '}') AS json\n");
                sql.Append("FROM [dbo].[accidentes]");

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

            sql.Append("SELECT ACCID AS \"idAccidente\",\n");
            sql.Append("       TO_DATE(ACCFECHA, 'yyyymmdd') AS \"fecha\",\n");
            sql.Append("       TO_CHAR(TO_TIMESTAMP(ACCHORA, 'hh24mi'), 'hh24:mi') AS \"hora\",\n");
            sql.Append("       ACCIDMUNICIPIO AS \"idMunicipio\",\n");
            sql.Append("       ACCIDCARRETERA AS \"idCarretera\",\n");
            sql.Append("       ACCIDTRAMO AS \"idTramo\",\n");
            sql.Append("       ACCKILOMETRO AS \"kilometro\",\n");
            sql.Append("       ACCIDESTATUS AS \"estatusReporte\",\n");
            sql.Append("       NULL AS \"idClasificacionAccidente\",\n");
            sql.Append("       NULL AS \"idCausaAccidente\",\n");
            sql.Append("       ACCDESCRIPCIONCAUSAS AS \"descripcionCausas\",\n");
            sql.Append("       NULL AS \"idFactorAccidente\",\n");
            sql.Append("       NULL AS \"idFactorOpcionAccidente\",\n");
            sql.Append("       ACCIDCAPTURISTA AS \"actualizadoPor\",\n");
            sql.Append("       CURRENT_DATE AS \"fechaActualizacion\",\n");
            sql.Append("       CASE WHEN ACDBAJA = 0");
            sql.Append("                THEN 1");
            sql.Append("            ELSE 0");
            sql.Append("       END AS \"estatus\",\n");
            sql.Append("       REPLACE(REPLACE(ACCDAÑOSCAMINO, CHR(36), CHR(0)), CHR(44), CHR(0)) AS \"montoCamino\",\n");
            sql.Append("       REPLACE(REPLACE(ACCDAÑOSCARGA, CHR(36), CHR(0)), CHR(44), CHR(0)) AS \"montoCarga\",\n");
            sql.Append("       REPLACE(REPLACE(ACCDAÑOSPROPIETARIOS, CHR(36), CHR(0)), CHR(44), CHR(0)) AS \"montoPropietarios\",\n");
            sql.Append("       REPLACE(REPLACE(ACCOTROSDAÑOS, CHR(36), CHR(0)), CHR(44), CHR(0)) AS \"montoOtros\",\n");
            sql.Append("       NULL AS \"montoVehiculo\",\n");
            sql.Append("       ACCIDELABORA AS \"idElabora\",\n");
            sql.Append("       ACCIDSUPERVISOR AS \"idSupervisa\",\n");
            sql.Append("       ACCIDAUTORIZA AS \"idAutoriza\",\n");
            sql.Append("       ACCIDELABORACONSIGNACION AS \"idElaboraConsignacion\",\n");
            sql.Append("       ACCLATITUD AS \"latitud\",\n");
            sql.Append("       ACCLONGITUD AS \"longitud\",\n");
            sql.Append("       ACCIDMUNICIPIOCONSIGNA AS \"idCiudad\",\n");
            sql.Append("       ACCIDCERTIFICADO AS \"idCertificado\",\n");
            sql.Append("       ACCENTREGAOTROS AS \"entregaOtros\",\n");
            sql.Append("       ACCENTREGAOBJETOS AS \"entregaObjetos\",\n");
            sql.Append("       NULL AS \"consignacionHechos\",\n");
            sql.Append("       ACCOFICIO AS \"numeroOficio\",\n");
            sql.Append("       ACCIDAGENCIA AS \"idAgenciaMinisterio\",\n");
            sql.Append("       ACCRECIBEMINISTERIO AS \"recibeMinisterio\",\n");
            sql.Append("       ACCIDAUTORIDADENTREGA AS \"idAutoridadEntrega\",\n");
            sql.Append("       ACCIDDISPOSICION AS \"idAutoridadDisposicion\",\n");
            sql.Append("       ACCARMAS AS \"armas\",\n");
            sql.Append("       ACCDESCRIPCIONARMAS AS \"armasTexto\",\n");
            sql.Append("       ACCDROGAS AS \"drogas\",\n");
            sql.Append("       ACCDESCRIPCIONDROGAS AS \"drogasTexto\",\n");
            sql.Append("       ACCVALORES AS \"valores\",\n");
            sql.Append("       ACCDESCRIPCIONVALORES AS \"valoresTexto\",\n");
            sql.Append("       ACCPRENDAS AS \"prendas\",\n");
            sql.Append("       ACCDESCRPCIONPRENDAS AS \"prendasTexto\",\n");
            sql.Append("       ACCOTRO AS \"otros\",\n");
            sql.Append("       ACCDESCRIPCIONOTROS AS \"otrosTexto\",\n");
            sql.Append("       NULL AS \"idEstatusReporte\",\n");
            sql.Append("       NULL AS \"numeroReporte\",\n");
            sql.Append("       ACCIDDELEGACION AS \"idOficinaDelegacion\",\n");
            sql.Append("       ACCCONVENIOOBSERVACIONES AS \"observacionesConvenio\",\n");
            sql.Append("       ACCCONVENIO AS \"convenio\",\n");
            sql.Append("       NULL AS \"idEntidadCompetencia\"\n");
            sql.Append("FROM sitteg.ACCIDENTES\n");
            sql.Append("WHERE ACCID BETWEEN :ini AND :fin\n");
            sql.Append("ORDER BY ACCID");

            pams.Add("sql", sql.ToString());
            
            sql.Clear();

            log.Debug("Recuperando los datos para la tabla Accidentes, realizando paginación de 100 elementos.");

            List<Accidentes>? accs = null;

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

            log.Info("Se concluye el flujo de migración para Accidentes.");
        }
    }
}