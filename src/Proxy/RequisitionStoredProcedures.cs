﻿namespace Linn.Stores2.Proxy
{
    using System.Data;
    using System.Threading.Tasks;

    using Linn.Common.Domain;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Stores2.Domain.LinnApps.External;

    using Oracle.ManagedDataAccess.Client;

    public class RequisitionStoredProcedures : IRequisitionStoredProcedures
    {
        public async Task<ProcessResult> UnallocateRequisition(int reqNumber, int? lineNumber, int cancelledBy)
        {
            using var connection = new OracleConnection(
                ConnectionStrings.ManagedConnectionString());

            var cmd = new OracleCommand("STORES_OO.UNALLOC_REQ_WRAPPER", connection)
                          {
                              CommandType = CommandType.StoredProcedure
                          };

            var reqNumberParameter = new OracleParameter("p_req_number", OracleDbType.Int32)
                                             {
                                                 Direction = ParameterDirection.Input,
                                                 Value = reqNumber
                                             };
            cmd.Parameters.Add(reqNumberParameter);

            var lineNumberParameter = new OracleParameter("p_line_number", OracleDbType.Int32)
                                         {
                                             Direction = ParameterDirection.Input,
                                             Value = lineNumber
                                         };
            cmd.Parameters.Add(lineNumberParameter);

            var cancelledByParameter = new OracleParameter("p_unalloc_by", OracleDbType.Int32)
                                          {
                                              Direction = ParameterDirection.Input,
                                              Value = reqNumber
                                          };
            cmd.Parameters.Add(cancelledByParameter);

            var qtyToAllocateParameter = new OracleParameter("p_qty_to_allocate", OracleDbType.Int32)
                                           {
                                               Direction = ParameterDirection.Input,
                                               Value = null
                                           };
            cmd.Parameters.Add(qtyToAllocateParameter);

            var qtyAllocatedParameter = new OracleParameter("p_qty_allocated", OracleDbType.Int32)
                                             {
                                                 Direction = ParameterDirection.Input,
                                                 Value = null
                                             };
            cmd.Parameters.Add(qtyAllocatedParameter);

            var commitParameter = new OracleParameter("p_commit", OracleDbType.Boolean)
                                            {
                                                Direction = ParameterDirection.Input,
                                                Value = true
                                            };
            cmd.Parameters.Add(commitParameter);
            
            var successParameter = new OracleParameter("p_success", OracleDbType.Int32)
                             {
                                 Direction = ParameterDirection.InputOutput
                             };
            cmd.Parameters.Add(successParameter);

            var messageParameter = new OracleParameter("p_message", OracleDbType.Varchar2)
                                      {
                                          Direction = ParameterDirection.InputOutput,
                                          Size = 500
                                      };
            cmd.Parameters.Add(messageParameter);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return new ProcessResult(
                (int)successParameter.Value == 1,
                messageParameter.Value.ToString());
        }
    }
}