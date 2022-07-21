import React, { createContext, useContext } from 'react';
import axios, { AxiosResponse } from 'axios';
import useLocalStorage from '../../../hooks/useLocalStorage';

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
interface ForgotPasswordProps {
  email: string;
  password: string;
  code: string;
}

interface RegisterProps {
  email: string;
  firstName: string;
  lastName: string;
  password: string;
}

export interface AuthContextProps {
  user: User | null;
  signIn: (userName: string, password: string) => Promise<User | null>;
  signOut: () => void;
  register: (user: RegisterProps) => Promise<AxiosResponse<any> | null>;
  forgotPassword: (email: string) => Promise<void>;
  resetPassword: (email: ForgotPasswordProps) => Promise<void>;
  getToken: () => string | null;
}

const AuthContext = createContext<AuthContextProps>({
  user: null,
  signIn: () => Promise.resolve(null),
  signOut: () => {},
  register: () => Promise.resolve<AxiosResponse<any> | null>(null),
  forgotPassword: () => Promise.resolve(),
  resetPassword: () => Promise.resolve(),
  getToken: () => null,
});

export const useAuth = () => {
  return useContext(AuthContext);
};

const register = async (values: RegisterProps) => {
  var res = await axios.post('api/user/register', values);
  return res;
};

const forgotPassword = async (email: string) => {
  var res = await axios.post('api/user/resetpassword', { email });
  return res.data;
};

const resetPassword = async (values: ForgotPasswordProps) => {
  var res = await axios.post('api/user/verifyresetpassword', values);
  return res.data;
};

const useProviderAuth = (): AuthContextProps => {
  const [user, setUser] = useLocalStorage<User>('user', null);

  const signIn = async (email: string, password: string) => {
    try {
      var res = await axios.post('api/user/login', {
        email: email,
        password: password,
      });
      var user = res.data as User;
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
    setUser(null);
  };

  return {
    user,
    signIn: signIn,
    signOut: signOut,
    register: register,
    getToken: getToken,
    forgotPassword: forgotPassword,
    resetPassword: resetPassword,
  };
};

interface AuthProviderProps {
  children: JSX.Element;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const auth = useProviderAuth();

  return <AuthContext.Provider value={auth}>{children}</AuthContext.Provider>;
};
