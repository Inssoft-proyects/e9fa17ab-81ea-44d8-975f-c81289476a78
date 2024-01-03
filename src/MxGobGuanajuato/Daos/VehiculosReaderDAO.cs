
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace MxGobGuanajuato.Daos
{
    public sealed class VehiculosReaderDAO : IReaderData<Vehiculos>
    {
        public VehiculosReaderDAO(DBReaderConfigurer dbr)
        {
            this.dbr = dbr;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(VehiculosReaderDAO));

        private readonly DBReaderConfigurer dbr;

        public List<Vehiculos>? Get(IDictionary<string, object> p)
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

            List<Vehiculos>? vs = null;

            Vehiculos? v = null;

            while(odr.Read()) {
                try {
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idVehiculo")).IsNull)
                    {
                        log.Error("No se recupero el campo idVehiculo.");

                        break;
                    }

                    v = new() {
                        IdVehiculo = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idVehiculo")), 22).Value
                    };

                    if(odr.GetOracleString(odr.GetOrdinal("placas")).IsNull)
                        v.Placas = null;
                    else
                        v.Placas = odr.GetOracleString(odr.GetOrdinal("placas")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("serie")).IsNull)
                        v.Serie = null;
                    else
                        v.Serie = odr.GetOracleString(odr.GetOrdinal("serie")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("tarjeta")).IsNull)
                        v.Tarjeta = null;
                    else
                        v.Tarjeta = odr.GetOracleString(odr.GetOrdinal("tarjeta")).Value;
                    
                    if(odr.GetOracleDate(odr.GetOrdinal("vigenciaTarjeta")).IsNull)
                        v.VigenciaTarjeta = null;
                    else
                        v.VigenciaTarjeta = odr.GetOracleDate(odr.GetOrdinal("vigenciaTarjeta")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idMarcaVehiculo")).IsNull)
                        v.IdMarcaVehiculo = null;
                    else
                        v.IdMarcaVehiculo = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idMarcaVehiculo")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idSubmarca")).IsNull)
                        v.IdSubmarca = null;
                    else
                        v.IdSubmarca = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idSubmarca")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idTipoVehiculo")).IsNull)
                        v.IdTipoVehiculo = null;
                    else
                        v.IdTipoVehiculo = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idTipoVehiculo")), 22).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("modelo")).IsNull)
                        v.Modelo = null;
                    else
                        v.Modelo = odr.GetOracleString(odr.GetOrdinal("modelo")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idColor")).IsNull)
                        v.IdColor = null;
                    else
                        v.IdColor = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idColor")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idEntidad")).IsNull)
                        v.IdEntidad = null;
                    else
                        v.IdEntidad = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idEntidad")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idCatTipoServicio")).IsNull)
                        v.IdCatTipoServicio = null;
                    else
                        v.IdCatTipoServicio = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idCatTipoServicio")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idSubtipoServicio")).IsNull)
                        v.IdSubtipoServicio = null;
                    else
                        v.IdSubtipoServicio = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idSubtipoServicio")), 22).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("propietario")).IsNull)
                        v.Propietario = null;
                    else
                        v.Propietario = odr.GetOracleString(odr.GetOrdinal("propietario")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("numeroEconomico")).IsNull)
                        v.NumeroEconomico = null;
                    else
                        v.NumeroEconomico = odr.GetOracleString(odr.GetOrdinal("numeroEconomico")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("paisManufactura")).IsNull)
                        v.PaisManufactura = null;
                    else
                        v.PaisManufactura = odr.GetOracleString(odr.GetOrdinal("paisManufactura")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idPersona")).IsNull)
                        v.IdPersona = null;
                    else
                        v.IdPersona = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idPersona")), 22).Value;
                    
                    if(odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).IsNull)
                        v.FechaActualizacion = null;
                    else
                        v.FechaActualizacion = odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")).IsNull)
                        v.ActualizadoPor = null;
                    else
                        v.ActualizadoPor = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("estatus")).IsNull)
                        v.Estatus = null;
                    else
                        v.Estatus = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("estatus")), 1).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("motor")).IsNull)
                        v.Motor = null;
                    else
                        v.Motor = odr.GetOracleString(odr.GetOrdinal("motor")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("capacidad")).IsNull)
                        v.Capacidad = null;
                    else
                        v.Capacidad = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("capacidad")), 2).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("poliza")).IsNull)
                        v.Poliza = null;
                    else
                        v.Poliza = odr.GetOracleString(odr.GetOrdinal("poliza")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("carga")).IsNull)
                        v.Carga = null;
                    else
                        v.Carga = OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("carga")), 1).Value == 1;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("otros")).IsNull)
                        v.Otros = null;
                    else
                        v.Otros = odr.GetOracleString(odr.GetOrdinal("otros")).Value;
                    
                    vs ??= new();

                    vs.Add(v);
                } catch(IndexOutOfRangeException iore) {
                    log.Error(iore);

                    break;
                }
            }

            odr.Dispose();

            odr.Close();

            return vs;
        }
    }
}