import React from 'react';
import { useFormik } from 'formik';
import { useNavigate, useLocation, Link as RouterLink } from 'react-router-dom';
import { useAuth } from '..';
import {
  Button,
  Divider,
  Grid,
  Stack,
  TextField,
  Typography,
} from '@mui/material';
import { Box } from '@mui/system';
import * as yup from 'yup';
import AuthLayout from '../components/AuthLayout';
import { RequestError } from '../../../utils/agent';

export const validationSchema = yup.object({
  email: yup
    .string()
    .email('Enter a valid email')
    .required('Email is required'),
  password: yup
    .string()
    .min(4, 'Password should be of minimum 4 characters length')
    .matches(/[a-z]+/, 'Password must contain an lower case letter.')
    .matches(/[A-Z]+/, 'Password must contain an upper case letter.')
    .matches(/[0-9]+/, 'Password must contain a number.')
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
        navigate('/');
      } catch (error) {
        console.log({ error });
        var e = RequestError.parse(error);
        if (e.status === 403) {
          setSubmitError(`Unable to sign in. email or password is incorrect.`);
        } else {
          setSubmitError(`Error logging in. ${e.detail || e.title}`);
        }
      }
    },
  });

  return (
    <AuthLayout
      title="Login"
      footer={
        <Grid
          container
          spacing={1}
          alignItems="center"
          justifyContent="center"
          sx={{ textAlign: 'center' }}
        >
          <Grid item xs={12} sm={6}>
            <Typography component={RouterLink} to="/forgot-password">
              Forgot your password?
            </Typography>
          </Grid>
          <Grid item xs={12} sm={6}>
            <Typography component={RouterLink} to="/register">
              Create an account
            </Typography>
          </Grid>
        </Grid>
      }
    >
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
          maxWidth: '420px',
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
        <Typography textAlign="center" color="red">
          {submitError}
        </Typography>
        <Button
          sx={{ maxWidth: '420px' }}
          color="primary"
          variant="contained"
          fullWidth
          type="submit"
        >
          Submit
        </Button>
      </Box>
    </AuthLayout>
  );
};

export default Login;
