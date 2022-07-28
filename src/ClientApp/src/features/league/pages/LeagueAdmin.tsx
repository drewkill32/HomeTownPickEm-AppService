import {
  Avatar,
  Divider,
  IconButton,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
  Stack,
} from '@mui/material';
import { Navigate, useLocation } from 'react-router-dom';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import { useAuth } from '../../authentication';
import { useLeagueAdmin } from '../hooks/useLeagueAdmin';
import { blueGrey } from '@mui/material/colors';

export const LeagueAdmin = () => {
  const { user } = useAuth();
  let location = useLocation();

  const { data } = useLeagueAdmin();

  if (!user || !data) {
    return null;
  }
  if (!user.roles.includes('admin')) {
    return <Navigate to={'/unauthorized'} state={{ from: location }} />;
  }
  return (
    <Stack spacing={2} justifyContent="center" direction="row">
      <List sx={{ width: '100%' }}>
        {data.members.map((member) => (
          <ListItem
            key={member.id}
            secondaryAction={
              <IconButton>
                <MoreVertIcon />
              </IconButton>
            }>
            <ListItemAvatar>
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
            </ListItemAvatar>
            <ListItemText primary={member.fullName}></ListItemText>
          </ListItem>
        ))}
      </List>
      <Divider variant="middle" orientation="vertical" flexItem />

      <List sx={{ width: '100%' }}>
        {data.teams.map((team) => (
          <ListItem
            key={team.id}
            secondaryAction={
              <IconButton>
                <MoreVertIcon />
              </IconButton>
            }>
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
        ))}
      </List>
    </Stack>
  );
};
