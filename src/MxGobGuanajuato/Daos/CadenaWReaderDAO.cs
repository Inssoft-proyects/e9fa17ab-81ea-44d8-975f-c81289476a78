
using System.Data;
using log4net;
using Microsoft.Data.SqlClient;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;

namespace MxGobGuanajuato.Daos
{
    public sealed class CadenaWReaderDAO : IReaderData<String>
    {
        public CadenaWReaderDAO(DBWriterConfigurer dbw)
        {
            this.dbw = dbw;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(CadenaWReaderDAO));

        private readonly DBWriterConfigurer dbw;
        
        public List<string>? Get(IDictionary<string, object> p)
        {
            using SqlCommand scmd = dbw.GetCommand();

            scmd.CommandType = CommandType.Text;

            scmd.CommandText = (String)p["sql"];

            p.ToList().ForEach(e => {
                if(e.Key != "sql")
                    scmd.Parameters.AddWithValue(e.Key, e.Value).Value ??= DBNull.Value;
            });

            SqlDataReader? sdr = null;

            try {
                sdr = scmd.ExecuteReader();
            } catch(InvalidOperationException ioe) {
                log.Error(ioe);

                log.Info(p);

                return null;
            } catch(ArgumentException ae) {
                log.Error(ae);

                log.Info(p);

                return null;
            } catch(SqlException se) {
                log.Error(se);

                log.Info(p);

                return null;
            }

            List<string>? strs = null;

            while(sdr.Read()) {
                try{
                    if(!sdr.GetSqlString(sdr.GetOrdinal("json")).IsNull)
                    {
                        strs ??= new();
                        
                        strs.Add(sdr.GetSqlString(sdr.GetOrdinal("json")).Value);
                    }
                } catch(IndexOutOfRangeException ex) {
                    log.Error(ex);
                }
            }
            
            sdr.Dispose();

            sdr.Close();

            return strs;
        }
    }
}