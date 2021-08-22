import React from "react";
import {useAuth} from "../../hooks/useAuth";
import {Button, Input} from "reactstrap";
import {ErrorMessage, Formik} from "formik";
import {useHistory} from "react-router-dom";

const Register = () => {
    const auth = useAuth();
    const history = useHistory();

    return (
        <div>
            <Formik
                initialValues={{
                    email: "",
                    password: "",
                    firstName: "",
                    lastName: "",
                    confirmPassword: "",
                }}
                validate={(values) => {
                    const errors = {};
                    if (!values.email) {
                        errors.email = "Required";
                    } else if (
                        !/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i.test(values.email)
                    ) {
                        errors.email = "Invalid email address";
                    }
                    if (!values.password) {
                        errors.password = "Required";
                    }
                    if (!values.firstName) {
                        errors.firstName = "Required";
                    }
                    if (!values.lastName) {
                        errors.lastName = "Required";
                    }
                    if (!values.confirmPassword) {
                        errors.confirmPassword = "Required";
                    }
                    if (values.password !== values.confirmPassword) {
                        errors.confirmPassword = "Passwords do not match";
                    }

                    return errors;
                }}
                onSubmit={async (values) => {
                    try {
                        await auth.register({
                            email: values.email,
                            password: values.password,
                            firstName: values.firstName,
                            lastName: values.lastName,
                        });
                        history.push("/login", {
                            email: values.email,
                        });
                    } catch (error) {
                        console.log(error);
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
                            <label htmlFor="firstName" className="col-sm-2 col-form-label">
                                First Name
                            </label>
                            <Input
                                type="firstName"
                                name="firstName"
                                id="firstName"
                                placeholder="First Name"
                                onChange={handleChange}
                                onBlur={handleBlur}
                                value={values.firstName}
                            />
                            <ErrorMessage name="firstName"/>
                        </div>
                        <div className="form-group row mb-2">
                            <label htmlFor="lastName" className="col-sm-2 col-form-label">
                                Last Name
                            </label>
                            <Input
                                type="lastName"
                                name="lastName"
                                id="lastName"
                                placeholder="Last Name"
                                onChange={handleChange}
                                onBlur={handleBlur}
                                value={values.lastName}
                            />
                            <ErrorMessage name="lastName"/>
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
                        <div className="form-group row mb-2">
                            <label
                                htmlFor="confirmPassword"
                                className="col-sm-2 col-form-label"
                            >
                                Password
                            </label>
                            <Input
                                type="password"
                                name="confirmPassword"
                                id="confirmPassword"
                                placeholder="Confirm Password"
                                onChange={handleChange}
                                onBlur={handleBlur}
                                value={values.confirmPassword}
                            />
                            <ErrorMessage name="confirmPassword"/>
                        </div>
                        <Button
                            type="submit"
                            className="mt-4"
                            color="primary"
                            disabled={isSubmitting && !isValid}
                        >
                            Submit
                        </Button>
                    </form>
                )}
            </Formik>
        </div>
    );
};

export default Register;
