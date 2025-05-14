import type { AlertColor } from '@mui/material';
import Filters from './filters/Filters';
import ProductsTable from './results/ProductTable';

interface SearchProps {
  showAlert: (message: string, severity?: AlertColor) => void;
}

export default function Search({ showAlert }: SearchProps) {
  return (
    <>
      <Filters />
      <ProductsTable showAlert={showAlert} />
    </>
  );
}
