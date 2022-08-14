import { Grid, IconButton, List } from '@mui/material';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../authentication';
import { useLeagueAdmin } from '../hooks/useLeagueAdmin';
import { AddNewMemberButton } from '../components/AddNewMemberButton';
import React from 'react';
import { AdminList } from '../components/AdminList';
import { AdminMemberListItem } from '../components/AdminMemberListItem';
import { AddNewTeamButton } from '../components/AddNewTeamButton';
import { AdminMemberMenuButton } from '../components/AdminMemberMenuButton';
import { AdminTeamListItem } from '../components/AdminTeamListItem';
import { DeleteTeamButton } from '../components/DeleteTeamButton';
import DeleteIcon from '@mui/icons-material/Delete';
import { red } from '@mui/material/colors';
import { useMutation, useQueryClient } from 'react-query';
import { leagueAgent } from '../utils/leagueAgent';
import { useLeague } from '../contexts/LeagueProvider';
import { LeagueKeys } from '../utils/queryKeys';

function DeletePendingMember({ memberId }: { memberId: string }) {
  const queryClient = useQueryClient();
  const league = useLeague();
  const { mutateAsync } = useMutation(
    () =>
      leagueAgent.removePendingMember({
        leagueId: league!.id,
        season: league!.season,
        memberId: memberId,
      }),
    {
      onSuccess: async () => {
        await queryClient.invalidateQueries(LeagueKeys.LeagueAdmin);
      },
    }
  );

  return (
    <IconButton onClick={async () => await mutateAsync()}>
      <DeleteIcon sx={{ color: red[500] }} />
    </IconButton>
  );
}

function DeletePendingTeam({ teamId }: { teamId: number }) {
  const queryClient = useQueryClient();
  const league = useLeague();
  const { mutateAsync } = useMutation(
    () =>
      leagueAgent.removePendingTeam({
        leagueId: league!.id,
        season: league!.season,
        teamId: teamId,
      }),
    {
      onSuccess: async () => {
        await queryClient.invalidateQueries(LeagueKeys.LeagueAdmin);
      },
    }
  );

  return (
    <IconButton onClick={async () => await mutateAsync()}>
      <DeleteIcon sx={{ color: red[500] }} />
    </IconButton>
  );
}

export const LeagueAdmin = () => {
  const { user } = useAuth();
  const location = useLocation();

  const { data } = useLeagueAdmin();

  if (!user || !data) {
    return null;
  }
  if (user.claims['admin'] !== 'true') {
    return <Navigate to={'/unauthorized'} state={{ from: location }} />;
  }

  return (
    <Grid container spacing={2} justifyContent="center" direction="row">
      <Grid item xs={12} sm={6}>
        <AdminList
          title="Members"
          addButton={
            <AddNewMemberButton sx={{ marginRight: { xs: 3, sm: 0 } }} />
          }>
          <List sx={{ width: '100%' }}>
            {data.members.map((member) => (
              <AdminMemberListItem
                key={member.id}
                member={member}
                secondaryAction={<AdminMemberMenuButton member={member} />}
              />
            ))}
          </List>
        </AdminList>
      </Grid>

      <Grid item xs={12} sm={6}>
        <AdminList
          title="Teams"
          addButton={
            <AddNewTeamButton sx={{ marginRight: { xs: 3, sm: 0 } }} />
          }>
          <List sx={{ width: '100%' }}>
            {data.teams.map((team) => (
              <AdminTeamListItem
                key={team.id}
                team={team}
                secondaryAction={<DeleteTeamButton team={team} />}
              />
            ))}
          </List>
        </AdminList>
      </Grid>
      {data.pendingMembers.length > 0 && (
        <Grid item xs={12} sm={6}>
          <AdminList title="Pending Members">
            <List sx={{ width: '100%' }}>
              {data.pendingMembers.map((member) => (
                <AdminMemberListItem
                  key={member.id}
                  member={member}
                  secondaryAction={<DeletePendingMember memberId={member.id} />}
                />
              ))}
            </List>
          </AdminList>
        </Grid>
      )}

      {data.pendingTeams.length > 0 && (
        <Grid item xs={12} sm={6}>
          <AdminList title=" Pending Teams">
            <List sx={{ width: '100%' }}>
              {data.pendingTeams.map((team) => (
                <AdminTeamListItem
                  key={team.id}
                  team={team}
                  secondaryAction={<DeletePendingTeam teamId={team.id} />}
                />
              ))}
            </List>
          </AdminList>
        </Grid>
      )}
    </Grid>
  );
};
