import React from 'react';
import { useFormik } from 'formik';
import { Link as RouterLink } from 'react-router-dom';
import { useAuth } from '../';
import {
  Button,
  Grid,
  TextField,
  Typography,
  Slide,
  Alert,
} from '@mui/material';

import { SwitchTransition } from 'react-transition-group';
import { Box } from '@mui/system';
import * as yup from 'yup';
import AuthLayout from '../components/AuthLayout';
import { RequestError } from '../../../zod';

//Check your inbox for the next steps. If you don't receive an email, and it's not in your spam folder this could mean you signed up with a different address.

const validationSchema = yup.object({
  email: yup
    .string()
    .email('Enter a valid email')
    .required('Email is required'),
});

const ForgotPassword = () => {
  const { forgotPassword } = useAuth();

  const [submitError, setSubmitError] = React.useState('');
  const [submitted, setSubmitted] = React.useState(false);

  const containerRef = React.useRef(null);

  const formik = useFormik({
    initialValues: {
      email: process.env.REACT_APP_USERNAME || '',
    },
    validationSchema: validationSchema,
    onSubmit: async ({ email }) => {
      try {
        await forgotPassword(email);
        setSubmitted(true);
      } catch (error) {
        var e = RequestError.parse(error);
        setSubmitError(`Unable to send password reset. ${e.detail || e.title}`);
      }
    },
  });

  return (
    <AuthLayout
      title="Reset Password"
      footer={
        <Grid
          container
          spacing={1}
          alignItems="center"
          justifyContent="center"
          sx={{ textAlign: 'center' }}
        >
          <Grid item xs={12} sm={6}>
            <Typography component={RouterLink} to="/login">
              Login
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
        ref={containerRef}
        sx={{
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'center',
          gap: 2,
          width: '100%',
          overflow: 'hidden',
          minHeight: '172px',
        }}
      >
        <SwitchTransition>
          <Slide
            // @ts-ignore - typescript doesn't know about SwitchTransition key
            key={submitted}
            in={submitted}
            direction="left"
            appear={false}
            timeout={800}
            container={containerRef.current}
            addEndListener={(node, done) => {
              node.addEventListener('transitionend', done, false);
            }}
          >
            {submitted ? (
              <Alert severity="success" icon={false} sx={{ width: '100%' }}>
                <p>{`Email sent to ${formik.values.email}.`}</p>
                <p>
                  Check your inbox for the next steps. If you don't receive an
                  email, check your spam folder or verify your email address is
                  correct.
                </p>
              </Alert>
            ) : (
              <Box
                sx={{
                  width: '100%',
                  maxWidth: '420px',
                  display: 'flex',
                  flexDirection: 'column',
                  alignItems: 'center',
                  gap: 2,
                  mt: 2,
                  justifyContent: 'center',
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
            )}
          </Slide>
        </SwitchTransition>
      </Box>
    </AuthLayout>
  );
};
export default ForgotPassword;
