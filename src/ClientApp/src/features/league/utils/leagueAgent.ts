import axios, { AxiosResponse } from 'axios';
import {
  AddMemberData,
  AvailableTeamsResult,
  League,
  LeagueAdminResult,
  MakeCommissionerData,
  RemoveCommissionerData,
  RemoveMemberData,
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
const removeCommissioner = async (data: RemoveCommissionerData) => {
  await axios.post('/api/league/remove-commissioner', data);
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
  addMember,
  availableTeams,
  admin,
};
