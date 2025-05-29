namespace Linn.Stores2.Proxy.StoredProcedureClients
{
    using System.Data;
    using System.Threading.Tasks;
    using Linn.Common.Domain;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Stores2.Domain.LinnApps.External;
    using Oracle.ManagedDataAccess.Client;

    public class SerialNumberService : ISerialNumberService
    {
        public async Task<bool> GetSerialNumbersRequired(string partNumber)
        {
            using var connection = new OracleConnection(ConnectionStrings.ManagedConnectionString());
            {
                await connection.OpenAsync();
                var cmd = new OracleCommand("sernos_pack_v2.serial_nos_reqd_wrapper", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var resultParameter = new OracleParameter("v_result", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.ReturnValue,
                    Size = 1000
                };
                cmd.Parameters.Add(resultParameter);

                cmd.Parameters.Add(new OracleParameter("p_part_number", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = partNumber
                });

                await cmd.ExecuteNonQueryAsync();
                await connection.CloseAsync();

                if (resultParameter.Value.ToString() == "SUCCESS")
                {
                    return true;
                }
                return false;
            }
        }

        public async Task<ProcessResult> CheckSerialNumber(string transactionCode, string partNumber, int serialNumber)
        {
            using var connection = new OracleConnection(ConnectionStrings.ManagedConnectionString());
            {
                await connection.OpenAsync();
                var cmd = new OracleCommand("sernos_external.check_sernos_trans", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var resultParameter = new OracleParameter("v_result", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.ReturnValue,
                    Size = 1000
                };
                cmd.Parameters.Add(resultParameter);

                cmd.Parameters.Add(new OracleParameter("p_sernos_trans", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = transactionCode
                });

                cmd.Parameters.Add(new OracleParameter("p_part_number", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = partNumber
                });

                cmd.Parameters.Add(new OracleParameter("p_serial_number", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = serialNumber
                });

                await cmd.ExecuteNonQueryAsync();
                await connection.CloseAsync();

                if (resultParameter.Value.ToString().ToUpper() == "OK")
                {
                    return new ProcessResult(true, "Sernos check successful");
                }
                return new ProcessResult(false, resultParameter.Value.ToString());
            }
        }
    }
}
