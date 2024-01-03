using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace MxGobGuanajuato
{
    public sealed class GarantiasInfraccionReaderDAO : IReaderData<GarantiasInfraccion>
    {
        public GarantiasInfraccionReaderDAO(DBReaderConfigurer dbr)
        {
            this.dbr = dbr;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(GarantiasInfraccionReaderDAO));

        private readonly DBReaderConfigurer dbr;

        public List<GarantiasInfraccion>? Get(IDictionary<string, object> p)
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

            List<GarantiasInfraccion>? gis = null;

            GarantiasInfraccion? gi = null;

            while(odr.Read()) {
                try {
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idGarantia")).IsNull) {
                        log.Error("No se recupero el campo idGarantia.");

                        break;
                    }

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idCatGarantia")).IsNull) {
                        log.Error("No se recupero el campo idCatGarantia.");

                        break;
                    }

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idTipoPlaca")).IsNull) {
                        log.Error("No se recupero el campo idTipoPlaca.");

                        break;
                    }

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idTipoLicencia")).IsNull) {
                        log.Error("No se recupero el campo idTipoLicencia.");

                        break;
                    }

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idInfraccion")).IsNull) {
                        log.Error("No se recupero el campo idInfraccion.");

                        break;
                    }

                    gi = new() {
                        IdGarantia = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idGarantia")), 22).Value,
                        IdCatGarantia = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idCatGarantia")), 22).Value,
                        IdTipoPlaca = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idTipoPlaca")), 22).Value,
                        IdTipoLicencia = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idTipoLicencia")), 22).Value,
                        IdInfraccion = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idInfraccion")), 22).Value
                    };

                    if(odr.GetOracleString(odr.GetOrdinal("numPlaca")).IsNull)
                        gi.NumPlaca = null;
                    else
                        gi.NumPlaca = odr.GetOracleString(odr.GetOrdinal("numPlaca")).Value;

                    if(odr.GetOracleString(odr.GetOrdinal("numLicencia")).IsNull)
                        gi.NumLicencia = null;
                    else
                        gi.NumLicencia = odr.GetOracleString(odr.GetOrdinal("numLicencia")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("vehiculoDocumento")).IsNull)
                        gi.VehiculoDocumento = null;
                    else
                        gi.VehiculoDocumento = odr.GetOracleString(odr.GetOrdinal("vehiculoDocumento")).Value;
                    
                    if(odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).IsNull)
                        gi.FechaActualizacion = null;
                    else
                        gi.FechaActualizacion = odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")).IsNull)
                        gi.ActualizadoPor = null;
                    else
                        gi.ActualizadoPor = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("estatus")).IsNull)
                        gi.Estatus = null;
                    else
                        gi.Estatus = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("estatus")), 1).Value;
                    
                    gis ??= new();

                    gis.Add(gi);
                } catch(IndexOutOfRangeException iore) {
                    log.Error(iore);

                    break;
                }
            }

            odr.Dispose();

            odr.Close();

            return gis;
        }
    }
}