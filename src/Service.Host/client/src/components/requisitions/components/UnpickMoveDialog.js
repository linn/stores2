import React, { useState } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { ErrorCard, InputField } from '@linn-it/linn-form-components-library';

function UnpickMoveDialog({
    visible,
    warningText = null,
    move = null,
    doUnpick,
    maxWidth = 'md',
    onCancel,
    closeDialog
}) {
    const [unPickQty, setUnPickQty] = useState(false);
    return (
        <Dialog open={visible} fullWidth maxWidth={maxWidth} onClose={closeDialog}>
            <DialogTitle variant="h4">Unpick Req Move</DialogTitle>

            {warningText && (
                <DialogContent dividers>
                    <ErrorCard errorMessage={warningText} />
                </DialogContent>
            )}
            <DialogContent dividers>
                <Typography variant="body2">
                    Unpick up to {move.qty} {move.part} from{' '}
                    {move.fromLocationCode ? move.fromLocationCode : move.fromPalletNumber}
                </Typography>
            </DialogContent>
            <DialogContent dividers>
                <InputField
                    label="Qty to UnPick"
                    type="number"
                    value={unPickQty}
                    onChange={(_, newVal) => {
                        setUnPickQty(newVal);
                    }}
                    propertyName="reason"
                />
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={() => {
                        closeDialog();
                        onCancel?.();
                    }}
                    variant="outlined"
                >
                    Back
                </Button>
                <Button
                    disabled={!unPickQty}
                    variant="contained"
                    onClick={() => {
                        closeDialog();
                        doUnpick(unPickQty, move);
                    }}
                >
                    UnPick
                </Button>
            </DialogActions>
        </Dialog>
    );
}

export default UnpickMoveDialog;
