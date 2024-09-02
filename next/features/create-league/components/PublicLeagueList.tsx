import { PublicLeagueListItem } from "./PublicLeagueListItem";
import { createClient } from "@/utils/supabase/client";
import { getUser } from "@/server/user";

export async function PublicLeagueList() {
  const supabase = createClient();
  const { user } = await getUser();

  const { data, error } = await supabase
    .from("leagues")
    .select(
      `
      id,
      name,
      description,
      image_url,
      owner_id,
      league_seasons!inner(
      year, is_active
      )
      `,
    )
    .eq("is_public", true)
    .eq("league_seasons.is_active", true)
    .neq("owner_id", user!.id)
    .eq("league_seasons.year", new Date().getFullYear());

  let leagues = data ?? [];

  if (error) {
    console.error(error);
    leagues = [];
  }

  return (
    <div className="space-y-4 pb-6">
      <h2 className="text-center text-xl font-bold">Join a League</h2>
      {leagues.map((league) => (
        <PublicLeagueListItem key={league.id} {...league} />
      ))}
    </div>
  );
}
