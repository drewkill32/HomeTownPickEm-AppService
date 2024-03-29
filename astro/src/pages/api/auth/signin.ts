import { supabase } from "@/lib/supabase";
import type { APIRoute } from "astro";

export const POST: APIRoute = async ({ request, cookies, redirect }) => {
  const redirectUrl =
    new URL(request.url).searchParams.get("redirect_url") || "/dashboard";
  const formData = await request.formData();
  const email = formData.get("email")?.toString();
  const password = formData.get("password")?.toString();

  if (!email || !password) {
    return redirect(
      `/signin?error_msg=${encodeURIComponent("Email and password are required.")}`,
    );
  }

  const { data, error } = await supabase.auth.signInWithPassword({
    email,
    password,
  });

  if (error) {
    return redirect(`/signin?error_msg=${encodeURIComponent(error.message)}`);
  }

  const { access_token, refresh_token } = data.session;

  cookies.set("sb_access_token", access_token, {
    path: "/",
  });

  cookies.set("sb_refresh_token", refresh_token, {
    path: "/",
  });

  return redirect(redirectUrl);
};
