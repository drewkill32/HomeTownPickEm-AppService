import { Button, Divider, Grid, Paper, Stack, Typography } from '@mui/material';
import MainLayout from '../../../layout/MainLayout';

import { styled } from '@mui/material/styles';
import { useAuth } from '../../authentication';
import { useCurrentSeason } from '../hooks/useCurrentSeason';
import { useGetUserLeagues } from '../hooks/useGetUserLeagues';
import { LeagueCard } from '../components/LeagueCard';

const StyledPaper = styled(Paper)(({ theme }) => ({
  height: '100%',
  width: '100%',
  padding: theme.spacing(2),
}));

export const LeagueHome = () => {
  const { user } = useAuth();

  const { data: season } = useCurrentSeason();
  const { data: leagues } = useGetUserLeagues();

  if (!user || !season || !leagues) {
    return <MainLayout />;
  }

  return (
    <>
      <MainLayout>
        <Grid
          container
          spacing={2}
          justifyContent="center"
          alignItems="stretch">
          <Grid item sm={8} sx={{ width: '100%' }}>
            <StyledPaper>
              <Stack
                direction="row"
                spacing={3}
                justifyContent="space-between"
                sx={{
                  mb: 1,
                }}>
                <Typography variant="h5">My Leagues</Typography>
                {user.claims['admin'] === 'true' && (
                  <Button variant="contained">Create League</Button>
                )}
              </Stack>
              <Divider sx={{ my: 2 }} />
              <Grid
                container
                spacing={2}
                justifyContent="center"
                alignItems="stretch">
                {leagues.map((league) => (
                  <Grid item key={league.id}>
                    <LeagueCard
                      league={league}
                      season={season.season}
                      user={user}
                    />
                  </Grid>
                ))}
              </Grid>
            </StyledPaper>
          </Grid>
        </Grid>
      </MainLayout>
    </>
  );
};
export default LeagueHome;
