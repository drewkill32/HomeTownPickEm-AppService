import { Box, Button, TextField, Typography } from '@mui/material';
import { useFormik } from 'formik';
import { useState } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import AuthLayout from '../components/AuthLayout';
import { useAuth } from '../contexts/AuthProvider';
import * as yup from 'yup';
import { RequestError } from '../../../utils/agent';

const validationSchema = yup.object({
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
  confirmPassword: yup
    .string()
    .oneOf([yup.ref('password'), null], 'Passwords must match'),
});

const ConfirmPasswordReset = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const email = searchParams.get('email');
  const code = searchParams.get('code');
  const auth = useAuth();
  const [submitError, setSubmitError] = useState('');

  const formik = useFormik({
    initialValues: {
      email: email || '',
      code: code || '',
      password: '',
      confirmPassword: '',
    },
    validationSchema: validationSchema,
    onSubmit: async ({ email, password, code }) => {
      try {
        await auth.resetPassword({
          email,
          password,
          code,
        });
        navigate('/');
      } catch (error) {
        var e = RequestError.parse(error);
        setSubmitError(`Unable create an account. ${e.detail || e.title}`);
      }
    },
  });

  if (!email || !code) {
    return (
      <AuthLayout
        title="Unable to reset password"
        footer={
          <Button
            color="primary"
            variant="contained"
            fullWidth
            onClick={() => navigate('/')}
          >
            Return to login
          </Button>
        }
      >
        <Typography>The request to reset the password is invalid</Typography>
      </AuthLayout>
    );
  }

  return (
    <AuthLayout title="Reset Password">
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
          disabled
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
        <TextField
          fullWidth
          id="confirmPassword"
          name="confirmPassword"
          label="Confirm Password"
          type="confirmPassword"
          value={formik.values.confirmPassword}
          onChange={formik.handleChange}
          error={
            formik.touched.confirmPassword &&
            Boolean(formik.errors.confirmPassword)
          }
          helperText={
            formik.touched.confirmPassword && formik.errors.confirmPassword
          }
        />
        <input type="hidden" name="code" value={formik.values.code} />
        <Typography textAlign="center" color="red">
          {submitError}
        </Typography>
        <Button color="primary" variant="contained" fullWidth type="submit">
          Submit
        </Button>
      </Box>
    </AuthLayout>
  );
};
export default ConfirmPasswordReset;
