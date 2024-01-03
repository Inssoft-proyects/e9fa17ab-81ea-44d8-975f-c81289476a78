
using Oracle.ManagedDataAccess.Client;

namespace MxGobGuanajuato.Cnfs
{
    public sealed class DBReaderConfigurer {
        
        public DBReaderConfigurer(string cnnStr)
        {
            oc = new OracleConnection(cnnStr)
            {
                ConnectionString = cnnStr
            };
        }
        
        private readonly OracleConnection oc;

        public void Init()
        {
            oc.Open();

            return;
        }

        public OracleCommand GetCommand()
        {
            return oc.CreateCommand();
        }

        public void Close()
        {
            oc.Close();

            return;
        }
    }
}