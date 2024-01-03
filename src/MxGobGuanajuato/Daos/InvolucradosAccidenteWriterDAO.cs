using System.Data;
using System.Text;
using log4net;
using Microsoft.Data.SqlClient;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;

namespace MxGobGuanajuato.Daos
{
    public sealed class InvolucradosAccidenteWriterDAO : IWriterData<InvolucradosAccidente>
    {
        public InvolucradosAccidenteWriterDAO(DBWriterConfigurer dbw)
        {
            this.dbw = dbw;

            StringBuilder sql = new();

            sql.Append("INSERT INTO [dbo].[involucradosAccidente]([idAccidente], [idPersona], [idVehiculo], [idTipoInvolucrado],\n");
            sql.Append("										  [idEstadoVictima], [idHospital], [idInstitucionTraslado], [idAsiento],\n");
            sql.Append("										  [idCinturon], [fechaIngreso], [horaIngreso], [estatus])\n");
            sql.Append("	   VALUES(@idAccidente, @idPersona, @idVehiculo, @idTipoInvolucrado,\n");
            sql.Append("			  @idEstadoVictima, @idHospital, @idInstitucionTraslado, @idAsiento,\n");
            sql.Append("			  @idCinturon, @fechaIngreso, @horaIngreso, @estatus)");

            this.sql = sql.ToString();

            sql.Clear();
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(InvolucradosAccidenteWriterDAO));

        private readonly DBWriterConfigurer dbw;

        private readonly String sql;

        public int Set(List<InvolucradosAccidente> os)
        {
            int r = 0;

            using SqlCommand scmd = dbw.GetCommand();

            scmd.CommandType = CommandType.Text;

            scmd.CommandText = sql;

            os.ForEach(iacc => {
                scmd.Parameters.Add("@idAccidente", SqlDbType.Int).Value = iacc.IdAccidente;
                scmd.Parameters.Add("@idPersona", SqlDbType.Int).Value = iacc.IdPersona;
                scmd.Parameters.Add("@idVehiculo", SqlDbType.Int).Value = iacc.IdVehiculo;
                scmd.Parameters.AddWithValue("@idTipoInvolucrado", iacc.IdTipoInvolucrado).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idEstadoVictima", iacc.IdEstadoVictima).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idHospital", iacc.IdHospital).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idInstitucionTraslado", iacc.IdInstitucionTraslado).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idAsiento", iacc.IdAsiento).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@idCinturon", iacc.IdCinturon).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@fechaIngreso", iacc.FechaIngreso).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@horaIngreso", iacc.HoraIngreso).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatus", iacc.Estatus).Value ??= DBNull.Value;

                try {
                    r += scmd.ExecuteNonQuery();
                } catch(SqlException se) {
                    log.Error(se);
                    log.Info(iacc);
                }

                scmd.Parameters.Clear();
            });

            return r;
        }
    }
}