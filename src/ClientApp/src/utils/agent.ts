import axios from 'axios';
import { z } from 'zod';

const getUrl = () => {
  const url = `${window.location.protocol}//${window.location.hostname}`;
  if (window.location.port) {
    return `${url}:${window.location.port}`;
  }
  return url + '/';
};

const getUserToken = () => {
  const user = localStorage.getItem('user');
  if (user) {
    return JSON.parse(user).token as string;
  }
  return null;
};

export const RequestError = z.object({
  type: z.string(),
  title: z.string(),
  status: z.number(),
  detail: z.string(),
});

export type RequestError = z.infer<typeof RequestError>;

export function setupAxios() {
  axios.defaults.baseURL = process.env.REACT_APP_API_URL || getUrl();

  axios.interceptors.request //add bearer token to all requests
    .use((config) => {
      const token = getUserToken();
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    });
  axios.interceptors.response.use(
    (res) => res,
    (error) => {
      if (error.response && error.response.data) {
        return Promise.reject(error.response.data as RequestError);
      } else if (error.request) {
        return Promise.reject({
          type: 'Request error',
          title: 'Request error',
          status: 600,
          detail: 'The request was made but no response was received',
        } as RequestError);
      } else {
        return Promise.reject({
          type: 'Unknown error',
          title: 'Unknown error',
          status: 600,
          detail: 'Error settings up a request',
        } as RequestError);
      }
    }
  );
}
