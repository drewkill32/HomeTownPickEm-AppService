import React from 'react';
import { Typography, Paper, Divider } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import format from 'date-fns/format';

import formatDistanceToNow from 'date-fns/formatDistanceToNow';
import isBefore from 'date-fns/isBefore';
import grey from '@material-ui/core/colors/grey';
import { useGame } from '../../hooks/useGame';

import { useMakePick } from '../../hooks/useMakePick';
import isAfter from 'date-fns/isAfter';
import { SplitButton, PickButton } from './SplitButton';
import Head2HeadFooter from './Head2HeadFooter';

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
    selectHome,
    selectAway,
    selectSplit,
  } = useGame(currentGame);

  const { mutateAsync } = useMakePick();

  const handleClick = async (picks, type) => {
    switch (type) {
      case 'home':
        selectHome();
        await mutateAsync(picks);
        break;
      case 'split':
        selectSplit();
        await mutateAsync(picks);
        break;
      case 'away':
        selectAway();
        await mutateAsync(picks);
        break;
      default:
        break;
    }
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
          disabled={isAfter(new Date(), game.cutoffDate)}
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
          <Typography>{game.away.points}</Typography>
        </PickButton>
        {game.head2Head && (
          <SplitButton
            disabled={isAfter(new Date(), game.cutoffDate)}
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
          disabled={isAfter(new Date(), game.cutoffDate)}
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
          <Typography>{game.home.points}</Typography>
        </PickButton>
      </div>
      {game.winner !== 'pending' && game.head2Head && <Head2HeadFooter />}
    </Paper>
  );
};

export default PickLayout;
