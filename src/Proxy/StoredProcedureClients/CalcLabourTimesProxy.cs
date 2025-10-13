namespace Linn.Stores2.Proxy.StoredProcedureClients
{
    using System.Data;
    using System.Threading.Tasks;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Stores2.Domain.LinnApps.External;
    using Oracle.ManagedDataAccess.Client;

    public class CalcLabourTimesProxy : ICalcLabourHoursProxy
    {
        public async Task CalcLabourTimes(bool newOnly = false)
        {
            using var connection = new OracleConnection(ConnectionStrings.ManagedConnectionString());

            var cmd = new OracleCommand("CALC_LABOUR_TIMES", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            var arg1 = new OracleParameter("p_new_only", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Input,
                Value = newOnly ? "Y" : "N"
            };
            cmd.Parameters.Add(arg1);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
        }
    }
}
