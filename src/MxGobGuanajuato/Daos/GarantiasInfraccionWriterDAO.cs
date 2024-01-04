using System.Data;
using System.Text;
using log4net;
using Microsoft.Data.SqlClient;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;

namespace MxGobGuanajuato.Daos
{
    public sealed class GarantiasInfraccionWriterDAO : IWriterData<GarantiasInfraccion>
    {
        public GarantiasInfraccionWriterDAO(DBWriterConfigurer dbw)
        {
            this.dbw = dbw;

            StringBuilder sql = new();

            sql.Append("INSERT INTO [dbo].[garantiasInfraccion]([idGarantia], [idCatGarantia], [idTipoPlaca], [idTipoLicencia],\n");
            sql.Append("										[idInfraccion], [numPlaca], [numLicencia], [vehiculoDocumento],\n");
            sql.Append("										[fechaActualizacion], [actualizadoPor], [estatus])\n");
            sql.Append("	   VALUES(@idGarantia, @idCatGarantia, @idTipoPlaca, @idTipoLicencia,\n");
            sql.Append("			  @idInfraccion, @numPlaca, @numLicencia, @vehiculoDocumento,\n");
            sql.Append("			  @fechaActualizacion, @actualizadoPor, @estatus)");

            this.sql = sql.ToString();

            sql.Clear();
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(GarantiasInfraccionWriterDAO));

        private readonly DBWriterConfigurer dbw;

        private readonly String sql;

        public int Set(List<GarantiasInfraccion> os)
        {
            int r = 0;

            using SqlCommand scmd = dbw.GetCommand();

            scmd.CommandType = CommandType.Text;

            scmd.CommandText = sql;

            os.ForEach(gi => {
                scmd.Parameters.Add("@idGarantia", SqlDbType.Int).Value = gi.IdGarantia;
                scmd.Parameters.Add("@idCatGarantia", SqlDbType.Int).Value = gi.IdCatGarantia;
                scmd.Parameters.Add("@idTipoPlaca", SqlDbType.Int).Value = gi.IdTipoPlaca;
                scmd.Parameters.Add("@idTipoLicencia", SqlDbType.Int).Value = gi.IdTipoLicencia;
                scmd.Parameters.Add("@idInfraccion", SqlDbType.Int).Value = gi.IdInfraccion;
                scmd.Parameters.AddWithValue("@numPlaca", gi.NumPlaca).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@numLicencia", gi.NumLicencia).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@vehiculoDocumento", gi.VehiculoDocumento).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@fechaActualizacion", gi.FechaActualizacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@actualizadoPor", gi.ActualizadoPor).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatus", gi.Estatus).Value ??= DBNull.Value;

                try {
                    r += scmd.ExecuteNonQuery();
                } catch(SqlException se) {
                    log.Error(se);
                    log.Info(gi);
                }

                scmd.Parameters.Clear();
            });

            return r;
        }
    }
}