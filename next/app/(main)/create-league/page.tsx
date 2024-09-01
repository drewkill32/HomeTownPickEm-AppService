import { Separator } from "@/components/ui/separator";
import { PublicLeagueList, NewLeagueForm } from "@/features/create-league";

export default async function Page() {
  return (
    <div className="space-y-6">
      <div className="space-y-2">
        <h1 className="text-3xl font-bold">Create a new League</h1>
      </div>
      <NewLeagueForm />
      <Separator className="my-8" />
      <PublicLeagueList />
    </div>
  );
}
