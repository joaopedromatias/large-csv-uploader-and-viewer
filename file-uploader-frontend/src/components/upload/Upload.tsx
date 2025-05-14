import type { AlertColor } from '@mui/material';
import FileForm from './form/FileForm';
import { useAppStore } from '../state/state';
import Progress from './progress/Progress';

interface UploadProps {
  showAlert: (message: string, severity?: AlertColor) => void;
}

export default function Upload({ showAlert }: UploadProps) {
  const { jobId } = useAppStore();
  return (
    <>
      <FileForm showAlert={showAlert} />
      {jobId != null && <Progress jobId={jobId} showAlert={showAlert} />}
    </>
  );
}
