import React from "react";
import { useAuth } from "../hooks/useAuth";
import { Redirect } from "react-router-dom";

const Home = () => {
  const { user } = useAuth();

  const week = 1;
  if (user) {
    return <Redirect to={`/picks/${week}`} />;
  }
  return <Redirect to="/login" />;
};

export default Home;
