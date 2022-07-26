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

export const LeagueAdmin = () => {
  const { user } = useAuth();
  let location = useLocation();
  if (!user) {
    return null;
  }
  if (!user.roles.includes('admin')) {
    return <Navigate to={'/unauthorized'} state={{ from: location }} />;
  }
  return (
    <Stack spacing={2} justifyContent="center" direction="row">
      <List sx={{ width: '100%' }}>
        <ListItem
          secondaryAction={
            <IconButton>
              <MoreVertIcon />
            </IconButton>
          }
        >
          <ListItemAvatar>
            <Avatar>AK</Avatar>
          </ListItemAvatar>
          <ListItemText primary="Drew Killion"></ListItemText>
        </ListItem>

        <ListItem
          secondaryAction={
            <IconButton>
              <MoreVertIcon />
            </IconButton>
          }
        >
          <ListItemAvatar>
            <Avatar>AK</Avatar>
          </ListItemAvatar>
          <ListItemText primary="Drew Killion"></ListItemText>
        </ListItem>
      </List>
      <Divider variant="middle" orientation="vertical" flexItem />

      <List sx={{ width: '100%' }}>
        <ListItem
          secondaryAction={
            <IconButton>
              <MoreVertIcon />
            </IconButton>
          }
        >
          <ListItemAvatar>
            <Avatar>AK</Avatar>
          </ListItemAvatar>
          <ListItemText primary="Drew Killion"></ListItemText>
        </ListItem>
      </List>
    </Stack>
  );
};
