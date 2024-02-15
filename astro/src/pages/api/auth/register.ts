import type { APIRoute } from "astro";
import { supabase } from "@/lib/supabase";

export const POST: APIRoute = async ({ request, redirect }) => {
  const formData = await request.formData();
  const email = formData.get("email")?.toString();
  const password = formData.get("password")?.toString();

  if (!email || !password) {
    return redirect(
      `/register?error_msg=${encodeURIComponent("Email and password are required")}`,
    );
  }

  const { error } = await supabase.auth.signUp({ email, password });

  if (error) {
    return redirect(`/register?error_msg=${encodeURIComponent(error.message)}`);
  }

  return redirect("/signin");
};
