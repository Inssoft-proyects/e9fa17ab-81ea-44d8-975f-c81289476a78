
using Microsoft.Data.SqlClient;

namespace MxGobGuanajuato.Cnfs
{
    public sealed class DBWriterConfigurer {
        
        public DBWriterConfigurer(string cnnStr)
        {
            sc = new SqlConnection(cnnStr)
            {
                ConnectionString = cnnStr
            };
        }
        
        private readonly SqlConnection sc;

        public void Init()
        {
            sc.Open();

            return;
        }

        public SqlCommand GetCommand()
        {
            return sc.CreateCommand();
        }

        public void Close()
        {
            sc.Close();

            return;
        }
    }
}