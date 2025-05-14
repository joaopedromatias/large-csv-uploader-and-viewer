import { LinearProgress, Paper, Stack, Typography, type AlertColor } from '@mui/material';
import { useEffect, useState } from 'react';
import { useAppStore } from '../../state/state';

interface ProgressProps {
  jobId: number;
  showAlert: (message: string, severity?: AlertColor) => void;
}

export default function Progress({ jobId, showAlert }: ProgressProps) {
  const { setCurrentStep } = useAppStore();
  const [progress, setProgress] = useState(0);

  useEffect(() => {
    const url = `${import.meta.env.VITE_API_URL}/JobProgress/Stream?jobId=${jobId}`;
    const evtSource = new EventSource(url);

    evtSource.addEventListener('message', (e) => {
      const value = parseInt(e.data);
      setProgress((prev) => (value > prev ? value : prev));
    });

    evtSource.addEventListener('done', () => {
      setProgress(100);
      evtSource.close();

      setTimeout(() => {
        showAlert('Process completed sucessfully', 'success');
        setCurrentStep('ready');
      }, 1000);
    });

    evtSource.onerror = () => {
      showAlert('Connection error', 'error');
      evtSource.close();
    };

    return () => {
      evtSource.close();
    };
  }, [jobId]);

  return (
    <Paper elevation={3} sx={{ p: 5, maxWidth: 500, mx: 'auto', mt: 7 }}>
      <Stack spacing={3}>
        <Typography variant="h6">Processing File</Typography>
        <LinearProgress
          variant="determinate"
          value={progress}
          sx={{
            height: 10,
            borderRadius: 2,
            '& .MuiLinearProgress-bar': {
              transition: 'width 0.5s ease-in-out',
            },
          }}
        />
        <Typography variant="body2" align="center">
          {Math.round(progress)}%
        </Typography>
      </Stack>
    </Paper>
  );
}
