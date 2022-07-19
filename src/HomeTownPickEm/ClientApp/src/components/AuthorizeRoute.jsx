import React from 'react';
import { Navigate, Route, useLocation } from 'react-router-dom';
import { useAuth } from '../features/authentication';

const AuthorizeRoute = ({ children }) => {
  const { user } = useAuth();
  let location = useLocation();
  if (!user) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }
  return children;
};

export default AuthorizeRoute;
