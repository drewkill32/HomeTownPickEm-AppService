import { Outlet } from 'react-router-dom';

import { RequireAuth } from '../features/authentication';

export const Unauthorized = () => {
  return <div>Unauthorized</div>;
};
