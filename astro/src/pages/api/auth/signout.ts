import type { APIRoute } from "astro";

export const GET: APIRoute = async ({ cookies, redirect }) => {
  cookies.delete("sb_access_token", { path: "/" });
  cookies.delete("sb_refresh_token", { path: "/" });
  return redirect("/signin");
};
