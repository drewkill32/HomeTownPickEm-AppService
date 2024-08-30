import { Box, Fade, Paper, TextField, Theme, Typography } from '@mui/material';
import React, { FormEvent, useEffect, useState } from 'react';
import { useTiebreakerGame } from '../../features/tiebreaker/hooks/useTiebreakerGame';
import { useLeague } from '../../features/league';
import { useWeek } from '../../features/SeasonPicks/hooks/useWeek';
import { makeStyles } from '@mui/styles';
import Button from '@mui/material/Button';
import { useTieBreakerPick } from '../../features/tiebreaker/hooks/useTieBreakerPick';
import { useSelectTiebreaker } from '../../features/tiebreaker/hooks/useSelectTiebreaker';

import CheckCircleIcon from '@mui/icons-material/CheckCircle';

const useStyles = makeStyles<Theme>(() => ({
  logo: {
    width: '7.4vmin',
    minWidth: '40px',
    minHeight: '40px',
    height: '7.4vmin',
    display: 'block',
    margin: '0 auto',
  },
}));

const TieBreakerPick = () => {
  const { seasonId } = useLeague();
  const { week } = useWeek();
  const { data: game } = useTiebreakerGame({ seasonId: seasonId, week: week });

  const { data: totalScore } = useTieBreakerPick(game?.weeklyGameId ?? '', {
    enabled: Boolean(game),
  });

  const { mutate, isLoading, isSuccess } = useSelectTiebreaker();

  const [score, setScore] = useState(totalScore?.totalPoints?.toString() ?? '');
  const [isVisible, setIsVisible] = useState(false);

  const classes = useStyles();

  useEffect(() => {
    setScore(totalScore?.totalPoints?.toString() ?? '');
  }, [totalScore]);

  useEffect(() => {
    if (isSuccess) {
      setIsVisible(true);

      const timer = setTimeout(() => {
        setIsVisible(false);
      }, 1500);

      // Cleanup function to clear the timer if the component unmounts
      return () => clearTimeout(timer);
    }
  }, [isSuccess]);

  if (!game || !totalScore) {
    return null;
  }

  const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const formData = new FormData(e.target as HTMLFormElement);
    const totalPoints = formData.get('totalPoints');

    if (!totalPoints) {
      return;
    }

    mutate({
      weeklyGameId: game.weeklyGameId,
      totalPoints: parseInt(totalPoints.toString()),
    });
  };

  return (
    <>
      <Typography variant="h6" align="center">
        Weekly Tie Breaker
      </Typography>
      <Paper
        elevation={3}
        sx={{
          mb: 6,
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          flexDirection: 'column',
          gap: 2,
          padding: 3,
        }}>
        <Box
          sx={{
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
          }}>
          <div>
            <img
              className={classes.logo}
              src={game.awayTeam.logo}
              alt={game.awayTeam.name}
            />
            <Typography textAlign="center">
              {game.awayTeam.abbreviation}
            </Typography>
          </div>
          <Typography variant="h6" align="center" sx={{ marginInline: 3 }}>
            @
          </Typography>
          <div>
            <img
              className={classes.logo}
              src={game.homeTeam.logo}
              alt={game.homeTeam.name}
            />
            <Typography textAlign="center">
              {game.homeTeam.abbreviation}
            </Typography>
          </div>
        </Box>

        <Box
          component={'form'}
          onSubmit={handleSubmit}
          sx={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            gap: 2,
          }}>
          <TextField
            id="tiebreakerScore"
            type="number"
            name="totalPoints"
            label="Total Points"
            variant="standard"
            value={score}
            onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
              setScore(event.target.value);
            }}
          />
          <Button variant="contained" type="submit" disabled={isLoading}>
            Save
          </Button>
          <Fade in={isVisible}>
            <CheckCircleIcon color="success" />
          </Fade>
        </Box>
        <Typography>
          Tiebreaker: Predict the combined score of both teams in this week's
          designated game.
        </Typography>
      </Paper>
    </>
  );
};

export default TieBreakerPick;
