
using System.Globalization;
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace MxGobGuanajuato.Daos
{
    public sealed class AccidentesReaderDAO : IReaderData<Accidentes>
    {
        public AccidentesReaderDAO(DBReaderConfigurer dbr)
        {
            this.dbr = dbr;
        }
        
        private static readonly ILog log = LogManager.GetLogger(typeof(AccidentesReaderDAO));

        private readonly DBReaderConfigurer dbr;

        public List<Accidentes>? Get(IDictionary<string, object> p)
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

            List<Accidentes>? accs = null;

            Accidentes? acc = null;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("hr-HR");

            while(odr.Read()) {
                try {
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idAccidente")).IsNull)
                    {
                        log.Error("No se recupero el campo idAccidente.");

                        break;
                    }

                    acc = new() {
                        IdAccidente = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idAccidente")), 22).Value
                    };

                    if(odr.GetOracleDate(odr.GetOrdinal("fecha")).IsNull)
                        acc.Fecha = null;
                    else
                        acc.Fecha = odr.GetOracleDate(odr.GetOrdinal("fecha")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("hora")).IsNull)
                        acc.Hora = null;
                    else
                        try {
                            acc.Hora = TimeSpan.ParseExact(odr.GetOracleString(odr.GetOrdinal("hora")).Value, @"hh\:mm", CultureInfo.InvariantCulture);
                        } catch(ArgumentNullException ane) {
                            log.Error(ane);

                            acc.Hora = null;
                        } catch(FormatException fe) {
                            log.Error(fe);

                            acc.Hora = null;
                        } catch(OverflowException oe) {
                            log.Error(oe);

                            acc.Hora = null;
                        }
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idMunicipio")).IsNull)
                        acc.IdMunicipio = null;
                    else
                        acc.IdMunicipio = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idMunicipio")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idCarretera")).IsNull)
                        acc.IdCarretera = null;
                    else
                        acc.IdCarretera = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idCarretera")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idTramo")).IsNull)
                        acc.IdTramo = null;
                    else
                        acc.IdTramo = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idTramo")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("kilometro")).IsNull)
                        acc.Kilometro = null;
                    else
                        acc.Kilometro = Convert.ToString(OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("kilometro")), 13).Value);
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("estatusReporte")).IsNull)
                        acc.EstatusReporte = null;
                    else
                        acc.EstatusReporte = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("estatusReporte")), 22).Value;
                    
                    try {
                        if(odr.GetOracleDecimal(odr.GetOrdinal("idClasificacionAccidente")).IsNull)
                            acc.IdClasificacionAccidente = null;
                        else
                            acc.IdClasificacionAccidente = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idClasificacionAccidente")), 22).Value;
                    } catch(InvalidCastException ex) {
                        log.Error(ex);

                        acc.IdClasificacionAccidente = null;
                    }
                    
                    try {
                        if(odr.GetOracleDecimal(odr.GetOrdinal("idCausaAccidente")).IsNull)
                            acc.IdCausaAccidente = null;
                        else
                            acc.IdCausaAccidente = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idCausaAccidente")), 22).Value;
                    } catch(InvalidCastException ex) {
                        log.Error(ex);

                        acc.IdCausaAccidente = null;
                    } 
                    
                    if(odr.GetOracleString(odr.GetOrdinal("descripcionCausas")).IsNull)
                        acc.DescripcionCausas = null;
                    else
                        acc.DescripcionCausas = odr.GetOracleString(odr.GetOrdinal("descripcionCausas")).Value;
                    
                    try {
                        if(odr.GetOracleDecimal(odr.GetOrdinal("idFactorAccidente")).IsNull)
                            acc.IdFactorAccidente = null;
                        else
                            acc.IdFactorAccidente = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idFactorAccidente")), 22).Value;
                    } catch(InvalidCastException ex) {
                        log.Error(ex);

                        acc.IdFactorAccidente = null;
                    } 
                    
                    try {
                        if(odr.GetOracleDecimal(odr.GetOrdinal("idFactorOpcionAccidente")).IsNull)
                            acc.IdFactorOpcionAccidente = null;
                        else
                            acc.IdFactorOpcionAccidente = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idFactorOpcionAccidente")), 22).Value;
                    } catch(InvalidCastException ex) {
                        log.Error(ex);

                        acc.IdFactorOpcionAccidente = null;
                    }

                    if(odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).IsNull)
                        acc.FechaActualizacion = null;
                    else
                        acc.FechaActualizacion = odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")).IsNull)
                        acc.ActualizadoPor = null;
                    else
                        acc.ActualizadoPor = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("estatus")).IsNull)
                        acc.Estatus = null;
                    else
                        acc.Estatus = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("estatus")), 1).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("montoCamino")).IsNull)
                        acc.MontoCamino = null;
                    else
                        try {
                            acc.MontoCamino = Double.Parse(odr.GetOracleString(odr.GetOrdinal("montoCamino")).Value);
                        } catch(ArgumentNullException ane) {
                            log.Error(ane);

                            acc.MontoCamino = null;
                        } catch(FormatException fe) {
                            log.Error(fe);

                            acc.MontoCamino = null;
                        } catch(OverflowException oe) {
                            log.Error(oe);

                            acc.MontoCamino = null;
                        }
                    
                    if(odr.GetOracleString(odr.GetOrdinal("montoCarga")).IsNull)
                        acc.MontoCarga = null;
                    else
                        try {
                            acc.MontoCarga = Double.Parse(odr.GetOracleString(odr.GetOrdinal("montoCarga")).Value);
                        } catch(ArgumentNullException ane) {
                            log.Error(ane);

                            acc.MontoCarga = null;
                        } catch(FormatException fe) {
                            log.Error(fe);

                            acc.MontoCarga = null;
                        } catch(OverflowException oe) {
                            log.Error(oe);

                            acc.MontoCarga = null;
                        }
                    
                    if(odr.GetOracleString(odr.GetOrdinal("montoPropietarios")).IsNull)
                        acc.MontoPropietarios = null;
                    else
                        try {
                            acc.MontoPropietarios = Double.Parse(odr.GetOracleString(odr.GetOrdinal("montoPropietarios")).Value);
                        } catch(ArgumentNullException ane) {
                            log.Error(ane);

                            acc.MontoPropietarios = null;
                        } catch(FormatException fe) {
                            log.Error(fe);

                            acc.MontoPropietarios = null;
                        } catch(OverflowException oe) {
                            log.Error(oe);

                            acc.MontoPropietarios = null;
                        }
                    
                    if(odr.GetOracleString(odr.GetOrdinal("montoOtros")).IsNull)
                        acc.MontoOtros = null;
                    else
                        try {
                            acc.MontoOtros = Double.Parse(odr.GetOracleString(odr.GetOrdinal("montoOtros")).Value);
                        } catch(ArgumentNullException ane) {
                            log.Error(ane);

                            acc.MontoOtros = null;
                        } catch(FormatException fe) {
                            log.Error(fe);

                            acc.MontoOtros = null;
                        } catch(OverflowException oe) {
                            log.Error(oe);

                            acc.MontoOtros = null;
                        }
                    
                    try {
                        if(odr.GetOracleDecimal(odr.GetOrdinal("montoVehiculo")).IsNull)
                            acc.MontoVehiculo = null;
                        else
                            acc.MontoVehiculo = (double)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("montoVehiculo")), 22).Value;
                    } catch(InvalidCastException ex) {
                        log.Error(ex);

                        acc.MontoVehiculo = null;
                    }
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idElabora")).IsNull)
                        acc.IdElabora = null;
                    else
                        acc.IdElabora = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idElabora")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idSupervisa")).IsNull)
                        acc.IdSupervisa = null;
                    else
                        acc.IdSupervisa = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idSupervisa")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idAutoriza")).IsNull)
                        acc.IdAutoriza = null;
                    else
                        acc.IdAutoriza = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idAutoriza")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idElaboraConsignacion")).IsNull)
                        acc.IdElaboraConsignacion = null;
                    else
                        acc.IdElaboraConsignacion = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idElaboraConsignacion")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("latitud")).IsNull)
                        acc.Latitud = null;
                    else
                        acc.Latitud = (double)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("latitud")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("longitud")).IsNull)
                        acc.Longitud = null;
                    else
                        acc.Longitud = (double)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("longitud")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idCiudad")).IsNull)
                        acc.IdCiudad = null;
                    else
                        acc.IdCiudad = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idCiudad")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idCertificado")).IsNull)
                        acc.IdCertificado = null;
                    else
                        acc.IdCertificado = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idCertificado")), 22).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("entregaOtros")).IsNull)
                        acc.EntregaOtros = null;
                    else
                        acc.EntregaOtros = odr.GetOracleString(odr.GetOrdinal("entregaOtros")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("entregaObjetos")).IsNull)
                        acc.EntregaObjetos = null;
                    else
                        acc.EntregaObjetos = odr.GetOracleString(odr.GetOrdinal("entregaObjetos")).Value;
                    
                    try {
                        if(odr.GetOracleString(odr.GetOrdinal("consignacionHechos")).IsNull)
                            acc.ConsignacionHechos = null;
                        else
                            acc.ConsignacionHechos = odr.GetOracleString(odr.GetOrdinal("consignacionHechos")).Value;
                    } catch(InvalidCastException ex) {
                        log.Error(ex);

                        acc.ConsignacionHechos = null;
                    }
                    
                    if(odr.GetOracleString(odr.GetOrdinal("numeroOficio")).IsNull)
                        acc.NumeroOficio = null;
                    else
                        acc.NumeroOficio = odr.GetOracleString(odr.GetOrdinal("numeroOficio")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idAgenciaMinisterio")).IsNull)
                        acc.IdAgenciaMinisterio = null;
                    else
                        acc.IdAgenciaMinisterio = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idAgenciaMinisterio")), 22).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("recibeMinisterio")).IsNull)
                        acc.RecibeMinisterio = null;
                    else
                        acc.RecibeMinisterio = odr.GetOracleString(odr.GetOrdinal("recibeMinisterio")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idAutoridadEntrega")).IsNull)
                        acc.IdAutoridadEntrega = null;
                    else
                        acc.IdAutoridadEntrega = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idAutoridadEntrega")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idAutoridadDisposicion")).IsNull)
                        acc.IdAutoridadDisposicion = null;
                    else
                        acc.IdAutoridadDisposicion = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idAutoridadDisposicion")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("armas")).IsNull)
                        acc.Armas = null;
                    else
                        acc.Armas = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("armas")), 1).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("drogas")).IsNull)
                        acc.Drogas = null;
                    else
                        acc.Drogas = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("drogas")), 1).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("valores")).IsNull)
                        acc.Valores = null;
                    else
                        acc.Valores = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("valores")), 1).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("prendas")).IsNull)
                        acc.Prendas = null;
                    else
                        acc.Prendas = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("prendas")), 1).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("otros")).IsNull)
                        acc.Otros = null;
                    else
                        acc.Otros = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("otros")), 1).Value;
                    
                    try {
                        if(odr.GetOracleDecimal(odr.GetOrdinal("idEstatusReporte")).IsNull)
                            acc.IdEstatusReporte = null;
                        else
                            acc.IdEstatusReporte = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idEstatusReporte")), 22).Value;
                    } catch(InvalidCastException ex) {
                        log.Error(ex);

                        acc.IdEstatusReporte = null;
                    }
                    
                    try {
                        if(odr.GetOracleString(odr.GetOrdinal("numeroReporte")).IsNull)
                            acc.NumeroReporte = null;
                        else
                            acc.NumeroReporte = odr.GetOracleString(odr.GetOrdinal("numeroReporte")).Value;
                    } catch(InvalidCastException ex) {
                        log.Error(ex);

                        acc.NumeroReporte = null;
                    }

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idOficinaDelegacion")).IsNull)
                        acc.IdOficinaDelegacion = null;
                    else
                        acc.IdOficinaDelegacion = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idOficinaDelegacion")), 22).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("armasTexto")).IsNull)
                        acc.ArmasTexto = null;
                    else
                        acc.ArmasTexto = odr.GetOracleString(odr.GetOrdinal("armasTexto")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("drogasTexto")).IsNull)
                        acc.DrogasTexto = null;
                    else
                        acc.DrogasTexto = odr.GetOracleString(odr.GetOrdinal("drogasTexto")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("valoresTexto")).IsNull)
                        acc.ValoresTexto = null;
                    else
                        acc.ValoresTexto = odr.GetOracleString(odr.GetOrdinal("valoresTexto")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("prendasTexto")).IsNull)
                        acc.PrendasTexto = null;
                    else
                        acc.PrendasTexto = odr.GetOracleString(odr.GetOrdinal("prendasTexto")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("otrosTexto")).IsNull)
                        acc.OtrosTexto = null;
                    else
                        acc.OtrosTexto = odr.GetOracleString(odr.GetOrdinal("otrosTexto")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("convenio")).IsNull)
                        acc.Convenio = null;
                    else
                        acc.Convenio = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("convenio")), 22).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("observacionesConvenio")).IsNull)
                        acc.ObservacionesConvenio = null;
                    else
                        acc.ObservacionesConvenio = odr.GetOracleString(odr.GetOrdinal("observacionesConvenio")).Value;
                    
                    try {
                        if(odr.GetOracleDecimal(odr.GetOrdinal("idEntidadCompetencia")).IsNull)
                            acc.IdEntidadCompetencia = null;
                        else
                            acc.IdEntidadCompetencia = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idEntidadCompetencia")), 22).Value;
                    } catch(InvalidCastException ex) {
                        log.Error(ex);

                        acc.IdEntidadCompetencia = null;
                    }
                    
                    accs ??= new();

                    accs.Add(acc);
                } catch(IndexOutOfRangeException iore) {
                    log.Error(iore);

                    break;
                }
            }

            odr.Dispose();

            odr.Close();

            return accs;
        }
    }
}