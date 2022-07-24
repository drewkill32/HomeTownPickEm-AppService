import { Outlet } from 'react-router-dom';

import { RequireAuth, useAuth } from '../features/authentication';

const Home = () => {
  const { getUser } = useAuth();
  const { data: user, isLoading } = getUser();

  if (isLoading || !user) {
    return null;
  }
  return (
    <RequireAuth>
      <Outlet />
    </RequireAuth>
  );
};

export default Home;
