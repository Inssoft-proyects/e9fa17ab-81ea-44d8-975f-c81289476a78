using System.Runtime.Serialization.Formatters.Binary;
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace MxGobGuanajuato.Daos
{
    public sealed class InfraccionesReaderDAO : IReaderData<Infracciones>
    {
        public InfraccionesReaderDAO(DBReaderConfigurer dbr)
        {
            this.dbr = dbr;
        }
        private static readonly ILog log = LogManager.GetLogger(typeof(InfraccionesReaderDAO));

        private readonly DBReaderConfigurer dbr;

        public List<Infracciones>? Get(IDictionary<string, object> p)
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

            List<Infracciones>? infs = null;

            Infracciones? inf = null;

            MemoryStream? ms = null;

            BinaryFormatter bf = new();
            
            #pragma warning disable SYSLIB0011
            while(odr.Read())
            {
                try { 
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idInfraccion")).IsNull)
                    {
                        log.Error("No se recupero el campo idFraccion.");

                        break;
                    }
                    
                    inf = new()
                    {
                        IdInfraccion = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idInfraccion")), 22).Value
                    };
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idOficial")).IsNull)
                        inf.IdOficial = null;
                    else
                        inf.IdOficial = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idOficial")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idDependencia")).IsNull)
                        inf.IdDependencia = null;
                    else
                        inf.IdDependencia = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idDependencia")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idDelegacion")).IsNull)
                        inf.IdDelegacion = null;
                    else
                        inf.IdDelegacion = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idDelegacion")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idVehiculo")).IsNull)
                        inf.IdVehiculo = null;
                    else
                        inf.IdVehiculo = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idVehiculo")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idAplicacion")).IsNull)
                        inf.IdAplicacion = null;
                    else
                        inf.IdAplicacion = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idAplicacion")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idGarantia")).IsNull)
                        inf.IdGarantia = null;
                    else
                        inf.IdGarantia = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idGarantia")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idEstatusInfraccion")).IsNull)
                        inf.IdEstatusInfraccion = null;
                    else
                        inf.IdEstatusInfraccion = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idEstatusInfraccion")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idMunicipio")).IsNull)
                        inf.IdMunicipio = null;
                    else
                        inf.IdMunicipio = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idMunicipio")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idTramo")).IsNull)
                        inf.IdTramo = null;
                    else
                        inf.IdTramo = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idTramo")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idCarretera")).IsNull)
                        inf.IdCarretera = null;
                    else
                        inf.IdCarretera = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idCarretera")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idPersona")).IsNull)
                        inf.IdPersona = null;
                    else
                        inf.IdPersona = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idPersona")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idPersonaInfraccion")).IsNull)
                        inf.IdPersonaInfraccion = null;
                    else
                        inf.IdPersonaInfraccion = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idPersonaInfraccion")), 22).Value;

                    if(odr.GetOracleString(odr.GetOrdinal("placasVehiculo")).IsNull)
                        inf.PlacasVehiculo = null;
                    else
                        inf.PlacasVehiculo = odr.GetOracleString(odr.GetOrdinal("placasVehiculo")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("folioInfraccion")).IsNull)
                        inf.FolioInfraccion = null;
                    else
                        inf.FolioInfraccion = odr.GetOracleString(odr.GetOrdinal("folioInfraccion")).Value;
                    
                    if(odr.GetOracleDate(odr.GetOrdinal("fechaInfraccion")).IsNull)
                        inf.FechaInfraccion = null;
                    else
                        inf.FechaInfraccion = odr.GetOracleDate(odr.GetOrdinal("fechaInfraccion")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("kmCarretera")).IsNull)
                        inf.KmCarretera = null;
                    else
                        inf.KmCarretera = Convert.ToString(OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("kmCarretera")), 11).Value);
                    
                    if(odr.GetOracleString(odr.GetOrdinal("observaciones")).IsNull)
                        inf.Observaciones = null;
                    else
                        inf.Observaciones = odr.GetOracleString(odr.GetOrdinal("observaciones")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("lugarCalle")).IsNull)
                        inf.LugarCalle = null;
                    else
                        inf.LugarCalle = odr.GetOracleString(odr.GetOrdinal("lugarCalle")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("lugarNumero")).IsNull)
                        inf.LugarNumero = null;
                    else
                        inf.LugarNumero = odr.GetOracleString(odr.GetOrdinal("lugarNumero")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("lugarColonia")).IsNull)
                        inf.LugarColonia = null;
                    else
                        inf.LugarColonia = odr.GetOracleString(odr.GetOrdinal("lugarColonia")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("lugarEntreCalle")).IsNull)
                        inf.LugarEntreCalle = null;
                    else
                        inf.LugarEntreCalle = odr.GetOracleString(odr.GetOrdinal("lugarEntreCalle")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("infraccionCortesia")).IsNull)
                        inf.InfraccionCortesia = null;
                    else
                        inf.InfraccionCortesia = OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("infraccionCortesia")), 1).Value == 1;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("NumTarjetaCirculacion")).IsNull)
                        inf.NumTarjetaCirculacion = null;
                    else
                        inf.NumTarjetaCirculacion = odr.GetOracleString(odr.GetOrdinal("NumTarjetaCirculacion")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("oficioRevocacion")).IsNull)
                        inf.OficioRevocacion = null;
                    else
                        inf.OficioRevocacion = odr.GetOracleString(odr.GetOrdinal("oficioRevocacion")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("estatusProceso")).IsNull)
                        inf.EstatusProceso = null;
                    else
                        inf.EstatusProceso = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("estatusProceso")), 22).Value;

                    if(odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).IsNull)
                        inf.FechaActualizacion = null;
                    else
                        inf.FechaActualizacion = odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")).IsNull)
                        inf.ActualizadoPor = null;
                    else
                        inf.ActualizadoPor = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")), 1).Value;

                    if(odr.GetOracleDecimal(odr.GetOrdinal("estatus")).IsNull)
                        inf.Estatus = null;
                    else
                        inf.Estatus = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("estatus")), 1).Value;

                    if(odr.GetOracleString(odr.GetOrdinal("reciboPago")).IsNull)
                        inf.ReciboPago = null;
                    else
                        inf.ReciboPago = odr.GetOracleString(odr.GetOrdinal("reciboPago")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("monto")).IsNull)
                        inf.Monto = null;
                    else
                        inf.Monto = (double)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("monto")), 12).Value;

                    if(odr.GetOracleDecimal(odr.GetOrdinal("montoPagado")).IsNull)
                        inf.MontoPagado = null;
                    else
                        inf.MontoPagado = (double)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("montoPagado")), 12).Value;

                    if(odr.GetOracleDate(odr.GetOrdinal("fechaPago")).IsNull)
                        inf.FechaPago = null;
                    else
                        inf.FechaPago = odr.GetOracleDate(odr.GetOrdinal("fechaPago")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("lugarPago")).IsNull)
                        inf.LugarPago = null;
                    else
                        inf.LugarPago = odr.GetOracleString(odr.GetOrdinal("lugarPago")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idEstatusEnvio")).IsNull)
                        inf.IdEstatusEnvio = null;
                    else
                        inf.IdEstatusEnvio = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idEstatusEnvio")), 1).Value;

                    if(odr.GetOracleString(odr.GetOrdinal("oficioEnvio")).IsNull)
                        inf.OficioEnvio = null;
                    else
                        inf.OficioEnvio = odr.GetOracleString(odr.GetOrdinal("oficioEnvio")).Value;
                    
                    if(odr.GetOracleDate(odr.GetOrdinal("fechaEnvio")).IsNull)
                        inf.FechaEnvio = null;
                    else
                        inf.FechaEnvio = odr.GetOracleDate(odr.GetOrdinal("fechaEnvio")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idOficinaRenta")).IsNull)
                        inf.IdOficinaRenta = null;
                    else
                        inf.IdOficinaRenta = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idOficinaRenta")), 22).Value;
                    
                    try {
                        if(odr.GetOracleBlob(odr.GetOrdinal("inventario")).IsEmpty)
                            inf.Inventario = null;
                        else {
                            ms = new();

                            try {
                                bf.Serialize(ms, odr.GetOracleBlob(odr.GetOrdinal("inventario")));
                            } catch(IndexOutOfRangeException iore) {
                                log.Debug(iore);
                            } catch(ArgumentNullException ane) {
                                log.Debug(ane);
                            }

                            inf.Inventario = ms.ToArray();

                            ms.Dispose();
                            ms.Close();
                        }
                    } catch(InvalidCastException ice) {
                        log.Debug(ice);

                        inf.Inventario = null;
                    }

                    if(odr.GetOracleString(odr.GetOrdinal("partner")).IsNull)
                        inf.Partner = null;
                    else
                        inf.Partner = odr.GetOracleString(odr.GetOrdinal("partner")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("cuenta")).IsNull)
                        inf.Cuenta = null;
                    else
                        inf.Cuenta = odr.GetOracleString(odr.GetOrdinal("cuenta")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("objeto")).IsNull)
                        inf.Objeto = null;
                    else
                        inf.Objeto = odr.GetOracleString(odr.GetOrdinal("objeto")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("documento")).IsNull)
                        inf.Documento = null;
                    else
                        inf.Documento = odr.GetOracleString(odr.GetOrdinal("documento")).Value;

                    if(odr.GetOracleDecimal(odr.GetOrdinal("transito")).IsNull)
                        inf.Transito = null;
                    else
                        inf.Transito = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("transito")), 22).Value;
                    
                    infs ??= new();

                    infs.Add(inf);
                } catch(IndexOutOfRangeException iore) {
                    log.Error(iore);

                    break;
                }
            }
            #pragma warning restore SYSLIB0011

            odr.Dispose();

            odr.Close();

            return infs;
        }
    }
}