import React from 'react';
import { useAuth } from '../hooks/useAuth';
import { Redirect } from 'react-router-dom';

const Home = () => {
  const { user } = useAuth();

  if (user) {
    if (user.leagues.length == 1) {
      const [league, season] = user.leagues[0].split(':');
      return <Redirect to={`/${league}/${season}/leaderboard`} />;
    }
    //todo: navigate to league selection page
  }
  return <Redirect to="/login" />;
};

export default Home;
