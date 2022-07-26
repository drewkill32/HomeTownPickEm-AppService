import { Alert, Button, Grid, TextField, Typography } from '@mui/material';
import { useFormik } from 'formik';
import { useState } from 'react';
import { useNavigate, Link as RouterLink } from 'react-router-dom';
import AuthLayout from '../components/AuthLayout';
import { useAuth } from '../contexts/AuthProvider';
import * as yup from 'yup';
import { RequestError } from '../../../zod';

const validationSchema = yup.object({
  email: yup
    .string()
    .email('Enter a valid email')
    .required('Email is required'),
  firstName: yup.string().required('First name is required'),
  lastName: yup.string().required('Last name is required'),
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

const Register = () => {
  const navigate = useNavigate();
  const { register } = useAuth();
  const [submitError, setSubmitError] = useState('');

  const formik = useFormik({
    initialValues: {
      email: process.env.REACT_APP_USERNAME || '',
      password: process.env.REACT_APP_PASSWORD || '',
      confirmPassword: process.env.REACT_APP_PASSWORD || '',
      firstName: '',
      lastName: '',
    },
    validationSchema: validationSchema,
    onSubmit: async (values) => {
      try {
        await register({
          email: values.email,
          password: values.password,
          firstName: values.firstName,
          lastName: values.lastName,
        });
        navigate('/');
      } catch (error) {
        var e = RequestError.parse(error);
        setSubmitError(`Unable create an account. ${e.detail}`);
      }
    },
  });

  return (
    <AuthLayout
      title="Create an Account"
      footer={
        <Typography component={RouterLink} to="/login">
          Already have an account?
        </Typography>
      }
    >
      <Grid
        container
        spacing={2}
        component="form"
        justifyContent="center"
        alignItems="center"
        onSubmit={formik.handleSubmit}
      >
        <Grid item xs={12}>
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
        </Grid>
        <Grid item xs={6}>
          <TextField
            fullWidth
            id="firstName"
            name="firstName"
            label="First Name"
            value={formik.values.firstName}
            onChange={formik.handleChange}
            error={formik.touched.firstName && Boolean(formik.errors.firstName)}
            helperText={formik.touched.firstName && formik.errors.firstName}
          />
        </Grid>
        <Grid item xs={6}>
          <TextField
            fullWidth
            id="lastName"
            name="lastName"
            label="Last Name"
            value={formik.values.lastName}
            onChange={formik.handleChange}
            error={formik.touched.lastName && Boolean(formik.errors.lastName)}
            helperText={formik.touched.lastName && formik.errors.lastName}
          />
        </Grid>
        <Grid item xs={6}>
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
        </Grid>
        <Grid item xs={6}>
          <TextField
            fullWidth
            id="confirmPassword"
            name="confirmPassword"
            label="Confirm Password"
            type="password"
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
        </Grid>

        <Grid item xs={12}>
          <Alert
            severity="success"
            icon={false}
            sx={{
              '& ul': {
                paddingLeft: 2,
              },
              '& li': {
                listStyleType: 'none',
              },
            }}
          >
            <p>Password must:</p>
            <ul>
              <li>Be at least 4 characters</li>
              <li>Contain at least one lower case letter</li>
              <li>Contain at least one upper case letter</li>
              <li>Contain at least one number</li>
            </ul>
          </Alert>
        </Grid>
        <Grid item xs={12}>
          <Typography textAlign="center" gutterBottom color="red">
            {submitError}
          </Typography>
        </Grid>
        <Grid item xs={12} textAlign="center">
          <Button
            sx={{ maxWidth: '420px', mt: 2 }}
            color="primary"
            variant="contained"
            fullWidth
            type="submit"
          >
            Submit
          </Button>
        </Grid>
      </Grid>
    </AuthLayout>
  );
};
export default Register;
