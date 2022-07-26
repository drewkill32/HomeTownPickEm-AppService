import { z } from 'zod';

export const UserToken = z.object({
  access_token: z.string(),
  expires_in: z.number(),
  refresh_token: z.string(),
});

export const RequestError = z.object({
  type: z.string(),
  title: z.string(),
  status: z.number(),
  detail: z.optional(z.string()),
});

export const JwtToken = z.object({
  exp: z.number(),
  role: z.optional(z.union([z.string(), z.array(z.string())])),
});

export const LocationState = z.object({
  from: z.optional(
    z.union([
      z.string(),
      z.object({
        pathname: z.string(),
        search: z.string(),
      }),
    ])
  ),
});

export type JwtTokenType = z.infer<typeof JwtToken>;
export type UserTokenType = z.infer<typeof UserToken>;
export type RequestErrorType = z.infer<typeof RequestError>;
