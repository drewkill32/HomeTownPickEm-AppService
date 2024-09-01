import {
  Divider,
  List,
  ListItem,
  ListItemSecondaryAction,
  ListItemText,
  Typography,
} from '@mui/material';
import { useQuery } from 'react-query';
import axios from 'axios';
import { useParams } from 'react-router-dom';

const LeaderboardTable = () => {
  const { league, season } = useParams();
  const { isLoading, data: ranks } = useQuery(
    ['leaderboard', league, season],
    () =>
      axios
        .get(`/api/league/${league}/${season}/leaderboard`)
        .then((res) => res.data),
  );

  if (isLoading)
    return (
      <p>
        <em>Loading...</em>
      </p>
    );

  if (!ranks) {
    return null;
  }
  return (
    <List>
      <ListItem>
        <ListItemText>
          <Typography color="primary" sx={{ ml: 6 }}>
            Name
          </Typography>
        </ListItemText>
        <ListItemSecondaryAction>
          <Typography color="primary">Points</Typography>
        </ListItemSecondaryAction>
      </ListItem>
      <Divider sx={{ mb: 2 }} />
      {ranks.map((rank, index) => (
        <ListItem key={index}>
          <img
            onError={(e) => (e.target.src = '/img/helmet.png')}
            loading={index < 15 ? 'eager' : 'lazy'}
            src={rank.teamLogo}
            alt={rank.teamName}
            width="30"
            height="30"
            style={{ marginInline: '0.5rem' }}
          />
          <ListItemText>{rank.user}</ListItemText>
          <ListItemSecondaryAction>{rank.totalPoints}</ListItemSecondaryAction>
        </ListItem>
      ))}
    </List>
  );
};

export default LeaderboardTable;
