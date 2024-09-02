"use server";

import {
  JoinLeagueReturnType,
  joinLeagueSchema,
} from "@/features/create-league/validation";

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
