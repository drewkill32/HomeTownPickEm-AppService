import { z } from "zod";

export const newLeagueSchema = z.object({
  leagueName: z.string().min(3).max(50),
  description: z.string().optional(),
  password: z.string().optional(),
  public: z.coerce.boolean(),
});
