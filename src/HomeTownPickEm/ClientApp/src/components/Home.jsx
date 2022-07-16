import React from 'react';
import { useAuth } from '../hooks/useAuth';
import { Redirect } from 'react-router-dom';

const Home = () => {
  const { user } = useAuth();

  if (user) {
    return <Redirect to="/leaderboard" />;
  }
  return <Redirect to="/login" />;
};

export default Home;
