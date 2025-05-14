namespace Linn.Stores2.Domain.LinnApps.Requisitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Linn.Common.Authorisation;
    using Linn.Common.Domain;
    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Persistence;
    using Linn.Stores2.Domain.LinnApps.Accounts;
    using Linn.Stores2.Domain.LinnApps.Exceptions;
    using Linn.Stores2.Domain.LinnApps.External;
    using Linn.Stores2.Domain.LinnApps.Parts;
    using Linn.Stores2.Domain.LinnApps.Stock;
    using Linn.Stores2.Domain.LinnApps.Stores;

    using Org.BouncyCastle.Asn1.X509.Qualified;

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

        private readonly ISalesProxy salesProxy;

        private readonly IRepository<PotentialMoveDetail, PotentialMoveDetailKey> potentialMoveRepository;

        private readonly IBomVerificationProxy bomVerificationProxy;

        private readonly IRepository<BookInOrderDetail, BookInOrderDetailKey> bookInOrderDetailRepository;

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
            IStockService stockService,
            ISalesProxy salesProxy,
            IRepository<PotentialMoveDetail, PotentialMoveDetailKey> potentialMoveRepository,
            IBomVerificationProxy bomVerificationProxy,
            IRepository<BookInOrderDetail, BookInOrderDetailKey> bookInOrderDetailRepository)
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
            this.salesProxy = salesProxy;
            this.potentialMoveRepository = potentialMoveRepository;
            this.bomVerificationProxy = bomVerificationProxy;
            this.bookInOrderDetailRepository = bookInOrderDetailRepository;
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
                await this.CheckMoves(
                    toAdd.PartNumber,
                    movesToAdd,
                    header.ReqType != "F" && FieldIsNeededOrOptional(header.StoresFunction.ToLocationRequired));

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

        public async Task CreateLinesAndBookAutoRequisitionHeader(RequisitionHeader header)
        {
            try
            {
                DoProcessResultCheck(
                    await this.requisitionStoredProcedures.CreateRequisitionLines(header.ReqNumber, null));

                // from REQ_UT REQHEADS.POST-INSERT
                // function codes that are function_Type A and process stage 1 incl LOAN OUT, SUKIT
                if (header.StoresFunction.ProcessStage == 2)
                {
                    DoProcessResultCheck(await this.requisitionStoredProcedures.CanBookRequisition(
                        header.ReqNumber,
                        null,
                        header.Quantity.GetValueOrDefault()));

                    DoProcessResultCheck(await this.requisitionStoredProcedures.DoRequisition(
                        header.ReqNumber,
                        null,
                        header.CreatedBy.Id));
                }
            }
            catch (DomainException e)
            {
                // cancel the req so it doesn't hang around unbooked and mess up validation
                var savedReq = await this.repository.FindByIdAsync(header.ReqNumber);
                savedReq.Cancel(e.Message, header.CreatedBy);
                if (savedReq.Lines?.Count > 0)
                {
                    foreach (var requisitionLine in savedReq.Lines)
                    {
                        requisitionLine.Cancel(header.CreatedBy.Id, e.Message, DateTime.Now);
                    }
                }

                await this.transactionManager.CommitAsync();

                throw;
            }
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
                        var pickedLine = pickedRequisition.Lines
                            .SingleOrDefault(l => l.LineNumber == lineWithPicks.LineNumber);
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

                    await this.CheckMoves(
                        line.PartNumber,
                        movesToAdd,
                        current.ReqType != "F" && FieldIsNeededOrOptional(current.StoresFunction?.ToLocationRequired));
                    await this.AddMovesToLine(existingLine, movesToAdd);
                }
                else
                {
                    // adding a new line
                    // note - this will pick stock/create ontos, create nominal postings etc
                    // might need to rethink if not all new lines need this behaviour (update strategies? some other pattern)
                    await this.CheckMoves(
                        line.PartNumber,
                        line.Moves.ToList(),
                        current.ReqType != "F" && FieldIsNeededOrOptional(current.StoresFunction?.ToLocationRequired));
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
            int? document1Line = null,
            string newPartNumber = null,
            IEnumerable<LineCandidate> lines = null,
            string isReverseTransaction = "N",
            int? originalDocumentNumber = null,
            IEnumerable<BookInOrderDetail> bookInOrderDetails = null,
            DateTime? dateReceived = null)
        {
            // just try and construct a req with a single line
            // exceptions will be thrown if any of the validation fails
            var function = !string.IsNullOrEmpty(functionCode)
                               ? await this.storesFunctionRepository.FindByIdAsync(functionCode)
                               : null;
            var dept = !string.IsNullOrEmpty(departmentCode)
                           ? await this.departmentRepository.FindByIdAsync(departmentCode)
                           : null;
            var nom = !string.IsNullOrEmpty(nominalCode)
                          ? await this.nominalRepository.FindByIdAsync(nominalCode)
                          : null;
            var fromLocation = !string.IsNullOrEmpty(fromLocationCode)
                                   ? await this.storageLocationRepository.FindByAsync(x =>
                                         x.LocationCode == fromLocationCode)
                                   : null;

            var toLocation = !string.IsNullOrEmpty(toLocationCode)
                                 ? await this.storageLocationRepository.FindByAsync(x =>
                                       x.LocationCode == toLocationCode)
                                 : null;
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
                batchDate,
                null,
                null,
                null,
                isReverseTransaction ?? "N",
                originalDocumentNumber,
                dateReceived);

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

            if (lines != null)
            {
                foreach (var candidate in lines)
                {
                    req.AddLine(await this.ValidateLineCandidate(candidate, req.StoresFunction, req.ReqType));
                }
            }
            else if (firstLine != null)
            {
                req.AddLine(await this.ValidateLineCandidate(firstLine, req.StoresFunction, req.ReqType));
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

                var orderRef = $"{po.DocumentType.Substring(0, 1)}{po.OrderNumber}";
                if (function.BatchRequired == "Y" && batchRef != orderRef)
                {
                    throw new CreateRequisitionException(   
                            "You are trying to pass stock for payment from a different PO");
                }
                
                // SUKIT validation to replace STORES_OO.CAN_BE_KITTED
                if (function.FunctionCode == "SUKIT")
                {
                    await this.CheckPurchaseOrderForOverAndFullyKitted(req, po);
                }

                if (function.FunctionCode == "BOOKLD")
                {
                    await this.CheckBookInOrderAndDetails(
                        document1Number,
                        quantity,
                        isReverseTransaction,
                        part,
                        bookInOrderDetails?.ToList());
                }

                if (function.FunctionCode == "BOOKSU")
                {
                    if (part.StockControlled != "Y")
                    {
                        throw new CreateRequisitionException($"{function.FunctionCode} requires part to be stock controlled and {part.PartNumber} is not.");
                    }

                    if (!req.IsReverseTrans())
                    {
                        if (req.Quantity < 0)
                        {
                            throw new CreateRequisitionException($"Cannot book a negative quantity against order {document1Number}.");
                        }

                        var qtyLeft = po.Details.First(a => a.Line == req.Document1Line).Deliveries
                            .Sum(a => a.QuantityOutstanding);
                        if (qtyLeft < req.Quantity)
                        {
                            throw new CreateRequisitionException($"The qty remaining on order {document1Number}/{document1Line} is {qtyLeft}. Cannot book {req.Quantity}.");
                        }
                    }
                }
            }
            else if (function.PartSource == "RO")
            {
                var ro = await this.documentProxy.GetPurchaseOrder(document1Number.GetValueOrDefault());

                if (ro == null)
                {
                    throw new CreateRequisitionException($"RO {document1Number} does not exist!");
                }

                // not a separate part source for credit orders get lumped in with RO
                if (ro.DocumentType != "RO" && ro.DocumentType != "CO")
                {
                    throw new CreateRequisitionException($"Order {document1Number} is not a returns/credit order!");
                }

                await this.CheckReturnOrderForFullyBooked(req, ro);
            }
            else if (function.PartSource == "WO")
            {
                await this.CheckValidWorksOrder(document1Number, part, isReverseTransaction, quantity);
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

            if (req.IsReverseTrans() && req.Quantity != null)
            {
                if (req.OriginalReqNumber == null && req.StoresFunction.FunctionCode != "BOOKLD")
                {
                    throw new CreateRequisitionException("An original req number must be supplied for a reverse");
                }

                if (req.OriginalReqNumber.HasValue)
                {
                    DoProcessResultCheck(
                        await this.storesService.ValidReverseQuantity(req.OriginalReqNumber.Value, req.Quantity.Value));
                }
            }

            if (function.ReceiptDateRequired == "Y" && !req.DateReceived.HasValue)
            {
                throw new RequisitionException($"A receipt date is required for function {function.FunctionCode}.");
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

                if (FieldIsNeededOrOptional(req.StoresFunction.FromLocationRequired) && req.FromLocation == null
                    && req.FromPalletNumber == null)
                {
                    throw new RequisitionException("A from location or pallet is required");
                }

                if (FieldIsNeededOrOptional(req.StoresFunction.FromStockPoolRequired)
                    && string.IsNullOrEmpty(req.FromStockPool) && req.StoresFunction.FunctionCode != "SUKIT")
                {
                    throw new RequisitionException("A from stock pool is required");
                }

                if (FieldIsNeededOrOptional(req.StoresFunction.FromStateRequired) && string.IsNullOrEmpty(req.FromState)
                    && !req.IsReverseTrans())
                {
                    throw new RequisitionException("A from state is required");
                }

                if (FieldIsNeededOrOptional(req.StoresFunction.ToLocationRequired) && req.ToLocation == null
                    && req.ToPalletNumber == null)
                {
                    throw new RequisitionException("A to location or pallet is required");
                }

                if (FieldIsNeededWithPart(req.StoresFunction.ToStockPoolRequired, req.Part)
                    && string.IsNullOrEmpty(req.ToStockPool))
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

                if (req.StoresFunction.NewPartNumberRequired())
                {
                    var newPart = !string.IsNullOrEmpty(newPartNumber)
                                      ? await this.partRepository.FindByIdAsync(newPartNumber)
                                      : null;
                    DoProcessResultCheck(this.storesService.ValidPartNumberChange(part, newPart));
                }

                await this.DoChecksForAutoHeader(req);
            }

            return req;
        }

        public async Task<RequisitionLine> ValidateLineCandidate(
            LineCandidate candidate,
            StoresFunction storesFunction = null,
            string reqType = null)
        {
            var part = !string.IsNullOrEmpty(candidate?.PartNumber)
                ? await this.partRepository.FindByIdAsync(candidate.PartNumber)
                : null;

            var transactionDefinition = !string.IsNullOrEmpty(candidate?.TransactionDefinition)
                                            ? await this.transactionDefinitionRepository.FindByIdAsync(
                                                  candidate.TransactionDefinition)
                                            : null;

            var line = new RequisitionLine(
                0,
                candidate.LineNumber,
                part,
                candidate.Qty,
                transactionDefinition,
                candidate.Document1,
                candidate.Document1Line.GetValueOrDefault(),
                candidate.Document1Type);

            if (candidate.Moves != null && candidate.Moves.Any())
            {
                await this.CheckMoves(
                    candidate.PartNumber,
                    candidate.Moves.ToList(),
                    reqType != "F" && FieldIsNeededOrOptional(storesFunction?.ToLocationRequired));
            }

            if ((candidate.Moves == null || !candidate.Moves.Any()) &&
                line.TransactionDefinition.RequiresOntoTransactions)
            {
                throw new CreateRequisitionException(
                    $"Must specify moves onto for {line.TransactionDefinition.TransactionCode}");
            }

            return line;
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
                               r.Document1Name == header.Document1Name && r.Document1 == header.Document1.Value
                                                                       && r.Document1Line == header.Document1Line
                                                                       && r.Cancelled == "N" && r.Quantity != null);

                if (reqs.Any())
                {
                    var bookQty = reqs.Sum(r => r.Quantity.GetValueOrDefault());

                    if (!header.IsReverseTrans())
                    {
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
                    else if (bookQty == 0)
                    {
                        throw new DocumentException("Line has never been booked");
                    }
                }
            }
        }

        public async Task<IEnumerable<PotentialMoveDetail>> AddPotentialMoveDetails(
            string documentType,
            int documentId,
            decimal? quantity,
            string partNumber,
            int? builtById,
            int? toLocationId,
            int? toPalletNumber)
        {
            var potentialMoves =
                await this.potentialMoveRepository.FilterByAsync(
                    a => a.DocumentId == documentId && a.DocumentType == documentType);
            var seq = 1;
            if (potentialMoves != null && potentialMoves.Any())
            {
                seq = potentialMoves.Max(a => a.Sequence) + 1;
            }

            var potentialMove = new PotentialMoveDetail
                                    {
                                        PartNumber = partNumber,
                                        Quantity = quantity,
                                        BuiltBy = builtById,
                                        Sequence = seq,
                                        DocumentType = documentType,
                                        DocumentId = documentId,
                                        LocationId = toLocationId,
                                        PalletNumber = toPalletNumber
                                    };
            await this.potentialMoveRepository.AddAsync(potentialMove);

            return new List<PotentialMoveDetail> { potentialMove }.AsEnumerable();
        }

        public async Task AddBookInOrderDetails(IList<BookInOrderDetail> details)
        {
            if (details != null && details.Count > 0)
            {
                var first = details.First();
                var existing = await this.bookInOrderDetailRepository.FilterByAsync(a =>
                                   a.OrderNumber == first.OrderNumber && a.OrderLine == first.OrderLine);

                if (existing.Count > 0)
                {
                    foreach (var existingBookInOrderDetail in existing)
                    { 
                       this.bookInOrderDetailRepository.Remove(existingBookInOrderDetail);
                    }
                }

                foreach (var bookInOrderDetail in details)
                {
                    await this.bookInOrderDetailRepository.AddAsync(bookInOrderDetail);
                }
            }
        }

        public async Task CheckPurchaseOrderForOverAndFullyKitted(RequisitionHeader header, PurchaseOrderResult purchaseOrder)
        {
            if (header.Document1Name == "PO" && purchaseOrder != null && purchaseOrder.OrderQty(header.Document1Line).HasValue)
            {
                var reqs = await this.repository.FilterByAsync(r =>
                    r.Document1Name == header.Document1Name && r.Document1 == header.Document1.Value &&
                    r.Cancelled == "N" && r.Quantity != null && r.StoresFunction.FunctionCode == "SUKIT" && r.ReqNumber != header.ReqNumber);

                if (reqs.Any())
                {
                    var kittedQty = reqs.Sum(r => r.Quantity.GetValueOrDefault());

                    if (kittedQty >= purchaseOrder.OrderQty(header.Document1Line))
                    {
                        throw new DocumentException($"Full order qty {purchaseOrder.OrderQty(header.Document1Line)} on order {header.Document1} has already been kitted");
                    }

                    if (header.Quantity.HasValue)
                    {
                        if (kittedQty + header.Quantity > purchaseOrder.OrderQty(header.Document1Line))
                        {
                            throw new DocumentException($"Only {purchaseOrder.OrderQty(header.Document1Line) - kittedQty} left to kit for order {header.Document1}");
                        }
                    }
                }
            }
        }

        public async Task CheckReturnOrderForFullyBooked(RequisitionHeader header, PurchaseOrderResult purchaseOrder)
        {
            if (header.Document1Name == "RO" && purchaseOrder != null
                                             && purchaseOrder.OrderQty(header.Document1Line).HasValue)
            {
                // call to STORES_OO.QTY_RETURNED could rewrite in c# but a bit involved
                var qty = await this.requisitionStoredProcedures.GetQtyReturned(
                              header.Document1.Value,
                              header.Document1Line.Value);
                if (header.IsReverseTrans() && header.Quantity.Value > qty)
                {
                    throw new DocumentException($"Returns Order {header.Document1}/{header.Document1Line} has not yet been booked");
                }

                if (header.IsReverseTransaction != "Y" && header.Quantity.Value - qty <= 0)
                {
                    throw new DocumentException($"Returns Order {header.Document1}/{header.Document1Line} is fully booked");
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

            return code == "Y";
        }

        private async Task CheckBookInOrderAndDetails(
            int? document1Number,
            decimal? reqQuantity,
            string isReverseTransaction,
            Part part,
            IList<BookInOrderDetail> bookInOrderDetails)
        {
            if (part == null)
            {
                throw new CreateRequisitionException("A sundry part must be specified for BOOKLD");
            }

            if (part.StockControlled == "Y")
            {
                throw new CreateRequisitionException($"Part {part.PartNumber} is stock controlled and BOOKLD must be sundry.");
            }
            
            if (bookInOrderDetails == null || bookInOrderDetails.Count == 0)
            {
                throw new CreateRequisitionException("No book in order details supplied for BOOKLD transaction");
            }

            if (!reqQuantity.HasValue || reqQuantity.Value == 0
                                      || bookInOrderDetails.Any(b => !b.Quantity.HasValue)
                                      || bookInOrderDetails.Any(b => b.Quantity == 0))
            {
                throw new CreateRequisitionException($"You must specify a quantity on req and all lines to book {document1Number}.");
            }

            if (isReverseTransaction == "Y" && (reqQuantity > 0 || bookInOrderDetails.Any(a => a.Quantity > 0)))
            {
                throw new CreateRequisitionException($"You must specify a negative quantity for reverse but {reqQuantity} supplied.");
            }

            if (isReverseTransaction == "N" && (reqQuantity < 0 || bookInOrderDetails.Any(a => a.Quantity < 0)))
            {
                throw new CreateRequisitionException($"You must specify a positive quantity for BOOKLD but {reqQuantity} supplied.");
            }

            var bookInDetailsQuantity = bookInOrderDetails.Sum(b => b.Quantity);
            if (reqQuantity.Value != bookInDetailsQuantity)
            {
                throw new CreateRequisitionException($"Book in order detail quantity ({bookInDetailsQuantity}) does not match req quantity ({reqQuantity}).");
            }

            if (bookInOrderDetails.Any(p => p.PartNumber != part.PartNumber))
            {
                throw new CreateRequisitionException("Part number is missing or incorrect on book in order details.");
            }

            if (bookInOrderDetails.Any(p =>
                    string.IsNullOrEmpty(p.DepartmentCode) || string.IsNullOrEmpty(p.NominalCode)))
            {
                throw new CreateRequisitionException("Department or Nominal missing on book in order details.");
            }

            foreach (var bookInOrderDetail in bookInOrderDetails)
            {
                DoProcessResultCheck(
                    await this.storesService.ValidDepartmentNominal(
                        bookInOrderDetail.DepartmentCode,
                        bookInOrderDetail.NominalCode));
            }
        }

        private async Task CheckValidWorksOrder(
            int? document1Number,
            Part part,
            string isReverseTransaction,
            decimal? quantity)
        {
            var worksOrder = await this.documentProxy.GetWorksOrder(document1Number.GetValueOrDefault());

            if (worksOrder == null)
            {
                throw new CreateRequisitionException($"Works Order {document1Number} does not exist.");
            }

            if (!string.IsNullOrEmpty(worksOrder.DateCancelled))
            {
                throw new CreateRequisitionException($"Works Order {document1Number} is cancelled.");
            }

            if ((!quantity.HasValue || quantity.Value == 0) && isReverseTransaction != "Y")
            {
                throw new CreateRequisitionException($"You must specify a quantity to book in from works order {document1Number}.");
            }

            if (worksOrder.PartNumber != part.PartNumber)
            {
                throw new CreateRequisitionException($"Works Order {document1Number} is for a different part.");
            }

            if ((worksOrder.Quantity == 0 || worksOrder.Quantity <= worksOrder.QuantityBuilt) && isReverseTransaction != "Y")
            {
                throw new CreateRequisitionException($"There is nothing left to build on Works Order {document1Number}.");
            }

            if ((worksOrder.Quantity == 0 || worksOrder.QuantityBuilt == 0) && isReverseTransaction == "Y")
            {
                throw new CreateRequisitionException($"Nothing built on Works Order {document1Number} that can be reversed.");
            }

            var salesPart = await this.salesProxy.GetSalesArticle(worksOrder.PartNumber);
            if (salesPart != null && isReverseTransaction != "Y"
                                  && (salesPart.TypeOfSerialNumber == "S" || salesPart.TypeOfSerialNumber == "P1"
                                                                          || salesPart.TypeOfSerialNumber == "P2"))
            {
                throw new CreateRequisitionException("You cannot book serial numbered parts in. Please use the relevant works order screen.");
            }

            if (part.BomVerifyFreqWeeks.GetValueOrDefault() > 0)
            {
                var verifications = await this.bomVerificationProxy.GetBomVerifications(worksOrder.PartNumber);
                if (verifications == null || !verifications.Any())
                {
                    throw new CreateRequisitionException($"Part number {worksOrder.PartNumber} requires bom verification.");
                }

                var latestDate = verifications.Max(a => a.DateVerified);
                var dateDue = latestDate.AddDays(part.BomVerifyFreqWeeks.GetValueOrDefault() * 7);
                if (dateDue < DateTime.Today)
                {
                    throw new CreateRequisitionException($"Part number {worksOrder.PartNumber} was due for bom verification on {dateDue:dd-MMM-yyyy}.");
                }
            }
        }

        private async Task CheckMoves(
            string partNumber,
            IList<MoveSpecification> moves,
            bool toLocationRequired = false)
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

                if (!m.ToPallet.HasValue && string.IsNullOrEmpty(m.ToLocation) && toLocationRequired)
                {
                    throw new RequisitionException("You must provide a to location or pallet");
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

                        var part = await this.partRepository.FindByIdAsync(partNumber);
                        var state = await this.stateRepository.FindByIdAsync(m.ToState);

                        DoProcessResultCheck(await this.storesService.ValidOntoLocation(part, toLocation, null, state));

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

            if (header.Part != null && header.IsReverseTransaction != "Y" && header.StoresFunction?.ToLocationRequired != "N")
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

            if (!string.IsNullOrEmpty(header.ToStockPool))
            {
                DoProcessResultCheck(this.storesService.ValidStockPool(header.Part, stockPool));
            }

            if (header.Part != null && !header.IsReverseTrans())
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
            }
        }
    }
}
