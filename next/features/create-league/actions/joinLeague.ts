"use server";

import { z } from "zod";

export const joinLeagueSchema = z.object({
  userId: z.string(),
  leagueId: z.string(),
});

export type JoinLeagueReturnType = { error?: string };

export const joinLeague = async (
  prevState: JoinLeagueReturnType,
  formData: FormData,
): Promise<JoinLeagueReturnType> => {
  const data = joinLeagueSchema.parse(Object.fromEntries(formData));
  console.log(data);
  return {
    error: "Not implemented yet",
  };
};
