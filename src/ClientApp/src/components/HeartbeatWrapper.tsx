import React, { useEffect, useState } from 'react';
import { useQuery, useQueryClient } from 'react-query';
import axios from 'axios';
import { useLocation } from 'react-router-dom';
import { Box, CircularProgress, Stack, Typography, Fade } from '@mui/material';
import SportsFootballIcon from '@mui/icons-material/SportsFootball';
import { RequestErrorType } from '../models';
interface HeartbeatWrapperProps {
  children: React.ReactNode;
}

const loadingMessages = [
  'Gathering Scores',
  'Rotating Texas Logo 180 degress',
  'Scoring touchdowns',
  'Starting Michigan State Riot',
  "Calculating Florida's losing season",
];

const LoadingLayout = ({ children }: { children: React.ReactNode }) => {
  return (
    <Fade
      in
      appear
      unmountOnExit
      timeout={800}
      style={{ transitionDelay: '800ms' }}>
      <Box
        sx={{
          height: '100vh',
          width: '100vw',
          bgcolor: 'primary.main',
          display: 'flex',
          justifyContent: 'center',
          alignItems: 'center',
          flexDirection: 'column',
          gap: 5,
          color: 'white',
        }}>
        <SportsFootballIcon sx={{ fontSize: 40 }} />
        <Typography variant="h5">St. Pete Pick'em</Typography>
        {children}
      </Box>
    </Fade>
  );
};

export function HeartbeatWrapper({ children }: HeartbeatWrapperProps) {
  const location = useLocation();
  const queryClient = useQueryClient();
  const { error, isLoading } = useQuery<string, RequestErrorType>(
    'heartbeat',
    () =>
      axios
        .get('/heartbeat', {
          timeout: 60000,
        })
        .then((res) => res.data),
    {
      staleTime: 5000,
      retry: false,
    }
  );
  let cnt = 0;
  const [loadingMessage, setLoadingMessage] = useState(loadingMessages[cnt]);
  useEffect(() => {
    const interval = setInterval(() => {
      setLoadingMessage(loadingMessages[cnt++ % loadingMessages.length]);
    }, 1800);

    return () => clearInterval(interval);
    //eslint-disable-next-line
  }, []);

  useEffect(() => {
    queryClient.invalidateQueries('heartbeat');
  }, [location, queryClient]);

  if (isLoading) {
    return (
      <LoadingLayout>
        <Stack sx={{ width: '85vw' }} alignItems="center" spacing={2}>
          <Typography textAlign="center">{loadingMessage}</Typography>
        </Stack>
        <CircularProgress size={80} sx={{ color: 'white' }} />
      </LoadingLayout>
    );
  }

  if (error) {
    return (
      <LoadingLayout>
        <Typography variant="h5" textAlign="center" color="error">
          {error?.title}
        </Typography>
        <Typography textAlign="center" color="error">
          {error?.detail}
        </Typography>
      </LoadingLayout>
    );
  }
  return <div>{children}</div>;
}
