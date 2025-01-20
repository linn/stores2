import React, { useState } from 'react';
import Dialog from '@mui/material/Dialog';
import PropTypes from 'prop-types';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import { InputField } from '@linn-it/linn-form-components-library';

function CancelWithReasonDialog({
    visible,
    title,
    confirmButtonText,
    primaryText,
    secondaryText,
    onConfirm,
    maxWidth,
    onCancel,
    closeDialog
}) {
    const [reason, setReason] = useState(false);
    return (
        <Dialog open={visible} fullWidth maxWidth={maxWidth} onClose={closeDialog}>
            <DialogTitle variant="h4">{title}</DialogTitle>

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

CancelWithReasonDialog.propTypes = {
    visible: PropTypes.bool.isRequired,
    title: PropTypes.string,
    confirmButtonText: PropTypes.string,
    primaryText: PropTypes.string,
    secondaryText: PropTypes.string,
    onConfirm: PropTypes.func.isRequired,
    maxWidth: PropTypes.string,
    onCancel: PropTypes.func,
    closeDialog: PropTypes.func.isRequired
};

CancelWithReasonDialog.defaultProps = {
    title: 'Enter a reason to cancel',
    confirmButtonText: 'Confirm',
    primaryText: 'Enter a reason',
    secondaryText: '',
    maxWidth: 'md',
    onCancel: null
};

export default CancelWithReasonDialog;
