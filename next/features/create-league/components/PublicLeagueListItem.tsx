"use client";

import { useSupabase } from "@/components/SupabaseContext";
import { Button } from "@/components/ui/button";
import { joinLeague } from "../actions/joinLeague";
import { useFormState } from "react-dom";
import { useToast } from "@/components/ui/use-toast";
import { useEffect } from "react";
import { Tables } from "@/database.types";

type PublicLeagueListItemProps = Pick<
  Tables<"leagues">,
  "id" | "name" | "description" | "image_url"
>;
export const PublicLeagueListItem = ({
  id: leagueId,
  name,
  description,
  image_url,
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
    <div className="flex items-center gap-1.5">
      <img
        className="img rounded-full"
        alt={name}
        src={image_url ?? "https://placehold.co/42"}
      ></img>
      <div className="flex-grow">
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
