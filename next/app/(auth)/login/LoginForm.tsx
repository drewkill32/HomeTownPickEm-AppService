"use client";

import Link from "next/link";
import { login } from "./login";
import { useForm } from "react-hook-form";
import { Loader2 } from "lucide-react";
import { LoginFormInputs } from "@/app/(auth)/login/schema";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
  SubmitButton,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";

type Props = {
  message?: string | undefined;
  redirectUrl?: string | undefined;
  newUser?: boolean;
};

export const LoginForm = ({ message, redirectUrl, newUser }: Props) => {
  const form = useForm<LoginFormInputs>({
    mode: "all",
    criteriaMode: "all",
    defaultValues: {
      email: "",
      password: "",
      redirectUrl: redirectUrl || "",
    },
  });

  const { register } = form;
  //if submit is successful, the action will redirect

  return (
    <Form {...form}>
      <div className="m-auto mx-auto my-4 max-w-sm px-3 text-card-foreground">
        <form
          className="rounded-lg border bg-card px-5 shadow-sm"
          action={login}
        >
          <div className="space-y-4 p-6">
            <FormField
              control={form.control}
              name="email"
              render={({ field }) => (
                <FormItem>
                  <FormLabel className="font-medium">Email</FormLabel>
                  <FormControl>
                    <Input {...field} placeholder="me@email.com" />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem>
                  <FormLabel className="flex items-center">
                    <span className="font-medium">Password</span>
                    <Link
                      className="ml-auto inline-block text-sm hover:underline"
                      href="/forgot-password"
                    >
                      Forgot your password?
                    </Link>
                  </FormLabel>
                  <FormControl>
                    <Input {...field} type="password" placeholder="password" />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <input type="hidden" {...register("redirectUrl")} />
            {newUser && (
              <div className="mb-2 rounded-sm border border-green-500 p-3">
                <p className="text-center text-sm font-semibold text-green-500">
                  Check email to continue sign in process
                </p>
              </div>
            )}
          </div>

          <SubmitButton
            className="w-full"
            submitting={
              <>
                <Loader2 className="mr-2 h-6 w-6 animate-spin text-secondary" />
                Signing in...
              </>
            }
          >
            Sign in
          </SubmitButton>
          {message && (
            <div className="mb-2 rounded-sm border border-red-500 p-3">
              <h2 className="text-center text-sm font-bold text-red-500">
                Error logging in
              </h2>
              <hr className="mx-auto my-2 h-px w-4/5 border-0 bg-red-500" />
              <p className="text-center text-sm font-semibold text-red-500">
                {message}
              </p>
            </div>
          )}

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
    </Form>
  );
};
