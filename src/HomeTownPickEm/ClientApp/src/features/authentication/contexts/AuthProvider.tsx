import React, { createContext, useContext } from 'react';
import Cookies from 'js-cookie';

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
    var response = await fetch('api/user/register', {
      method: 'POST',
      body: JSON.stringify(user),
      headers: {
        'Content-Type': 'application/json',
      },
    });
    if (response.status === 201) {
      return await response.json();
    }
    throw new Error('Error creating user');
  } catch (error) {
    throw error;
  }
};
const useProviderAuth = (): AuthContextProps => {
  const [user, setUser] = React.useState<User | null>(getUserCookie());

  const signIn = async (email: string, password: string) => {
    try {
      var response = await fetch('api/user/login', {
        method: 'POST',
        body: JSON.stringify({ email: email, password: password }),
        headers: {
          'Content-Type': 'application/json',
        },
      });
      if (response.status === 200) {
        const user = await response.json();
        setUserCookie(user);
        setUser(user);
        return user;
      }
      throw new Error('Error logging in user');
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
