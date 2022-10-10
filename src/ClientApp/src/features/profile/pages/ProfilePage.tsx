import {
  Divider,
  FormControl,
  Grid,
  IconButton,
  InputLabel,
  MenuItem,
  Select,
  Stack,
  TextField,
  Tooltip,
  Typography,
} from '@mui/material';
import MainLayout from '../../../layout/MainLayout';
import { useAuth } from '../../authentication';

import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import ContentCopyIcon from '@mui/icons-material/ContentCopy';
import RefreshIcon from '@mui/icons-material/Refresh';
import { useEffect, useState } from 'react';
import { fromUnixTime, formatDistance } from 'date-fns/esm';
import { LoadingButton } from '@mui/lab';
import { useMutation } from 'react-query';
import axios from 'axios';
import { useCurrentSeason } from '../../league/hooks/useCurrentSeason';
import { useSchedule } from '../../../hooks/useSchedule';

interface ProfileItemProps {
  children: React.ReactNode;
}

const ProfileItem = ({ children }: ProfileItemProps) => {
  return (
    <Stack
      sx={{ textAlign: 'center' }}
      direction="row"
      alignItems="center"
      spacing={2}
      justifyContent="space-between">
      {children}
    </Stack>
  );
};

interface FetchDataButtonProps {
  children: React.ReactNode;
  type: 'calendar' | 'teams' | 'games';
  week?: number;
}

const getUrl = (type: 'calendar' | 'teams' | 'games') => {
  switch (type) {
    case 'calendar':
      return '/api/Calendar/load';
    case 'teams':
      return '/api/teams/load';
    case 'games':
      return '/api/games/load';
  }
};

const FetchDataButton = ({ children, type, week }: FetchDataButtonProps) => {
  const url = getUrl(type);

  const { data: season } = useCurrentSeason();
  const { mutateAsync, isLoading } = useMutation(() =>
    axios.post(url, {
      year: season?.season,
      week: week === -1 ? null : week,
    })
  );
  const handleClick = async () => {
    await mutateAsync();
  };
  return (
    <LoadingButton
      sx={{
        minWidth: '165px',
      }}
      loading={isLoading}
      variant="contained"
      onClick={handleClick}>
      {children}
    </LoadingButton>
  );
};

export const ProfilePage = () => {
  const { user, token, refreshToken } = useAuth();
  const [copied, setCopied] = useState(false);
  const [week, setWeek] = useState(1);
  const [showToken, setShowToken] = useState(false);
  const { data: schedule } = useSchedule();
  const { data: season } = useCurrentSeason();

  useEffect(() => {
    if (season) {
      setWeek(season.week);
    }
  }, [season]);

  if (!user) {
    return null;
  }

  const isAdmin = user.claims['admin'] === 'true';
  const accessToken = token?.jwt || '';

  const weeks = schedule?.map((s) => s.week) || [];

  return (
    <MainLayout>
      <>
        <Stack spacing={2} sx={{ mx: 3, pb: 2 }}>
          <Typography variant="h4">Profile</Typography>
          <Divider variant="inset" />
          <Typography variant="h4">Coming Soon</Typography>
          {isAdmin && (
            <>
              {' '}
              <Typography variant="h5">Admin Area</Typography>
              <Divider variant="inset" />
              <ProfileItem>
                <TextField
                  fullWidth
                  id="token"
                  label="JWT Token"
                  type={showToken ? 'text' : 'password'}
                  value={accessToken}
                />
                <IconButton onClick={() => setShowToken((t) => !t)}>
                  {showToken ? <VisibilityOffIcon /> : <VisibilityIcon />}
                </IconButton>
                <Tooltip
                  title="copied to clipboard"
                  open={copied}
                  disableFocusListener
                  disableHoverListener
                  disableTouchListener>
                  <IconButton
                    color="primary"
                    onClick={() => {
                      if (token?.jwt) {
                        navigator.clipboard.writeText(token.jwt);
                        setCopied(true);
                        setTimeout(() => setCopied(false), 1200);
                      }
                    }}>
                    <ContentCopyIcon />
                  </IconButton>
                </Tooltip>
                <IconButton
                  onClick={async () => {
                    await refreshToken();
                  }}>
                  <RefreshIcon />
                </IconButton>
              </ProfileItem>
              <ProfileItem>
                <Typography>{`expires ${formatDistance(
                  fromUnixTime(token?.decoded.exp || 0),
                  Date.now(),
                  {
                    addSuffix: true,
                  }
                )}`}</Typography>
              </ProfileItem>
              <ProfileItem>
                <a
                  href={process.env.REACT_APP_API_URL || '#'}
                  target="_blank"
                  rel="noreferrer">
                  API
                </a>
              </ProfileItem>
              <ProfileItem>
                <Typography variant="h6">Data Load</Typography>
              </ProfileItem>
              <Divider variant="inset" />
              <ProfileItem>
                <Grid container spacing={3}>
                  <Grid item md={3} xs={12}>
                    <FetchDataButton type="calendar">
                      Update Calendar
                    </FetchDataButton>
                  </Grid>
                  <Grid item md={3} xs={12}>
                    <FetchDataButton type="teams">Update Teams</FetchDataButton>
                  </Grid>
                  <Grid item md={3} xs={12}>
                    <FormControl sx={{ width: '165px' }}>
                      <InputLabel id="week-label">Week</InputLabel>
                      <Select
                        labelId="week-label"
                        label="Week"
                        onChange={(e) => setWeek(Number(e.target.value))}
                        value={week}>
                        <MenuItem value={-1}>{'<All>'}</MenuItem>
                        {weeks.map((w) => (
                          <MenuItem key={w} value={w}>
                            {w}
                          </MenuItem>
                        ))}
                      </Select>
                    </FormControl>
                  </Grid>
                  <Grid item md={3} xs={12}>
                    <FetchDataButton type="games" week={week}>
                      Update Games
                    </FetchDataButton>
                  </Grid>
                </Grid>
              </ProfileItem>
            </>
          )}
        </Stack>
      </>
    </MainLayout>
  );
};
