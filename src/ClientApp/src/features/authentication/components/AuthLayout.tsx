import { Divider, Typography } from '@mui/material';
import { Box } from '@mui/system';
import SportsFootballIcon from '@mui/icons-material/SportsFootball';

interface ForgotPasswordProps {
  children?: JSX.Element;
  title: string;
  footer?: JSX.Element;
}

const ForgotPassword = ({ children, title, footer }: ForgotPasswordProps) => {
  return (
    <Box
      sx={{
        height: '100%',
        minHeight: '100vh',
        width: '100%',
        overflowX: 'hidden',
        bgcolor: 'primary.light',
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
        gap: 3,
        padding: 3,
      }}
    >
      <SportsFootballIcon
        sx={{ fontSize: '45px', color: 'primary.contrastText' }}
      />
      <Typography
        variant="h5"
        noWrap
        sx={{
          fontFamily: 'monospace',
          fontWeight: 700,
          letterSpacing: '.3rem',
          color: 'primary.contrastText',
          textDecoration: 'none',
          mb: 2,
        }}
      >
        St. Pete Pick'em
      </Typography>
      <Box
        sx={{
          width: '100%',
          bgcolor: 'white',
          padding: 2,
          borderRadius: 4,
          flexDirection: 'column',
          display: 'flex',
          gap: 1,
          alignItems: 'center',
          justifyContent: 'center',
        }}
      >
        <Typography variant="h4" color="primary">
          {title}
        </Typography>
        {children}
        {Boolean(footer) && <Divider flexItem sx={{ my: 1 }} />}

        {footer}
      </Box>
    </Box>
  );
};
export default ForgotPassword;
