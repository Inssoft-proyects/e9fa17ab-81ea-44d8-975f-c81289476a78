using log4net;
using MxGobGuanajuato.Base;
using MxGobGuanajuato.Cnfs;
using MxGobGuanajuato.Dtos;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace MxGobGuanajuato.Daos
{
    public sealed class AccidenteCausasReaderDAO : IReaderData<AccidenteCausas>
    {
        public AccidenteCausasReaderDAO(DBReaderConfigurer dbr)
        {
            this.dbr = dbr;
        }
        
        private static readonly ILog log = LogManager.GetLogger(typeof(AccidenteCausasReaderDAO));

        private readonly DBReaderConfigurer dbr;

        public List<AccidenteCausas>? Get(IDictionary<string, object> p)
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

            List<AccidenteCausas>? accs = null;

            AccidenteCausas? acc = null;

            while(odr.Read()) {
                try {
                    if(odr.GetOracleDecimal(odr.GetOrdinal("idAccidenteCausa")).IsNull)
                    {
                        log.Error("No se recupero el campo idAccidenteCausa.");

                        break;
                    }

                    acc = new() {
                        IdAccidenteCausa = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idAccidenteCausa")), 22).Value
                    };

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idAccidente")).IsNull)
                        acc.IdAccidente = null;
                    else
                        acc.IdAccidente = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idAccidente")), 22).Value;

                    if(odr.GetOracleDecimal(odr.GetOrdinal("idCausaAccidente")).IsNull)
                        acc.IdCausaAccidente = null;
                    else
                        acc.IdCausaAccidente = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("idCausaAccidente")), 22).Value;
                    
                    if(odr.GetOracleDecimal(odr.GetOrdinal("indice")).IsNull)
                        acc.Indice = null;
                    else
                        acc.Indice = (int)OracleDecimal.SetPrecision(odr.GetOracleDecimal(odr.GetOrdinal("indice")), 10).Value;
                    
                    accs ??= new();

                    accs.Add(acc);
                } catch(IndexOutOfRangeException iore) {
                    log.Error(iore);

                    break;
                }
            }

            odr.Dispose();

            odr.Close();

            return accs;
        }
    }
}