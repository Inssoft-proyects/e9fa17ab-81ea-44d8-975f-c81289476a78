
using System.Data;
using System.Data.SqlTypes;
using System.Text;
using Common.Logging;
using Microsoft.Data.SqlClient;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;

namespace MxGobGuanajuato.Daos
{
    public sealed class AccidenteCausasWriterDAO : IWriterData<AccidenteCausas>
    {
        public AccidenteCausasWriterDAO(DBWriterConfigurer dbw)
        {
            this.dbw = dbw;

            StringBuilder sql = new();

            sql.Append("INSERT INTO [dbo].[accidenteCausas]([idAccidenteCausa], [idAccidente], [idCausaAccidente], [indice])\n");
            sql.Append("	   VALUES(@idAccidenteCausa, @idAccidente, @idCausaAccidente, @indice)");

            this.sql = sql.ToString();

            sql.Clear();
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(AccidenteCausasWriterDAO));

        private readonly DBWriterConfigurer dbw;

        private readonly String sql;

        public int Set(List<AccidenteCausas> os)
        {
            int r = 0;

            using SqlCommand scmd = dbw.GetCommand();

            scmd.CommandType = CommandType.Text;

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[accidenteCausas] ON";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);

                return r;
            }

            scmd.CommandText = sql;

            os.ForEach(acc => {
                try {
                    scmd.Parameters.Add("@idAccidenteCausa", SqlDbType.Int).Value = acc.IdAccidenteCausa;
                    scmd.Parameters.AddWithValue("@idAccidente", acc.IdAccidente).Value ??= DBNull.Value;
                    scmd.Parameters.AddWithValue("@idCausaAccidente", acc.IdCausaAccidente).Value ??= DBNull.Value;
                    scmd.Parameters.AddWithValue("@indice", acc.Indice).Value ??= DBNull.Value;

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

            scmd.CommandText = "SET IDENTITY_INSERT [dbo].[accidenteCausas] OFF";

            try {
                scmd.ExecuteNonQuery();
            } catch(SqlException se) {
                log.Error(se);
            }

            return r;
        }
    }
}