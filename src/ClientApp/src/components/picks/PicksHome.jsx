import React, { useCallback, useMemo } from 'react';
import {
  CircularProgress,
  Container,
  LinearProgress,
  Typography,
} from '@mui/material';
import { makeStyles } from '@mui/styles';

import { withStyles } from '@mui/styles';
import useGetPicks from '../../hooks/useGetPicks';
import PickLayout from './PickLayout';
import { Box } from '@mui/system';

const useStyles = makeStyles(() => ({
  root: {
    position: 'relative',
  },
  container: {
    paddingBottom: '70px',
    maxHeight: '70%',
    backgroundColor: '#fafafa',
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
  const normalize = useCallback(
    (value) => ((value - 0) * 100) / (gameCount - 0),
    [gameCount]
  );

  if (isLoading) {
    return <CircularProgress />;
  }
  return (
    <div className={classes.root}>
      <Container
        maxWidth="sm"
        sx={{
          marginTop: '30px',
          paddingBottom: '40px',
        }}>
        {games.length === 0 ? (
          <Container>
            <Typography variant="h6" align="center">
              There are league games this week
            </Typography>
          </Container>
        ) : (
          games.map((game) => <PickLayout key={game.id} game={game} />)
        )}
      </Container>
      <Box
        sx={{
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
        }}>
        <p>'All Changes will autosave</p>
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'flex-start',
            width: '140px',
            '& > *': {
              padding: 0,
              marginBottom: '5px',
            },
          }}>
          <p>
            {selCount}/{gameCount} Picks Made
          </p>
          <BorderLinearProgress
            sx={{ width: '100%' }}
            variant="determinate"
            value={normalize(selCount)}
          />
        </Box>
      </Box>
    </div>
  );
};

const BorderLinearProgress = withStyles(() => ({
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
