namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Domain;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;

    public class RequisitionManager : IRequisitionManager
    {
        private readonly IAuthorisationService authService;
        
        private readonly IRepository<RequisitionHeader, int> repository;

        private readonly IRequisitionStoredProcedures requisitionStoredProcedures;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<Part, string> partRepository;

        private readonly IRepository<StorageLocation, int> storageLocationRepository;

        private readonly IRepository<StoresTransactionDefinition, string> transactionDefinitionRepository;

        private readonly ITransactionManager transactionManager;

        private readonly IStoresService storesService;
        
        private readonly IStockService stockService;

        private readonly IRepository<StoresPallet, int> palletRepository;

        private readonly IRepository<StockState, string> stateRepository;

        private readonly IRepository<StockPool, string> stockPoolRepository;

        private readonly IRepository<StoresFunction, string> storesFunctionRepository;

        private readonly IRepository<Department, string> departmentRepository;

        private readonly IRepository<Nominal, string> nominalRepository;

        private readonly IDocumentProxy documentProxy;

        public RequisitionManager(
            IAuthorisationService authService,
            IRepository<RequisitionHeader, int> repository,
            IRequisitionStoredProcedures requisitionStoredProcedures,
            IRepository<Employee, int> employeeRepository,
            IRepository<Part, string> partRepository,
            IRepository<StorageLocation, int> storageLocationRepository,
            IRepository<StoresTransactionDefinition, string> transactionDefinitionRepository,
            ITransactionManager transactionManager,
            IStoresService storesService,
            IRepository<StoresPallet, int> palletRepository,
            IRepository<StockState, string> stateRepository,
            IRepository<StockPool, string> stockPoolRepository,
            IRepository<StoresFunction, string> storesFunctionRepository,
            IRepository<Department, string> departmentRepository,
            IRepository<Nominal, string> nominalRepository,
            IDocumentProxy documentProxy,
            IStockService stockService)
        {
            this.authService = authService;
            this.repository = repository;
            this.requisitionStoredProcedures = requisitionStoredProcedures;
            this.employeeRepository = employeeRepository;
            this.partRepository = partRepository;
            this.storageLocationRepository = storageLocationRepository;
            this.transactionDefinitionRepository = transactionDefinitionRepository;
            this.transactionManager = transactionManager;
            this.storesService = storesService;
            this.palletRepository = palletRepository;
            this.stateRepository = stateRepository;
            this.stockPoolRepository = stockPoolRepository;
            this.storesFunctionRepository = storesFunctionRepository;
            this.departmentRepository = departmentRepository;
            this.nominalRepository = nominalRepository;
            this.documentProxy = documentProxy;
            this.stockService = stockService;
        }
        
        public async Task<RequisitionHeader> CancelHeader(
            int reqNumber,
            int cancelledBy,
            IEnumerable<string> privileges,
            string reason,
            bool requiresAuth = true)
        {
            if (requiresAuth && !this.authService.HasPermissionFor(
                    AuthorisedActions.CancelRequisition, privileges))
            {
                throw new UnauthorisedActionException(
                    "You do not have permission to cancel a requisition");
            }

            var req = await this.repository.FindByIdAsync(reqNumber);
            
            if (string.IsNullOrEmpty(req.StoresFunction.CancelFunction))
            {
                var by = await this.employeeRepository.FindByIdAsync(cancelledBy);
                req.Cancel(reason, by);
            }
            else if (req.StoresFunction.CancelFunction == "UNALLOC_REQ")
            {
                var unallocateReqResult = await this.requisitionStoredProcedures.UnallocateRequisition(
                    reqNumber, null, cancelledBy);

                if (!unallocateReqResult.Success)
                {
                    throw new CancelRequisitionException(unallocateReqResult.Message);
                }
            }
            else
            {
                throw new CancelRequisitionException(
                    "Cannot cancel req - invalid cancel function");
            }

            var deleteAllocsOntoResult = await this.requisitionStoredProcedures.DeleteAllocOntos(
                                             reqNumber,
                                             null,
                                             req.Document1,
                                             req.Document1Name);

            if (!deleteAllocsOntoResult.Success)
            {
                throw new CancelRequisitionException(deleteAllocsOntoResult.Message);
            }

            return req;
        }

        public async Task<RequisitionHeader> CancelLine(
            int reqNumber, 
            int lineNumber,
            int cancelledBy,
            IEnumerable<string> privileges,
            string reason)
        {
            if (!this.authService.HasPermissionFor(
                    AuthorisedActions.CancelRequisition, privileges))
            {
                throw new UnauthorisedActionException(
                    "You do not have permission to cancel a requisition");
            }

            var req = await this.repository.FindByIdAsync(reqNumber);

            if (string.IsNullOrEmpty(req.StoresFunction.CancelFunction))
            {
                var by = await this.employeeRepository.FindByIdAsync(cancelledBy);
                req.CancelLine(lineNumber, reason, by);
            }
            else if (req.StoresFunction.CancelFunction == "UNALLOC_REQ")
            {
                var unallocateReqResult = await this.requisitionStoredProcedures.UnallocateRequisition(
                                              reqNumber, lineNumber, cancelledBy);

                if (!unallocateReqResult.Success)
                {
                    throw new RequisitionException(unallocateReqResult.Message);
                }
            }
            else
            {
                throw new RequisitionException(
                    "Cannot cancel req - invalid cancel function");
            }

            var deleteAllocsOntoResult = await this.requisitionStoredProcedures.DeleteAllocOntos(
                                             reqNumber,
                                             lineNumber,
                                             req.Document1,
                                             req.Document1Name);

            if (!deleteAllocsOntoResult.Success)
            {
                throw new RequisitionException(deleteAllocsOntoResult.Message);
            }

            return req;
        }

        public async Task<RequisitionHeader> BookRequisition(
            int reqNumber, 
            int? lineNumber,
            int bookedBy,
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(
                    AuthorisedActions.BookRequisition, privileges))
            {
                throw new UnauthorisedActionException(
                    "You do not have permission to book a requisition");
            }

            var doRequisitionResult = await this.requisitionStoredProcedures.DoRequisition(
                reqNumber,
                lineNumber,
                bookedBy);

            if (!doRequisitionResult.Success)
            {
                throw new RequisitionException(doRequisitionResult.Message);
            }

            var req = await this.repository.FindByIdAsync(reqNumber);
            return req;
        }

        public async Task<RequisitionHeader> AuthoriseRequisition(
            int reqNumber, 
            int authorisedBy,
            IEnumerable<string> privileges)
        {
            var req = await this.repository.FindByIdAsync(reqNumber);

            if (req == null)
            {
                throw new RequisitionException("Req not found");
            }

            if (!this.authService.HasPermissionFor(
                    req.AuthorisePrivilege(), privileges))
            {
                throw new UnauthorisedActionException(
                    "You do not have permission to authorise this requisition");
            }

            var by = await this.employeeRepository.FindByIdAsync(authorisedBy);

            if (by == null)
            {
                throw new RequisitionException("Authorised by not found");
            }

            req.Authorise(by);

            return req;
        }

        public async Task AddMovesToLine(RequisitionLine line, IEnumerable<MoveSpecification> moves)
        {
            var moveSpecifications = moves.ToList();
            await this.CheckMoves(line.Part.PartNumber, moveSpecifications);
            
            // only implementing moves onto for now
            foreach (var moveOnto
                     in moveSpecifications.Where(x => x.ToPallet.HasValue || !string.IsNullOrEmpty(x.ToLocation)))
            {
                var insertOntosResult = await this.requisitionStoredProcedures.InsertReqOntos(
                    line.ReqNumber,
                    moveOnto.Qty,
                    line.LineNumber,
                    moveOnto.ToLocationId,
                    moveOnto.ToPallet,
                    moveOnto.ToStockPool,
                    moveOnto.ToState,
                    "FREE");

                if (!insertOntosResult.Success)
                {
                    throw new InsertReqOntosException($"Failed in insert_req_ontos: {insertOntosResult.Message}");
                }
            }
        }

        public async Task AddRequisitionLine(RequisitionHeader header, LineCandidate toAdd)
        {
            var part = await this.partRepository.FindByIdAsync(toAdd.PartNumber);
            var transactionDefinition = await this.transactionDefinitionRepository.FindByIdAsync(toAdd.TransactionDefinition);

            header.AddLine(new RequisitionLine(header.ReqNumber, toAdd.LineNumber, part, toAdd.Qty, transactionDefinition));

            // we need this so the line exists for the stored procedure calls coming next
            await this.transactionManager.CommitAsync();

            var createdMoves = false;
            var movesToAdd = toAdd.Moves?.ToList();

            if (movesToAdd != null && movesToAdd.Any())
            {
                await this.CheckMoves(toAdd.PartNumber, movesToAdd);

                // for now, assuming moves are either a write on or off, i.e. not a move from one place to another
                // write offs
                foreach (var pick in movesToAdd.Where(x => x.FromPallet.HasValue 
                                                           || !string.IsNullOrEmpty(x.FromLocation)))
                {
                    var fromLocation = string.IsNullOrEmpty(pick.FromLocation)
                                           ? null
                                           : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == pick.FromLocation);
                    var pickResult = await this.requisitionStoredProcedures.PickStock(
                                         toAdd.PartNumber,
                                         header.ReqNumber,
                                         toAdd.LineNumber,
                                         pick.Qty,
                                         fromLocation?.LocationId,
                                         pick.FromPallet,
                                         pick.FromStockPool,
                                         toAdd.TransactionDefinition);

                    if (!pickResult.Success)
                    {
                        throw new PickStockException("failed in pick_stock: " + pickResult.Message);
                    }

                    createdMoves = true;
                }

                // write ons
                foreach (var moveOnto 
                         in movesToAdd.Where(x => x.ToPallet.HasValue || !string.IsNullOrEmpty(x.ToLocation)))
                {
                    var insertOntosResult = await this.requisitionStoredProcedures.InsertReqOntos(
                                                header.ReqNumber,
                                                moveOnto.Qty,
                                                toAdd.LineNumber,
                                                moveOnto.ToLocationId,
                                                moveOnto.ToPallet,
                                                moveOnto.ToStockPool,
                                                moveOnto.ToState,
                                                "FREE",
                                                createdMoves ? "U" : "I");

                    if (!insertOntosResult.Success)
                    {
                        throw new InsertReqOntosException($"Failed in insert_req_ontos: {insertOntosResult.Message}");
                    }
                }
            }
            else if (header.ManualPick == "N" && transactionDefinition.RequiresStockAllocations)
            {
                // todo can we make automatic from picks see CREATE_REQ_MOVES in REQ_UT.fmb

                var autopickResult = await this.requisitionStoredProcedures.PickStock(
                    toAdd.PartNumber,
                    header.ReqNumber,
                    toAdd.LineNumber,
                    toAdd.Qty,
                    header.FromLocation?.LocationId,
                    header.FromPalletNumber,
                    header.FromStockPool,
                    toAdd.TransactionDefinition);

                if (!autopickResult.Success)
                {
                    throw new PickStockException("failed in pick_stock: " + autopickResult.Message);
                }

                // todo what about the ontos
                if (transactionDefinition.RequiresOntoTransactions)
                {
                    var ontoResult = await this.requisitionStoredProcedures.InsertReqOntos(
                        header.ReqNumber,
                        toAdd.Qty,
                        toAdd.LineNumber,
                        header.ToLocation?.LocationId,
                        header.ToPalletNumber,
                        header.ToStockPool,
                        header.ToState,
                        "FREE",
                        "U");

                    if (!ontoResult.Success)
                    {
                        throw new InsertReqOntosException($"Failed in insert_req_ontos: {ontoResult.Message}");
                    }
                }

            }

            // todo - consider what has to happen if a move is from one place to another
            // i.e. cases where both a 'from' and a 'to' location or pallet is set
            var createPostingsResult = await this.requisitionStoredProcedures.CreateNominals(
                                           header.ReqNumber,
                                           toAdd.Qty,
                                           toAdd.LineNumber,
                                           header.Nominal?.NominalCode,
                                           header.Department?.DepartmentCode);

            if (!createPostingsResult.Success)
            {
                throw new CreateNominalPostingException("failed in create_nominals: " + createPostingsResult.Message);
            }
        }

        public async Task CheckAndBookRequisitionHeader(RequisitionHeader header)
        {
            await this.DoChecksForAutoHeader(header);

            await this.repository.AddAsync(header);

            DoProcessResultCheck(  
                await this.requisitionStoredProcedures.CreateRequisitionLines(header.ReqNumber, null));

            DoProcessResultCheck(await this.requisitionStoredProcedures.CanBookRequisition(
                                     header.ReqNumber,
                                     null,
                                     header.Quantity.GetValueOrDefault()));

            DoProcessResultCheck(await this.requisitionStoredProcedures.DoRequisition(
                                     header.ReqNumber,
                                     null,
                                     header.CreatedBy.Id));
        }

        public async Task<RequisitionHeader> CreateLoanReq(int loanNumber)
        {
            var proxyResult = await this.requisitionStoredProcedures.CreateLoanReq(loanNumber);

            if (!proxyResult.Success)
            {
                throw new CreateRequisitionException(proxyResult.Message);
            }

            var reqNumber = Convert.ToInt32(proxyResult.Message);

            return await this.repository.FindByIdAsync(reqNumber);
        }

        public async Task<RequisitionHeader> PickStockOnRequisitionLine(RequisitionHeader header, LineCandidate lineWithPicks)
        {
            var line = header.Lines.SingleOrDefault(l => l.LineNumber == lineWithPicks.LineNumber);

            if (line == null)
            {
                throw new PickStockException("Could not find line");
            }

            // if no moves before
            if (!line.Moves.Any())
            {
                foreach (var pick in lineWithPicks.Moves.Where(x => x.FromPallet.HasValue || !string.IsNullOrEmpty(x.FromLocation)))
                {
                    var fromLocation = string.IsNullOrEmpty(pick.FromLocation)
                        ? null
                        : await this.storageLocationRepository.FindByAsync(x => x.LocationCode == pick.FromLocation);

                    // warning this call only makes the From side of the req_moves you still need to fill out other side
                    var pickResult = await this.requisitionStoredProcedures.PickStock(
                            lineWithPicks.PartNumber,
                            header.ReqNumber,
                            lineWithPicks.LineNumber,
                            pick.Qty,
                            fromLocation?.LocationId,
                            pick.FromPallet,
                            header.FromStockPool,
                            lineWithPicks.TransactionDefinition);

                    if (!pickResult.Success)
                    {
                        throw new PickStockException("failed in pick_stock: " + pickResult.Message);
                    }

                    // now fetch the header with the moves
                    var pickedRequisition = await this.repository.FindByIdAsync(header.ReqNumber);

                    // now if our transaction is an onto transaction need to fix moves
                    var transaction =
                            await this.transactionDefinitionRepository.FindByIdAsync(
                                lineWithPicks.TransactionDefinition);

                    if (transaction.RequiresOntoTransactions)
                    {
                        var pickedLine = pickedRequisition.Lines.SingleOrDefault(l => l.LineNumber == lineWithPicks.LineNumber);
                        if (pickedLine != null)
                        {
                            foreach (var move in pickedLine.Moves)
                            {
                                move.SetOntoFieldsFromHeader(header);
                            }

                            // we need these saved now in case we are picking multiple req lines and lose the changes
                            await this.transactionManager.CommitAsync();
                        }
                    }

                    return pickedRequisition;
                }
            }

            return header;
        }

        public async Task UpdateRequisition(
            RequisitionHeader current, 
            string updatedComments,
            IEnumerable<LineCandidate> lineUpdates)
        {
            // todo - permission checks? will be different for different req types I assume
            current.Update(updatedComments);

            foreach (var line in lineUpdates)
            {
                var existingLine = current.Lines.SingleOrDefault(l => l.LineNumber == line.LineNumber);
                
                // are we updating an existing line?
                if (existingLine != null)
                {
                    // picking stock for an existing line
                    if (line.StockPicked.GetValueOrDefault())
                    {
                        await this.PickStockOnRequisitionLine(current, line);
                    }
                    
                    // adding moves for an existing line
                    var movesToAdd = line.Moves.Where(x => x.IsAddition).ToList();

                    if (movesToAdd.Count <= 0)
                    {
                        continue;
                    }

                    await this.CheckMoves(line.PartNumber, movesToAdd);
                    await this.AddMovesToLine(existingLine, movesToAdd);
                }
                else
                {
                    // adding a new line
                    // note - this will pick stock/create ontos, create nominal postings etc
                    // might need to rethink if not all new lines need this behaviour (update strategies? some other pattern)
                    await this.CheckMoves(line.PartNumber, line.Moves.ToList());
                    await this.AddRequisitionLine(current, line);
                }
            }
        }

        public async Task<RequisitionHeader> Validate(
            int createdBy,
            string functionCode,
            string reqType,
            int? document1Number,
            string document1Type,
            string departmentCode,
            string nominalCode,
            LineCandidate firstLine = null,
            string reference = null,
            string comments = null,
            string manualPick = null,
            string fromStockPool = null,
            string toStockPool = null,
            int? fromPalletNumber = null,
            int? toPalletNumber = null,
            string fromLocationCode = null,
            string toLocationCode = null,
            string partNumber = null,
            decimal? quantity = null,
            string fromState = null,
            string toState = null,
            string batchRef = null,
            DateTime? batchDate = null,
            int? document1Line = null)
        {
            // just try and construct a req with a single line
            // exceptions will be thrown if any of the validation fails
            var function = !string.IsNullOrEmpty(functionCode) 
                               ? await this.storesFunctionRepository.FindByIdAsync(functionCode) : null;
            var dept = !string.IsNullOrEmpty(departmentCode) 
                           ? await this.departmentRepository.FindByIdAsync(departmentCode) : null;
            var nom = !string.IsNullOrEmpty(nominalCode)
                          ? await this.nominalRepository.FindByIdAsync(nominalCode) : null;
            var fromLocation = !string.IsNullOrEmpty(fromLocationCode) 
                                   ? await this.storageLocationRepository.FindByAsync(x => x.LocationCode == fromLocationCode) : null;
            
            var toLocation = !string.IsNullOrEmpty(toLocationCode)
                                 ? await this.storageLocationRepository.FindByAsync(x => x.LocationCode == toLocationCode) : null;
            var part = !string.IsNullOrEmpty(partNumber) ? await this.partRepository.FindByIdAsync(partNumber) : null;

            var employee = await this.employeeRepository.FindByIdAsync(createdBy);

            var req = new RequisitionHeader(
                employee,
                function,
                reqType,
                document1Number,
                document1Type,
                dept,
                nom,
                reference,
                comments,
                manualPick,
                fromStockPool,
                toStockPool,
                fromPalletNumber,
                toPalletNumber,
                fromLocation,
                toLocation,
                part,
                quantity,
                document1Line,
                toState,
                fromState,
                batchRef,
                batchDate);

            if (functionCode == "LOAN OUT")
            {
                var loan = await this.documentProxy.GetLoan(document1Number.GetValueOrDefault());
                if (loan == null)
                {
                    throw new CreateRequisitionException($"Loan Number {document1Number} does not exist");
                }

                return req;
            }

            if (function.LinesRequired == "Y" && firstLine == null)
            {
                throw new CreateRequisitionException($"Lines are required for {functionCode}");
            }
            
            if (firstLine != null)
            {
                var firstLinePart = !string.IsNullOrEmpty(firstLine?.PartNumber)
                                        ? await this.partRepository.FindByIdAsync(firstLine.PartNumber)
                                        : null;
                var transactionDefinition = !string.IsNullOrEmpty(firstLine?.TransactionDefinition)
                                                ? await this.transactionDefinitionRepository.FindByIdAsync(firstLine.TransactionDefinition) : null;

                if (firstLine.Moves != null && firstLine.Moves.Any())
                {
                    await this.CheckMoves(firstLine.PartNumber, firstLine.Moves.ToList());
                }

                req.AddLine(new RequisitionLine(
                    0,
                    1,
                    firstLinePart,
                    firstLine.Qty,
                    transactionDefinition,
                    firstLine.Document1,
                    firstLine.Document1Line.GetValueOrDefault(),
                    firstLine.Document1Type));
            }

            // move below to its own function?
            if (function.PartSource == "PO")
            {
                var po = await this.documentProxy.GetPurchaseOrder(document1Number.GetValueOrDefault());

                if (po == null)
                {
                    throw new CreateRequisitionException($"PO {document1Number} does not exist!");
                }
                
                if (!po.IsAuthorised)
                {
                    throw new CreateRequisitionException($"PO {document1Number} is not authorised!");
                }

                if (po.IsFilCancelled)
                {
                    throw new CreateRequisitionException($"PO {document1Number} is FIL Cancelled!");
                }

                if (batchRef != $"P{document1Number.Value}")
                {
                    throw new CreateRequisitionException(   
                            "You are trying to pass stock for payment from a different PO");
                }   
            }
            else if (function.PartSource == "C" && function.Document1Required())
            {
                if (document1Type != "C")
                {
                    throw new CreateRequisitionException("Function requires a credit note");
                }

                var document = await this.GetDocument(
                    document1Type,
                    document1Number.Value,
                    document1Line);

                await this.CheckDocumentLineForOverAndFullyBooked(req, document);
            }

            if (req.Part == null && req.Lines.Count == 0 && function.PartSource != "C" && function.LinesRequired != "N")
            {
                throw new RequisitionException("Lines are required if header does not specify part");
            }

            if (function.AutomaticFunctionType() && req.Lines.Count == 0)
            {
                if (FieldIsNeededOrOptional(req.StoresFunction.QuantityRequired) && req.Quantity == null)
                {
                    throw new RequisitionException("A quantity must be entered");
                }

                if (FieldIsNeededOrOptional(req.StoresFunction.FromLocationRequired) && req.FromLocation == null && req.FromPalletNumber == null)
                {
                    throw new RequisitionException("A from location or pallet is required");
                }

                if (FieldIsNeededOrOptional(req.StoresFunction.FromStockPoolRequired) && string.IsNullOrEmpty(req.FromStockPool))
                {
                    throw new RequisitionException("A from stock pool is required");
                }

                if (FieldIsNeededOrOptional(req.StoresFunction.FromStateRequired) && string.IsNullOrEmpty(req.FromState))
                {
                    throw new RequisitionException("A from state is required");
                }

                if (FieldIsNeededOrOptional(req.StoresFunction.ToLocationRequired) && req.ToLocation == null && req.ToPalletNumber == null)
                {
                    throw new RequisitionException("A to location or pallet is required");
                }

                if (FieldIsNeededWithPart(req.StoresFunction.ToStockPoolRequired, req.Part) && string.IsNullOrEmpty(req.ToStockPool))
                {
                    throw new RequisitionException("A to stock pool is required");
                }

                if (FieldIsNeededOrOptional(req.StoresFunction.ToStateRequired) && string.IsNullOrEmpty(req.ToState))
                {
                    throw new RequisitionException("A to state is required");
                }

                if (req.StoresFunction.PartNumberRequired() && req.Part == null)
                {
                    throw new RequisitionException("Part number is required");
                }

                await this.DoChecksForAutoHeader(req);
            }

            return req;
        }

        public async Task<DocumentResult> GetDocument(string docName, int docNumber, int? lineNumber)
        {
            if (docName == "C")
            {
                var result = await this.documentProxy.GetCreditNote(docNumber, lineNumber);

                if (result == null)
                {
                    if (lineNumber.HasValue)
                    {
                        throw new DocumentException($"Could not find credit note {docNumber} with line {lineNumber}");
                    }

                    throw new DocumentException($"Could not find credit note {docNumber}");
                }

                if (lineNumber.HasValue && result.Quantity == 0)
                {
                    throw new DocumentException($"Credit note {docNumber} line {lineNumber} is cancelled");
                }

                return result;
            }

            return null;
        }

        public async Task CheckDocumentLineForOverAndFullyBooked(RequisitionHeader header, DocumentResult document)
        {
            if (header.HasDocument1WithLine() && document.Quantity.HasValue)
            {
                var reqs = await this.repository.FilterByAsync(r =>
                    r.Document1Name == header.Document1Name && r.Document1 == header.Document1.Value && r.Document1Line == header.Document1Line &&
                    r.Cancelled == "N" && r.Quantity != null);

                if (reqs.Any())
                {
                    var bookQty = reqs.Sum(r => r.Quantity.GetValueOrDefault());

                    if (header.Quantity.HasValue)
                    {
                        if (bookQty + header.Quantity > document.Quantity)
                        {
                            throw new DocumentException("Trying to overbook this line");
                        }
                    }
                    else if (bookQty == document.Quantity)
                    {
                        throw new DocumentException("Line is already fully booked");
                    }
                }
            }
        }


        private static void DoProcessResultCheck(ProcessResult result)
        {
            if (!result.Success)
            {
                throw new RequisitionException(result.Message);
            }
        }

        private static bool FieldIsNeededOrOptional(string code)
        {
            if (code == "Y" || code == "O")
            {
                return true;
            }

            return false;
        }

        private static bool FieldIsNeededWithPart(string code, Part part)
        {
            if (part != null)
            {
                return FieldIsNeededOrOptional(code);
            }

            return (code == "Y");
        }

        private async Task CheckMoves(string partNumber, IList<MoveSpecification> moves)
        {
            foreach (var m in moves)
            {
                if (m.Qty <= 0)
                {
                    throw new RequisitionException("Move qty is invalid");
                }

                if (!m.ToPallet.HasValue && string.IsNullOrEmpty(m.ToLocation) && !m.FromPallet.HasValue
                    && string.IsNullOrEmpty(m.FromLocation))
                {
                    throw new RequisitionException("Moves are missing location information");
                }

                // just checking moves onto for now, but could extend if required
                if (m.ToPallet.HasValue || !string.IsNullOrEmpty(m.ToLocation))
                {
                    if ((m.ToPallet.HasValue && !string.IsNullOrEmpty(m.ToLocation))
                        || (!m.ToPallet.HasValue && string.IsNullOrEmpty(m.ToLocation)))
                    {
                        throw new InsertReqOntosException(
                            "Move onto must specify either location code or pallet number");
                    }

                    if (m.ToPallet.HasValue)
                    {
                        var pallet = await this.palletRepository.FindByIdAsync(m.ToPallet.Value);

                        if (pallet == null || pallet.DateInvalid.HasValue)
                        {
                            throw new InsertReqOntosException($"Pallet {m.ToPallet.Value} is invalid");
                        }

                        var canPutPartOnPallet = await this.requisitionStoredProcedures.CanPutPartOnPallet(
                                                     partNumber,
                                                     m.ToPallet.Value);
                        if (!canPutPartOnPallet)
                        {
                            throw new CannotPutPartOnPalletException(
                                $"Cannot put part {partNumber} onto P{m.ToPallet}");
                        }
                    }
                    else if (!string.IsNullOrEmpty(m.ToLocation))
                    {
                        var toLocation =
                            await this.storageLocationRepository.FindByAsync(x => x.LocationCode == m.ToLocation);
                        if (toLocation == null)
                        {
                            throw new InsertReqOntosException($"Location {m.ToLocation} not found");
                        }

                        m.ToLocationId = toLocation.LocationId;
                    }
                }
            }
        }

        private async Task DoChecksForAutoHeader(RequisitionHeader header)
        {
            StoresPallet toPallet = null;
            if (header.ToPalletNumber.HasValue)
            {
                toPallet = await this.palletRepository.FindByIdAsync(header.ToPalletNumber.Value);
                if (toPallet == null || toPallet.DateInvalid.HasValue)
                {
                    throw new RequisitionException($"Pallet number {header.ToPalletNumber} is invalid");
                }
            }

            var toState = await this.stateRepository.FindByIdAsync(header.ToState);
            if (!string.IsNullOrEmpty(header.ToState) && toState == null)
            {
                throw new RequisitionException($"To State {header.ToState} does not exist");
            }

            var stockPool = await this.stockPoolRepository.FindByIdAsync(header.ToStockPool);
            if (!string.IsNullOrEmpty(header.ToStockPool) && stockPool == null)
            {
                throw new RequisitionException($"To Stock Pool {header.ToStockPool} does not exist");
            }

            if (header.Part != null)
            {
                DoProcessResultCheck(await this.storesService.ValidOntoLocation(
                    header.Part,
                    header.ToLocation,
                    toPallet,
                    toState));
            }

            if (!string.IsNullOrEmpty(header.FromState))
            {
                DoProcessResultCheck(await this.storesService.ValidState(
                    null,
                    header.StoresFunction,
                    header.FromState,
                    "F"));
            }

            if (!string.IsNullOrEmpty(header.ToState))
            {
                DoProcessResultCheck(await this.storesService.ValidState(
                    null,
                    header.StoresFunction,
                    header.ToState,
                    "O"));
            }

            DoProcessResultCheck(this.storesService.ValidStockPool(header.Part, stockPool));
            
            if (header.Part != null)
            {
                if (header.FromLocation != null || header.FromPalletNumber.HasValue)
                {
                    DoProcessResultCheck(
                        await this.stockService.ValidStockLocation(
                            header.FromLocation?.LocationId,
                            header.FromPalletNumber,
                            header.Part?.PartNumber,
                            header.Quantity.GetValueOrDefault(),
                            header.FromState));
                }

                DoProcessResultCheck(this.storesService.ValidStockPool(header.Part, stockPool));
            }
        }
    }
}
