import { Avatar, ListItem, ListItemAvatar, ListItemText } from '@mui/material';
import { blueGrey } from '@mui/material/colors';
import React from 'react';
import { LeagueAdminTeam } from '../types';

interface AdminTeamListItemProps {
  team: LeagueAdminTeam;
  secondaryAction?: React.ReactNode;
}

export function AdminTeamListItem({
  team,
  secondaryAction,
}: AdminTeamListItemProps) {
  return (
    <ListItem secondaryAction={secondaryAction}>
      <ListItemAvatar>
        <Avatar
          sx={{
            bgcolor: team.altColor || blueGrey[500],
            '& img': {
              width: '25px',
              height: '25px',
            },
          }}
          src={team.logo}
          alt={team.name}
        />
      </ListItemAvatar>
      <ListItemText primary={team.name}></ListItemText>
    </ListItem>
  );
}
