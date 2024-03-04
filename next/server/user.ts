import { createClient } from "@/utils/supabase/server";

export const user = async () => {
  const supabase = createClient();

  const {
    data: { user },
  } = await supabase.auth.getUser();

  return {
    user,
    isAuthenticated: Boolean(user),
  };
};
