/* eslint-disable no-throw-literal */
import axios from 'axios';
import jwt_decode from 'jwt-decode';
import { getUnixTime } from 'date-fns';
import Semaphore from './Semaphore';
import { UserTokenType, RequestErrorType } from '../models';
import { UserToken, JwtToken } from '../zod';

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

const getUserToken = (): UserTokenType | null => {
  const token = localStorage.getItem('token');
  if (token) {
    var result = JSON.parse(token);
    if (UserToken.parse(result)) {
      return result;
    }
  }
  return null;
};

const setUserToken = (token: UserTokenType | null) => {
  if (token) {
    var t = UserToken.parse(token);
    localStorage.setItem('token', JSON.stringify(t));
  } else {
    localStorage.removeItem('token');
  }
  return token;
};

const fetchNewRefreshToken = async (
  token: UserTokenType
): Promise<UserTokenType | null> => {
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
    // noinspection ExceptionCaughtLocallyJS
    throw new Error(`${res.status} ${res.statusText}`);
  } catch (error) {
    console.error('Error getting refresh token', { error });
  }
  return null;
};

const throttler = new Semaphore<UserTokenType | null>();

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
            fetchNewRefreshToken({ ...userToken } as UserTokenType)
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
        throw error.response.data as RequestErrorType;
      }

      if (error.request) {
        throw {
          type: 'Request error',
          title: 'Request error',
          status: 600,
          detail: 'The request was made but no response was received.',
        } as RequestErrorType;
      }

      //fallback error we have no idea what happened
      throw {
        type: 'Unknown error',
        title: 'Unknown error',
        status: 600,
        detail: 'Error settings up a request',
      } as RequestErrorType;
    }
  );
}
