import React, { useCallback, useMemo } from 'react';
import { CircularProgress, Container, LinearProgress } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';

import { withStyles } from '@material-ui/styles';
import useGetPicks from '../../hooks/useGetPicks';
import PickLayout from './PickLayout';
import Schedule from '../Schedule';
import { getUTCDate } from '../../utils/dates';
import isAfter from 'date-fns/isAfter';

const useStyles = makeStyles((theme) => ({
  root: {
    position: 'relative',
  },
  container: {
    paddingBottom: '70px',
    maxHeight: '70%',
  },
  footer: {
    zIndex: '999',
    position: 'fixed',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingInline: '2rem',
    background:
      'linear-gradient(180deg, rgba(255,255,255,1) 0%, rgba(235,235,235,1) 85%, #a1a1a1 100%)',
    bottom: 0,
    left: 0,
    right: 0,
    height: '60px',
    borderTop: '1px solid #353535',
  },
  progress: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'flex-start',
    width: '120px',
    '& > *': {
      padding: 0,
      marginBottom: '5px',
    },
  },
}));

const PicksHome = () => {
  const classes = useStyles();

  const { isLoading, data: games } = useGetPicks();

  const gameCount = useMemo(() => (games ? games.length : 0), [games]);

  const selCount = useMemo(
    () =>
      games
        ? games.filter((g) => g.picks.every((p) => p.selectedTeamId !== null))
            .length
        : 0,
    [games]
  );
  const normalise = useCallback(
    (value) => ((value - 0) * 100) / (gameCount - 0),
    [gameCount]
  );

  const isPastCutoff = useMemo(() => {
    if (!games) return false;

    const minCutoff = games.reduce((a, b) =>
      a.cutoffDate < b.cutoffDate ? a : b
    ).cutoffDate;

    return isAfter(getUTCDate(), minCutoff);
  }, [games]);
  if (isLoading) {
    return <CircularProgress />;
  }
  return (
    <div className={classes.root}>
      <Schedule />
      <Container maxWidth="sm" className={classes.container}>
        {games.map((game) => (
          <PickLayout key={game.id} game={game} />
        ))}
      </Container>
      <div className={classes.footer}>
        <p>
          {isPastCutoff ? 'All picks are locked' : 'All Changes will autosave'}
        </p>
        <div className={classes.progress}>
          <p>
            {selCount}/{gameCount} Picks Made
          </p>
          <BorderLinearProgress
            variant="determinate"
            value={normalise(selCount)}
          />
        </div>
      </div>
    </div>
  );
};

const BorderLinearProgress = withStyles((theme) => ({
  root: {
    height: 6,
    borderRadius: 2,
    width: '120px',
  },
  colorPrimary: {
    backgroundColor: '#acacac',
  },
  bar: {
    borderRadius: 5,
    backgroundColor: '#1a90ff',
  },
}))(LinearProgress);

export default PicksHome;
