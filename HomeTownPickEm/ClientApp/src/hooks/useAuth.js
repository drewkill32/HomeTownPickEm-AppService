import React, {createContext, useState} from "react";
import Cookies from "js-cookie";

const getUserCookie = () => {
  const userJson = Cookies.get("user");
  if (userJson) {
    return JSON.parse(userJson);
  }
  return null;
};

const setUserCookie = (user) => {
  if (user) {
    Cookies.set("user", JSON.stringify({id: user}), {expires: 30});
  } else {
    Cookies.remove("user");
  }
};

const signOut = () => {
  Cookies.remove("user");
};

const signIn = async (email, password) => {
  return null;
};
const useProviderAuth = () => {
  const [user, setUser] = useState(getUserCookie());

  return {
    user,
    signIn: signIn,
    signOut: signOut,
    register: register,
  };
};

export const AuthContext = createContext({
  user: null,
  signIn: () => Promise.resolve(null),
  signOut: () => {
  },
  register: () => Promise.resolve(null),
  forgotPassword: () => Promise.resolve(),
});

const ProviderAuth = ({children}) => {
  const auth = useProviderAuth();

  return <AuthContext.Provider value={auth}>{children}</AuthContext.Provider>;
};
