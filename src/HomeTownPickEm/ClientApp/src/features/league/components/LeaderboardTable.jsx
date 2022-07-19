import {
  Avatar,
  Container,
  List,
  ListItem,
  ListItemAvatar,
  ListItemSecondaryAction,
  ListItemText,
  Typography,
} from '@mui/material';

const LeaderboardTable = ({ ranks }) => {
  return (
    <Container maxWidth="md">
      <List>
        <ListItem>
          <ListItemText>
            <Typography color="primary">Name</Typography>
          </ListItemText>
          <ListItemSecondaryAction>
            <Typography color="primary">Points</Typography>
          </ListItemSecondaryAction>
        </ListItem>
        {ranks.map((rank, index) => (
          <ListItem key={index}>
            <img
              onError={(e) => (e.target.src = 'https://placehold.jp/50x50.png')}
              loading={index < 15 ? 'eager' : 'lazy'}
              src={rank.teamLogo}
              alt={rank.teamName}
              width="30"
              height="30"
              style={{ marginInline: '0.5rem' }}
            />
            <ListItemText>{rank.user}</ListItemText>
            <ListItemSecondaryAction>
              {rank.totalPoints}
            </ListItemSecondaryAction>
          </ListItem>
        ))}
      </List>
    </Container>
  );
};

export default LeaderboardTable;
