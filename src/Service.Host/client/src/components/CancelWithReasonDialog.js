import React, { useState } from 'react';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import { ErrorCard, InputField } from '@linn-it/linn-form-components-library';

function CancelWithReasonDialog({
    visible,
    title = 'Enter a reason to cancel',
    confirmButtonText = 'Confirm',
    primaryText = 'Enter a reason',
    secondaryText = '',
    warningText = null,
    onConfirm,
    maxWidth = 'md',
    onCancel,
    closeDialog
}) {
    const [reason, setReason] = useState(false);
    return (
        <Dialog open={visible} fullWidth maxWidth={maxWidth} onClose={closeDialog}>
            <DialogTitle variant="h4">{title}</DialogTitle>

            {warningText && (
                <DialogContent dividers>
                    <ErrorCard errorMessage={warningText} />
                </DialogContent>
            )}

            <DialogContent dividers>
                <InputField
                    fullWidth
                    label={primaryText}
                    helperText={secondaryText}
                    value={reason}
                    onChange={(_, newVal) => {
                        setReason(newVal);
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
                    disabled={!reason}
                    variant="contained"
                    onClick={() => {
                        closeDialog();
                        onConfirm(reason);
                    }}
                >
                    {confirmButtonText}
                </Button>
            </DialogActions>
        </Dialog>
    );
}

export default CancelWithReasonDialog;
