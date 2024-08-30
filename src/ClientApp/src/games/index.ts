export interface TiebreakerGame {
  id: number;
  weeklyGameId: string;
  season: string;
  week: number;
  seasonType: string;
  startDate: Date;
  startTimeTbd: boolean;
  homePoints: null;
  homeTeam: Team;
  awayTeam: Team;
  awayPoints: null;
  gameFinal: boolean;
  winner: null;
}

export interface Team {
  id: number;
  school: string;
  mascot: string;
  abbreviation: string;
  conference: string;
  division: null;
  color: string;
  altColor: string;
  logo: string;
  name: string;
}
