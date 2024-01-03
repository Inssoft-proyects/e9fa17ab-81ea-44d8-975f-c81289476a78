
using System.Data;
using System.Text;
using log4net;
using Microsoft.Data.SqlClient;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;

namespace MxGobGuanajuato.Daos
{
    public sealed class CatMotivosInfraccionWriterDAO : IWriterData<CatMotivosInfraccion>
    {
        public CatMotivosInfraccionWriterDAO(DBWriterConfigurer dbw)
        {
            this.dbw = dbw;

            StringBuilder sql = new();

            sql.Append("INSERT INTO [dbo].[catMotivosInfraccion]([idCatMotivoInfraccion], [nombre], [IdSubConcepto], [fechaActualizacion],\n");
            sql.Append("										 [actualizadoPor], [estatus], [IdConcepto], [calificacionMinima],\n");
            sql.Append("										 [calificacionMaxima], [fundamento])\n");
            sql.Append("	   VALUES(@idCatMotivoInfraccion, @nombre, @IdSubConcepto, @fechaActualizacion,\n");
            sql.Append("	   		  @actualizadoPor, @estatus, @IdConcepto, @calificacionMinima,\n");
            sql.Append("			  @calificacionMaxima, @fundamento)");

            this.sql = sql.ToString();

            sql.Clear();
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(CatMotivosInfraccionWriterDAO));

        private readonly DBWriterConfigurer dbw;

        private readonly String sql;

        public int Set(List<CatMotivosInfraccion> os)
        {
            int r = 0;

            using SqlCommand scmd = dbw.GetCommand();

            scmd.CommandType = CommandType.Text;

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[catMotivosInfraccion] ON";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);

                return r;
            }

            scmd.CommandText = sql;

            os.ForEach(cmi => {
                scmd.Parameters.Add("@idCatMotivoInfraccion", SqlDbType.Int).Value = cmi.IdCatMotivoInfraccion;
                scmd.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = cmi.Nombre;
                scmd.Parameters.Add("@IdSubConcepto", SqlDbType.Int).Value = cmi.IdSubConcepto;
                scmd.Parameters.AddWithValue("@fechaActualizacion", cmi.FechaActualizacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@actualizadoPor", cmi.ActualizadoPor).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatus", cmi.Estatus).Value ??= DBNull.Value;
                scmd.Parameters.Add("@IdConcepto", SqlDbType.Int).Value = cmi.IdConcepto;
                scmd.Parameters.Add("@calificacionMinima", SqlDbType.Int).Value = cmi.CalificacionMinima;
                scmd.Parameters.Add("@calificacionMaxima", SqlDbType.Int).Value = cmi.CalificacionMaxima;
                scmd.Parameters.Add("@fundamento", SqlDbType.VarChar, 100).Value = cmi.Fundamento;

                try {
                    r += scmd.ExecuteNonQuery();
                } catch(SqlException se) {
                    log.Error(se);
                    log.Info(cmi);
                }

                scmd.Parameters.Clear();
            });

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[catMotivosInfraccion] OFF";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);
            }

            return r;
        }
    }
}