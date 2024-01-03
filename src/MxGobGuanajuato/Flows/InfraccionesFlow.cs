
using System.Text;
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Dtos;
using Newtonsoft.Json;

namespace MxGobGuanajuato.Flows
{
    public sealed class InfraccionesFlow : IFlowData
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(InfraccionesFlow));

        private IReaderData<String>? crr;

        private IReaderData<String>? cwr;

        private IReaderData<Infracciones>? infr;

        private IWriterData<Infracciones>? infw;

        public IReaderData<String> CRR {set {this.crr = value;}}

        public IReaderData<String> CWR {set {this.cwr = value;}}

        public IReaderData<Infracciones> INFR {set {this.infr = value;}}

        public IWriterData<Infracciones> INFW {set {this.infw = value;}}

        public void Inside(IDictionary<string, object> p)
        {
            log.Info("Vamos a comenzar el flujo de migración para Infracciones.");
            
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
                sql.Append("       '\"idMax\": ', COALESCE(MAX(idInfraccion), 0),\n");
                sql.Append("       '}') AS json\n");
                sql.Append("FROM [dbo].[Infracciones]");

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

            sql.Append("SELECT inf.INFID AS \"idInfraccion\",\n");
            sql.Append("       inf.INFIDOFICIAL AS \"idOficial\",\n");
            sql.Append("       inf.INFIDDELEGACION AS \"idDependencia\",\n");
            sql.Append("       inf.INFIDDELEGACION AS \"idDelegacion\",\n");
            sql.Append("       inf.INFIDVEHICULO AS \"idVehiculo\",\n");
            sql.Append("       inf.INFIDTIPOAPLICACION AS \"idAplicacion\",\n");
            sql.Append("       inf.INFIDTIPOGARANTIA AS \"idGarantia\",\n");
            sql.Append("       inf.INFIDESTATUS AS \"idEstatusInfraccion\",\n");
            sql.Append("       inf.INFIDMUNICIPIO AS \"idMunicipio\",\n");
            sql.Append("       inf.INFIDTRAMO AS \"idTramo\",\n");
            sql.Append("       inf.INFIDCARRETERA AS \"idCarretera\",\n");
            sql.Append("       inf.INFIDPROPIETARIOVEHICULO AS \"idPersona\",\n");
            sql.Append("       inf.INFIDCONDUCTOR AS \"idPersonaInfraccion\",\n");
            sql.Append("       inf.INFNOPLACA AS \"placasVehiculo\",\n");
            sql.Append("       inf.INFFOLIO AS \"folioInfraccion\",\n");
            sql.Append("       TO_DATE(inf.INFECHA, 'yyyymmdd')  AS \"fechaInfraccion\",\n");
            sql.Append("       inf.INFKILOMETRO AS \"kmCarretera\",\n");
            sql.Append("       inf.INFOBSERVACIONES AS \"observaciones\",\n");
            sql.Append("       inf.INFCALLE AS \"lugarCalle\",\n");
            sql.Append("       inf.INFNUMERO AS \"lugarNumero\",\n");
            sql.Append("       inf.INFCOLONIA AS \"lugarColonia\",\n");
            sql.Append("       inf.INFENTRECALLE AS \"lugarEntreCalle\",\n");
            
            /*sql.Append("       CASE\n");
            sql.Append("            WHEN inf.INFIDTIPOCORTESIA = 2 OR inf.INFIDTIPOCORTESIA = 4\n");
            sql.Append("                THEN 1\n");
            sql.Append("            ELSE 0\n");
            sql.Append("       END as \"infraccionCortesia\",\n");*/

            sql.Append("       inf.INFIDTIPOCORTESIA AS \"infraccionCortesia\",\n");

            sql.Append("       inf.INFTARJETAVEHICULO AS \"NumTarjetaCirculacion\",\n");
            sql.Append("       inf.INFOFICIOCONDONACION AS \"oficioRevocacion\",\n");
            sql.Append("       inf.INFIDESTATUS AS \"estatusProceso\",\n");
            sql.Append("       TO_DATE(inf.INFECHA, 'yyyymmdd') AS \"fechaActualizacion\",\n");
            sql.Append("       0 AS \"actualizadoPor\",\n");
            sql.Append("       CASE\n");
            sql.Append("            WHEN INFBAJA = 0\n");
            sql.Append("                THEN 1\n");
            sql.Append("            ELSE 0\n");
            sql.Append("       END AS \"estatus\",\n");
            sql.Append("       inf.INFRECIBO AS \"reciboPago\",\n");
            sql.Append("       inf.INFMONTOCALIFICACION AS \"monto\",\n");

            sql.Append("       inf.INFMONTOPAGADO AS \"montoPagado\",\n");

            sql.Append("       TO_DATE(inf.INFFECHAPAGO, 'yyyymmdd') AS \"fechaPago\",\n");
            sql.Append("       ofre.ORNOMBRE AS \"lugarPago\",\n");
            sql.Append("       CASE\n");
            sql.Append("            WHEN inf.INFIDESTATUS = 7\n");
            sql.Append("                THEN 1\n");
            sql.Append("            ELSE 0\n");
            sql.Append("       END AS \"idEstatusEnvio\",\n");
            sql.Append("       einf.EINOFICIO AS \"oficioEnvio\",\n");
            sql.Append("       TO_DATE(einf.EINFECHA, 'yyyymmdd') AS \"fechaEnvio\",\n");
            sql.Append("       einf.EINIDOFICINA AS \"idOficinaRenta\",\n");

            sql.Append("       NULL AS \"inventario\",\n");
            /*sql.Append("       NULL AS \"partner\",\n");
            sql.Append("       NULL AS \"cuenta\",\n");
            sql.Append("       NULL AS \"objeto\",\n");
            sql.Append("       NULL AS \"documento\"\n");*/

            sql.Append("       inf.INFBUSINESSPARTNERFIN AS \"partner\",\n");
            sql.Append("       inf.INFCUENTAFINANZAS AS \"cuenta\",\n");
            sql.Append("       inf.INFOBJETOFINANZAS AS \"objeto\",\n");
            sql.Append("       inf.INFNUMDOCUMENTOFINANZAS AS \"documento\",\n");
            sql.Append("       inf.INFTRANSITO AS \"transito\"\n");

            sql.Append("FROM SITTEG.INFRACCIONES inf\n");
            sql.Append("LEFT JOIN SITTEG.INFRACCIONESENVIADAS infenv ON infenv.IEIDINFRACCION=inf.INFID\n");
            sql.Append("LEFT JOIN SITTEG.ENVIOINFRACCIONES einf ON einf.EINID=infenv.IEIDENVIO\n");
            sql.Append("LEFT JOIN SITTEG.OFICINASRENTAS ofre ON inf.INFIDLUGARPAGO=ofre.ORID\n");
            sql.Append("LEFT JOIN SITTEG.INFRACCIONESMOTIVOS infmo ON infmo.IMIDINFRACCION=inf.INFID\n");
            sql.Append("LEFT JOIN SITTEG.MOTIVOSINFRACCION catmot ON infmo.IMIDMOTIVO=catmot.MIID\n");
            sql.Append("WHERE inf.INFID BETWEEN :ini AND :fin\n");
            sql.Append("ORDER BY inf.INFID");
            
            pams.Add("sql", sql.ToString());
            
            sql.Clear();

            log.Debug("Recuperando los datos para la tabla Infracciones, realizando paginación de 100 elementos.");

            List<Infracciones>? infs = null;

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

                if((infs = infr?.Get(pams)) == null) {
                    log.Error("No se recupero ningún registro de SITTEG.");
                    log.Info("Marca inicio -> " + mrkIni);
                    log.Info("Marca fin ->" + mrkFin);

                    break;
                }

                log.Debug("Se recuperaron " + infs.Count + " registros.");

                if(infw != null)
                    ei = infw.Set(infs);

                if(ei != infs.Count) {
                    log.Error("No se realizo la inserción de todos los registros en SREGINA.");
                    log.Info("Marca inicio de la pagina -> " + mrkIni);
                    log.Info("Marca fin de la pagina ->" + mrkFin);
                }

                ec += ei;
                
                mrkIni = mrkFin + 1;
            }

            log.Debug("Se migraron " + ec + " registros.");

            log.Info("Se concluye el flujo de migración para Infracciones.");
        }
    }
}