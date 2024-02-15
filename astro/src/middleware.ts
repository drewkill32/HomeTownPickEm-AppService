import type { MiddlewareHandler } from "astro";

import { supabase } from "@/lib/supabase";

export const onRequest: MiddlewareHandler = async (
  { locals, cookies, redirect, request },
  next,
) => {
  const accessToken = cookies.get("sb_access_token");
  const refreshToken = cookies.get("sb_refresh_token");

  if (accessToken && refreshToken) {
    const { data, error } = await supabase.auth.setSession({
      refresh_token: refreshToken.value,
      access_token: accessToken.value,
    });
    if (error) {
      cookies.delete("sb-access-token", {
        path: "/",
      });
      cookies.delete("sb-refresh-token", {
        path: "/",
      });
      locals.user = null;
      redirect(
        `/signin?error_msg=${encodeURIComponent(error.message)}&redirect_url=${encodeURIComponent(request.url)}`,
      );
    }

    locals.user = data?.user || null;
  }

  locals.isAuthenticated = Boolean(locals.user);
  var redirectUrl = request.url;
  locals.protect = () => {
    if (!locals.isAuthenticated) {
      return redirect(
        `/signin?redirect_url=${encodeURIComponent(redirectUrl)}`,
      );
    }
  };

  next();
};
