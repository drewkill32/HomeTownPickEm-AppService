import React, {useEffect, useState} from "react";
import {Redirect} from "react-router-dom";
import {useAuth} from "../../hooks/useAuth";

const Logout = () => {
  const {signOut} = useAuth();
  const [isSignedOut, setIsSignedOut] = useState(false);
  useEffect(() => {
    signOut();
    setIsSignedOut(true);
  }, [signOut]);
  console.log("signout");
  if (isSignedOut) {
    return <Redirect to="/login"/>;
  } else {
    return <div>Signing out...</div>;
  }
};

export default Logout;
