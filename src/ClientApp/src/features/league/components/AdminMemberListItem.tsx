import { LeagueAdminMember } from '../types';
import {
  Avatar,
  Badge,
  ListItem,
  ListItemAvatar,
  ListItemText,
} from '@mui/material';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import { blueGrey } from '@mui/material/colors';
import React from 'react';

interface AdminMemberListItemProps {
  member: LeagueAdminMember;
  secondaryAction?: React.ReactNode;
}

export function AdminMemberListItem({
  member,
  secondaryAction,
}: AdminMemberListItemProps) {
  return (
    <ListItem secondaryAction={secondaryAction}>
      <ListItemAvatar>
        <Badge
          badgeContent={
            member.isCommissioner && (
              <CheckCircleIcon
                color="primary"
                sx={{ fontSize: '15px', marginLeft: '-10px', marginTop: '5px' }}
              />
            )
          }>
          <Avatar
            sx={{
              bgcolor: member.color || blueGrey[500],
              '& img': {
                width: '25px',
                height: '25px',
              },
            }}
            src={member.profileImg}>
            {member.initials}
          </Avatar>
        </Badge>
      </ListItemAvatar>
      <ListItemText primary={member.fullName}></ListItemText>
    </ListItem>
  );
}
