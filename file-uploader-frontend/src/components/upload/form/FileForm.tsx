import { UploadFile as UploadFileIcon } from '@mui/icons-material';
import { Button, Paper, Stack, Typography, type AlertColor } from '@mui/material';
import { useState, type ChangeEvent, type FormEvent } from 'react';
import { useAppStore } from '../../state/state';

interface FormFileProps {
  showAlert: (message: string, severity?: AlertColor) => void;
}

export default function FileForm({ showAlert }: FormFileProps) {
  const { setJobId } = useAppStore();
  const [file, setFile] = useState<File | null>(null);

  const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      setFile(e.target.files[0]);
    }
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append('file', file!);

    try {
      const res = await fetch(`${import.meta.env.VITE_API_URL}/UploadFile`, {
        method: 'POST',
        body: formData,
      });

      if (!res.ok) throw new Error(await res.text());

      const data: { jobId: number } = await res.json();

      showAlert(`Upload completed successfully!`, 'success');
      setJobId(data.jobId);
    } catch (err: unknown) {
      showAlert(`Error while uploading: ${(err as Error).message}`, 'error');
    }
  };

  return (
    <Paper elevation={3} sx={{ p: 5, maxWidth: 500, mx: 'auto', mt: 7 }}>
      <form onSubmit={handleSubmit}>
        <Stack spacing={3}>
          <Typography variant="h6">Upload File</Typography>

          <Button variant="outlined" component="label" startIcon={<UploadFileIcon />}>
            {file ? file.name : 'Choose file'}
            <input type="file" accept="text/csv" hidden onChange={handleFileChange} required />
          </Button>

          <Button type="submit" variant="contained" disabled={!file}>
            Upload
          </Button>
        </Stack>
      </form>
    </Paper>
  );
}
