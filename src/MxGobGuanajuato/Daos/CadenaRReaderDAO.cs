
using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using Oracle.ManagedDataAccess.Client;

namespace MxGobGuanajuato.Daos
{
    public sealed class CadenaRReaderDAO : IReaderData<String>
    {
        public CadenaRReaderDAO(DBReaderConfigurer dbr)
        {
            this.dbr = dbr;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(CadenaRReaderDAO));

        private readonly DBReaderConfigurer dbr;
        
        public List<string>? Get(IDictionary<string, object> p)
        {
            using OracleCommand ocmd = dbr.GetCommand();

            ocmd.BindByName = true;

            ocmd.CommandText = (String)p["sql"];

            p.ToList().ForEach(e => {
                if(e.Key != "sql")
                    ocmd.Parameters.Add(new OracleParameter(e.Key, e.Value));
            });

            OracleDataReader? odr = null;

            try {
                odr = ocmd.ExecuteReader();
            } catch(InvalidOperationException ioe) {
                log.Error(ioe);

                log.Info(p);

                return null;
            } catch(ArgumentException ae) {
                log.Error(ae);

                log.Info(p);

                return null;
            } catch(OracleException oe) {
                log.Error(oe);

                log.Info(p);

                return null;
            }

            List<string>? strs = null;

            while(odr.Read()) {
                try{
                    if(!odr.GetOracleString(odr.GetOrdinal("json")).IsNull)
                    {
                        strs ??= new();
                        
                        strs.Add(odr.GetOracleString(odr.GetOrdinal("json")).Value);
                    }
                } catch(IndexOutOfRangeException ex) {
                    log.Error(ex);
                }
            }
            
            odr.Dispose();

            odr.Close();

            return strs;
        }
    }
}