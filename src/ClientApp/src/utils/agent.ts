import axios from 'axios';
import { string } from 'yup';
import { z } from 'zod';
import jwt_decode from 'jwt-decode';
import { getUnixTime } from 'date-fns';
import Semaphore from './Semaphore';
import { formControlClasses } from '@mui/material';

const getUrl = () => {
  if (process.env.REACT_APP_API_URL) {
    return process.env.REACT_APP_API_URL;
  }
  const url = `${window.location.protocol}//${window.location.hostname}`;
  if (window.location.port) {
    return `${url}:${window.location.port}`;
  }
  return url + '/';
};

const getUserToken = (): UserToken | null => {
  const token = localStorage.getItem('token');
  if (token) {
    var result = JSON.parse(token);
    if (UserToken.parse(result)) {
      return result;
    }
  }
  return null;
};

const setUserToken = (token: UserToken | null) => {
  if (token) {
    var t = UserToken.parse(token);
    localStorage.setItem('token', JSON.stringify(t));
  } else {
    localStorage.removeItem('token');
  }
  return token;
};

export const UserToken = z.object({
  access_token: z.string(),
  expires_in: z.number(),
  refresh_token: z.string(),
});

export type UserToken = z.infer<typeof UserToken>;

export const RequestError = z.object({
  type: z.string(),
  title: z.string(),
  status: z.number(),
  detail: z.optional(z.string()),
});

const JwtToken = z.object({
  exp: z.number(),
});

export type RequestError = z.infer<typeof RequestError>;

const fetchNewRefreshToken = async (
  token: UserToken
): Promise<UserToken | null> => {
  try {
    // use fetch here to avoid using axios infinite loop
    const res = await fetch(`${getUrl()}/api/user/refresh`, {
      method: 'POST',
      body: JSON.stringify({
        refresh_token: token.refresh_token,
        access_token: token.access_token,
      }),
      headers: {
        'Content-Type': 'application/json',
      },
    });
    if (res.status === 200) {
      const newToken = await res.json();
      const t = UserToken.parse(newToken);
      console.log(
        `refreshed token: ${token.refresh_token} with token: ${t.refresh_token}`
      );
      setUserToken(t);
      return t;
    }
    if (res.status === 401) {
      console.log(`401 on token: ${token.refresh_token}`);
      setUserToken(null);
      window.location.reload();
    }
    throw new Error(`${res.status} ${res.statusText}`);
  } catch (error) {
    console.error('Error getting refresh token', { error });
  }
  return null;
};

const throttler = new Semaphore<UserToken | null>();

export function setupAxios() {
  axios.defaults.baseURL = getUrl();

  axios.interceptors.request //add bearer token to all requests
    .use(async (config) => {
      var userToken = getUserToken();
      if (userToken) {
        var decoded = jwt_decode(userToken.access_token);
        var exp = JwtToken.parse(decoded).exp;
        if (exp <= getUnixTime(new Date())) {
          const newToken = await throttler.callFunction(() =>
            fetchNewRefreshToken({ ...userToken } as UserToken)
          );
          if (newToken) {
            config.headers.Authorization = `Bearer ${newToken.access_token}`;
          }
        } else {
          console.log('using non-expired token');
          config.headers.Authorization = `Bearer ${userToken.access_token}`;
        }
      }
      return config;
    });

  axios.interceptors.response.use(
    (res) => res,
    async (error) => {
      if (error.response && error.response.data) {
        throw error.response.data as RequestError;
      }

      if (error.request) {
        throw {
          type: 'Request error',
          title: 'Request error',
          status: 600,
          detail: 'The request was made but no response was received.',
        } as RequestError;
      }

      //fallback error we have no idea what happened
      throw {
        type: 'Unknown error',
        title: 'Unknown error',
        status: 600,
        detail: 'Error settings up a request',
      } as RequestError;
    }
  );
}
