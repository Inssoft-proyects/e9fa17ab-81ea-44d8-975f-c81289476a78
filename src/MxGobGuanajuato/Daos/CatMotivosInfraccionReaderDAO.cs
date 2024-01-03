using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Dtos;
using MxGobGuanajuato.Cnfs;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace MxGobGuanajuato.Daos
{
    public sealed class CatMotivosInfraccionReaderDAO : IReaderData<CatMotivosInfraccion>
    {
        public CatMotivosInfraccionReaderDAO(DBReaderConfigurer dbr)
        {
            this.dbr = dbr;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(CatMotivosInfraccionReaderDAO));

        private readonly DBReaderConfigurer dbr;

        public List<CatMotivosInfraccion>? Get(IDictionary<string, object> p)
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

            List<CatMotivosInfraccion>? cmis = null;

            CatMotivosInfraccion? cmi = null;

            int id = -1;

            while(odr.Read()) {
                try {
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idCatMotivoInfraccion")).IsNull)
                    {
                        log.Error("No se recupero el campo idCatMotivoInfraccion.");

                        break;
                    }

                    id = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idCatMotivoInfraccion")), 22).Value;

                    if(odr.GetOracleDecimal(odr.GetOrdinal("calificacionMinima")).IsNull)
                    {
                        log.Error("No se recupero el campo calificacionMinima para el idCatMotivoInfraccion -> " + id);

                        break;
                    }

                    if(odr.GetOracleDecimal(odr.GetOrdinal("calificacionMaxima")).IsNull)
                    {
                        log.Error("No se recupero el campo calificacionMaxima para el idCatMotivoInfraccion -> " + id);

                        break;
                    }

                    if(odr.GetOracleString(odr.GetOrdinal("fundamento")).IsNull)
                    {
                        log.Error("No se recupero el campo fundamento para el idCatMotivoInfraccion -> " + id);

                        break;
                    }

                    if(odr.GetOracleDecimal(odr.GetOrdinal("IdConcepto")).IsNull)
                    {
                        log.Error("No se recupero el campo IdConcepto para el idCatMotivoInfraccion -> " + id);

                        break;
                    }

                    if(odr.GetOracleDecimal(odr.GetOrdinal("IdSubConcepto")).IsNull)
                    {
                        log.Error("No se recupero el campo IdSubConcepto para el idCatMotivoInfraccion -> " + id);

                        break;
                    }
                    
                    cmi = new()
                        {
                            IdCatMotivoInfraccion = id,
                            Nombre = odr.GetOracleString(odr.GetOrdinal("nombre")).Value,
                            CalificacionMinima =  (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("calificacionMinima")), 7).Value,
                            CalificacionMaxima = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("calificacionMaxima")), 7).Value,
                            Fundamento =  odr.GetOracleString(odr.GetOrdinal("fundamento")).Value,
                            IdConcepto =  (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("IdConcepto")), 22).Value,
                            IdSubConcepto =  (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("IdSubConcepto")), 22).Value
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