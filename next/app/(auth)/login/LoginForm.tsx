"use client";

import Link from "next/link";
import { login } from "./login";
import { useForm } from "react-hook-form";
import { Loader2 } from "lucide-react";
import { LoginFormInputs } from "@/app/(auth)/login/schema";
import { useEffect } from "react";

type Props = {
  message?: string | undefined;
  redirectUrl?: string | undefined;
  newUser?: string | undefined;
};

export const LoginForm = ({ message, redirectUrl, newUser }: Props) => {
  const {
    register,
    formState: { isValid, isSubmitting, isSubmitSuccessful },
    handleSubmit,
  } = useForm<LoginFormInputs>({
    mode: "all",
    criteriaMode: "all",
    defaultValues: {
      email: "",
      password: "",
      redirectUrl: redirectUrl || "",
    },
  });

  //if submit is successful, the action will redirect
  const isRedirecting = isSubmitting || isSubmitSuccessful;

  const onSubmit = async (data: LoginFormInputs) => {
    try {
      const formData = new FormData();
      formData.append("email", data.email);
      formData.append("password", data.password);
      formData.append("redirectUrl", data.redirectUrl);
      await login(formData);
      //there is a brief period where isSubmitting false but the redirect hasn't happened yet
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    //get the event before the window is unloaded
    const handleBeforeUnload = (event: BeforeUnloadEvent) => {
      console.log("beforeunload");
    };
    window.addEventListener("beforeunload", handleBeforeUnload);
    return () => {
      window.removeEventListener("beforeunload", handleBeforeUnload);
    };
  }, []);

  return (
    <div className="m-auto mx-auto my-4 max-w-sm px-3 text-card-foreground">
      <form
        className="rounded-lg border bg-card shadow-sm"
        action={login}
        onSubmit={handleSubmit(onSubmit)}
      >
        <div className="space-y-4 p-6">
          {message && (
            <div className="mb-2 rounded-sm border border-red-500 p-3">
              <h2 className="text-center text-sm font-bold text-red-500">
                ERROR
              </h2>
              <hr className="mx-auto my-2 h-px w-4/5 border-0 bg-red-500" />
              <p className="text-center text-sm font-semibold text-red-500">
                {message}
              </p>
            </div>
          )}

          <div className="space-y-2">
            <label
              className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
              htmlFor="email"
            >
              Email
            </label>
            <input
              className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"
              id="email"
              {...register("email", { required: true })}
              placeholder="me@example.com"
              required
              type="email"
            />
          </div>
          <div className="space-y-2">
            <div className="flex items-center">
              <label
                className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
                htmlFor="password"
              >
                Password
              </label>
              <Link
                className="ml-auto inline-block text-sm hover:underline"
                href="/forgot-password"
              >
                Forgot your password?
              </Link>
            </div>
            <input
              className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"
              id="password"
              required
              type="password"
              placeholder="password"
              {...register("password", { required: true })}
            />
          </div>
          <input type="hidden" name="redirectUrl" value={redirectUrl} />
          {newUser && (
            <div className="mb-2 rounded-sm border border-green-500 p-3">
              <p className="text-center text-sm font-semibold text-green-500">
                {newUser}
              </p>
            </div>
          )}
        </div>

        <div className="flex items-center p-6">
          <button
            type="submit"
            disabled={isRedirecting || !isValid}
            className="mx-auto inline-flex h-10 w-full items-center justify-center whitespace-nowrap rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground ring-offset-background transition-colors hover:bg-primary/90 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50"
          >
            {isRedirecting ? (
              <>
                <Loader2 className="mr-2 h-6 w-6 animate-spin text-secondary" />
                Sign in
              </>
            ) : (
              "Sign in"
            )}
          </button>
        </div>
        <div className="p-2 text-center">
          <p>
            New to St. Pete Pick'em?{" "}
            <Link className="hover:underline" href="/sign-up">
              Create an account
            </Link>
          </p>
        </div>
      </form>
    </div>
  );
};
