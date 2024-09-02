"use server";
import { createClient } from "@/utils/supabase/server";
import { redirect } from "next/navigation";
import { loginSchema } from "@/app/(auth)/login/schema";

export const login = async (formData: FormData) => {
  const result = loginSchema.safeParse(Object.fromEntries(formData));
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
