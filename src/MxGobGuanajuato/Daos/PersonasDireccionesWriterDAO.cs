
using System.Data;
using System.Text;
using log4net;
using Microsoft.Data.SqlClient;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;

namespace MxGobGuanajuato.Daos
{
    public sealed class PersonasDireccionesWriterDAO : IWriterData<PersonasDirecciones>
    {
        public PersonasDireccionesWriterDAO(DBWriterConfigurer dbw)
        {
            this.dbw = dbw;

            StringBuilder sql = new();

            sql.Append("INSERT INTO [dbo].[personasDirecciones]([idPersonasDirecciones], [idEntidad], [idMunicipio], [codigoPostal],\n");
            sql.Append("										[colonia], [calle], [numero], [telefono],\n");
            sql.Append("										[correo], [idPersona], [actualizadoPor], [fechaActualizacion],\n");
            sql.Append("										[estatus])\n");
            sql.Append("	   VALUES(@idPersonasDirecciones, @idEntidad, @idMunicipio, @codigoPostal,\n");
            sql.Append("			  @colonia, @calle, @numero, @telefono,\n");
            sql.Append("			  @correo, @idPersona, @actualizadoPor, @fechaActualizacion,\n");
            sql.Append("			  @estatus)");

            this.sql = sql.ToString();

            sql.Clear();
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(PersonasDireccionesWriterDAO));

        private readonly DBWriterConfigurer dbw;

        private readonly String sql;

        public int Set(List<PersonasDirecciones> os)
        {
            int r = 0;

            using SqlCommand scmd = dbw.GetCommand();

            scmd.CommandType = CommandType.Text;

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[personasDirecciones] ON";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);

                return r;
            }

            scmd.CommandText = sql;

            os.ForEach(pd => {
                scmd.Parameters.Add("@idPersonasDirecciones", SqlDbType.Int).Value = pd.IdPersonasDirecciones;
                scmd.Parameters.AddWithValue("@idEntidad", pd.IdEntidad).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idMunicipio", pd.IdMunicipio).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@codigoPostal", pd.CodigoPostal).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@colonia", pd.Colonia).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@calle", pd.Calle).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@numero", pd.Numero).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@telefono", pd.Telefono).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@correo", pd.Correo).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idPersona", pd.IdPersona).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@actualizadoPor", pd.ActualizadoPor).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@fechaActualizacion", pd.FechaActualizacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatus", pd.Estatus).Value ??= DBNull.Value;

                try {
                    r += scmd.ExecuteNonQuery();
                } catch(SqlException se) {
                    log.Error(se);
                    log.Info(pd);
                }

                scmd.Parameters.Clear();
            });

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[personasDirecciones] OFF";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);
            }

            return r;
        }
    }
}