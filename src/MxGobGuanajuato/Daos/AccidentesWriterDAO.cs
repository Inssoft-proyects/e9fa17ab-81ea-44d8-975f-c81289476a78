
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
    public sealed class AccidentesWriterDAO : IWriterData<Accidentes>
    {
        public AccidentesWriterDAO(DBWriterConfigurer dbw)
        {
            this.dbw = dbw;

            StringBuilder sql = new();

            sql.Append("INSERT INTO [dbo].[accidentes]([idAccidente], [fecha], [hora], [idMunicipio],\n");
            sql.Append("							   [idCarretera], [idTramo], [kilometro], [estatusReporte],\n");
            sql.Append("							   [idClasificacionAccidente], [idCausaAccidente], [descripcionCausas], [idFactorAccidente],\n");
            sql.Append("							   [idFactorOpcionAccidente], [fechaActualizacion], [actualizadoPor], [estatus],\n");
            sql.Append("							   [montoCamino], [montoCarga], [montoPropietarios], [montoOtros],\n");
            sql.Append("							   [montoVehiculo], [idElabora], [idSupervisa], [idAutoriza],\n");
            sql.Append("							   [idElaboraConsignacion], [latitud], [longitud], [idCiudad],\n");
            sql.Append("							   [idCertificado], [entregaOtros], [entregaObjetos], [consignacionHechos],\n");
            sql.Append("							   [numeroOficio], [idAgenciaMinisterio], [recibeMinisterio], [idAutoridadEntrega],\n");
            sql.Append("							   [idAutoridadDisposicion], [armas], [drogas], [valores],\n");
            sql.Append("							   [prendas], [otros], [idEstatusReporte],\n");
            sql.Append("							   [idOficinaDelegacion], [armasTexto], [drogasTexto], [valoresTexto],\n");
            sql.Append("							   [prendasTexto], [otrosTexto], [convenio], [observacionesConvenio],\n");
            sql.Append("							   [idEntidadCompetencia])\n");
            sql.Append("	   VALUES(@idAccidente, @fecha, @hora, @idMunicipio,\n");
            sql.Append("			  @idCarretera, @idTramo, @kilometro, @estatusReporte,\n");
            sql.Append("			  @idClasificacionAccidente, @idCausaAccidente, @descripcionCausas, @idFactorAccidente,\n");
            sql.Append("			  @idFactorOpcionAccidente, @fechaActualizacion, @actualizadoPor, @estatus,\n");
            sql.Append("			  @montoCamino, @montoCarga, @montoPropietarios, @montoOtros,\n");
            sql.Append("			  @montoVehiculo, @idElabora, @idSupervisa, @idAutoriza,\n");
            sql.Append("			  @idElaboraConsignacion, @latitud, @longitud, @idCiudad,\n");
            sql.Append("			  @idCertificado, @entregaOtros, @entregaObjetos, @consignacionHechos,\n");
            sql.Append("			  @numeroOficio, @idAgenciaMinisterio, @recibeMinisterio, @idAutoridadEntrega,\n");
            sql.Append("			  @idAutoridadDisposicion, @armas, @drogas, @valores,\n");
            sql.Append("			  @prendas, @otros, @idEstatusReporte,\n");
            sql.Append("			  @idOficinaDelegacion, @armasTexto, @drogasTexto, @valoresTexto,\n");
            sql.Append("			  @prendasTexto, @otrosTexto, @convenio, @observacionesConvenio,\n");
            sql.Append("			  @idEntidadCompetencia)");

            this.sql = sql.ToString();

            sql.Clear();
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(AccidentesWriterDAO));

        private readonly DBWriterConfigurer dbw;

        private readonly String sql;

        public int Set(List<Accidentes> os)
        {
            int r = 0;

            using SqlCommand scmd = dbw.GetCommand();

            scmd.CommandType = CommandType.Text;

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[accidentes] ON";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);

                return r;
            }

            scmd.CommandText = sql;

            os.ForEach(acc => {
                scmd.Parameters.Add("@idAccidente", SqlDbType.Int).Value = acc.IdAccidente;
                scmd.Parameters.AddWithValue("@fecha", acc.Fecha).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@hora", acc.Hora).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idMunicipio", acc.IdMunicipio).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idCarretera", acc.IdCarretera).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idTramo", acc.IdTramo).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@kilometro", acc.Kilometro).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatusReporte", acc.EstatusReporte).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idClasificacionAccidente", acc.IdClasificacionAccidente).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idCausaAccidente", acc.IdCausaAccidente).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@descripcionCausas", acc.DescripcionCausas).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idFactorAccidente", acc.IdFactorAccidente).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idFactorOpcionAccidente", acc.IdFactorOpcionAccidente).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@fechaActualizacion", acc.FechaActualizacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@actualizadoPor", acc.ActualizadoPor).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatus", acc.Estatus).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@montoCamino", acc.MontoCamino).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@montoCarga", acc.MontoCarga).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@montoPropietarios", acc.MontoPropietarios).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@montoOtros", acc.MontoOtros).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@montoVehiculo", acc.MontoVehiculo).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idElabora", acc.IdElabora).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idSupervisa", acc.IdSupervisa).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idAutoriza", acc.IdAutoriza).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idElaboraConsignacion", acc.IdElaboraConsignacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@latitud", acc.Latitud).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@longitud", acc.Longitud).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idCiudad", acc.IdCiudad).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idCertificado", acc.IdCertificado).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@entregaOtros", acc.EntregaOtros).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@entregaObjetos", acc.EntregaObjetos).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@consignacionHechos", acc.ConsignacionHechos).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@numeroOficio", acc.NumeroOficio).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idAgenciaMinisterio", acc.IdAgenciaMinisterio).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@recibeMinisterio", acc.RecibeMinisterio).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idAutoridadEntrega", acc.IdAutoridadEntrega).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idAutoridadDisposicion", acc.IdAutoridadDisposicion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@armas", acc.Armas).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@drogas", acc.Drogas).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@valores", acc.Valores).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@prendas", acc.Prendas).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@otros", acc.Otros).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idEstatusReporte", acc.IdEstatusReporte).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idOficinaDelegacion", acc.IdOficinaDelegacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@armasTexto", acc.ArmasTexto).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@drogasTexto", acc.DrogasTexto).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@valoresTexto", acc.ValoresTexto).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@prendasTexto", acc.PrendasTexto).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@otrosTexto", acc.OtrosTexto).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@convenio", acc.Convenio).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@observacionesConvenio", acc.ObservacionesConvenio).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idEntidadCompetencia", acc.IdEntidadCompetencia).Value ??= DBNull.Value;

                try {
                    r += scmd.ExecuteNonQuery();
                } catch(SqlException se) {
                    log.Error(se);
                    log.Info(acc);
                } catch(SqlTypeException ste) {
                    log.Error(ste);
                    log.Info(acc);
                }

                scmd.Parameters.Clear();
            });

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[accidentes] OFF";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);
            }

            return r;
        }
    }
}