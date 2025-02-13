namespace Linn.Stores2.Proxy
{
    using System;
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

            var cmd = new OracleCommand("STORES_WRAPPER.UNALLOC_REQ_WRAPPER", connection)
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

            var commitParameter = new OracleParameter("p_commit", OracleDbType.Boolean) // todo - won't work? amend wrapper?
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

        public async Task<ProcessResult> DeleteAllocOntos(
            int reqNumber, int? lineNumber, int? docNumber, string docType)
        {
            using var connection = new OracleConnection(
                ConnectionStrings.ManagedConnectionString());

            var cmd = new OracleCommand("STORES_WRAPPER.DELETE_ALLOCS_ONTO_WRAPPER", connection)
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

            var docNumberParameter = new OracleParameter("p_doc_number", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = docNumber
            };
            cmd.Parameters.Add(docNumberParameter);

            var docTypeParameter = new OracleParameter("p_doc_type", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Input,
                Value = docType,
                Size = 50
            };
            cmd.Parameters.Add(docTypeParameter);

            var successParameter = new OracleParameter("p_success", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.InputOutput,
                Size = 10
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
                successParameter.Value.ToString() == "TRUE",
                messageParameter.Value.ToString());
        }

        public async Task<ProcessResult> DoRequisition(int reqNumber, int? lineNumber, int bookedBy)
        {
            using var connection = new OracleConnection(
                ConnectionStrings.ManagedConnectionString());

            var cmd = new OracleCommand("STORES_WRAPPER.DO_REQUISITION_WRAPPER", connection)
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

            var bookedByParameter = new OracleParameter("p_booked_by", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = bookedBy
            };
            cmd.Parameters.Add(bookedByParameter);

            var successParameter = new OracleParameter("p_success", OracleDbType.Int32)
            {
                Direction = ParameterDirection.InputOutput,
                Value = 1
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
                successParameter.Value.ToString() == "1",
                messageParameter.Value.ToString());
        }

        public async Task<ProcessResult> CreateNominals(
            int reqNumber,
            decimal qty,
            int lineNumber,
            string nominal,
            string department,
            bool preCommit = true)
        {
            using var connection = new OracleConnection(
               ConnectionStrings.ManagedConnectionString());

            var cmd = new OracleCommand("stores_wrapper.create_nominals", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            var reqNumberParameter = new OracleParameter("p_req_number", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = reqNumber
            };
            cmd.Parameters.Add(reqNumberParameter);
            var qtyParameter = new OracleParameter("p_qty", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = qty
            };
            cmd.Parameters.Add(qtyParameter);
            var lineNumberParameter = new OracleParameter("p_line_number", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = lineNumber
            };
            cmd.Parameters.Add(lineNumberParameter);

            var nominalParameter = new OracleParameter("p_nominal", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Input,
                Size = 500,
                Value = nominal
            };
            cmd.Parameters.Add(nominalParameter);
            var deptParameter = new OracleParameter("p_department", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Input,
                Size = 500,
                Value = department
            };
            cmd.Parameters.Add(deptParameter);
            var successParameter = new OracleParameter("p_success", OracleDbType.Int32)
            {
                Direction = ParameterDirection.InputOutput,
                Value = 1
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
                successParameter.Value.ToString() == "1",
                messageParameter.Value.ToString());
        }

        public async Task<ProcessResult> PickStock(
            string partNumber,
            int reqNumber,
            int lineNumber,
            decimal lineQty,
            int? locationId,
            int? palletNumber,
            string stockPoolCode,
            string transactionCode)
        {
            using var connection = new OracleConnection(
                ConnectionStrings.ManagedConnectionString());

            var cmd = new OracleCommand("stores_wrapper.pick_stock", connection)
                          {
                              CommandType = CommandType.StoredProcedure
                          };

            var partNumberParameter = new OracleParameter("p_part_number", OracleDbType.Varchar2)
                                       {
                                           Direction = ParameterDirection.InputOutput,
                                           Size = 500,
                                           Value = partNumber
                                       };
            cmd.Parameters.Add(partNumberParameter);

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

            var lineQtyParameter = new OracleParameter("p_line_qty", OracleDbType.Decimal)
                                          {
                                              Direction = ParameterDirection.Input,
                                              Value = lineQty
                                          };
            cmd.Parameters.Add(lineQtyParameter);

            var locIdParameter = new OracleParameter("p_location_id", OracleDbType.Int32)
                                          {
                                              Direction = ParameterDirection.Input,
                                              Value = locationId
                                          };
            cmd.Parameters.Add(locIdParameter);

            var palletNumberParameter = new OracleParameter("p_pallet_number", OracleDbType.Int32)
                                     {
                                         Direction = ParameterDirection.Input,
                                         Value = palletNumber
                                     };
            cmd.Parameters.Add(palletNumberParameter);

            var stockPoolParameter = new OracleParameter("p_stock_pool", OracleDbType.Varchar2)
                                          {
                                              Direction = ParameterDirection.InputOutput,
                                              Size = 500,
                                              Value = stockPoolCode
                                          };
            cmd.Parameters.Add(stockPoolParameter);

            var transCodeParameter = new OracleParameter("p_transaction_code", OracleDbType.Varchar2)
                                         {
                                             Direction = ParameterDirection.InputOutput,
                                             Size = 500,
                                             Value = transactionCode
                                         };
            cmd.Parameters.Add(transCodeParameter);

            var successParameter = new OracleParameter("p_success", OracleDbType.Int32)
                                       {
                                           Direction = ParameterDirection.Output
                                       };
            cmd.Parameters.Add(successParameter);

            var qtyPickedParameter = new OracleParameter("p_qty_picked", OracleDbType.Decimal)
                                       {
                                           Direction = ParameterDirection.Output
                                       };

            cmd.Parameters.Add(qtyPickedParameter);

            var messageParameter = new OracleParameter("p_message", OracleDbType.Varchar2)
                                       {
                                           Direction = ParameterDirection.InputOutput,
                                           Size = 500
                                       };
            cmd.Parameters.Add(messageParameter);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();
            var isSuccess = successParameter.Value.ToString() == "1";

            return new ProcessResult(
                isSuccess,
                isSuccess ? $"Picked {qtyPickedParameter.Value} successfully." : messageParameter.Value.ToString());
        }
    }
}
