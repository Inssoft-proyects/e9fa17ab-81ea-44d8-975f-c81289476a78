using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace MxGobGuanajuato.Daos
{
    public sealed class MotivosInfraccionReaderDAO : IReaderData<MotivosInfraccion>
    {
        public MotivosInfraccionReaderDAO(DBReaderConfigurer dbr)
        {
            this.dbr = dbr;
        }
        private static readonly ILog log = LogManager.GetLogger(typeof(MotivosInfraccionReaderDAO));

        private readonly DBReaderConfigurer dbr;

        public List<MotivosInfraccion>? Get(IDictionary<string, object> p)
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

            List<MotivosInfraccion>? mis = null;

            MotivosInfraccion? mi = null;

            int id = -1;

            while(odr.Read())
            {
                try {
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idMotivoInfraccion")).IsNull)
                    {
                        log.Error("No se recupero el campo idMotivoInfraccion.");

                        break;
                    }

                    id = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idMotivoInfraccion")), 22).Value;

                    if(odr.GetOracleDecimal(odr.GetOrdinal("calificacionMinima")).IsNull)
                    {
                        log.Error("No se recupero el campo calificacionMinima para el idMotivoInfraccion -> " + id);

                        break;
                    }

                    if(odr.GetOracleDecimal(odr.GetOrdinal("calificacionMaxima")).IsNull)
                    {
                        log.Error("No se recupero el campo calificacionMaxima para el idMotivoInfraccion -> " + id);

                        break;
                    }

                    mi = new()
                    {
                        IdMotivoInfraccion = id,
                        CalificacionMinima = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("calificacionMinima")), 7).Value,
                        CalificacionMaxima = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("calificacionMaxima")), 7).Value
                    };

                    if(odr.GetOracleDecimal(odr.GetOrdinal("calificacion")).IsNull)
                        mi.Calificacion  = null;
                    else
                        mi.Calificacion = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("calificacion")), 7).Value;
                    
                    if(odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).IsNull)
                        mi.FechaActualizacion = null;
                    else
                        mi.FechaActualizacion = odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")).IsNull)
                        mi.ActualizadoPor = null;
                    else
                        mi.ActualizadoPor = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")), 1).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("estatus")).IsNull)
                        mi.Estatus = null;
                    else
                        mi.Estatus = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("estatus")), 1).Value;

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idCatMotivosInfraccion")).IsNull)
                        mi.IdCatMotivosInfraccion = null;
                    else
                        mi.IdCatMotivosInfraccion = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idCatMotivosInfraccion")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idInfraccion")).IsNull)
                        mi.IdInfraccion = null;
                    else
                        mi.IdInfraccion = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idInfraccion")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("IdConcepto")).IsNull)
                        mi.IdConcepto = null;
                    else
                        mi.IdConcepto =  (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("IdConcepto")), 1).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("IdSubConcepto")).IsNull)
                        mi.IdSubConcepto = null;
                    else
                        mi.IdSubConcepto = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("IdSubConcepto")), 1).Value;

                    mis ??= new();

                    mis.Add(mi);
                } catch(IndexOutOfRangeException iore) {
                    log.Error(iore);

                    break;
                }
            }

            odr.Dispose();

            odr.Close();

            return mis;
        }
    }
}