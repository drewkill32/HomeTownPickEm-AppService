import axios, { AxiosResponse } from 'axios';
import {
  AddMemberData,
  AddTeamData,
  AvailableTeamsResult,
  League,
  LeagueAdminResult,
  MakeCommissionerData,
  RemoveCommissionerData,
  RemoveLeagueData,
  RemoveMemberData,
  RemoveTeamData,
} from '../types';

const removeMember = async (data: RemoveMemberData) => {
  await axios.post('/api/league/remove-member', data);
};

const makeCommissioner = async (data: MakeCommissionerData) => {
  await axios.post('/api/league/make-commissioner', data);
};

const availableTeams = async (league: League | null) => {
  return axios
    .get(`/api/League/${league?.id}/${league?.season}/availableTeams`)
    .then((res: AxiosResponse<AvailableTeamsResult[]>) => res.data);
};

const addMember = async (data: AddMemberData) => {
  await axios.post('/api/user/invite', data);
};

const getLeague = async (slug: string, season: string) => {
  return await axios
    .get(`/api/league/${slug}/${season}`)
    .then((res: AxiosResponse<League>) => res.data);
};

const addTeam = async (data: AddTeamData) => {
  await axios.post('/api/league/add-team', data);
};

const removeTeam = async (data: RemoveTeamData) => {
  await axios.post('/api/league/remove-team', data);
};

const removePendingTeam = async (data: RemoveTeamData) => {
  await axios.post('/api/league/remove-pending-team', data);
};

const removePendingMember = async (data: RemoveMemberData) => {
  await axios.post('/api/league/remove-pending-member', data);
};
const removeCommissioner = async (data: RemoveCommissionerData) => {
  await axios.post('/api/league/remove-commissioner', data);
};
const removeLeague = async ({ leagueId, season }: RemoveLeagueData) => {
  await axios.delete(`/api/League?LeagueId=${leagueId}&Season=${season}`);
};

const admin = async (league: League | null) => {
  return axios
    .get(`/api/League/${league?.id}/${league?.season}/MembersTeams`)
    .then((res: AxiosResponse<LeagueAdminResult>) => res.data);
};

export const leagueAgent = {
  removeMember,
  removeCommissioner,
  makeCommissioner,
  removeLeague,
  addMember,
  removePendingTeam,
  removePendingMember,
  availableTeams,
  addTeam,
  getLeague,
  removeTeam,
  admin,
};
