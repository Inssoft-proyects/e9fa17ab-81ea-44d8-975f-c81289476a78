
using System.Data;
using System.Data.SqlTypes;
using System.Text;
using log4net;
using Microsoft.Data.SqlClient;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;

namespace MxGobGuanajuato.Daos
{
    public sealed class InfraccionesWriterDAO : IWriterData<Infracciones>
    {
        public InfraccionesWriterDAO(DBWriterConfigurer dbw)
        {
            this.dbw = dbw;

            StringBuilder sql = new();

            sql.Append("INSERT INTO [dbo].[infracciones]([idInfraccion], [idOficial], [idDependencia], [idDelegacion],\n");
            sql.Append("								 [idVehiculo], [idAplicacion], [idGarantia], [idEstatusInfraccion],\n");
            sql.Append("								 [idMunicipio], [idTramo], [idCarretera], [idPersona],\n");
            sql.Append("							     [idPersonaInfraccion], [placasVehiculo], [folioInfraccion], [fechaInfraccion],\n");
            sql.Append("								 [kmCarretera], [observaciones], [lugarCalle], [lugarNumero],\n");
            sql.Append("								 [lugarColonia], [lugarEntreCalle], [infraccionCortesia], [NumTarjetaCirculacion],\n");
            sql.Append("								 [oficioRevocacion], [estatusProceso], [fechaActualizacion], [actualizadoPor],\n");
            sql.Append("								 [estatus], [reciboPago], [monto], [montoPagado],\n");
            sql.Append("								 [fechaPago], [lugarPago], [idEstatusEnvio], [oficioEnvio],\n");
            sql.Append("								 [fechaEnvio], [idOficinaRenta], [inventario], [partner],\n");
            sql.Append("								 [cuenta], [objeto], [documento], [transito])\n");
            sql.Append("	   VALUES(@idInfraccion, @idOficial, @idDependencia, @idDelegacion,\n");
            sql.Append("	   		  @idVehiculo, @idAplicacion, @idGarantia, @idEstatusInfraccion,\n");
            sql.Append("			  @idMunicipio, @idTramo, @idCarretera, @idPersona,\n");
            sql.Append("			  @idPersonaInfraccion, @placasVehiculo, @folioInfraccion, @fechaInfraccion,\n");
            sql.Append("			  @kmCarretera, @observaciones, @lugarCalle, @lugarNumero,\n");
            sql.Append("			  @lugarColonia, @lugarEntreCalle, @infraccionCortesia, @NumTarjetaCirculacion,\n");
            sql.Append("			  @oficioRevocacion, @estatusProceso, @fechaActualizacion, @actualizadoPor,\n");
            sql.Append("			  @estatus, @reciboPago, @monto, @montoPagado,\n");
            sql.Append("			  @fechaPago, @lugarPago, @idEstatusEnvio, @oficioEnvio,\n");
            sql.Append("			  @fechaEnvio, @idOficinaRenta, @inventario, @partner,\n");
            sql.Append("			  @cuenta, @objeto, @documento, @transito)");

            this.sql = sql.ToString();

            sql.Clear();
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(InfraccionesWriterDAO));

        private readonly DBWriterConfigurer dbw;

        private readonly String sql;

        public int Set(List<Infracciones> os)
        {
            int r = 0;

            using SqlCommand scmd = dbw.GetCommand();

            scmd.CommandType = CommandType.Text;

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[Infracciones] ON";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);

                return r;
            }

            scmd.CommandText = sql;

            os.ForEach(cmi => {
                scmd.Parameters.Add("@idInfraccion", SqlDbType.Int).Value = cmi.IdInfraccion;
                scmd.Parameters.AddWithValue("@idOficial", cmi.IdOficial).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idDependencia", cmi.IdDependencia).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idDelegacion", cmi.IdDelegacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idVehiculo", cmi.IdVehiculo).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idAplicacion", cmi.IdAplicacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idGarantia", cmi.IdGarantia).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idEstatusInfraccion", cmi.IdEstatusInfraccion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idMunicipio", cmi.IdMunicipio).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idTramo", cmi.IdTramo).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idCarretera", cmi.IdCarretera).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idPersona", cmi.IdPersona).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idPersonaInfraccion", cmi.IdPersonaInfraccion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@placasVehiculo", cmi.PlacasVehiculo).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@folioInfraccion", cmi.FolioInfraccion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@fechaInfraccion", cmi.FechaInfraccion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@kmCarretera", cmi.KmCarretera).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@observaciones", cmi.Observaciones).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@lugarCalle", cmi.LugarCalle).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@lugarNumero", cmi.LugarNumero).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@lugarColonia", cmi.LugarColonia).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@lugarEntreCalle", cmi.LugarEntreCalle).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@infraccionCortesia", cmi.InfraccionCortesia).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@NumTarjetaCirculacion", cmi.NumTarjetaCirculacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@oficioRevocacion", cmi.OficioRevocacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatusProceso", cmi.EstatusProceso).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@fechaActualizacion", cmi.FechaActualizacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@actualizadoPor", cmi.ActualizadoPor).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatus", cmi.Estatus).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@reciboPago", cmi.ReciboPago).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@monto", cmi.Monto).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@montoPagado", cmi.MontoPagado).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@fechaPago", cmi.FechaPago).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@lugarPago", cmi.LugarPago).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idEstatusEnvio", cmi.IdEstatusEnvio).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@oficioEnvio", cmi.OficioEnvio).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@fechaEnvio", cmi.FechaEnvio).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idOficinaRenta", cmi.IdOficinaRenta).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@inventario", cmi.Inventario ?? SqlBinary.Null);
                scmd.Parameters.AddWithValue("@partner", cmi.Partner).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@cuenta", cmi.Cuenta).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@objeto", cmi.Objeto).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@documento", cmi.Documento).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@transito", cmi.Transito).Value ??= DBNull.Value;

                try {
                    r += scmd.ExecuteNonQuery();
                } catch(SqlException se) {
                    log.Error(se);
                    log.Info(cmi);
                }  catch(SqlTypeException ste) {
                    log.Error(ste);
                    log.Info(cmi);
                }

                scmd.Parameters.Clear();
            });

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[Infracciones] OFF";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);
            }

            return r;
        }
    }
}