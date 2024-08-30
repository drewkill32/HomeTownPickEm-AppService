import React, { useCallback, useEffect, useMemo } from 'react';
import {
  Container,
  FormControlLabel,
  LinearProgress,
  Switch,
  Typography,
} from '@mui/material';
import { makeStyles } from '@mui/styles';

import { withStyles } from '@mui/styles';
import useGetPicks from '../../hooks/useGetPicks';
import PickLayout from './PickLayout';
import { Box } from '@mui/system';
import { PickLayoutSkeleton } from './PicksHomeSkeleton';
import { useLayout } from '../../layout/LayoutContext';
import { isPast } from 'date-fns';
import { useSchedule } from '../../hooks/useSchedule';
import { useWeek } from '../../features/SeasonPicks/hooks/useWeek';
import useLocalStorage from '../../hooks/useLocalStorage';
import TieBreakerPick from './TieBreakerPick';

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

function LockPicks(props) {
  const { week } = useWeek();
  const { data } = useSchedule();
  if (!data) {
    return null;
  }
  const cur = data.find((x) => x.week === week);
  const allGamesComplete = isPast(cur.lastGameStart);
  if (allGamesComplete) {
    return null;
  }
  return (
    <Box
      sx={{
        display: 'flex',
        alignContent: 'center',
        justifyContent: 'center',
        paddingBottom: 3,
      }}>
      <FormControlLabel
        control={<Switch checked={props.checked} onChange={props.onChange} />}
        label="Lock all Picks"
      />
    </Box>
  );
}

const PicksHome = () => {
  const { setPaddingBottom } = useLayout();
  const [locked, setLocked] = useLocalStorage('locked', false);
  const classes = useStyles();

  useEffect(() => {
    setPaddingBottom('60px');
    return () => setPaddingBottom(0);
  }, [setPaddingBottom]);

  const { isLoading, data: games } = useGetPicks();

  const gameCount = useMemo(() => (games ? games.length : 0), [games]);

  const selCount = useMemo(
    () => (games ? games.filter((g) => g.picks.length > 0).length : 0),
    [games],
  );
  const normalize = useCallback(
    (value) => ((value - 0) * 100) / (gameCount - 0),
    [gameCount],
  );

  if (isLoading || !games) {
    return <PickLayoutSkeleton />;
  }

  return (
    <div className={classes.root}>
      <Container
        maxWidth="sm"
        sx={{
          marginTop: '30px',
          paddingBottom: '40px',
        }}>
        <>
          <TieBreakerPick />
          <LockPicks
            checked={locked}
            onChange={(e) => setLocked(e.target.checked)}
          />
          {games.length === 0 ? (
            <Container>
              <Typography variant="h6" align="center">
                There are no league games this week
              </Typography>
            </Container>
          ) : (
            games.map((game) => (
              <PickLayout locked={locked} key={game.id} game={game} />
            ))
          )}
        </>
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
        <Typography>All Changes will autosave</Typography>
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
          <Typography noWrap>
            {selCount}/{gameCount} Picks Made
          </Typography>
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
