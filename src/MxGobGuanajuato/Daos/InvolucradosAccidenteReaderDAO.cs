using System.Globalization;
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace MxGobGuanajuato.Daos
{
    public sealed class InvolucradosAccidenteReaderDAO : IReaderData<InvolucradosAccidente>
    {
        public InvolucradosAccidenteReaderDAO(DBReaderConfigurer dbr) 
        {
            this.dbr = dbr;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(InvolucradosAccidenteReaderDAO));

        private readonly DBReaderConfigurer dbr;

        public List<InvolucradosAccidente>? Get(IDictionary<string, object> p)
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

            List<InvolucradosAccidente>? iaccs = null;

            InvolucradosAccidente? iacc = null;

            while(odr.Read()) {
                try {
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idAccidente")).IsNull) {
                        log.Error("No se recupero el campo idAccidente.");

                        break;
                    }

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idPersona")).IsNull) {
                        log.Error("No se recupero el campo idPersona.");

                        break;
                    }

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idVehiculo")).IsNull) {
                        log.Error("No se recupero el campo idVehiculo.");

                        break;
                    }

                    iacc = new() {
                        IdAccidente = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idAccidente")), 22).Value,
                        IdPersona = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idPersona")), 22).Value,
                        IdVehiculo = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idVehiculo")), 22).Value
                    };

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idTipoInvolucrado")).IsNull)
                        iacc.IdTipoInvolucrado = null;
                    else
                        iacc.IdTipoInvolucrado = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idTipoInvolucrado")), 10).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idEstadoVictima")).IsNull)
                        iacc.IdEstadoVictima = null;
                    else
                        iacc.IdEstadoVictima = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idEstadoVictima")), 10).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idHospital")).IsNull)
                        iacc.IdHospital = null;
                    else
                        iacc.IdHospital = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idHospital")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idInstitucionTraslado")).IsNull)
                        iacc.IdInstitucionTraslado = null;
                    else
                        iacc.IdInstitucionTraslado = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idInstitucionTraslado")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idAsiento")).IsNull)
                        iacc.IdAsiento = null;
                    else
                        iacc.IdAsiento = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idAsiento")), 10).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idCinturon")).IsNull)
                        iacc.IdCinturon = null;
                    else
                        iacc.IdCinturon = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idCinturon")), 10).Value;
                    
                    if(odr.GetOracleDate(odr.GetOrdinal("fechaIngreso")).IsNull)
                        iacc.FechaIngreso = null;
                    else
                        iacc.FechaIngreso = odr.GetOracleDate(odr.GetOrdinal("fechaIngreso")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("horaIngreso")).IsNull)
                        iacc.HoraIngreso = null;
                    else
                        try {
                            iacc.HoraIngreso = TimeSpan.ParseExact(odr.GetOracleString(odr.GetOrdinal("horaIngreso")).Value, @"hh\:mm", CultureInfo.InvariantCulture);
                        } catch(ArgumentNullException ane) {
                            log.Error(ane);

                            iacc.HoraIngreso = null;
                        } catch(FormatException fe) {
                            log.Error(fe);

                            iacc.HoraIngreso = null;
                        } catch(OverflowException oe) {
                            log.Error(oe);

                            iacc.HoraIngreso = null;
                        }
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("estatus")).IsNull)
                        iacc.Estatus = null;
                    else
                        iacc.Estatus = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("estatus")), 1).Value;
                    
                    iaccs ??= new();

                    iaccs.Add(iacc);
                } catch(IndexOutOfRangeException iore) {
                    log.Error(iore);

                    break;
                }
            }

            odr.Dispose();

            odr.Close();

            return iaccs;
        }
    }
}