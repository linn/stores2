﻿namespace Linn.Stores2.Proxy.StoredProcedureClients
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
                Value = cancelledBy
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

            var commitParameter = new OracleParameter("p_commit", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = 1
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
                successParameter.Value.ToString() == "1",
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
            using var connection = new OracleConnection(ConnectionStrings.ManagedConnectionString());

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
            string department)
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
            var qtyParameter = new OracleParameter("p_qty", OracleDbType.Decimal)
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
                Value = palletNumber != null ? null : locationId
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

        public async Task<ProcessResult> CreateRequisitionLines(int reqNumber, int? serialNumber)
        {
            await using var connection = new OracleConnection(ConnectionStrings.ManagedConnectionString());
            var cmd = new OracleCommand("STORES_WRAPPER.CREATE_REQ_LINES", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new OracleParameter("p_req_number", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = reqNumber
            });

            cmd.Parameters.Add(new OracleParameter("p_serial_number", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = serialNumber
            });

            var messageParameter = new OracleParameter("p_message", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Output,
                Size = 500
            };
            cmd.Parameters.Add(messageParameter);

            var successParameter = new OracleParameter("p_success", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(successParameter);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return new ProcessResult(successParameter.Value?.ToString() == "1", messageParameter.Value?.ToString());
        }

        public async Task<ProcessResult> InsertReqOntos(
            int reqNumber,
            decimal qty,
            int lineNumber,
            int? locationId,
            int? palletNumber,
            string stockPool,
            string state,
            string category,
            string insertOrUpdate = "I")
        {
            await using var connection = new OracleConnection(ConnectionStrings.ManagedConnectionString());

            var cmd = new OracleCommand("STORES_WRAPPER.insert_req_ontos", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new OracleParameter("p_req_number", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = reqNumber
            });

            cmd.Parameters.Add(new OracleParameter("p_qty", OracleDbType.Decimal)
            {
                Direction = ParameterDirection.Input,
                Value = qty
            });

            cmd.Parameters.Add(new OracleParameter("p_line_number", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = lineNumber
            });

            cmd.Parameters.Add(new OracleParameter("p_location_id", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = locationId
            });

            cmd.Parameters.Add(new OracleParameter("p_pallet_number", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = palletNumber
            });

            cmd.Parameters.Add(new OracleParameter("p_stock_pool", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Input,
                Value = stockPool
            });

            cmd.Parameters.Add(new OracleParameter("p_state", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Input,
                Value = state
            });

            cmd.Parameters.Add(new OracleParameter("p_category", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Input,
                Value = category
            });

            cmd.Parameters.Add(new OracleParameter("p_serial_number", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input
                // todo - don't need to pass this for my use case, but might later
            });

            cmd.Parameters.Add(new OracleParameter("p_ins_upd", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Input,
                Value = insertOrUpdate
            });

            var successParameter = new OracleParameter("p_success", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(successParameter);

            var messageParameter = new OracleParameter("p_message", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Output,
                Size = 500
            };
            cmd.Parameters.Add(messageParameter);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return new ProcessResult(successParameter.Value?.ToString() == "1", messageParameter.Value?.ToString());
        }

        public async Task<bool> CanPutPartOnPallet(string partNumber, int palletNumber)
        {
            await using var connection = new OracleConnection(ConnectionStrings.ManagedConnectionString());

            var cmd = new OracleCommand("pallet_analysis_pack.can_put_part_on_pallet_wrapper", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            var result = new OracleParameter(null, OracleDbType.Int32)
            {
                Direction = ParameterDirection.ReturnValue,
            };
            cmd.Parameters.Add(result);

            var arg1 = new OracleParameter("p_part_number", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Input,
                Size = 14,
                Value = partNumber
            };
            cmd.Parameters.Add(arg1);

            var arg2 = new OracleParameter("p_pallet_number", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Input,
                Size = 50,
                Value = palletNumber.ToString()
            };
            cmd.Parameters.Add(arg2);


            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return int.Parse(result.Value.ToString()) == 0;
        }

        public async Task<ProcessResult> CreateLoanReq(int loanNumber)
        {
            await using var connection = new OracleConnection(ConnectionStrings.ManagedConnectionString());

            var cmd = new OracleCommand("STORES_WRAPPER.CREATE_LOAN_REQ", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add(new OracleParameter("p_loan_number", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = loanNumber
            });

            var messageParameter = new OracleParameter("p_message", OracleDbType.Varchar2)
            {
                Direction = ParameterDirection.Output,
                Size = 500
            };
            cmd.Parameters.Add(messageParameter);

            var successParameter = new OracleParameter("p_success", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Output,
                Value = 0
            };
            cmd.Parameters.Add(successParameter);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return new ProcessResult(successParameter.Value?.ToString() == "1", messageParameter.Value?.ToString());
        }

        public async Task<decimal> GetQtyReturned(int returnOrderNumber, int lineNumber)
        {
            await using var connection = new OracleConnection(ConnectionStrings.ManagedConnectionString());

            var cmd = new OracleCommand("stores_oo.qty_returned", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            var result = new OracleParameter(null, OracleDbType.Decimal)
            {
                Direction = ParameterDirection.ReturnValue,
            };
            cmd.Parameters.Add(result);

            var arg1 = new OracleParameter("p_order_number", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = returnOrderNumber
            };
            cmd.Parameters.Add(arg1);

            var arg2 = new OracleParameter("p_order_line", OracleDbType.Int32)
            {
                Direction = ParameterDirection.Input,
                Value = lineNumber
            };
            cmd.Parameters.Add(arg2);


            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return decimal.Parse(result.Value.ToString());
        }

        public async Task<ProcessResult> UnPickStock(
            int reqNumber, 
            int lineNumber, 
            int seq, 
            int? orderNumber,
            int? orderLine,
            decimal qtyToUnPick,
            int? stockLocatorId,
            int unpickedBy,
            bool reallocate = false,
            bool updSodAllocQty = false)
        {
            using (var connection = new OracleConnection(ConnectionStrings.ManagedConnectionString()))
            {
                var cmd = new OracleCommand("stores_oo.unpick_stock_wrapper", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                var arg1 = new OracleParameter("p_req_number", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = reqNumber
                };
                cmd.Parameters.Add(arg1);

                var arg2 = new OracleParameter("p_line_number", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = lineNumber
                };
                cmd.Parameters.Add(arg2);

                var arg3 = new OracleParameter("p_seq", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = seq
                };
                cmd.Parameters.Add(arg3);

                var arg4 = new OracleParameter("p_order_number", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = orderNumber
                };
                cmd.Parameters.Add(arg4);

                var arg5 = new OracleParameter("p_order_line", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = orderLine
                };
                cmd.Parameters.Add(arg5);

                var arg6 = new OracleParameter("p_qty_to_unpick", OracleDbType.Decimal)
                {
                    Direction = ParameterDirection.Input,
                    Value = qtyToUnPick
                };
                cmd.Parameters.Add(arg6);

                var arg7 = new OracleParameter("p_stock_locator_id", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = stockLocatorId
                };
                cmd.Parameters.Add(arg7);

                var arg8 = new OracleParameter("p_upd_sod_alloc_qty", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = updSodAllocQty ? "Y" : "N",
                    Size = 1
                };
                cmd.Parameters.Add(arg8);

                var arg9 = new OracleParameter("p_amended_by", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.Input,
                    Value = unpickedBy
                };
                cmd.Parameters.Add(arg9);

                var successParameter = new OracleParameter("p_success", OracleDbType.Int32)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = 1
                };
                cmd.Parameters.Add(successParameter);

                var arg11 = new OracleParameter("p_realloc", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.Input,
                    Value = reallocate ? "Y" : "N",
                    Size = 1
                };
                cmd.Parameters.Add(arg11);

                var messageParam = new OracleParameter("p_message", OracleDbType.Varchar2)
                {
                    Direction = ParameterDirection.InputOutput,
                    Size = 100
                };
                cmd.Parameters.Add(messageParam);

                await connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await connection.CloseAsync();

                if (int.TryParse(successParameter.Value.ToString(), out var success))
                {
                    if (success == 1)
                    {
                        return new ProcessResult(true, null);
                    }

                    return new ProcessResult(
                        false,
                        $"Failed to unpick line: {reqNumber},{lineNumber},{seq}. {messageParam.Value}");
                }

                return new ProcessResult(false, "Failed in procedure call: stores_oo.unpick_stock");
            }
        }
    }
}
