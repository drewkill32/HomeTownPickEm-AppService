import React, { createContext, useContext } from 'react';
import axios, { AxiosResponse } from 'axios';
import useLocalStorage from '../../../hooks/useLocalStorage';
import {
  useQuery,
  useQueryClient,
  UseQueryOptions,
  UseQueryResult,
} from 'react-query';
import { RequestError, UserToken } from '../../../utils/agent';
import { getUnixTime } from 'date-fns';
import { useLocalQuery } from '../../../hooks/useLocalQuery';

export interface User {
  leagues: [
    {
      id: number;
      name: string;
      slug: string;
      years: [string];
    }
  ];
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
  getUser: (
    options?: UseQueryOptions<User, RequestError>
  ) => UseQueryResult<User, RequestError>;
  signIn: (userName: string, password: string) => Promise<void>;
  signOut: () => Promise<void>;
  register: (user: RegisterProps) => Promise<AxiosResponse<any> | null>;
  forgotPassword: (email: string) => Promise<void>;
  resetPassword: (email: ForgotPasswordProps) => Promise<void>;
  getToken: () => string | null;
  isAuthenticated: boolean;
}

const useUser = (options?: UseQueryOptions<User, RequestError>) => {
  return useLocalQuery<User>(
    'profile',
    () => axios.get('api/user/profile').then((res) => res.data),
    {
      ...options,
      staleTime: 1000 * 60 * 60,
    }
  );
};

const AuthContext = createContext<AuthContextProps>({
  getUser: () => ({} as UseQueryResult<User, RequestError>),
  signIn: () => Promise.resolve(),
  signOut: () => Promise.resolve(),
  register: () => Promise.resolve<AxiosResponse<any> | null>(null),
  forgotPassword: () => Promise.resolve(),
  resetPassword: () => Promise.resolve(),
  getToken: () => null,
  isAuthenticated: false,
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
  const [token, setToken] = useLocalStorage<UserToken>('token', null);
  const queryClient = useQueryClient();
  const signIn = async (email: string, password: string) => {
    try {
      var res = await axios.post('api/user/login', {
        email: email,
        password: password,
      });
      var t = res.data as UserToken;
      setToken(t);
    } catch (error) {
      throw error;
    }
  };

  const getToken = () => {
    if (token) {
      return token.access_token;
    }
    return null;
  };

  const isAuthenticated = token && token.expires_in > getUnixTime(Date.now());
  console.log({
    isAuthenticated,
    token,
    exp: token?.expires_in,
    now: getUnixTime(Date.now()),
  });

  const signOut = async () => {
    try {
      await axios.post('/api/user/logout', token);
    } catch (error) {
      console.error('Error logging out', error);
    } finally {
      setToken(null);
      queryClient.clear();
      localStorage.clear();
    }
  };

  return {
    getUser: useUser,
    signIn: signIn,
    signOut: signOut,
    register: register,
    getToken: getToken,
    forgotPassword: forgotPassword,
    resetPassword: resetPassword,
    isAuthenticated: isAuthenticated,
  };
};

interface AuthProviderProps {
  children: JSX.Element;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const auth = useProviderAuth();

  return <AuthContext.Provider value={auth}>{children}</AuthContext.Provider>;
};
