export interface League {
  id: number;
  name: string;
  slug: string;
  imageUrl: string;
  season: string;
}

export interface LeagueAdminResult {
  members: LeagueAdminMember[];
  teams: LeagueAdminTeam[];
  pendingMembers: LeagueAdminMember[];
  pendingTeams: LeagueAdminTeam[];
}

export interface RemoveMemberData {
  leagueId: number;
  season: string;
  memberId: string;
}

export interface MakeCommissionerData {
  leagueId: number;
  memberId: string;
}

export interface AddMemberData {
  email: string;
  lastName: string;
  firstName: string;
  leagueId: number;
  season: string;
  teamId?: number | null;
}

export interface AddTeamData {
  leagueId: number;
  season: string;
  teamId?: number | null;
}

export interface RemoveTeamData extends AddTeamData {}

export interface RemoveCommissionerData extends MakeCommissionerData {}

export interface RemoveLeagueData {
  leagueId: number;
  season: string;
}

export interface LeagueAdminMember {
  firstName: string;
  lastName: string;
  email: string;
  id: string;
  profileImg: string;
  fullName: string;
  initials: string;
  color: string;
  isCommissioner: boolean;
}

export interface AvailableTeamsResult {
  id: number;
  name: string;
  logo: string;
}

export interface LeagueAdminTeam {
  color: string;
  altColor: string;
  logo: string;
  school: string;
  mascot: string;
  name: string;
  id: number;
}
