type Product = {
  id: number;
  name: string;
  expiration: string;
  priceInUsd: number;
  priceInBrl: number;
  priceInEur: number;
  priceInJpy: number;
  priceInGbp: number;
  priceInArs: number;
};

type AppSteps = 'upload-file' | 'ready';

type OrderKeys = 'name' | 'price' | 'expiration';

type AppFilters = {
  name?: string;
  expiration?: string;
  orderKey: OrderKeys;
  orderDesc: boolean;
  page: number;
  pageSize: number;
};
