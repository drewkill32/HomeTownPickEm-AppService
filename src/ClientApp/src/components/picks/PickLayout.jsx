import { Link, Box, Divider, Paper, Tooltip, Typography } from '@mui/material';
import { makeStyles } from '@mui/styles';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import format from 'date-fns/format';

import { isBefore, formatDistance } from 'date-fns';
import grey from '@mui/material/colors/grey';
import { useGame } from '../../hooks/useGame';

import { useMakePick } from '../../hooks/useMakePick';
import { PickButton, SplitButton } from './PickButtons';
import Head2HeadFooter from './Head2HeadFooter';
import ArrowDropUpIcon from '@mui/icons-material/ArrowDropUp';
import UserPicks from './UserPicks';
import { useParams } from 'react-router-dom';
import { useAuth } from '../../features/authentication';

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
    position: 'relative',
  },
}));

const DisabledTooltip = ({ children, disabled }) => {
  if (disabled) {
    return (
      <Tooltip
        enterDelay={500}
        title="You are unable to change your pick. Either the game has started or the pick is manually locked.">
        {children}
      </Tooltip>
    );
  }
  return children;
};

const PickLayout = ({ game: currentGame, locked }) => {
  const classes = useStyles();

  const { league, season } = useParams();

  const { game, homeSelected, awaySelected, splitSelected, pastCutoff } =
    useGame(currentGame);

  const { user } = useAuth();

  const { mutateAsync } = useMakePick();
  const isLocked = locked || pastCutoff;
  const handleClick = async (selectedTeamIds, type) => {
    await mutateAsync({
      leagueSlug: league,
      season: season,
      gameId: game.id,
      selectedTeamIds,
    });
  };

  return (
    <Paper className={classes.root}>
      {isBefore(game.currentDateTime, game.cutoffDate) && (
        <Typography gutterBottom className={classes.subtitle} align="center">
          Pick will lock in{' '}
          {formatDistance(game.currentDateTime, game.cutoffDate)}
          <LockOutlinedIcon className={classes.subtitleLock} />
        </Typography>
      )}
      <Divider />
      <Typography gutterBottom align="center">
        {format(game.startDate, 'E MMM do h:mm a')}
      </Typography>
      <DisabledTooltip disabled={isLocked}>
        <div className={classes.buttonContainer}>
          {isLocked && (
            <LockOutlinedIcon
              sx={{
                position: 'absolute',
                left: '50%',
                top: '50%',
                opacity: 0.7,
                transform: 'translate(-50%, -50%)',
                fontSize: '80px',
                zIndex: '999',
                color: grey[500],
              }}
            />
          )}
          <PickButton
            team={game.away}
            disabled={isLocked}
            noWrap={game.winner !== 'Pending'}
            onClick={() =>
              handleClick(
                game.head2Head ? [game.away.id, game.away.id] : [game.away.id],
                'away',
              )
            }
            selected={awaySelected}>
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
              disabled={isLocked}
              selected={splitSelected}
              onClick={() => handleClick([game.away.id, game.home.id], 'split')}
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
            disabled={isLocked}
            noWrap={game.winner !== 'Pending'}
            onClick={() =>
              handleClick(
                game.head2Head ? [game.home.id, game.home.id] : [game.home.id],
                'home',
              )
            }
            selected={homeSelected}>
            <Box>
              {pastCutoff && game.home.percentPicked !== null && (
                <Typography noWrap className={classes.percentPicked}>
                  {game.home.percentPicked}% picked
                </Typography>
              )}
              <Typography variant="h6" gutterBottom>
                {game.home.points}
              </Typography>
              {game.winner === 'Home' && <ArrowDropUpIcon color={'error'} />}
            </Box>
          </PickButton>
        </div>
      </DisabledTooltip>
      {game.winner === 'Pending' && game.head2Head && <Head2HeadFooter />}
      {user.claims['admin'] === 'true' && (
        <Link
          href={`https://www.espn.com/college-football/game/_/gameId/${game.id}`}
          rel="noopener noreferrer"
          target="_blank"
          align="center"
          gutterBottom>
          ESPN
        </Link>
      )}

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
