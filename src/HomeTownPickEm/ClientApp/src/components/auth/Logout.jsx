import React, { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../../features/authentication';

const Logout = () => {
  const { signOut } = useAuth();
  const [isSignedOut, setIsSignedOut] = useState(false);
  useEffect(() => {
    signOut();
    setIsSignedOut(true);
  }, [signOut]);
  console.log('signout');
  if (isSignedOut) {
    return <Navigate to="/login" />;
  } else {
    return <div>Signing out...</div>;
  }
};

export default Logout;
