
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace MxGobGuanajuato.Daos
{
    public sealed class PersonasReaderDAO : IReaderData<Personas>
    {
        public PersonasReaderDAO(DBReaderConfigurer dbr)
        {
            this.dbr = dbr;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(PersonasReaderDAO));

        private readonly DBReaderConfigurer dbr;

        public List<Personas>? Get(IDictionary<string, object> p)
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

            List<Personas>? pes = null;

            Personas? pe = null;

            while(odr.Read()) {
                try {
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idPersona")).IsNull)
                    {
                        log.Error("No se recupero el campo idPersona.");

                        break;
                    }

                    pe = new()
                    {
                        IdPersona = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idPersona")), 22).Value
                    };

                    if(odr.GetOracleString(odr.GetOrdinal("numeroLicencia")).IsNull)
                        pe.NumeroLicencia = null;
                    else
                        pe.NumeroLicencia = odr.GetOracleString(odr.GetOrdinal("numeroLicencia")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("RFC")).IsNull)
                        pe.RFC = null;
                    else
                        pe.RFC = odr.GetOracleString(odr.GetOrdinal("RFC")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("CURP")).IsNull)
                        pe.CURP = null;
                    else
                        pe.CURP = odr.GetOracleString(odr.GetOrdinal("CURP")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("nombre")).IsNull)
                        pe.Nombre = null;
                    else
                        pe.Nombre = odr.GetOracleString(odr.GetOrdinal("nombre")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("apellidoPaterno")).IsNull)
                        pe.ApellidoPaterno = null;
                    else
                        pe.ApellidoPaterno = odr.GetOracleString(odr.GetOrdinal("apellidoPaterno")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("apellidoMaterno")).IsNull)
                        pe.ApellidoMaterno = null;
                    else
                        pe.ApellidoMaterno = odr.GetOracleString(odr.GetOrdinal("apellidoMaterno")).Value;
                    
                    if(odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).IsNull)
                        pe.FechaActualizacion = null;
                    else
                        pe.FechaActualizacion = odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")).IsNull)
                        pe.ActualizadoPor = null;
                    else
                        pe.ActualizadoPor = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")), 1).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("estatus")).IsNull)
                        pe.Estatus = null;
                    else
                        pe.Estatus = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("estatus")), 1).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idCatTipoPersona")).IsNull)
                        pe.IdCatTipoPersona = null;
                    else
                        pe.IdCatTipoPersona = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idCatTipoPersona")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idGenero")).IsNull)
                        pe.IdGenero = null;
                    else
                        pe.IdGenero = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idGenero")), 22).Value;
                    
                    if(odr.GetOracleDate(odr.GetOrdinal("fechaNacimiento")).IsNull)
                        pe.FechaNacimiento = null;
                    else
                        pe.FechaNacimiento = odr.GetOracleDate(odr.GetOrdinal("fechaNacimiento")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idTipoLicencia")).IsNull)
                        pe.IdTipoLicencia = null;
                    else
                        pe.IdTipoLicencia = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idTipoLicencia")), 22).Value;
                    
                    if(odr.GetOracleDate(odr.GetOrdinal("vigenciaLicencia")).IsNull)
                        pe.VigenciaLicencia = null;
                    else
                        pe.VigenciaLicencia = odr.GetOracleDate(odr.GetOrdinal("vigenciaLicencia")).Value;
                    
                    pes ??= new();

                    pes.Add(pe);
                } catch(IndexOutOfRangeException iore) {
                    log.Error(iore);

                    break;
                }
            }

            odr.Dispose();

            odr.Close();

            return pes;
        }
    }
}