import { Link } from 'react-router-dom';
import './Sidebar.css';

/** Компонент бокового меню. Для кастомизации можно передавать пропсами массив */
const Sidebar = () => {
  return (
    <div className="sidebar">
      <Link to="/">Машины</Link>
      <Link to="/dictionary/body-types">Типы кузова</Link>
      <Link to="/dictionary/brands">Бренды</Link>
    </div>
  );
};

export default Sidebar;