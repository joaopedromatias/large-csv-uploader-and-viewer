import { useEffect, useState } from 'react';
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  TablePagination,
  type AlertColor,
  Box,
  CircularProgress,
} from '@mui/material';
import { useDebounce } from '../../hooks/useDebounce';
import { useAppStore } from '../../state/state';
import { formatDate } from '../../utils/formatDate';

interface ProductsTableProps {
  showAlert: (message: string, severity?: AlertColor) => void;
}

export default function ProductsTable({ showAlert }: ProductsTableProps) {
  const { filters, setFilters } = useAppStore();
  const searchFilters = useDebounce(filters, 1000);

  const [isLoading, setIsLoading] = useState(false);
  const [products, setProducts] = useState<Product[]>([]);

  useEffect(() => {
    const parameters = new URLSearchParams();
    if (searchFilters.name) parameters.set('name', searchFilters.name);
    if (searchFilters.expiration) parameters.set('expiration', searchFilters.expiration);
    parameters.set('orderKey', searchFilters.orderKey);
    parameters.set('orderDesc', String(searchFilters.orderDesc));
    parameters.set('page', String(searchFilters.page));
    parameters.set('pageSize', String(searchFilters.pageSize));

    fetch(`${import.meta.env.VITE_API_URL}/Product/Search?` + parameters.toString())
      .then((res) => res.json())
      .then((data) => {
        setProducts(data.products);
      })
      .catch((err) => showAlert(err.message, 'error'))
      .finally(() => setIsLoading(false));
  }, [searchFilters]);

  useEffect(() => {
    setIsLoading(true);
  }, [filters]);

  const handleChangePage = (_: unknown, newPage: number) => {
    setFilters({ page: newPage });
  };

  const handleChangePageSize = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newSize = Number(event.target.value);
    setFilters({ pageSize: newSize, page: 0 });
  };

  if (isLoading)
    return (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="top"
        sx={{ mt: '150px' }}
        height="100vh"
      >
        <CircularProgress />
      </Box>
    );

  return (
    <Box sx={{ maxWidth: '90%', mx: 'auto' }}>
      <TableContainer component={Paper}>
        <Table>
          <TableHead sx={{ backgroundColor: '#2784cc' }}>
            <TableRow>
              <TableCell sx={{ color: 'white', fontWeight: 'bold' }}>Id</TableCell>
              <TableCell sx={{ color: 'white', fontWeight: 'bold' }}>Name</TableCell>
              <TableCell sx={{ color: 'white', fontWeight: 'bold' }}>Expiration</TableCell>
              <TableCell sx={{ color: 'white', fontWeight: 'bold' }}>Price (USD)</TableCell>
              <TableCell sx={{ color: 'white', fontWeight: 'bold' }}>Price (BRL)</TableCell>
              <TableCell sx={{ color: 'white', fontWeight: 'bold' }}>Price (EUR)</TableCell>
              <TableCell sx={{ color: 'white', fontWeight: 'bold' }}>Price (GBP)</TableCell>
              <TableCell sx={{ color: 'white', fontWeight: 'bold' }}>Price (JPY)</TableCell>
              <TableCell sx={{ color: 'white', fontWeight: 'bold' }}>Price (ARS)</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {products.map((product, index) => (
              <TableRow
                key={index}
                sx={{
                  backgroundColor: index % 2 === 0 ? '#f5f5f5' : '#ffffff',
                  '&:hover': {
                    backgroundColor: '#e0e0e0',
                  },
                }}
              >
                <TableCell>{product.id}</TableCell>
                <TableCell>{product.name}</TableCell>
                <TableCell>{formatDate(product.expiration)}</TableCell>
                <TableCell>$ {product.priceInUsd.toLocaleString('en-US')}</TableCell>
                <TableCell>R$ {product.priceInBrl.toLocaleString('en-US')}</TableCell>
                <TableCell>€ {product.priceInEur.toLocaleString('en-US')}</TableCell>
                <TableCell>£ {product.priceInGbp.toLocaleString('en-US')}</TableCell>
                <TableCell>￥ {product.priceInJpy.toLocaleString('en-US')}</TableCell>
                <TableCell>$ {product.priceInArs.toLocaleString('en-US')}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <TablePagination
        component="div"
        count={-1}
        page={filters.page}
        onPageChange={handleChangePage}
        rowsPerPage={filters.pageSize}
        onRowsPerPageChange={handleChangePageSize}
        rowsPerPageOptions={[20, 50, 100]}
        labelRowsPerPage="Items per page"
      />
    </Box>
  );
}
