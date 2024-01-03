
using System.Data;
using System.Text;
using log4net;
using Microsoft.Data.SqlClient;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;

namespace MxGobGuanajuato.Daos
{
    public sealed class VehiculosWriterDAO : IWriterData<Vehiculos>
    {
        public VehiculosWriterDAO(DBWriterConfigurer dbw)
        {
            this.dbw = dbw;

            StringBuilder sql = new();

            sql.Append("INSERT INTO [dbo].[vehiculos]([idVehiculo], [placas], [serie], [tarjeta],\n");
            sql.Append("							  [vigenciaTarjeta], [idMarcaVehiculo], [idSubmarca], [idTipoVehiculo],\n");
            sql.Append("							  [modelo], [idColor], [idEntidad], [idCatTipoServicio],\n");
            sql.Append("							  [idSubtipoServicio], [propietario], [numeroEconomico], [paisManufactura],\n");
            sql.Append("							  [idPersona], [fechaActualizacion], [actualizadoPor], [estatus],\n");
            sql.Append("							  [motor], [capacidad], [poliza], [carga],\n");
            sql.Append("							  [otros])\n");
            sql.Append("	   VALUES(@idVehiculo, @placas, @serie, @tarjeta,\n");
            sql.Append("			  @vigenciaTarjeta, @idMarcaVehiculo, @idSubmarca, @idTipoVehiculo,\n");
            sql.Append("			  @modelo, @idColor, @idEntidad, @idCatTipoServicio,\n");
            sql.Append("			  @idSubtipoServicio, @propietario, @numeroEconomico, @paisManufactura,\n");
            sql.Append("			  @idPersona, @fechaActualizacion, @actualizadoPor, @estatus,\n");
            sql.Append("			  @motor, @capacidad, @poliza, @carga,\n");
            sql.Append("			  @otros)");

            this.sql = sql.ToString();

            sql.Clear();
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(VehiculosWriterDAO));

        private readonly DBWriterConfigurer dbw;

        private readonly String sql;

        public int Set(List<Vehiculos> os)
        {
            int r = 0;

            using SqlCommand scmd = dbw.GetCommand();

            scmd.CommandType = CommandType.Text;

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[vehiculos] ON";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);

                return r;
            }

            scmd.CommandText = sql;

            os.ForEach(v => {
                scmd.Parameters.Add("@idVehiculo", SqlDbType.Int).Value = v.IdVehiculo;
                scmd.Parameters.AddWithValue("@placas", v.Placas).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@serie", v.Serie).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@tarjeta", v.Tarjeta).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@vigenciaTarjeta", v.VigenciaTarjeta).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idMarcaVehiculo", v.IdMarcaVehiculo).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idSubmarca", v.IdSubmarca).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idTipoVehiculo", v.IdTipoVehiculo).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@modelo", v.Modelo).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatus", v.Estatus).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idColor", v.IdColor).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idEntidad", v.IdEntidad).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idCatTipoServicio", v.IdCatTipoServicio).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idSubtipoServicio", v.IdSubtipoServicio).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@propietario", v.Propietario).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@numeroEconomico", v.NumeroEconomico).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@paisManufactura", v.PaisManufactura).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idPersona", v.IdPersona).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@fechaActualizacion", v.FechaActualizacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@actualizadoPor", v.ActualizadoPor).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatus", v.Estatus).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@motor", v.Motor).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@capacidad", v.Capacidad).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@poliza", v.Poliza).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@carga", v.Carga).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@otros", v.Otros).Value ??= DBNull.Value;

                try {
                    r += scmd.ExecuteNonQuery();
                } catch(SqlException se) {
                    log.Error(se);
                    log.Info(v);
                }

                scmd.Parameters.Clear();
            });

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[vehiculos] OFF";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);
            }

            return r;
        }
    }
}