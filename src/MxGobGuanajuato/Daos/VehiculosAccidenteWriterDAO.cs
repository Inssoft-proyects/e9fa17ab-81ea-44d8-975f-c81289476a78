
using System.Data;
using System.Text;
using log4net;
using Microsoft.Data.SqlClient;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;

namespace MxGobGuanajuato.Daos
{
    public sealed class VehiculosAccidenteWriterDAO : IWriterData<VehiculosAccidente>
    {
        public VehiculosAccidenteWriterDAO(DBWriterConfigurer dbw)
        {
            this.dbw = dbw;

            StringBuilder sql = new();

            sql.Append("INSERT INTO [dbo].[vehiculosAccidente]([idVehiculoAccidente], [idVehiculo], [idAccidente], [idPersona],\n");
            sql.Append("									   [montoVehiculo], [placa], [serie], [estatus])\n");
            sql.Append("	   VALUES(@idVehiculoAccidente, @idVehiculo, @idAccidente, @idPersona,\n");
            sql.Append("			  @montoVehiculo, @placa, @serie, @estatus)");

            this.sql = sql.ToString();

            sql.Clear();
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(VehiculosAccidenteWriterDAO));

        private readonly DBWriterConfigurer dbw;

        private readonly String sql;

        public int Set(List<VehiculosAccidente> os)
        {
            int r = 0;

            using SqlCommand scmd = dbw.GetCommand();

            scmd.CommandType = CommandType.Text;

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[vehiculosAccidente] ON";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);

                return r;
            }

            scmd.CommandText = sql;

            os.ForEach(vacc => {
                scmd.Parameters.Add("@idVehiculoAccidente", SqlDbType.Int).Value = vacc.IdVehiculoAccidente;
                scmd.Parameters.Add("@idVehiculo", SqlDbType.Int).Value = vacc.IdVehiculo;
                scmd.Parameters.Add("@idAccidente", SqlDbType.Int).Value = vacc.IdAccidente;

                scmd.Parameters.AddWithValue("@idPersona", vacc.IdPersona).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@montoVehiculo", vacc.MontoVehiculo).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@placa", vacc.Placa).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@serie", vacc.Serie).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatus", vacc.Estatus).Value ??= DBNull.Value;

                try {
                    r += scmd.ExecuteNonQuery();
                } catch(SqlException se) {
                    log.Error(se);
                    log.Info(vacc);
                }

                scmd.Parameters.Clear();
            });

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[vehiculosAccidente] OFF";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);
            }

            return r;
        }
    }
}