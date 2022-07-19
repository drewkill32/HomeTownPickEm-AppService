import React from 'react';
import { useFormik } from 'formik';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../features/authentication';
import { Button, Stack, TextField, Typography } from '@mui/material';
import { Box } from '@mui/system';
import SportsFootballIcon from '@mui/icons-material/SportsFootball';
import * as yup from 'yup';

const validationSchema = yup.object({
  email: yup
    .string()
    .email('Enter a valid email')
    .required('Email is required'),
  password: yup
    .string()
    .min(8, 'Password should be of minimum 8 characters length')
    .required('Password is required'),
});

const Login = () => {
  const auth = useAuth();
  const { state } = useLocation();
  //TOOO: navigate back to page that was visited before login
  //TODO: password validation
  const navigate = useNavigate();
  const [submitError, setSubmitError] = React.useState('');

  const formik = useFormik({
    initialValues: {
      email: process.env.REACT_APP_USERNAME || '',
      password: process.env.REACT_APP_PASSWORD || '',
    },
    validationSchema: validationSchema,
    onSubmit: async ({ email, password }) => {
      try {
        await auth.signIn(email, password);
        console.log('Login successful');
        navigate('/');
      } catch (error) {
        console.log(error);
        setSubmitError('Unable to log in try again');
      }
    },
  });

  return (
    <Box
      sx={{
        height: '100vh',
        width: '100%',
        overflow: 'hidden',
        bgcolor: 'primary.light',
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
        gap: 3,
        px: 3,
      }}
    >
      <SportsFootballIcon
        sx={{ fontSize: '45px', color: 'primary.contrastText' }}
      />
      <Typography
        variant="h5"
        noWrap
        sx={{
          fontFamily: 'monospace',
          fontWeight: 700,
          letterSpacing: '.3rem',
          color: 'primary.contrastText',
          textDecoration: 'none',
          mb: 2,
        }}
      >
        St. Pete Pick'em
      </Typography>

      <Box
        component="form"
        onSubmit={formik.handleSubmit}
        sx={{
          mx: 3,
          padding: 3,
          borderRadius: 4,
          bgcolor: 'white',
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'center',
          gap: 2,
          width: '100%',
          maxWidth: '400px',
        }}
      >
        <TextField
          fullWidth
          id="email"
          name="email"
          label="Email"
          value={formik.values.email}
          onChange={formik.handleChange}
          error={formik.touched.email && Boolean(formik.errors.email)}
          helperText={formik.touched.email && formik.errors.email}
        />
        <TextField
          fullWidth
          id="password"
          name="password"
          label="Password"
          type="password"
          value={formik.values.password}
          onChange={formik.handleChange}
          error={formik.touched.password && Boolean(formik.errors.password)}
          helperText={formik.touched.password && formik.errors.password}
        />
        <Typography color="red">{submitError}</Typography>
        <Button color="primary" variant="contained" fullWidth type="submit">
          Submit
        </Button>
      </Box>
    </Box>
  );
};

export default Login;
