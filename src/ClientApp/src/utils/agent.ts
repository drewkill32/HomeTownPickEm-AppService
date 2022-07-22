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

const fetchNewRefreshToken = async (token: UserToken): Promise<UserToken> => {
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
    const newToken = await res.json();
    setUserToken(newToken);
    return UserToken.parse(newToken);
  } catch (error) {
    console.error('Error getting refresh token', error);
    localStorage.removeItem('token');
    window.location.reload();
    return {} as UserToken; // never reached
  }
};

const throttler = new Semaphore<UserToken>();

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
          config.headers.Authorization = `Bearer ${newToken.access_token}`;
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
      console.log('error', { error });
      //if we get a 401, we can try to refresh the token
      // request failed. maybe the server is down or network error
      if (!error.response) {
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

      if (error.response) {
        if (error.response.status === 401) {
          setUserToken(null);
          window.location.reload();
          return;
        }
        if (error.response.data) {
          throw error.response.data as RequestError;
        }
      }
    }
  );
}
