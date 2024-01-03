using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace MxGobGuanajuato.Daos
{
    public sealed class VehiculosAccidenteReaderDAO : IReaderData<VehiculosAccidente>
    {
        public VehiculosAccidenteReaderDAO(DBReaderConfigurer dbr)
        {
            this.dbr = dbr;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(VehiculosAccidenteReaderDAO));

        private readonly DBReaderConfigurer dbr;

        public List<VehiculosAccidente>? Get(IDictionary<string, object> p)
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

            List<VehiculosAccidente>? vaccs = null;

            VehiculosAccidente? vacc = null;

            while(odr.Read()) {
                try {
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idVehiculoAccidente")).IsNull)
                    {
                        log.Error("No se recupero el campo idVehiculoAccidente.");

                        break;
                    }

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idVehiculo")).IsNull)
                    {
                        log.Error("No se recupero el campo idVehiculo.");

                        break;
                    }

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idAccidente")).IsNull)
                    {
                        log.Error("No se recupero el campo idAccidente.");

                        break;
                    }

                    vacc = new() {
                        IdVehiculoAccidente = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idVehiculoAccidente")), 22).Value,
                        IdVehiculo = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idVehiculo")), 22).Value,
                        IdAccidente = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idAccidente")), 22).Value
                    };

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idPersona")).IsNull)
                        vacc.IdPersona = null;
                    else
                        vacc.IdPersona = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idPersona")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("montoVehiculo")).IsNull)
                        vacc.MontoVehiculo = null;
                    else
                        vacc.MontoVehiculo = (double)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("montoVehiculo")), 12).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("placa")).IsNull)
                        vacc.Placa = null;
                    else
                        vacc.Placa = odr.GetOracleString(odr.GetOrdinal("placa")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("serie")).IsNull)
                        vacc.Serie = null;
                    else
                        vacc.Serie = odr.GetOracleString(odr.GetOrdinal("serie")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("estatus")).IsNull)
                        vacc.Estatus = null;
                    else
                        vacc.Estatus = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("estatus")), 1).Value;
                    
                    vaccs ??= new();

                    vaccs.Add(vacc);
                } catch(IndexOutOfRangeException iore) {
                    log.Error(iore);

                    break;
                }
            }

            odr.Dispose();

            odr.Close();

            return vaccs;
        }
    }
}