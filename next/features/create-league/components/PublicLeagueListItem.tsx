"use client";

import { useSupabase } from "@/components/SupabaseContext";
import { Button } from "@/components/ui/button";
import { joinLeague } from "../actions/joinLeague";
import { useFormState } from "react-dom";
import { useToast } from "@/components/ui/use-toast";
import { useEffect } from "react";

type PublicLeagueListItemProps = {
  id: string;
  name: string;
  description: string;
};
export const PublicLeagueListItem = ({
  id: leagueId,
  name,
  description,
}: PublicLeagueListItemProps) => {
  const { user } = useSupabase();
  if (!user) return null;

  const { toast } = useToast();
  const [state, formAction] = useFormState(joinLeague, {
    error: "",
  });

  useEffect(() => {
    if (state.error) {
      toast({
        variant: "destructive",
        title: "Uh oh! Something went wrong.",
        description: state.error,
      });
    }
  }, [state]);

  return (
    <div className="flex items-center justify-between">
      <div>
        <h3 className="text-lg font-bold">{name}</h3>
        <p className="text-sm leading-none text-gray-500 dark:text-gray-400">
          {description}
        </p>
      </div>
      <form action={formAction}>
        <input type="hidden" value={user.id} name="userId" />
        <input type="hidden" value={leagueId} name="leagueId" />
        <Button size="sm" type="submit">
          Join
        </Button>
      </form>
    </div>
  );
};
