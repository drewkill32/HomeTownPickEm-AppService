import { Divider, Paper, Typography } from '@mui/material';
import { makeStyles } from '@mui/styles';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import format from 'date-fns/format';

import formatDistanceToNow from 'date-fns/formatDistanceToNow';
import isBefore from 'date-fns/isBefore';
import grey from '@mui/material/colors/grey';
import { useGame } from '../../hooks/useGame';

import { useMakePick } from '../../hooks/useMakePick';
import { PickButton, SplitButton } from './PickButtons';
import Head2HeadFooter from './Head2HeadFooter';
import ArrowDropUpIcon from '@mui/icons-material/ArrowDropUp';
import UserPicks from './UserPicks';

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
    flexDirection: 'column',
    marginBottom: theme.spacing(3),
  },
  details: {
    display: 'flex',
    flexDirection: 'column',
  },

  content: {
    flex: '1 0 auto',
  },
  subtitle: {
    color: grey[500],
    fontSize: '0.7rem',
  },
  percentPicked: {
    fontSize: '0.9rem',
    color: grey[700],
  },
  subtitleLock: {
    color: grey[500],
    fontSize: '0.7rem',
    marginLeft: theme.spacing(1),
  },
  buttonContainer: {
    display: 'flex',
    justifyContent: 'space-between',
    gap: theme.spacing(2),
    paddingInline: theme.spacing(2),
    marginBottom: theme.spacing(1),
  },
}));

const PickLayout = ({ game: currentGame }) => {
  const classes = useStyles();

  const {
    game,
    homeSelected,
    awaySelected,
    splitSelected,
    pastCutoff,
    selectHome,
    selectAway,
    selectSplit,
  } = useGame(currentGame);

  const { mutateAsync } = useMakePick();
  const handleClick = async (picks, type) => {
    switch (type) {
      case 'home':
        selectHome();
        break;
      case 'split':
        selectSplit();
        break;
      case 'away':
        selectAway();
        break;
      default:
        break;
    }
    await mutateAsync(picks);
  };

  return (
    <Paper className={classes.root}>
      {isBefore(new Date(), game.cutoffDate) && (
        <Typography gutterBottom className={classes.subtitle} align="center">
          Pick will lock in {formatDistanceToNow(game.cutoffDate)}
          <LockOutlinedIcon className={classes.subtitleLock} />
        </Typography>
      )}
      <Divider />
      <Typography gutterBottom align="center">
        {format(game.startDate, 'E MMM do h:mm a')}
      </Typography>
      <div className={classes.buttonContainer}>
        <PickButton
          team={game.away}
          disabled={pastCutoff}
          noWrap={pastCutoff}
          onClick={() =>
            handleClick(
              game.picks.map((p) => ({
                pickId: p.id,
                selectedTeamId: game.away.id,
              })),
              'away'
            )
          }
          selected={awaySelected}
        >
          {pastCutoff && game.away.percentPicked !== null && (
            <Typography noWrap className={classes.percentPicked}>
              {game.away.percentPicked}% picked
            </Typography>
          )}
          <Typography variant="h6">{game.away.points}</Typography>
          {game.winner === 'Away' && <ArrowDropUpIcon />}
        </PickButton>
        {game.head2Head && (
          <SplitButton
            disabled={pastCutoff}
            selected={splitSelected}
            onClick={() =>
              handleClick(
                [
                  { pickId: game.picks[0].id, selectedTeamId: game.away.id },
                  { pickId: game.picks[1].id, selectedTeamId: game.home.id },
                ],
                'split'
              )
            }
            teams={[
              {
                ...game.away,
              },
              {
                ...game.home,
              },
            ]}
          />
        )}
        <PickButton
          team={game.home}
          disabled={pastCutoff}
          noWrap={pastCutoff}
          onClick={() =>
            handleClick(
              game.picks.map((p) => ({
                pickId: p.id,
                selectedTeamId: game.home.id,
              })),
              'home'
            )
          }
          selected={homeSelected}
        >
          {pastCutoff && game.home.percentPicked !== null && (
            <Typography noWrap className={classes.percentPicked}>
              {game.home.percentPicked}% picked
            </Typography>
          )}
          <Typography variant="h6">{game.home.points}</Typography>
          {game.winner === 'Home' && <ArrowDropUpIcon />}
        </PickButton>
      </div>
      {game.winner === 'Pending' && game.head2Head && <Head2HeadFooter />}
      {}
      {pastCutoff && (
        <UserPicks
          homeId={game.home.id}
          awayId={game.away.id}
          gameId={game.id}
        />
      )}
    </Paper>
  );
};

export default PickLayout;
