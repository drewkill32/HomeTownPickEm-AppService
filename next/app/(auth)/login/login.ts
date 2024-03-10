"use server";
import { createClient } from "@/utils/supabase/server";
import { headers } from "next/headers";
import { redirect } from "next/navigation";
import { z } from "zod";

const signUpSchema = z.object({
  email: z.string().email("Invalid email address"),
  password: z.string(),
  redirectUrl: z
    .string()
    .refine(
      (value) => {
        const origin = headers().get("origin");
        return (
          value.startsWith("/") ||
          value.startsWith(origin || "") ||
          value === ""
        );
      },
      {
        message: "Redirect URL must start with '/' or be an empty string",
      },
    )
    .optional(),
});

export const login = async (formData: FormData) => {
  const result = signUpSchema.safeParse(Object.fromEntries(formData));
  if (!result.success) {
    return redirect(
      `/login?message=${encodeURIComponent("Email and password are required.")}`,
    );
  }
  const { email, password, redirectUrl } = result.data;

  const supabase = createClient();
  const { error } = await supabase.auth.signInWithPassword({
    email,
    password,
  });

  if (error) {
    return redirect("/login?message=Could not authenticate user");
  }
  const redirectTo = redirectUrl || "/leagues";
  return redirect(redirectTo);
};
