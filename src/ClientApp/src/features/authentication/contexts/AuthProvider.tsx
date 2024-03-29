import React, { createContext, useCallback, useContext, useMemo } from 'react';
import axios, { AxiosResponse } from 'axios';
import useLocalStorage from '../../../hooks/useLocalStorage';
import { useQueryClient } from 'react-query';
import { JwtTokenType, User, UserTokenType } from '../../../models';
import { JwtToken } from '../../../zod';
import { getUnixTime } from 'date-fns';
import jwt_decode from 'jwt-decode';
import { fetchNewRefreshToken } from '../../../utils/agent';
import Semaphore from '../../../utils/Semaphore';

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
  code: string;
}

const throttler = new Semaphore<UserTokenType | null>();

export interface AuthContextProps {
  user: User | null;
  signIn: (userName: string, password: string) => Promise<void>;
  signOut: () => Promise<void>;
  register: (user: RegisterProps) => Promise<AxiosResponse<any> | null>;
  forgotPassword: (email: string) => Promise<void>;
  resetPassword: (email: ForgotPasswordProps) => Promise<void>;
  token: { jwt: string; decoded: JwtTokenType } | null;
  isAuthenticated: boolean;
  refreshToken: () => Promise<{ jwt: string; decoded: JwtTokenType } | null>;
}

const AuthContext = createContext<AuthContextProps>({
  user: null,
  signIn: () => Promise.resolve(),
  signOut: () => Promise.resolve(),
  register: () => Promise.resolve<AxiosResponse<any> | null>(null),
  forgotPassword: () => Promise.resolve(),
  resetPassword: () => Promise.resolve(),
  token: null,
  isAuthenticated: false,
  refreshToken: () =>
    Promise.resolve<{ jwt: string; decoded: JwtTokenType } | null>(null),
});

export const useAuth = () => {
  return useContext(AuthContext);
};

const register = async (values: RegisterProps) => {
  return await axios.post('api/user/register', values);
};

const forgotPassword = async (email: string) => {
  const res = await axios.post('api/user/resetpassword', { email });
  return res.data;
};

const resetPassword = async (values: ForgotPasswordProps) => {
  const res = await axios.post('api/user/verifyresetpassword', values);
  return res.data;
};

const useProviderAuth = (): AuthContextProps => {
  const [token, setToken] = useLocalStorage<UserTokenType>('token', null);
  const [user, setUser] = useLocalStorage<User>('profile', null);
  const queryClient = useQueryClient();

  const signIn = async (email: string, password: string) => {
    try {
      const res = await axios.post('api/user/login', {
        email: email,
        password: password,
      });
      const t = res.data as UserTokenType;
      const user = await axios
        .get('api/user/profile', {
          headers: {
            Authorization: `Bearer ${t.access_token}`,
          },
        })
        .then((res) => res.data as User);
      setToken(t);
      setUser(user);
    } catch (error) {
      throw error;
    }
  };

  const jwtToken = useMemo(() => {
    if (token) {
      const decoded = jwt_decode(token.access_token);
      return {
        jwt: token.access_token,
        decoded: JwtToken.parse(decoded),
      };
    }
    return null;
  }, [token]);

  const isAuthenticated = token && token.expires_in > getUnixTime(Date.now());

  const signOut = async () => {
    try {
      await axios.post('/api/user/logout', token);
    } catch (error) {
      console.error('Error logging out', error);
    } finally {
      setToken(null);
      setUser(null);
      queryClient.clear();
      localStorage.clear();
    }
  };
  const refreshToken = useCallback(async () => {
    const t = await throttler.callFunction(() => fetchNewRefreshToken(token));
    setToken(t);
    if (t) {
      const decoded = jwt_decode(token.access_token);
      return {
        jwt: t.access_token,
        decoded: JwtToken.parse(decoded),
      };
    }
    return null;
  }, [token, setToken]);

  return {
    user: user,
    signIn: signIn,
    signOut: signOut,
    register: register,
    token: jwtToken,
    forgotPassword: forgotPassword,
    resetPassword: resetPassword,
    isAuthenticated: isAuthenticated,
    refreshToken: refreshToken,
  };
};

interface AuthProviderProps {
  children: JSX.Element;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const auth = useProviderAuth();

  return <AuthContext.Provider value={auth}>{children}</AuthContext.Provider>;
};
