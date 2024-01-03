
using System.Data;
using System.Text;
using log4net;
using Microsoft.Data.SqlClient;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;

namespace MxGobGuanajuato.Daos
{
    public sealed class MotivosInfraccionWriterDAO : IWriterData<MotivosInfraccion>
    {
        public MotivosInfraccionWriterDAO(DBWriterConfigurer dbw)
        {
            this.dbw = dbw;

            StringBuilder sql = new();

            sql.Append("INSERT INTO [dbo].[motivosInfraccion]([idMotivoInfraccion], [calificacionMinima], [calificacionMaxima], [calificacion],\n");
            sql.Append("									  [fechaActualizacion], [actualizadoPor], [estatus], [idCatMotivosInfraccion],\n");
            sql.Append("									  [idInfraccion], [IdConcepto], [IdSubConcepto], [prioridad])\n");
            sql.Append("	   VALUES(@idMotivoInfraccion, @calificacionMinima, @calificacionMaxima, @calificacion,\n");
            sql.Append("			  @fechaActualizacion, @actualizadoPor, @estatus, @idCatMotivosInfraccion,\n");
            sql.Append("			  @idInfraccion, @IdConcepto, @IdSubConcepto, @prioridad)");

            this.sql = sql.ToString();

            sql.Clear();
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(MotivosInfraccionWriterDAO));

        private readonly DBWriterConfigurer dbw;

        private readonly String sql;

        public int Set(List<MotivosInfraccion> os)
        {
            int r = 0;

            using SqlCommand scmd = dbw.GetCommand();

            scmd.CommandType = CommandType.Text;

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[motivosInfraccion] ON";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);

                return r;
            }

            scmd.CommandText = sql;

            os.ForEach(mi => {
                scmd.Parameters.Add("@idMotivoInfraccion", SqlDbType.Int).Value = mi.IdMotivoInfraccion;
                scmd.Parameters.Add("@calificacionMinima", SqlDbType.Int).Value = mi.CalificacionMinima;
                scmd.Parameters.Add("@calificacionMaxima", SqlDbType.Int).Value = mi.CalificacionMaxima;
                scmd.Parameters.AddWithValue("@calificacion", mi.Calificacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@fechaActualizacion", mi.FechaActualizacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@actualizadoPor", mi.ActualizadoPor).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatus", mi.Estatus).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idCatMotivosInfraccion", mi.IdCatMotivosInfraccion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idInfraccion", mi.IdInfraccion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@IdConcepto", mi.IdConcepto).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@IdSubConcepto", mi.IdSubConcepto).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@prioridad", mi.Prioridad).Value ??= DBNull.Value;
                
                try {
                    r += scmd.ExecuteNonQuery();
                } catch(SqlException se) {
                    log.Error(se);
                    log.Info(mi);
                }

                scmd.Parameters.Clear();
            });

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[motivosInfraccion] OFF";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);
            }

            return r;
        }
    }
}