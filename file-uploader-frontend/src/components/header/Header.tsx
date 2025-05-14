import { AppBar, Toolbar, Typography, Box } from '@mui/material';
import CloudUploadIcon from '@mui/icons-material/CloudUpload';

export default function Header() {
  return (
    <AppBar position="static" elevation={1} color="default">
      <Toolbar>
        <CloudUploadIcon sx={{ mr: 1, color: 'primary.main' }} />
        <Typography variant="h6" color="textPrimary" fontWeight="medium">
          File Uploader & Viewer
        </Typography>
        <Box sx={{ flexGrow: 1 }} />
      </Toolbar>
    </AppBar>
  );
}
