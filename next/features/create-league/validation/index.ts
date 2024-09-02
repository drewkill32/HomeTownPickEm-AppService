import { z } from "zod";

export const newLeagueSchema = z.object({
  name: z.string().min(3).max(50),
  description: z.string().optional(),
  password: z.string().optional(),
  public: z.coerce.boolean(),
});

export type NewLeague = z.infer<typeof newLeagueSchema>;

export type NewLeagueNameKeys = keyof NewLeague;

export const joinLeagueSchema = z.object({
  userId: z.string(),
  leagueId: z.string(),
});

export type JoinLeagueReturnType = { error?: string };
