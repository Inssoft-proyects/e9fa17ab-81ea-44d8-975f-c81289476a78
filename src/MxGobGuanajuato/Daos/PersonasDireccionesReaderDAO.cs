
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace MxGobGuanajuato.Daos
{
    public sealed class PersonasDireccionesReaderDAO : IReaderData<PersonasDirecciones>
    {
        public PersonasDireccionesReaderDAO(DBReaderConfigurer dbr)
        {
            this.dbr = dbr;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(PersonasDireccionesReaderDAO));

        private readonly DBReaderConfigurer dbr;

        public List<PersonasDirecciones>? Get(IDictionary<string, object> p)
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

            List<PersonasDirecciones>? pds = null;

            PersonasDirecciones? pd = null;

            while(odr.Read()) {
                try {
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idPersonasDirecciones")).IsNull) {
                        log.Error("No se recupero el campo idPersonasDirecciones.");

                        break;
                    }

                    pd = new()
                    {
                        IdPersonasDirecciones = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idPersonasDirecciones")), 22).Value
                    };

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idEntidad")).IsNull)
                        pd.IdEntidad = null;
                    else
                        pd.IdEntidad = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idEntidad")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idMunicipio")).IsNull)
                        pd.IdMunicipio = null;
                    else
                        pd.IdMunicipio = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idMunicipio")), 22).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("codigoPostal")).IsNull)
                        pd.CodigoPostal = null;
                    else
                        pd.CodigoPostal = odr.GetOracleString(odr.GetOrdinal("codigoPostal")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("colonia")).IsNull)
                        pd.Colonia = null;
                    else
                        pd.Colonia = odr.GetOracleString(odr.GetOrdinal("colonia")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("calle")).IsNull)
                        pd.Calle = null;
                    else
                        pd.Calle = odr.GetOracleString(odr.GetOrdinal("calle")).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("numero")).IsNull)
                        pd.Numero = null;
                    else
                        pd.Numero = odr.GetOracleString(odr.GetOrdinal("numero")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("telefono")).IsNull)
                        pd.Telefono = null;
                    else
                        pd.Telefono = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("telefono")), 22).Value;
                    
                    if(odr.GetOracleString(odr.GetOrdinal("correo")).IsNull)
                        pd.Correo = null;
                    else
                        pd.Correo = odr.GetOracleString(odr.GetOrdinal("correo")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idPersona")).IsNull)
                        pd.IdPersona = null;
                    else
                        pd.IdPersona = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idPersona")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")).IsNull)
                        pd.ActualizadoPor = null;
                    else
                        pd.ActualizadoPor = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("actualizadoPor")), 22).Value;
                    
                    if(odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).IsNull)
                        pd.FechaActualizacion = null;
                    else
                        pd.FechaActualizacion = odr.GetOracleDate(odr.GetOrdinal("fechaActualizacion")).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("estatus")).IsNull)
                        pd.Estatus = null;
                    else
                        pd.Estatus = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("estatus")), 1).Value;
                    
                    pds ??= new();

                    pds.Add(pd);
                } catch(IndexOutOfRangeException iore) {
                    log.Error(iore);

                    break;
                }
            }

            odr.Dispose();

            odr.Close();

            return pds;
        }
    }
}