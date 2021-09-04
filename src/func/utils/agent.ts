import axios from 'axios';

export const login = async () => {
  const {
    APP_BASE_URL: baseUrl,
    APP_USERNAME: userName,
    APP_PASSWORD: password,
  } = process.env;
  const res = await axios.post(`${baseUrl}/api/user/login`, {
    email: userName,
    password: password,
  });
  const { token } = await res.data;
  return token;
};

export const updatePicks = async () => {
  const token = await login();
  const { APP_BASE_URL: baseUrl } = process.env;
  const res = await axios.post(
    `${baseUrl}/api/picks/st-pete-pick-em/updatepickscores`,
    {},
    {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );
};
