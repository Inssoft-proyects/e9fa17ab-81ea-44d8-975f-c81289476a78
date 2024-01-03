
using System.Data;
using System.Text;
using log4net;
using Microsoft.Data.SqlClient;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;

namespace MxGobGuanajuato.Daos
{
    public sealed class PersonasInfraccionesWriterDAO : IWriterData<PersonasInfracciones>
    {
        public PersonasInfraccionesWriterDAO(DBWriterConfigurer dbw)
        {
            this.dbw = dbw;

            StringBuilder sql = new();

            sql.Append("INSERT INTO [dbo].[personasInfracciones]([idPersonaInfraccion], [idInfraccion], [numeroLicencia], [CURP],\n");
            sql.Append("										 [RFC], [nombre], [apellidoPaterno], [apellidoMaterno],\n");
            sql.Append("										 [idCatTipoPersona], [fechaActualizacion], [actualizadoPor], [estatus])\n");
            sql.Append("	   VALUES(@idPersonaInfraccion, @idInfraccion, @numeroLicencia, @CURP,\n");
            sql.Append("			  @RFC, @nombre, @apellidoPaterno, @apellidoMaterno,\n");
            sql.Append("			  @idCatTipoPersona, @fechaActualizacion, @actualizadoPor, @estatus)");

            this.sql = sql.ToString();

            sql.Clear();
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(PersonasInfraccionesWriterDAO));

        private readonly DBWriterConfigurer dbw;

        private readonly String sql;

        public int Set(List<PersonasInfracciones> os)
        {
            int r = 0;

            using SqlCommand scmd = dbw.GetCommand();

            scmd.CommandType = CommandType.Text;

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[personasInfracciones] ON";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);

                return r;
            }

            scmd.CommandText = sql;

            os.ForEach(pi => {
                scmd.Parameters.Add("@idPersonaInfraccion", SqlDbType.Int).Value = pi.IdPersonaInfraccion;
                scmd.Parameters.Add("@idInfraccion", SqlDbType.Int).Value = pi.IdInfraccion;
                scmd.Parameters.AddWithValue("@numeroLicencia", pi.NumeroLicencia).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@CURP", pi.Curp).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@RFC", pi.Rfc).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@nombre", pi.Nombre).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@apellidoPaterno", pi.ApellidoPaterno).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@apellidoMaterno", pi.ApellidoMaterno).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idCatTipoPersona", pi.IdCatTipoPersona).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@fechaActualizacion", pi.FechaActualizacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@actualizadoPor", pi.ActualizadoPor).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatus", pi.Estatus).Value ??= DBNull.Value;

                try {
                    r += scmd.ExecuteNonQuery();
                } catch(SqlException se) {
                    log.Error(se);
                    log.Info(pi);
                }

                scmd.Parameters.Clear();
            });

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[personasInfracciones] OFF";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);
            }

            return r;
        }
    }
}