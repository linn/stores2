import React, { useEffect, useState, useRef } from 'react';
import Button from '@mui/material/Button';
import Grid from '@mui/material/Grid';
import { useNavigate } from 'react-router-dom';
import { InputField } from '@linn-it/linn-form-components-library';

function SearchTab() {
    const navigate = useNavigate();
    const inputRef = useRef(null);

    const [options, setOptions] = useState({});

    const handleOptionChange = (property, newValue) => {
        if (newValue) {
            setOptions(o => ({ ...o, [property]: newValue }));
        } else {
            const opt = { ...options };
            delete opt[property];
            setOptions(() => opt);
        }
    };

    const goToImportBook = () => {
        navigate(`/stores2/import-books/${options.importBookId}`);
    };

    useEffect(() => {
        if (inputRef.current) {
            inputRef.current.focus();
        }
    }, []);

    return (
        <>
            <Grid container spacing={3}>
                <Grid size={3}>
                    <InputField
                        fullWidth
                        value={options.importBookId}
                        type="number"
                        onChange={handleOptionChange}
                        label="Import Book Id"
                        helperText="if you know the Import Book Id"
                        propertyName="importBookId"
                        autoFocus
                        ref={inputRef}
                    />
                </Grid>
                <Grid size={1}>
                    <Button
                        onClick={goToImportBook}
                        variant="outlined"
                        style={{ marginTop: '29px' }}
                    >
                        Go
                    </Button>
                </Grid>
            </Grid>
        </>
    );
}

export default SearchTab;
