
using System.Data;
using System.Text;
using log4net;
using Microsoft.Data.SqlClient;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;

namespace MxGobGuanajuato.Daos
{
    public sealed class CatConceptoInfraccionWriterDAO : IWriterData<CatConceptoInfraccion>
    {
        public CatConceptoInfraccionWriterDAO(DBWriterConfigurer dbw)
        {
            this.dbw = dbw;

            string query = string.Empty;
            query = @"  INSERT INTO [dbo].[catConceptoInfraccion]([idConcepto], 
                                                                  [concepto], 
                                                                  [fechaActualizacion],
                                                                  [actualizadoPor], 
                                                                  [estatus])
                        VALUES( @idConcepto, 
                                @concepto, 
                                @fechaActualizacion,
                                @actualizadoPor, 
                                @estatus)";
            this.sql = query;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(CatConceptoInfraccionWriterDAO));

        private readonly DBWriterConfigurer dbw;

        private readonly String sql;

        public int Set(List<CatConceptoInfraccion> os)
        {
            int r = 0;

            using SqlCommand scmd = dbw.GetCommand();

            scmd.CommandType = CommandType.Text;

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[catConceptoInfraccion] ON";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);

                return r;
            }

            scmd.CommandText = sql;

            os.ForEach(cmi => {
                scmd.Parameters.Add("@idConcepto", SqlDbType.Int).Value = cmi.IdConcepto;
                scmd.Parameters.Add("@concepto", SqlDbType.VarChar, 100).Value = cmi.Concepto;
                scmd.Parameters.AddWithValue("@fechaActualizacion", cmi.FechaActualizacion).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@actualizadoPor", cmi.ActualizadoPor).Value ??= DBNull.Value;
                scmd.Parameters.AddWithValue("@estatus", cmi.Estatus).Value ??= DBNull.Value;
                try {
                    r += scmd.ExecuteNonQuery();
                } catch(SqlException se) {
                    log.Error(se);
                    log.Info(cmi);
                }

                scmd.Parameters.Clear();
            });

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[catConceptoInfraccion] OFF";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);
            }

            return r;
        }
    }
}