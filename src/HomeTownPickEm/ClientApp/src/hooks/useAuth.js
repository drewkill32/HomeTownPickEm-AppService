import React, { createContext, useContext } from 'react';
import Cookies from 'js-cookie';

const getUserCookie = () => {
  const userJson = Cookies.get('user');
  if (userJson) {
    return JSON.parse(userJson);
  }
  return null;
};

const setUserCookie = (user) => {
  if (user) {
    Cookies.set('user', JSON.stringify(user), { expires: 30 });
  } else {
    Cookies.remove('user');
  }
};

export const useAuth = () => {
  return useContext(AuthContext);
};

const register = async (user) => {
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
const useProviderAuth = () => {
  const [user, setUser] = React.useState(getUserCookie());

  const signIn = async ({ email, password }) => {
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
  };
};

export const AuthContext = createContext({
  user: null,
  signIn: () => Promise.resolve(null),
  signOut: () => {},
  register: () => Promise.resolve(null),
  forgotPassword: () => Promise.resolve(),
  getToken: () => {},
});

export const ProviderAuth = ({ children }) => {
  const auth = useProviderAuth();

  return <AuthContext.Provider value={auth}>{children}</AuthContext.Provider>;
};
