import React, { useEffect, useState } from 'react';
import {
    DataGrid,
    gridExpandedSortedRowIdsSelector,
    useGridApiRef,
    GridSearchIcon
} from '@mui/x-data-grid';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Grid from '@mui/material/Grid';
import Tooltip from '@mui/material/Tooltip';
import IconButton from '@mui/material/IconButton';
import DeleteIcon from '@mui/icons-material/Delete';
import Link from '@mui/material/Link';
import { makeStyles } from '@mui/styles';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import SnackbarContent from '@mui/material/SnackbarContent';
import Snackbar from '@mui/material/Snackbar';
import { InputField, Search } from '@linn-it/linn-form-components-library';
import itemTypes from '../../../itemTypes';
import useGet from '../../../hooks/useGet';
import useSearch from '../../../hooks/useSearch';

function BookInPostingsDialog({
    open,
    setOpen,
    handleSelect,
    documentType,
    documentNumber,
    documentLine,
    orderDetail,
    existingBookInOrderDetails,
    isReverse = 'N'
}) {
    const [snackbar, setSnackbar] = useState(null);
    const handleCloseSnackbar = () => setSnackbar(null);
    const [bookInOrderDetails, setBookInOrderDetails] = useState(existingBookInOrderDetails ?? []);
    const [searchDialogOpen, setSearchDialogOpen] = useState({ forRow: null, forColumn: null });
    const [departmentSearchTerm, setDepartmentSearchTerm] = useState('');
    const [nominalSearchTerm, setNominalSearchTerm] = useState('');
    const [existingBookIns, setExistingBookIns] = useState([]);
    const [rowSelectionModel, setRowSelectionModel] = useState({
        type: 'include',
        ids: new Set([])
    });

    const apiRef = useGridApiRef();

    const {
        send: searchNominalAccounts,
        isLoading: searchNominalAccountsLoading,
        result: searchNominalAccountsResult,
        clearData: clearNominalAccountsSearch
    } = useGet(itemTypes.nominalAccounts.url);

    const {
        send: getDepartment,
        isLoading: departmentLoading,
        result: departmentValue,
        clearData: clearDepartment
    } = useGet(itemTypes.departments.url);

    const {
        send: getNominal,
        isLoading: nominalLoading,
        result: nominalValue,
        clearData: clearNominal
    } = useGet(itemTypes.nominals.url);

    const useStyles = makeStyles(() => ({
        invalidCode: {
            color: 'black',
            backgroundColor: 'yellow'
        }
    }));
    const classes = useStyles();

    const {
        send: searchBookIns,
        isLoading: searchBookInsLoading,
        result: searchBookInsResult,
        clearData: clearBookInsSearch
    } = useGet(itemTypes.sundryBookInDetails.url);

    useEffect(() => {
        if (open && documentNumber && documentLine) {
            searchBookIns(null, `?orderNumber=${documentNumber}&orderLine=${documentLine}`);
        }
    }, [documentLine, documentNumber, documentType, open, searchBookIns]);

    useEffect(() => {
        if (searchBookInsResult) {
            setExistingBookIns(searchBookInsResult.map((a, i) => ({ ...a, id: i })));
        }
    }, [searchBookInsResult]);

    const setCode = code => {
        if (code?.length && code.length < 10) {
            return code.padStart(10, '0');
        }

        return code;
    };

    const {
        search: searchDepartments,
        results: departmentsSearchResults,
        loading: departmentsSearchLoading
    } = useSearch(itemTypes.departments.url, 'departmentCode', 'departmentCode', 'description');

    const {
        search: searchNominals,
        results: nominalsSearchResults,
        loading: nominalsSearchLoading
    } = useSearch(itemTypes.nominals.url, 'nominalCode', 'nominalCode', 'description');

    const bookedInQuantity = orderDetail.purchaseDeliveries.reduce(
        (a, b) => a + b.qtyNetReceived,
        0
    );

    const searchRenderCell = params => (
        <>
            <GridSearchIcon
                style={{ cursor: 'pointer' }}
                onClick={() =>
                    setSearchDialogOpen({
                        forRow: params.id,
                        forColumn: params.field
                    })
                }
            />
            {params.value}
        </>
    );

    const addBookIn = () => {
        const qtyLeft = orderDetail.ourQty - bookedInQuantity;
        const selectedQty = bookInOrderDetails.reduce((a, b) => a + Number(b.quantity), 0);
        const id = (bookInOrderDetails.length ?? 0) + 1;
        const newBookInOrderDetail = {
            id,
            sequence: id,
            orderNumber: orderDetail.orderNumber,
            orderLine: orderDetail.line,
            partNumber: orderDetail.partNumber,
            quantity: Math.max(qtyLeft - selectedQty, 0),
            departmentCode: orderDetail.orderPosting.nominalAccount.department.departmentCode,
            departmentDescription:
                orderDetail.orderPosting.nominalAccount.department.departmentDescription,
            nominalCode: orderDetail.orderPosting.nominalAccount.nominal.nominalCode,
            nominalDescription: orderDetail.orderPosting.nominalAccount.nominal.description,
            isReverse
        };

        const bookInOrderDetailsToUpdate = [...bookInOrderDetails, newBookInOrderDetail];
        setBookInOrderDetails(bookInOrderDetailsToUpdate);
        searchNominalAccounts(
            null,
            `?departmentCode=${newBookInOrderDetail.departmentCode}&nominalCode=${newBookInOrderDetail.nominalCode}&exactMatch=true`
        );
    };

    const reverseBookIn = () => {
        if (rowSelectionModel?.ids.size === 1) {
            const selectedIndex = rowSelectionModel.ids.values().next().value;
            const id = (bookInOrderDetails.length ?? 0) + 1;
            const bookInToBeReversed = existingBookIns.find(a => a.id == selectedIndex);
            const newBookInOrderDetail = {
                id,
                sequence: id,
                orderNumber: orderDetail.orderNumber,
                orderLine: orderDetail.line,
                partNumber: orderDetail.partNumber,
                quantity: bookInToBeReversed.quantity * -1,
                departmentCode: bookInToBeReversed.departmentCode,
                departmentDescription: null,
                nominalCode: bookInToBeReversed.nominalCode,
                nominalDescription: null,
                isReverse,
                originalReqNumber: bookInToBeReversed.reqNumber
            };

            const bookInOrderDetailsToUpdate = [...bookInOrderDetails, newBookInOrderDetail];
            setBookInOrderDetails(bookInOrderDetailsToUpdate);
            getDepartment(bookInToBeReversed.departmentCode);
            getNominal(bookInToBeReversed.nominalCode);

            searchNominalAccounts(
                null,
                `?departmentCode=${newBookInOrderDetail.departmentCode}&nominalCode=${newBookInOrderDetail.nominalCode}&exactMatch=true`
            );
        }
    };

    const handleClose = () => {
        clearBookInsSearch();
        setOpen(false);
    };

    const handleDeleteRow = () => {
        const bookInOrderDetailsToUpdate = [...bookInOrderDetails];
        setBookInOrderDetails(bookInOrderDetailsToUpdate.slice(0, -1));
    };

    const handleConfirmClick = () => {
        const qtyLeft = orderDetail.ourQty - bookedInQuantity;
        const selectedQty = bookInOrderDetails.reduce((a, b) => a + Number(b.quantity), 0);
        if (Number(qtyLeft) < selectedQty && isReverse !== 'Y') {
            setSnackbar({
                message: `Quantity left on order line is ${qtyLeft} but quantity picked is ${selectedQty}`,
                backgroundColour: 'red'
            });
        } else if (bookedInQuantity < selectedQty * -1 && isReverse === 'Y') {
            setSnackbar({
                message: `Quantity booked on order line is ${bookedInQuantity} but trying to reverse ${selectedQty * -1}`,
                backgroundColour: 'red'
            });
        } else if (
            !bookInOrderDetails.every(
                a => a.departmentCode && a.nominalCode && a.nominalPostsAllowed === 'Y'
            )
        ) {
            setSnackbar({
                message: 'Department / nominal details are incomplete or invalid',
                backgroundColour: 'red'
            });
        } else {
            handleClose();
            handleSelect({ bookInOrderDetails, quantityBooked: selectedQty });
        }
    };

    useEffect(() => {
        if (departmentValue && bookInOrderDetails?.length) {
            const newBookInOrderDetails = [...bookInOrderDetails];
            const items = newBookInOrderDetails.filter(
                a => a.departmentCode === departmentValue.departmentCode
            );
            items.forEach(item => {
                const current = newBookInOrderDetails.find(a => a.id === item.id);
                current.departmentDescription = departmentValue.description;
            });

            clearDepartment();
            setBookInOrderDetails(newBookInOrderDetails);
        }
    }, [clearDepartment, departmentValue, bookInOrderDetails]);

    useEffect(() => {
        if (nominalValue && bookInOrderDetails?.length) {
            const newBookInOrderDetail = [...bookInOrderDetails];
            const items = newBookInOrderDetail.filter(
                a => a.nominalCode === nominalValue.nominalCode
            );
            items.forEach(item => {
                const current = newBookInOrderDetail.find(a => a.id === item.id);
                current.nominalDescription = nominalValue.description;
            });

            clearNominal();
            setBookInOrderDetails(newBookInOrderDetail);
        }
    }, [clearNominal, nominalValue, bookInOrderDetails]);

    useEffect(() => {
        if (searchNominalAccountsResult?.length && bookInOrderDetails?.length) {
            const currentResult = searchNominalAccountsResult[0];
            const newBookInOrderDetail = [...bookInOrderDetails];
            const items = newBookInOrderDetail.filter(
                a =>
                    a.nominalCode === currentResult?.nominal?.nominalCode &&
                    a.departmentCode === currentResult?.department?.departmentCode
            );
            items.forEach(item => {
                const current = newBookInOrderDetail.find(a => a.id === item.id);
                current.nominalPostsAllowed = currentResult.nominalPostsAllowed;
            });

            clearNominalAccountsSearch();
            setBookInOrderDetails(newBookInOrderDetail);
        }
    }, [clearNominalAccountsSearch, searchNominalAccountsResult, bookInOrderDetails]);

    const renderSearchDialog = c => {
        const handleClose = () => {
            setSearchDialogOpen({ forRow: null, forColumn: null });
        };

        const handleSearchResultSelect = selected => {
            const currentRow = bookInOrderDetails.find(r => r.id === searchDialogOpen.forRow);
            let newRow = {
                ...currentRow,
                [c.field]: selected.id
            };

            c.searchUpdateFieldNames?.forEach(f => {
                newRow = { ...newRow, [f.fieldName]: selected[f.searchResultFieldName] };
            });

            processRowUpdate(newRow);
            setSearchDialogOpen({ forRow: null, forColumn: null });
        };
        return (
            <div id={c.field} key={c.field}>
                <Dialog open={searchDialogOpen.forColumn === c.field} onClose={handleClose}>
                    <DialogTitle>Search</DialogTitle>
                    <DialogContent>
                        <Search
                            autoFocus
                            propertyName={`${c.field}-searchTerm`}
                            label=""
                            resultsInModal
                            resultLimit={100}
                            value={`${c.searchTerm}`}
                            handleValueChange={(_, newVal) => {
                                c.setSearchTerm(newVal);
                            }}
                            search={c.search}
                            searchResults={c.searchResults}
                            loading={c.searchLoading}
                            priorityFunction="closestMatchesFirst"
                            onResultSelect={handleSearchResultSelect}
                            clearSearch={() => {}}
                        />
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={handleClose}>Close</Button>
                    </DialogActions>
                </Dialog>
            </div>
        );
    };

    const updateNewRow = row => ({
        ...row,
        departmentCode: setCode(row.departmentCode),
        nominalCode: setCode(row.nominalCode)
    });

    const processRowUpdate = (newRow, oldRow) => {
        const updatedNewRow = updateNewRow(newRow);

        if (
            updatedNewRow.nominalCode?.length === 10 &&
            updatedNewRow.nominalCode !== oldRow?.nominalCode
        ) {
            updatedNewRow.nominalDescription = null;
            getNominal(updatedNewRow.nominalCode);
        }

        if (
            updatedNewRow.departmentCode?.length === 10 &&
            updatedNewRow.departmentCode !== oldRow?.departmentCode
        ) {
            updatedNewRow.departmentDescription = null;
            getDepartment(setCode(updatedNewRow.departmentCode));
        }

        if (
            updatedNewRow.departmentCode?.length === 10 &&
            updatedNewRow.nominalCode?.length === 10 &&
            (updatedNewRow.nominalCode !== oldRow?.nominalCode ||
                updatedNewRow.departmentCode !== oldRow?.departmentCode)
        ) {
            updatedNewRow.nominalPostsAllowed = null;
            searchNominalAccounts(
                null,
                `?departmentCode=${updatedNewRow.departmentCode}&nominalCode=${updatedNewRow.nominalCode}&exactMatch=true`
            );
        }

        if (newRow.comments) {
            updatedNewRow.comments = newRow.comments.toUpperCase();
        }

        setBookInOrderDetails(r => r.map(s => (s.id === newRow.id ? updatedNewRow : s)));

        return { ...newRow };
    };

    const orderDetailPostingColumns = [
        {
            field: 'qty',
            headerName: 'Qty',
            width: 110
        },
        {
            field: 'department',
            headerName: 'Department',
            width: 110
        },
        {
            field: 'departmentDescription',
            headerName: 'Desc',
            width: 210
        },
        {
            field: 'nominal',
            headerName: 'Nominal',
            width: 110
        },
        {
            field: 'nominalDescription',
            headerName: 'Desc',
            width: 210
        }
    ];

    const existingBookInColumns = [
        {
            field: 'orderNumber',
            headerName: 'Order',
            width: 110
        },
        {
            field: 'orderLine',
            headerName: 'Line',
            width: 100
        },
        {
            field: 'quantity',
            headerName: 'Qty',
            width: 110
        },
        {
            field: 'departmentCode',
            headerName: 'Dept',
            width: 110
        },
        {
            field: 'nominalCode',
            headerName: 'Nominal',
            width: 110
        },
        {
            field: 'transactionReference',
            headerName: 'Reference',
            width: 190
        }
    ];

    const handleRowSelection = rowSelectionModel => {
        setRowSelectionModel(rowSelectionModel);
    };

    const getCodeClass = (field, row) => {
        if (
            field === 'departmentCode' &&
            row.departmentCode?.length &&
            !row.departmentDescription?.length
        ) {
            return classes.invalidCode;
        }

        if (field === 'nominalCode' && row.nominalCode?.length && !row.nominalDescription?.length) {
            return classes.invalidCode;
        }

        if (
            row.nominalCode?.length &&
            row.departmentCode?.length &&
            row.nominalPostsAllowed !== 'Y'
        ) {
            return classes.invalidCode;
        }

        return null;
    };

    const bookInOrderDetailsColumns = [
        { field: 'quantity', headerName: 'Qty', editable: true, width: 160 },
        {
            field: 'departmentCode',
            headerName: 'Department',
            width: 150,
            editable: true,
            type: 'search',
            searchTerm: departmentSearchTerm,
            setSearchTerm: setDepartmentSearchTerm,
            search: searchDepartments,
            searchResults: departmentsSearchResults,
            searchLoading: departmentsSearchLoading,
            searchUpdateFieldNames: [
                { fieldName: 'departmentDescription', searchResultFieldName: 'description' },
                { fieldName: 'departmentCode', searchResultFieldName: 'departmentCode' }
            ],
            cellClassName: params => getCodeClass('departmentCode', params.row),
            renderCell: searchRenderCell
        },
        { field: 'departmentDescription', headerName: 'Description', width: 200 },
        {
            field: 'nominalCode',
            headerName: 'Nominal',
            width: 150,
            editable: true,
            type: 'search',
            searchTerm: nominalSearchTerm,
            setSearchTerm: setNominalSearchTerm,
            search: searchNominals,
            searchResults: nominalsSearchResults,
            searchLoading: nominalsSearchLoading,
            searchUpdateFieldNames: [
                { fieldName: 'nominalDescription', searchResultFieldName: 'description' },
                { fieldName: 'nominalCode', searchResultFieldName: 'nominalCode' }
            ],
            cellClassName: params => getCodeClass('nominalCode', params.row),
            renderCell: searchRenderCell
        },
        { field: 'nominalDescription', headerName: 'Nominal Name', width: 200 },
        {
            field: 'delete',
            headerName: '',
            width: 120,
            renderCell: params =>
                Number(params.row.id) === bookInOrderDetails.length ? (
                    <Tooltip title="Delete">
                        <div>
                            <IconButton aria-label="delete" size="small" onClick={handleDeleteRow}>
                                <DeleteIcon fontSize="inherit" />
                            </IconButton>
                        </div>
                    </Tooltip>
                ) : (
                    ''
                )
        }
    ];

    const handleCellKeyDown = (params, event) => {
        if (event.keyCode === 13) {
            if (params.colDef.type === 'search') {
                setSearchDialogOpen({
                    forRow: params.id,
                    forColumn: params.field
                });
                apiRef.current.stopCellEditMode({ id: params.id, field: params.field });
            } else {
                return;
            }
        }

        if (event.key !== 'Tab') {
            return;
        }

        const rowIds = gridExpandedSortedRowIdsSelector(apiRef.current.state);
        const visibleColumns = apiRef.current.getVisibleColumns();
        const nextCell = {
            rowIndex: rowIds.findIndex(id => id === params.id),
            colIndex: apiRef.current.getColumnIndex(params.field)
        };

        if (
            nextCell.colIndex === visibleColumns.length - 1 &&
            nextCell.rowIndex === rowIds.length - 1 &&
            !event.shiftKey
        ) {
            // Do nothing if we are at the last cell of the last row
            return;
        }

        if (nextCell.colIndex === 0 && nextCell.rowIndex === 0 && event.shiftKey) {
            // Do nothing if we are at the first cell of the first row
            return;
        }

        event.preventDefault();

        if (!event.shiftKey) {
            if (nextCell.colIndex < visibleColumns.length - 1) {
                nextCell.colIndex += 1;
            } else {
                nextCell.rowIndex += 1;
                nextCell.colIndex = 0;
            }
        } else if (nextCell.colIndex > 0) {
            nextCell.colIndex -= 1;
        } else {
            nextCell.rowIndex -= 1;
            nextCell.colIndex = visibleColumns.length - 1;
        }
        apiRef.current.scrollToIndexes(nextCell);

        const { field } = visibleColumns[nextCell.colIndex];
        const id = rowIds[nextCell.rowIndex];
        apiRef.current.setCellFocus(id, field);
    };

    const orderDetailPostingRows = [
        {
            ...orderDetail.orderPosting,
            id: orderDetail.orderPosting.lineNumber,
            department: orderDetail.orderPosting.nominalAccount.department.departmentCode,
            departmentDescription:
                orderDetail.orderPosting.nominalAccount.department.departmentDescription,
            nominal: orderDetail.orderPosting.nominalAccount.nominal.nominalCode,
            nominalDescription: orderDetail.orderPosting.nominalAccount.nominal.description
        }
    ];
    return (
        <Dialog open={open} onClose={handleClose} fullWidth maxWidth="xl">
            <DialogTitle color={isReverse === 'Y' ? 'warning' : 'black'}>
                Book In Details for {documentType} {documentNumber} / {documentLine}{' '}
                {isReverse === 'Y' ? '[REVERSE]' : ''}
            </DialogTitle>
            <DialogContent>
                {bookInOrderDetailsColumns
                    .filter(c => c.type === 'search')
                    .map(c => renderSearchDialog(c))}
                <Grid container spacing={3}>
                    <Grid size={2}>
                        <InputField
                            fullWidth
                            value={orderDetail.orderNumber}
                            onChange={() => {}}
                            label="Order Number"
                            disabled
                            propertyName="orderNumber"
                        />
                    </Grid>
                    <Grid size={1}>
                        <InputField
                            fullWidth
                            value={orderDetail.line}
                            onChange={() => {}}
                            label="Line"
                            disabled
                            propertyName="line"
                        />
                    </Grid>
                    <Grid size={1}>
                        <InputField
                            fullWidth
                            value={orderDetail.ourQty}
                            onChange={() => {}}
                            label="Qty"
                            disabled
                            propertyName="ourQty"
                        />
                    </Grid>
                    <Grid size={2}>
                        <InputField
                            fullWidth
                            value={bookedInQuantity}
                            onChange={() => {}}
                            label="Booked In Qty"
                            disabled
                            propertyName="bookedInQuantity"
                        />
                    </Grid>
                    <Grid size={2}>
                        <InputField
                            fullWidth
                            value={orderDetail.partNumber}
                            onChange={() => {}}
                            label="Part"
                            disabled
                            propertyName="partNumber"
                        />
                    </Grid>
                    <Grid size={4}>
                        <InputField
                            fullWidth
                            value={orderDetail.partDescription}
                            onChange={() => {}}
                            label="Description"
                            disabled
                            propertyName="partDescription"
                        />
                    </Grid>
                    <Grid size={9}>
                        <DataGrid
                            rows={orderDetailPostingRows ?? []}
                            columns={orderDetailPostingColumns}
                            hideFooter
                            density="compact"
                        />
                    </Grid>
                    <Grid size={3}>
                        <Link
                            target={'_blank'}
                            rel={'noopener noreferrer'}
                            variant="body1"
                            underline="hover"
                            href={`/ledgers/purchase/inv-post?orderNumber=${orderDetail.orderNumber}`}
                            color="secondary"
                        >
                            View Receipts/Invoices
                        </Link>
                    </Grid>
                    <Grid size={12}>
                        <Typography variant="h6">Existing Book Ins</Typography>
                    </Grid>
                    <Grid size={6}>
                        <DataGrid
                            rows={existingBookIns ?? []}
                            columns={existingBookInColumns}
                            hideFooter
                            density="compact"
                            disableMultipleRowSelection
                            rowSelectionModel={rowSelectionModel}
                            onRowSelectionModelChange={newRowSelectionModel => {
                                handleRowSelection(newRowSelectionModel);
                            }}
                            isRowSelectable={params => params.row.quantity > 0}
                            loading={searchBookInsLoading}
                            checkboxSelection
                            initialState={{
                                columns: {
                                    columnVisibilityModel: {
                                        orderNumber: false,
                                        orderLine: false
                                    }
                                }
                            }}
                        />
                    </Grid>
                    <Grid size={6} />
                    <Grid size={2}>
                        <Button disabled={isReverse === 'Y'} onClick={addBookIn}>
                            Add Book In
                        </Button>
                    </Grid>
                    <Grid size={2} />
                    <Grid size={2}>
                        <Button
                            disabled={isReverse === 'N' || !rowSelectionModel?.ids.size > 0}
                            onClick={reverseBookIn}
                        >
                            Reverse Selected
                        </Button>
                    </Grid>
                    <Grid size={6} />
                    <Grid size={12}>
                        <DataGrid
                            rows={bookInOrderDetails ?? []}
                            columns={bookInOrderDetailsColumns}
                            density="compact"
                            onCellKeyDown={handleCellKeyDown}
                            apiRef={apiRef}
                            processRowUpdate={processRowUpdate}
                            loading={
                                departmentsSearchLoading ||
                                nominalsSearchLoading ||
                                nominalLoading ||
                                departmentLoading ||
                                searchNominalAccountsLoading
                            }
                            hideFooterSelectedRowCount
                            hideFooter
                        />
                    </Grid>
                </Grid>
                {!!snackbar && (
                    <Snackbar
                        open
                        autoHideDuration={6000}
                        onClose={handleCloseSnackbar}
                        anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
                    >
                        <SnackbarContent
                            style={{
                                backgroundColor: snackbar.backgroundColour,
                                color: 'black'
                            }}
                            message={snackbar.message}
                        />
                    </Snackbar>
                )}
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={() => {
                        handleConfirmClick();
                    }}
                >
                    Confirm
                </Button>
            </DialogActions>
        </Dialog>
    );
}

export default BookInPostingsDialog;
