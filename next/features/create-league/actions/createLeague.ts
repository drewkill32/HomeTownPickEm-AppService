"use server";

import { newLeagueSchema } from "../validation";

export type JoinLeagueReturnType = { error?: string };
export const createLeague = async (
  _: JoinLeagueReturnType,
  formData: FormData,
): Promise<JoinLeagueReturnType> => {
  const data = Object.fromEntries(formData);
  const result = newLeagueSchema.safeParse(data);
  console.log("🚀 ~ result:", result);

  if (!result.success) {
    console.log({ error: result.error.errors });
    return {
      error: result.error.issues.map((issue) => issue.message).join(", "),
    };
  }
  return {
    error: "Not implemented yet",
  };
};
