export { JwtTokenType } from './zod';
export { UserTokenType } from './zod';
export { RequestErrorType } from './zod';

export interface UserLeague {
  id: number;
  name: string;
  slug: string;
  imageUrl: string;
  years: [string];
}

export interface UserTeam {
  school: string;
  mascot: string;
  color: string;
  abbreviation: string;
  logo: string;
}

export interface User {
  id: string;
  leagues: [UserLeague];
  claims: {
    [key: string]: string;
  };
  firstName: string;
  lastName: string;
  team: UserTeam;
}
