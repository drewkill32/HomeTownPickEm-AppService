import { Outlet } from 'react-router-dom';

import { RequireAuth } from '../features/authentication';

const Home = () => {
  return (
    <RequireAuth>
      <Outlet />
    </RequireAuth>
  );
};

export default Home;
