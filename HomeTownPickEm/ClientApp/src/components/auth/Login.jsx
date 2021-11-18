import React from 'react';
import {ErrorMessage, Formik} from 'formik';
import {useHistory, useLocation} from 'react-router-dom';
import {Button, Input} from 'reactstrap';
import {useAuth} from '../../hooks/useAuth';

// The main responsibility of this component is to handle the user's login process.
// This is the starting point for the login process. Any component that needs to authenticate
// a user can simply perform a redirect to this component with a returnUrl query parameter and
// let the component perform the login and return back to the return url.
const Login = () => {
    const auth = useAuth();
    const location = useLocation();
    const state = location.state;
    const history = useHistory();
    var params = new URLSearchParams(location.search);
    const redirectUri = params.get('redirectUri');
    const email = params.get('email') || '';
    const [submitError, setSubmitError] = React.useState(null);
    return (
        <div>
            <Formik
                initialValues={{
                    email: email,
                    password: '',
                }}
                validate={(values) => {
                    const errors = {};
                    if (!values.email) {
                        errors.email = 'Required';
                    } else if (
                        !/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i.test(values.email)
                    ) {
                        errors.email = 'Invalid email address';
                    }
                    if (!values.password) {
                        errors.password = 'Required';
                    }

                    return errors;
                }}
                onSubmit={async ({email, password}) => {
                    try {
                        await auth.signIn({
                            email: email,
                            password: password,
                        });
                        if (redirectUri) {
                            window.location.href = redirectUri;
                        } else {
                            history.push('/', {
                                login: true,
                            });
                        }
                    } catch (error) {
                        console.log(error);
                        setSubmitError('unable to log in try again');
                    }
                }}
            >
                {({
                      values,
                      errors,
                      touched,
                      handleChange,
                      handleBlur,
                      handleSubmit,
                      isSubmitting,
                      isValid,
                      /* and other goodies */
                  }) => (
                    <form onSubmit={handleSubmit}>
                        <div className="form-group row mb-2">
                            <label htmlFor="email" className="col-sm-2 col-form-label">
                                Email
                            </label>
                            <Input
                                type="email"
                                name="email"
                                id="email"
                                placeholder="Email"
                                onChange={handleChange}
                                onBlur={handleBlur}
                                value={values.email}
                            />
                            <ErrorMessage name="email"/>
                        </div>

                        <div className="form-group row mb-2">
                            <label htmlFor="password" className="col-sm-2 col-form-label">
                                Password
                            </label>
                            <Input
                                type="password"
                                name="password"
                                id="password"
                                placeholder="Password"
                                onChange={handleChange}
                                onBlur={handleBlur}
                                value={values.password}
                            />
                            <ErrorMessage name="password"/>
                        </div>
                        <div>{submitError}</div>
                        <Button
                            type="submit"
                            className="mt-4"
                            color="primary"
                            disabled={isSubmitting || !touched}
                        >
                            Submit
                        </Button>
                    </form>
                )}
            </Formik>
        </div>
  );
};

export default Login;
