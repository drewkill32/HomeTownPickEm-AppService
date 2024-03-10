import React from "react";
import { PublicLeagueListItem } from "./PublicLeagueListItem";

export function PublicLeagueList() {
  //TODO: fetch leagues from supabase
  const publicLeagues = [
    {
      id: "36c5d53c-a63c-4685-8324-fc223d08c76b",
      name: "League 1",
      description: "This is the first league",
    },
    {
      id: "0f07a66b-2858-40b8-9e36-825a8a732cdb",
      name: "League 2",
      description: "This is the second league",
    },
    {
      id: "1e621969-5889-4f1e-8c1f-cf983d2e8fad",
      name: "League 3",
      description: "This is the third league",
    },
  ];

  return (
    <div className="space-y-4 pb-6">
      <h2 className="text-center text-xl font-bold">Join a League</h2>
      {publicLeagues.map((league) => (
        <PublicLeagueListItem key={league.id} {...league} />
      ))}
    </div>
  );
}
