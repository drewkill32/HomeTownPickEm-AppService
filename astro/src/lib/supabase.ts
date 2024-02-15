import { createClient } from "@supabase/supabase-js";

console.log({ env: import.meta.env });
export const supabase = createClient(
  import.meta.env.SUPABASE_URL,
  import.meta.env.SUPABASE_ANON_KEY,
);
