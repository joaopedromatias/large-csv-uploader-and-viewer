import { create } from 'zustand';

interface AppState {
  // app step
  currentStep: AppSteps;
  setCurrentStep: (f: AppSteps) => void;
  // job id
  jobId: number | null;
  setJobId: (i: number | null) => void;
  // filters
  filters: AppFilters;
  setFilters: (f: Partial<AppFilters>) => void;
}

export const useAppStore = create<AppState>((set) => ({
  // app step
  currentStep: 'upload-file',
  setCurrentStep: (s) => set({ currentStep: s }),
  // job id
  jobId: null,
  setJobId: (i: number | null) => set({ jobId: i }),
  // filters
  filters: { orderKey: 'name', orderDesc: false, page: 0, pageSize: 20 },
  setFilters: (f) =>
    set((state) => ({
      filters: { ...state.filters, ...f },
    })),
}));
