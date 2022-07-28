import AuthLayout from './AuthLayout';
import { Button, Typography } from '@mui/material';

export function InvalidRefUrl(props: {
  title: string;
  subtitle: string;
  onClick: () => void;
}) {
  return (
    <AuthLayout
      title={props.title}
      footer={
        <Button
          color="primary"
          variant="contained"
          fullWidth
          onClick={props.onClick}>
          Return to login
        </Button>
      }>
      <Typography>{props.subtitle}</Typography>
    </AuthLayout>
  );
}
