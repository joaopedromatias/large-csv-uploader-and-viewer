import Header from './components/header/Header';
import { useAlert } from './components/hooks/useAlert';
import Search from './components/products/Search';
import { useAppStore } from './components/state/state';
import Upload from './components/upload/Upload';

function App() {
  const { currentStep } = useAppStore();
  const { AlertComponent, showAlert } = useAlert();

  return (
    <>
      <Header />
      {currentStep == 'upload-file' && <Upload showAlert={showAlert} />}
      {currentStep == 'ready' && <Search showAlert={showAlert} />}
      {AlertComponent}
    </>
  );
}

export default App;
