import React, { createContext, useContext } from 'react';
import Cookies from 'js-cookie';
import axios from 'axios';

const getUserCookie = () => {
  const userJson = Cookies.get('user');
  if (userJson) {
    return JSON.parse(userJson);
  }
  return null;
};

export interface User {
  token: string;
  leagues: [string];
  firstName: string;
  lastName: string;
  team: {
    school: string;
    mascot: string;
    color: string;
    abbreviation: string;
    logo: string;
  };
}

export interface AuthContextProps {
  user: User | null;
  signIn: (userName: string, password: string) => Promise<User | null>;
  signOut: () => void;
  register: (user: User) => Promise<User | null>;
  forgotPassword: (email: string) => Promise<void>;
  getToken: () => string | null;
}

const setUserCookie = (user: User | null) => {
  if (user) {
    Cookies.set('user', JSON.stringify(user), { expires: 30 });
  } else {
    Cookies.remove('user');
  }
};

const AuthContext = createContext<AuthContextProps>({
  user: null,
  signIn: () => Promise.resolve(null),
  signOut: () => {},
  register: () => Promise.resolve(null),
  forgotPassword: () => Promise.resolve(),
  getToken: () => null,
});

export const useAuth = () => {
  return useContext(AuthContext);
};

const register = async (user: User) => {
  try {
    var res = await axios.post('api/user/register', user);
    return res.data;
  } catch (error) {
    throw error;
  }
};
const useProviderAuth = (): AuthContextProps => {
  const [user, setUser] = React.useState<User | null>(getUserCookie());

  const signIn = async (email: string, password: string) => {
    try {
      var res = await axios.post('api/user/login', {
        email: email,
        password: password,
      });
      var user = res.data as User;
      setUserCookie(user);
      setUser(user);
      return user;
    } catch (error) {
      throw error;
    }
  };

  const getToken = () => {
    if (user) {
      return user.token;
    }
    return null;
  };

  const signOut = () => {
    setUserCookie(null);
    setUser(null);
  };

  return {
    user,
    signIn: signIn,
    signOut: signOut,
    register: register,
    getToken: getToken,
    forgotPassword: (email: string) => Promise.resolve(),
  };
};

interface AuthProviderProps {
  children: JSX.Element;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const auth = useProviderAuth();

  return <AuthContext.Provider value={auth}>{children}</AuthContext.Provider>;
};
