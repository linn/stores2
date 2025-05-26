import React from 'react';
import { GridOverlay, useGridApiContext } from '@mui/x-data-grid';

import Box from '@mui/material/Box';
import { useTheme } from '@mui/material';
import LinearProgress from '@mui/material/LinearProgress';
import Skeleton from '@mui/material/Skeleton';

function CustomLoadingOverlay() {
    const apiRef = useGridApiContext();
    const theme = useTheme();
    const visibleRowCount = apiRef.current.getRowsCount();

    const showSkeletonRows = visibleRowCount === 0;

    return (
        <GridOverlay sx={{ width: '100%', position: 'relative' }}>
            <Box
                sx={{
                    position: 'absolute',
                    top: 0,
                    left: 0,
                    width: '100%'
                }}
            >
                <LinearProgress />
            </Box>

            {showSkeletonRows && (
                <Box
                    sx={{
                        mt: 6,
                        width: '100%',
                        px: 2,
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 1
                    }}
                >
                    {Array.from({ length: 8 }).map((_, idx) => (
                        <Skeleton
                            key={idx}
                            variant="rectangular"
                            height={36}
                            sx={{
                                borderRadius: theme.shape.borderRadius,
                                width: '100%'
                            }}
                        />
                    ))}
                </Box>
            )}
        </GridOverlay>
    );
}

export default CustomLoadingOverlay;
