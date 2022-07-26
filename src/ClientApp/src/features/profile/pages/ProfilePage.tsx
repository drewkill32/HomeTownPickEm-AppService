import {
  Divider,
  IconButton,
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
import { useState } from 'react';
import { fromUnixTime, formatDistance } from 'date-fns/esm';
import { LoadingButton } from '@mui/lab';
import { useMutation } from 'react-query';
import axios from 'axios';
import { useCurrentSeason } from '../../league/hooks/useCurrentSeason';

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
      justifyContent="space-between"
    >
      {children}
    </Stack>
  );
};

interface FetchDataButtonProps {
  children: React.ReactNode;
  type: 'calendar' | 'teams' | 'games';
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

const FetchDataButton = ({ children, type }: FetchDataButtonProps) => {
  const url = getUrl(type);

  const { data: season } = useCurrentSeason();
  const { mutateAsync, isLoading } = useMutation(() =>
    axios.post(url, {
      year: season?.season,
    })
  );
  const handleClick = async () => {
    await mutateAsync();
  };
  return (
    <LoadingButton
      sx={{
        minWidth: '164px',
      }}
      loading={isLoading}
      variant="contained"
      onClick={handleClick}
    >
      {children}
    </LoadingButton>
  );
};

export const ProfilePage = () => {
  const { user, token: jwt } = useAuth();
  const [copied, setCopied] = useState(false);
  const [showToken, setShowToken] = useState(false);

  if (!user) {
    return null;
  }

  const isAdmin = user.roles.includes('admin');
  const accessToken = jwt?.token || '';

  return (
    <MainLayout>
      <>
        {isAdmin && (
          <Stack spacing={2} sx={{ mx: 3, pb: 2 }}>
            <Typography variant="h4">Profile</Typography>
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
                disableTouchListener
              >
                <IconButton
                  color="primary"
                  onClick={() => {
                    if (jwt?.token) {
                      navigator.clipboard.writeText(jwt.token);
                      setCopied(true);
                      setTimeout(() => setCopied(false), 1200);
                    }
                  }}
                >
                  <ContentCopyIcon />
                </IconButton>
              </Tooltip>
            </ProfileItem>
            <ProfileItem>
              <Typography>{`expires ${formatDistance(
                fromUnixTime(jwt?.decoded.exp || 0),
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
                rel="noreferrer"
              >
                API
              </a>
            </ProfileItem>
            <ProfileItem>
              <Typography variant="h6">Data Load</Typography>
            </ProfileItem>
            <Divider variant="inset" />
            <ProfileItem>
              <FetchDataButton type="calendar">Update Calendar</FetchDataButton>
              <FetchDataButton type="teams">Update Teams</FetchDataButton>
              <FetchDataButton type="games">Update Games</FetchDataButton>
            </ProfileItem>
          </Stack>
        )}
      </>
    </MainLayout>
  );
};
