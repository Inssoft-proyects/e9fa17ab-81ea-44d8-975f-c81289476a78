using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Dtos;
using MxGobGuanajuato.Cnfs;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace MxGobGuanajuato.Daos
{
    public sealed class CatConceptoInfraccionReaderDAO : IReaderData<CatConceptoInfraccion>
    {
        public CatConceptoInfraccionReaderDAO(DBReaderConfigurer dbr)
        {
            this.dbr = dbr;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(CatConceptoInfraccionReaderDAO));

        private readonly DBReaderConfigurer dbr;

        public List<CatConceptoInfraccion>? Get(IDictionary<string, object> p)
        {
            using OracleCommand ocmd = dbr.GetCommand();

            ocmd.BindByName = true;

            ocmd.CommandText = (String)p["sql"];

            p.ToList().ForEach(e => {
                if(e.Key != "sql")
                    ocmd.Parameters.Add(new OracleParameter(e.Key, e.Value));
            });

            OracleDataReader? odr = null;

            try {
                odr = ocmd.ExecuteReader();
            } catch(InvalidOperationException ioe) {
                log.Error(ioe);
                log.Info(p);

                return null;
            } catch(ArgumentException ae) {
                log.Error(ae);
                log.Info(p);

                return null;
            } catch(OracleException oe) {
                log.Error(oe);
                log.Info(p);

                return null;
            }

            List<CatConceptoInfraccion>? cmis = null;

            CatConceptoInfraccion? cmi = null;

            int id = -1;

            while(odr.Read()) {
                try {
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idConcepto")).IsNull)
                    {
                        log.Error("No se recupero el campo idConcepto.");

                        break;
                    }
                    id = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idConcepto")), 22).Value;

                    
                    if(odr.GetOracleString(odr.GetOrdinal("concepto")).IsNull)
                    {
                        log.Error("No se recupero el campo concepto para el idConcepto -> " + id);

                        break;
                    }
                    cmi = new()
                        {
                        IdConcepto = id,
                        Concepto = odr.GetOracleString(odr.GetOrdinal("concepto")).Value
                        };

                    if(odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).IsNull)
                        cmi.FechaActualizacion = null;
                    else
                        cmi.FechaActualizacion = odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")).IsNull)
                        cmi.ActualizadoPor = null;
                    else
                        cmi.ActualizadoPor =  (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("estatus")).IsNull)
                        cmi.Estatus = null;
                    else
                        cmi.Estatus =  (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("estatus")), 22).Value;

                    cmis ??= new();

                    cmis.Add(cmi);
                } catch(IndexOutOfRangeException iore) {
                    log.Error(iore);
                    log.Info(cmi);

                    break;
                }
            }

            odr.Dispose();

            odr.Close();

            return cmis;
        }
    }
}