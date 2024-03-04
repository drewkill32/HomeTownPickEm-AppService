"use server";
import { createClient } from "@/utils/supabase/server";
import { redirect } from "next/navigation";

export const login = async (formData: FormData) => {
  const email = formData.get("email")?.toString();
  const password = formData.get("password")?.toString();
  if (!email || !password) {
    return redirect(
      `/login?message=${encodeURIComponent("Email and password are required.")}`
    );
  }
  const supabase = createClient();

  const { error } = await supabase.auth.signInWithPassword({
    email,
    password,
  });

  if (error) {
    return redirect("/login?message=Could not authenticate user");
  }

  return redirect("/protected");
};
