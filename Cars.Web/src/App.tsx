import { Route, BrowserRouter, Routes } from 'react-router-dom'
import MainLayout from './components/MainLayout';
import CarsPage from './pages/CarsPage';
import BodyTypePage from './pages/BodyTypePage';
import BrandPage from './pages/BrandPage';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<MainLayout />}>
          <Route
            element={<CarsPage />}
            path="/"
          />
          <Route
            element={<BodyTypePage />}
            path="/dictionary/body-types"
          />
          <Route
            element={<BrandPage />}
            path="/dictionary/brands"
          />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App
