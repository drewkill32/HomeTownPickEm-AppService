import axios from 'axios';
import { useQuery } from 'react-query';
import {
  Divider,
  List,
  ListItem,
  ListItemSecondaryAction,
  ListItemText,
  Typography,
} from '@mui/material';
import Schedule from '../../../components/Schedule';
import { useLeague } from '../contexts/LeagueProvider';
import { useWeek } from '../../SeasonPicks/hooks/useWeek';

type WeeklyRank = {
  userId: string;
  user: string;
  totalPoints: number;
  img: string;
  tiebreaker?: {
    predicted: number;
    diff: number;
    absDiff: number;
  };
};

const WeeklyLeaderboard = () => {
  const { seasonId } = useLeague();
  const { week } = useWeek();

  const { isLoading, data: ranks } = useQuery(
    ['weekly-leaderboard', seasonId, week],
    () =>
      axios
        .get<
          WeeklyRank[]
        >(`/api/leaderboard/weekly/season/${seasonId}/week/${week}`)
        .then((res) => res.data),
  );
  if (isLoading)
    return (
      <p>
        <em>Loading...</em>
      </p>
    );
  if (ranks) {
    return (
      <>
        <Schedule sx={{ pb: 4 }} />
        <List>
          <ListItem>
            <ListItemText>
              <Typography color="primary" sx={{ ml: 6 }}>
                Name
              </Typography>
            </ListItemText>
            <ListItemSecondaryAction>
              <Typography color="primary">Points (Tiebreaker)</Typography>
            </ListItemSecondaryAction>
          </ListItem>
          <Divider sx={{ mb: 2 }} />
          {ranks.map((rank, index) => (
            <ListItem key={rank.userId}>
              <img
                onError={(e) => {
                  const target = e.target as HTMLImageElement;
                  return (target.src = '/img/helmet.png');
                }}
                loading={index < 15 ? 'eager' : 'lazy'}
                src={rank.img}
                alt={rank.user}
                width="30"
                height="30"
                style={{ marginInline: '0.5rem' }}
              />
              <ListItemText>{rank.user}</ListItemText>
              <ListItemSecondaryAction>
                <Typography>{`${rank.totalPoints} (${rank.tiebreaker?.diff ?? 'N/A'})`}</Typography>
              </ListItemSecondaryAction>
            </ListItem>
          ))}
        </List>
      </>
    );
  }
  return null;
};

export default WeeklyLeaderboard;
