namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Stores2.Domain.LinnApps;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;

    public class RequisitionHeader
    {
        public int ReqNumber { get; protected init; }

        public DateTime DateCreated { get; protected init; }
        
        public Employee CreatedBy { get; protected init; }

        public int? Document1 { get; protected set; }

        public ICollection<RequisitionLine> Lines { get; protected set; }
        
        public decimal? Quantity { get; protected set; }

        public string Document1Name { get; protected set; }
        
        public int? Document1Line { get; protected set; }

        public int? Document2 { get; protected set; }

        public string Document2Name { get; protected set; }

        public Part Part { get; protected set; }

        public StorageLocation ToLocation { get; protected set; }

        public StorageLocation FromLocation { get; protected set; }

        public int? FromPalletNumber { get; protected set; }

        public int? ToPalletNumber { get; protected set; }

        public string Cancelled { get; protected set; }

        public Employee CancelledBy { get; protected set; }

        public DateTime? DateCancelled { get; protected set; }

        public string CancelledReason { get; protected set; }
        
        public StoresFunction StoresFunction { get; protected init; }
        
        public string Comments { get; protected set; }
        
        public DateTime? DateBooked { get; protected set; }
        
        public Employee BookedBy { get; protected set; }

        public string Booked { get; protected set; }
        
        public string IsReversed { get; protected set; }
        
        public string IsReverseTransaction { get; set; }

        public ICollection<CancelDetails> CancelDetails { get; protected set; }

        public Department Department { get; protected init; }

        public Nominal Nominal { get; protected init; }

        public Employee AuthorisedBy { get; protected set; }

        public DateTime? DateAuthorised { get; protected set;  }

        public string ManualPick { get; protected set; }

        public string ReqType { get; set; }

        public string Reference { get; set; }

        public string FromStockPool { get; set; }

        public string ToStockPool { get; protected set; }

        public string FromState { get; protected set; }

        public string ToState { get; protected set; }

        public string FromCategory { get; set; }

        public string ToCategory { get; set; }

        public DateTime? BatchDate { get; set; }

        public string BatchRef { get; set; }

        public int? LoanNumber { get; protected set; }

        public string ReqSource { get; set; }
        
        public string UnitOfMeasure { get; protected set; }

        public string WorkStationCode { get; set; }

        public Part NewPart { get; set; }
		
        public int? OriginalReqNumber { get; set; }

        public int? Document3 { get; set; }

        public DateTime? DateReceived { get; set; }

        public string AuditLocation { get; set; }

        protected RequisitionHeader()
        {
        }
        
        public RequisitionHeader(
            Employee createdBy,
            StoresFunction function,
            string reqType,
            int? document1Number,
            string document1Type,
            Department department,
            Nominal nominal,
            string reference = null,
            string comments = null,
            string manualPick = null,
            string fromStockPool = null,
            string toStockPool = null,
            int? fromPalletNumber = null,
            int? toPalletNumber = null,
            StorageLocation fromLocation = null,
            StorageLocation toLocation = null,
            Part part = null,
            decimal? quantity = null,
            int? document1Line = null,
            string toState = null,
            string fromState = null, 
            string batchRef = null,
            DateTime? batchDate = null,
            string category = null,
            int? document2Number = null,
            string document2Type = null,
            string isReverseTrans = "N",
            RequisitionHeader isReversalOf = null,
            DateTime? dateReceived = null,
            string fromCategory = null,
            string auditLocation = null)
        {
            this.ReqSource = "STORES2";
            this.Booked = "N";
            this.CreatedBy = createdBy;
            this.Comments = comments;
            this.DateCreated = DateTime.Now;
            this.StoresFunction = function;
            this.Document1Name = string.IsNullOrEmpty(document1Type) ? "REQ" : document1Type;
            this.Document1Line = document1Line;
            this.Quantity = quantity;
            this.Part = part;
            this.UnitOfMeasure = part?.OurUnitOfMeasure;
            this.ToPalletNumber = toPalletNumber;
            this.FromPalletNumber = fromPalletNumber;
            this.ToState = toState;
            this.FromState = fromState;
            this.ManualPick = manualPick;
            this.FromStockPool = fromStockPool;
            this.ToStockPool = toStockPool;
            this.FromLocation = fromPalletNumber.HasValue ? null : fromLocation;
            this.ToLocation = toPalletNumber.HasValue ? null : toLocation;
            this.Cancelled = "N";
            this.BatchRef = batchRef;
            this.BatchDate = batchDate;
            this.ToCategory = category ?? function?.Category;
            this.Document1 = document1Number;
            this.Department = department;
            this.Nominal = nominal;
            this.Reference = reference;
            this.ReqType = reqType;
            this.Document2 = document2Number;
            this.Document2Name = document2Type;
            this.AuditLocation = auditLocation;
            
            // logic for creating a reversal req
            // mostly taken from GET_REQ_FOR_REVERSAL in REQ_UT.fmb
            if (isReversalOf != null)
            {
                if (this.Document1Name != isReversalOf.Document1Name)
                {
                    throw new CreateRequisitionException("Cannot reverse req with a different document1 type");
                }

                if (this.Document1 != isReversalOf.Document1)
                {
                    throw new CreateRequisitionException("Cannot reverse req with a different document1 number");
                }

                if (isReversalOf.IsReversed == "Y")
                {
                    throw new CreateRequisitionException($"req {isReversalOf.ReqNumber} is already reversed!");
                }
                
                this.OriginalReqNumber = isReversalOf.ReqNumber;
                this.Quantity = isReversalOf.Quantity * -1;
                this.Reference = isReversalOf.Reference;
                this.FromState = isReversalOf.FromState;
                this.ToStockPool = isReversalOf.ToStockPool;
                this.ToState = isReversalOf.ToState;
                this.FromStockPool = this.StoresFunction.FromStockPoolRequired == "Y" ? isReversalOf.FromStockPool : fromStockPool;
                this.BatchRef = this.StoresFunction.FunctionCode == "LOAN BACK"
                    ? $"Q{isReversalOf.ReqNumber}" : batchRef;
                this.BatchDate = this.StoresFunction.FunctionCode == "LOAN BACK"
                    ? isReversalOf.DateBooked : batchDate;
                this.FromLocation = isReversalOf.FromLocation;
                this.FromPalletNumber = isReversalOf.FromPalletNumber;
            }

            this.IsReverseTransaction = isReverseTrans;
            this.IsReversed = "N";
            this.DateReceived = dateReceived;
            this.Lines = new List<RequisitionLine>();

            if (!string.IsNullOrEmpty(fromCategory))
            {
                this.FromCategory = fromCategory;
            }
            
            var errors = this.Validate().ToList();

            if (errors.Any())
            {
                if (this.StoresFunction == null)
                {
                    throw new CreateRequisitionException($"{string.Join(", ", errors)}");
                }

                throw new CreateRequisitionException(
                    $"{string.Join(", ", errors)}");
            }
            
            if (function.FunctionCode == "GIST PO")
            {
                if (isReverseTrans == "Y")
                {
                    this.BatchRef = null;
                }
            }

            if (function.FunctionCode == "LOAN BACK")
            {
                this.FromState = "STORES";
                this.LoanNumber = document1Number;
            }
        }

        private IEnumerable<string> Validate()
        {
            if (this.StoresFunction == null)
            {
                yield return "Please choose a Function.";
                yield break;  // don't even have a function, so no need to continue with function specific validation
            }
            
            if (this.CreatedBy == null)
            {
                yield return "Invalid CreatedBy Employee";
            }

            if (this.StoresFunction.Document1Required())
            {
                if (this.Document1 == null)
                {
                    yield return $"Document1 number required: {this.StoresFunction.Document1Text}";
                }
            }

            if (this.StoresFunction.QuantityRequired == "Y")
            {
                if (this.Quantity.GetValueOrDefault() == 0)
                {
                    yield return $"Quantity required for: {this.StoresFunction.FunctionCode}";
                }
            }

            if (this.StoresFunction.AuditLocationRequired == "Y" && string.IsNullOrEmpty(this.AuditLocation))
            {
                yield return $"You must specify an audit location for {this.StoresFunction.FunctionCode}.";
            }

            if (this.StoresFunction.DepartmentNominalRequired == "Y")
            {
                if (this.Department == null || this.Nominal == null)
                {
                    yield return 
                        $"Nominal and Department must be specified for a {this.StoresFunction.FunctionCode} req";
                }

                if (this.StoresFunction.GetNominal() != null)
                {
                    if (this.StoresFunction.GetNominal().NominalCode != this.Nominal?.NominalCode)
                    {
                        yield return $"Nominal must be {this.StoresFunction.GetNominal().NominalCode}";
                    }
                }
            }

            if (this.StoresFunction.FromLocationRequired == "Y")
            {
                if (this.FromLocation == null && !this.FromPalletNumber.HasValue)
                {
                    yield return $"From location or pallet required for: {this.StoresFunction.FunctionCode}";
                }
            }

            if (this.StoresFunction.ToLocationRequired == "Y" && !this.IsReverseTrans())
            {
                if (this.ToLocation == null && !this.ToPalletNumber.HasValue)
                {
                    yield return $"To location or pallet required for: {this.StoresFunction.FunctionCode}";
                }
            }

            if (this.IsReverseTrans() && this.StoresFunction.CanBeReversed != "Y")
            {
                yield return $"You cannot reverse a {this.StoresFunction.FunctionCode} transaction";
            }
            else if (this.IsReverseTrans() && this.StoresFunction.FunctionCode != "BOOKLD"
                                      && !this.OriginalReqNumber.HasValue)
            {
                yield return "You must specify a req number to reverse";
            }
            
            if (this.StoresFunction.ReceiptDateRequired == "Y"  && !this.IsReverseTrans() && !this.DateReceived.HasValue)
            {
                throw new RequisitionException($"A receipt date is required for function {this.StoresFunction.FunctionCode}.");
            }

            // TODO - I noticed similar checks for valid From/To State (possible duplication) in IStoresService
            if (this.StoresFunction.FromStateRequired == "Y"  && !this.IsReverseTrans())
            {
                if (string.IsNullOrEmpty(this.FromState))
                {
                    yield return $"From state must be specified for {this.StoresFunction.FunctionCode}";
                }
            }

            if (this.StoresFunction.ToStateRequired == "Y" && string.IsNullOrEmpty(this.ToState))
            {
                yield return $"To state must be specified for {this.StoresFunction.FunctionCode}";
            }

            if (!string.IsNullOrEmpty(this.FromState) && !this.IsReverseTrans())
            {
                var validFromStates = this.StoresFunction.GetTransactionStates("F");
                if (validFromStates.Count > 0 // does no transaction states mean anything is allowed?
                    && !this.StoresFunction.GetTransactionStates("F").Contains(this.FromState))
                {
                    yield return 
                        $"From state must be one of "
                        + $"{string.Join(",", validFromStates)}";
                }
            }

            if (!string.IsNullOrEmpty(this.ToState))
            {
                var validToStates = this.StoresFunction.GetTransactionStates("O");
                if (validToStates.Count > 0 // does no transaction states mean anything is allowed?
                    && !this.StoresFunction.GetTransactionStates("O").Contains(this.ToState))
                {
                    yield return
                        $"To state must be one of "
                        + $"{string.Join(",", validToStates)}";
                }
            }

            if (this.StoresFunction.FromStockPoolRequired == "Y" && string.IsNullOrEmpty(this.FromStockPool))
            {
               yield return $"From stock pool must be specified for {this.StoresFunction.FunctionCode}";
            }

            if (this.StoresFunction.ToStockPoolRequired == "Y" && string.IsNullOrEmpty(this.ToStockPool))
            {
                yield return $"To stock pool must be specified for {this.StoresFunction.FunctionCode}";
            }
        }

        public void Update(string comments)
        {
            if (this.IsBooked())
            {
                throw new RequisitionException("Cannot amend a booked req");
            }

            this.Comments = comments;
        }

        public void AddLine(RequisitionLine toAdd)
        {
            if (this.IsBooked())
            {
                throw new RequisitionException("Cannot add lines to a booked req");
            }

            if (toAdd.TransactionDefinition == null)
            {
                throw new RequisitionException("Line must have a transaction definition");
            }

            var transactionTypesForFunction = this.StoresFunction.TransactionsTypes;
            var transactionReqType = transactionTypesForFunction
                ?.FirstOrDefault(t => t.TransactionDefinition.TransactionCode == toAdd.TransactionDefinition.TransactionCode)?.ReqType;
            if (transactionReqType != this.ReqType)
            {
                throw new RequisitionException(
                    $"Cannot add a {toAdd.TransactionDefinition?.TransactionCode} line to a req of type {this.ReqType}");
            }
            
            if (toAdd.Part == null || toAdd.Qty == 0)
            {
                throw new RequisitionException("Line must specify part number and qty");
            }
            
            this.Lines ??= new List<RequisitionLine>();
            toAdd.RequisitionHeader = this;
            var headerSpecifiesOntLocation = this.ToLocation != null || this.ToPalletNumber.HasValue;
            
            this.Lines.Add(toAdd);
        }
        
        public bool IsCancelled() => this.DateCancelled != null || this.Cancelled == "Y";

        public bool IsBooked() => this.DateBooked != null;

        public bool IsReverseTrans() => this.IsReverseTransaction == "Y";

        public bool IsAuthorised() => this.DateAuthorised != null || this.AuthorisedBy != null;

        public bool HasDocument1() => !string.IsNullOrEmpty(this.Document1Name) && this.Document1.HasValue;

        public bool HasDocument1WithLine() => this.HasDocument1() && this.Document1Line.HasValue;

        public void Book(Employee bookedBy)
        {
            this.DateBooked = DateTime.Now;
            this.BookedBy = bookedBy;
        }

        public void BookLine(int lineNumber, Employee bookedBy, DateTime when)
        {
            this.Lines.First(x => x.LineNumber == lineNumber).Book(when);

            if (!this.IsBooked() && this.Lines.All(l => l.IsBooked()))
            {
                this.Book(bookedBy);
            }
        }

        public void Cancel(string reason, Employee cancelledBy)
        {
            // note: this function does not represent a complete picture
            // a lot of extra logic surrounding cancelling the req is in stored procedures
            // see RequisitionService
            if (string.IsNullOrEmpty(reason))
            {
                throw new RequisitionException("Must provide a cancel reason");
            }

            if (this.IsBooked())
            {
                throw new RequisitionException("Cannot cancel a booked req");
            }

            var now = DateTime.Now;
            this.Cancelled = "Y";
            this.CancelledBy = cancelledBy;
            this.DateCancelled = now;
            this.CancelledReason = reason;

            this.CancelDetails ??= new List<CancelDetails>();

            this.CancelDetails.Add(new CancelDetails
                                       {
                                           ReqNumber = this.ReqNumber,
                                           DateCancelled = now,
                                           Reason = reason,
                                           CancelledBy = cancelledBy.Id
                                       });
            if (this.Lines == null)
            {
                return;
            }

            foreach (var l in this.Lines)
            {
                // Set some cancelled fields
                // This probably happens in the store procedures anyway
                // But no harm in making sure
                // (and actually updating this tracked entity so updates can be returned to the client)
                l.Cancel(cancelledBy.Id, reason, now);
            }
        }

        public void CancelLine(int lineNumber, string reason, Employee cancelledBy)
        {
            var now = DateTime.Now;
            
            // cancel line
            this.Lines.First(x => x.LineNumber == lineNumber).Cancel(cancelledBy.Id, reason, now);

            // cancel header if all lines are now cancelled
            if (this.Lines.All(x => x.DateCancelled.HasValue) 
                && !this.IsCancelled())
            {
                this.Cancel(reason, cancelledBy);
            }

            // book header if all non-cancelled lines are booked
            if (!this.DateBooked.HasValue
                && this.Lines.Where(x => !x.DateCancelled.HasValue).All(l => l.DateBooked.HasValue))
            {
                this.DateBooked = now;
            }
        }

        public void Authorise(Employee authorisedBy)
        {
            if (!this.IsAuthorised())
            {
                this.DateAuthorised = DateTime.Now;
                this.AuthorisedBy = authorisedBy;
            }
        }

        public bool RequiresAuthorisation()
        {
            if (!this.IsBooked() && !this.IsCancelled() && !this.IsAuthorised())
            {
                return this.Lines.Any(l => l.RequiresAuthorisation());
            }

            return false;
        }

        public string AuthorisePrivilege()
        {
            return this.RequiresAuthorisation() ? this.Lines.First(l => l.RequiresAuthorisation()).AuthorisePrivilege() : null;
        }

        public bool CanBookReq(int? lineNumber)
        {
            if (!this.IsBooked() && !this.IsCancelled() && this.StoresFunction != null)
            {
                var lines = this.Lines.Where(l => l.LineNumber == (lineNumber ?? l.LineNumber) && !l.IsBooked() && !l.IsCancelled());
                var requisitionLines = lines as RequisitionLine[] ?? lines.ToArray();
                if (requisitionLines.Any())
                {
                    if (requisitionLines.All(l => l.OkToBook()))
                    {
                        if (!this.RequiresAuthorisation())
                        {
                            var linesQty = this.Lines.Where(l => !l.HasDecrementTransaction() && !l.HasMaterialVarianceTransaction()).Sum(l => l.Qty);

                            if (linesQty > 0)
                            {
                                if (this.Quantity == null)
                                {
                                    // no header qty to check thus true
                                    return true;
                                }
                                else if (this.StoresFunction.FunctionCode == "PARTNO CH" ||
                                         this.StoresFunction.FunctionCode == "BOOKWO" ||
                                         this.StoresFunction.FunctionCode == "SUKIT")
                                {
                                    // you guys are exempt from this check although most times BOOKWO should pass it
                                    return true;
                                }
                                return linesQty == this.Quantity.Value;
                            }
                        }
                    }
                }
                else if (this.StoresFunction.AuditFunction())
                {
                    // audit functions don't need lines or checks
                    return true;
                }
            }

            return false;
        }

        public string AccountingCompanyCode()
        {
            if (this.Lines != null && this.Lines.Any())
            {
                var part = this.Lines.First(l => l.Part != null)?.Part;
                return part?.AccountingCompanyCode;
            }
            return null;
        }

        public void SetStateAndCategory(string fromState, 
                                        string toState, 
                                        string toCategory = "FREE", 
                                        string fromCategory = "FREE")
        {
            this.FromState = fromState;
            this.ToState = toState;
            this.ToCategory = toCategory;
            this.FromCategory = fromCategory;
        }

        public bool HasDeliveryNote()
        {
            // code from REQ_UT.fmb/CHECK_DISTRIBUTOR
            // used to check for DISTRIBUTORS but only relevant for sending stock to records distributors
            // used to check for DEM STOCK but seems to want to send it to 257 Drakemyre Drive no longer owned
            if (this.StoresFunction?.FunctionCode == "SUKIT" || this.StoresFunction?.FunctionCode == "SUREQ")
            {
                return true;
            }
            return false;
        }

        public bool ToStockPoolRequiredWithPart()
        {
            if (this.Part != null)
            {
                return this.StoresFunction.ToLocationRequiredOrOptional();
            }

            if (this.StoresFunction.ToStockPoolRequired == "O")
            {
                // For MOVELOC
                return false;
            }
            
            return this.StoresFunction.ToLocationIsRequired();
        }
    }
}
