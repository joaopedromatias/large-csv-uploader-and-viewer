import { TextField, Grid, Box, Switch, FormControlLabel, MenuItem } from '@mui/material';
import { useAppStore } from '../../state/state';

export default function Filters() {
  const { setFilters, filters } = useAppStore();

  const setName = (name: string) => {
    setFilters({ name });
  };

  const setExpiration = (expiration: string) => {
    setFilters({ expiration });
  };

  const setOrderDesc = (orderDesc: boolean) => {
    setFilters({ orderDesc });
  };

  const setOrderKey = (orderKey: OrderKeys) => {
    setFilters({ orderKey });
  };

  return (
    <Box sx={{ p: 2, mb: 2 }}>
      <Grid container spacing={4} justifyContent="center" alignItems="center">
        <Box>
          <TextField
            label="Name"
            variant="outlined"
            value={filters.name}
            onChange={(e) => setName(e.target.value)}
            slotProps={{ inputLabel: { shrink: true } }}
          />
        </Box>

        <Box>
          <TextField
            label="Expiration"
            type="date"
            variant="outlined"
            value={filters.expiration}
            onChange={(e) => setExpiration(e.target.value)}
            slotProps={{ inputLabel: { shrink: true } }}
          />
        </Box>

        <Box>
          <TextField
            label="Order by"
            select
            slotProps={{ inputLabel: { shrink: true } }}
            value={filters.orderKey}
            onChange={(e) => setOrderKey(e.target.value as OrderKeys)}
            sx={{ minWidth: 150 }}
          >
            <MenuItem value="name">Name</MenuItem>
            <MenuItem value="price">Price</MenuItem>
            <MenuItem value="expiration">Expiration</MenuItem>
          </TextField>
        </Box>

        <Box>
          <FormControlLabel
            control={
              <Switch
                checked={filters.orderDesc}
                onChange={(e) => setOrderDesc(e.target.checked)}
                value={filters.orderDesc}
              />
            }
            label={filters.orderDesc ? 'Descending Order' : 'Ascending Order'}
          />
        </Box>
      </Grid>
    </Box>
  );
}
