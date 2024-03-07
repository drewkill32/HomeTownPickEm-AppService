import { createClient } from "@/utils/supabase/server";
import Signout from "./Signout";

export default async function Page() {
  const supabase = createClient();
  await supabase.auth.signOut();
  return <Signout />;
}
