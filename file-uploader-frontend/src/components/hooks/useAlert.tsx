import { useState } from 'react';
import { Snackbar, Alert, type AlertColor } from '@mui/material';

export function useAlert() {
  const initialState = {
    open: false,
    message: '',
    severity: 'info',
  } as const;

  const [state, setState] = useState<{
    open: boolean;
    message: string;
    severity: AlertColor;
  }>(initialState);

  const showAlert = (message: string, severity: AlertColor = 'info') => {
    setState({ open: true, message, severity });
  };

  const handleClose = () => {
    setState((prev) => ({ ...prev, open: false }));
  };

  const AlertComponent = (
    <Snackbar
      open={state.open}
      autoHideDuration={4000}
      onClose={handleClose}
      anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
    >
      <Alert
        severity={state.severity}
        variant="filled"
        onClose={handleClose}
        sx={{ width: '100%' }}
      >
        {state.message}
      </Alert>
    </Snackbar>
  );

  return {
    showAlert,
    AlertComponent,
  };
}
