import { Outlet } from 'react-router-dom';
import Sidebar from './SideBar/Sidebar';

const MainLayout = () => {
    return (
        <div>
            <Sidebar />
            <div style={{ padding: '20px' }}>
                <Outlet />
            </div>
        </div>
    );
};

export default MainLayout;