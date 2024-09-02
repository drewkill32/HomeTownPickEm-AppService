"use server";

import { createClient } from "@/utils/supabase/server";
import { redirect } from "next/navigation";
import { headers } from "next/headers";

export const signUp = async (formData: FormData) => {
  const origin = headers().get("origin");
  const email = formData.get("email")?.toString();
  const password = formData.get("password")?.toString();
  if (!email || !password) {
    return redirect(
      `/sign-up?message=${encodeURIComponent("Email and password are required")}`,
    );
  }

  const supabase = createClient();

  const { error } = await supabase.auth.signUp({
    email,
    password,
    options: {
      emailRedirectTo: `${origin}/auth/callback`,
    },
  });

  if (error) {
    return redirect("/sign-up?message=Could not authenticate user");
  }

  return redirect("/login?newUser=true");
};
