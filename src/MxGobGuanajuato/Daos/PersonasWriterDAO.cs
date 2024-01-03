
using System.Data;
using System.Text;
using log4net;
using Microsoft.Data.SqlClient;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;

namespace MxGobGuanajuato.Daos
{
    public sealed class PersonasWriterDAO : IWriterData<Personas>
    {
        public PersonasWriterDAO(DBWriterConfigurer dbw)
        {
            this.dbw = dbw;

            StringBuilder sql = new();

            sql.Append("INSERT INTO [dbo].[personas]([idPersona], [numeroLicencia], [CURP], [RFC],\n");
            sql.Append("							 [nombre], [apellidoPaterno], [apellidoMaterno], [fechaActualizacion],\n");
            sql.Append("							 [actualizadoPor], [estatus], [idCatTipoPersona], [idGenero],\n");
            sql.Append("							 [fechaNacimiento], [idTipoLicencia], [vigenciaLicencia])\n");
            sql.Append("	   VALUES(@idPersona, @numeroLicencia, @CURP, @RFC,\n");
            sql.Append("			  @nombre, @apellidoPaterno, @apellidoMaterno, @fechaActualizacion,\n");
            sql.Append("			  @actualizadoPor, @estatus, @idCatTipoPersona@, @idGenero,\n");
            sql.Append("			  @fechaNacimiento, @idTipoLicencia, @vigenciaLicencia)");

            this.sql = sql.ToString();

            sql.Clear();
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(PersonasWriterDAO));

        private readonly DBWriterConfigurer dbw;

        private readonly String sql;

        public int Set(List<Personas> os)
        {
            int r = 0;

            using SqlCommand scmd = dbw.GetCommand();

            scmd.CommandType = CommandType.Text;

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[personas] ON";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);

                return r;
            }

            scmd.CommandText = sql;

            os.ForEach(pe => {
                scmd.Parameters.Add("@idPersona", SqlDbType.Int).Value = pe.IdPersona;
                scmd.Parameters.AddWithValue("@numeroLicencia", pe.NumeroLicencia).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@CURP", pe.CURP).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@RFC", pe.RFC).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@nombre", pe.Nombre).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@apellidoPaterno", pe.ApellidoPaterno).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@apellidoMaterno", pe.ApellidoMaterno).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@fechaActualizacion", pe.FechaActualizacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@actualizadoPor", pe.ActualizadoPor).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatus", pe.Estatus).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idCatTipoPersona", pe.IdCatTipoPersona).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idGenero", pe.IdGenero).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@fechaNacimiento", pe.FechaNacimiento).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idTipoLicencia", pe.IdTipoLicencia).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@vigenciaLicencia", pe.VigenciaLicencia).Value ??= DBNull.Value;

                try {
                    r += scmd.ExecuteNonQuery();
                } catch(SqlException se) {
                    log.Error(se);
                    log.Info(pe);
                }

                scmd.Parameters.Clear();
            });

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[personas] OFF";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);
            }

            return r;
        }
    }
}