using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace MxGobGuanajuato.Daos
{
    public sealed class PersonasInfraccionesReaderDAO : IReaderData<PersonasInfracciones>
    {
        public PersonasInfraccionesReaderDAO(DBReaderConfigurer dbr)
        {
            this.dbr = dbr;
        }
        private static readonly ILog log = LogManager.GetLogger(typeof(PersonasInfraccionesReaderDAO));

        private readonly DBReaderConfigurer dbr;

        public List<PersonasInfracciones>? Get(IDictionary<string, object> p)
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

            List<PersonasInfracciones>? pis = null;

            PersonasInfracciones? pi = null;

            while(odr.Read())
            {
                try {
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idInfraccion")).IsNull)
                    {
                        log.Error("No se recupero el campo idInfraccion.");

                        break;
                    }

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idPersonaInfraccion")).IsNull)
                    {
                        log.Error("No se recupero el campo idPersonaInfraccion.");

                        break;
                    }

                    pi = new()
                    {
                        IdPersonaInfraccion = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idPersonaInfraccion")), 22).Value,
                        IdInfraccion = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idInfraccion")), 22).Value
                    };

                    if(odr.GetOracleString(odr.GetOrdinal("numeroLicencia")).IsNull)
                        pi.NumeroLicencia = null;
                    else
                        pi.NumeroLicencia = odr.GetOracleString(odr.GetOrdinal("numeroLicencia")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("CURP")).IsNull)
                        pi.Curp = null;
                    else
                        pi.Curp = odr.GetOracleString(odr.GetOrdinal("CURP")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("RFC")).IsNull)
                        pi.Rfc = null;
                    else
                        pi.Rfc = odr.GetOracleString(odr.GetOrdinal("RFC")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("nombre")).IsNull)
                        pi.Nombre = null;
                    else
                        pi.Nombre = odr.GetOracleString(odr.GetOrdinal("nombre")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("apellidoPaterno")).IsNull)
                        pi.ApellidoPaterno = null;
                    else
                        pi.ApellidoPaterno = odr.GetOracleString(odr.GetOrdinal("apellidoPaterno")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("apellidoMaterno")).IsNull)
                        pi.ApellidoMaterno = null;
                    else
                        pi.ApellidoMaterno = odr.GetOracleString(odr.GetOrdinal("apellidoMaterno")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idCatTipoPersona")).IsNull)
                        pi.IdCatTipoPersona = null;
                    else
                        pi.IdCatTipoPersona = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idCatTipoPersona")), 1).Value;
                    
                    if(odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).IsNull)
                        pi.FechaActualizacion = null;
                    else
                        pi.FechaActualizacion = odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")).IsNull)
                        pi.ActualizadoPor = null;
                    else
                        pi.ActualizadoPor = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")), 1).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("estatus")).IsNull)
                        pi.Estatus = null;
                    else
                        pi.Estatus = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("estatus")), 1).Value;

                    pis ??= new();

                    pis.Add(pi);
                } catch(IndexOutOfRangeException iore) {
                    log.Error(iore);

                    break;
                }
            }

            odr.Dispose();

            odr.Close();

            return pis;
        }
    }
}