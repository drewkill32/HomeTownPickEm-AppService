import { useState } from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import { LoadingButton } from '@mui/lab';
import { FormControlLabel, Switch } from '@mui/material';
import {
  LeagueSettingsYear,
  useLeagueSettings,
} from '../hooks/useLeagueSettings';
import { CreateSeasonParams, useCreateSeason } from '../hooks/useCreateSeason';
import { UserLeague } from '../../../models';

interface NewSeasonConfirmDialogProps {
  league: UserLeague | undefined;
  year: string;
  handleClose: (result: 'ok' | 'cancel') => void;
}

interface CopySeasonContentProps {
  season: LeagueSettingsYear;
  teams: [boolean, (teams: boolean) => void];
  members: [boolean, (members: boolean) => void];
}

const CopySeasonContent = ({
  season,
  members,
  teams,
}: CopySeasonContentProps) => {
  const [m, setMembers] = members;
  const [t, setTeams] = teams;
  return (
    <DialogContent>
      <FormControlLabel
        sx={{
          display: 'flex',
          justifyContent: 'space-between',
        }}
        value="start"
        control={
          <Switch checked={t} onChange={() => setTeams(!t)} color="primary" />
        }
        label={`Copy ${season.teamCount} Team(s)`}
        labelPlacement="start"
      />
      <FormControlLabel
        sx={{
          display: 'flex',
          justifyContent: 'space-between',
        }}
        value="start"
        control={
          <Switch color="primary" checked={m} onChange={() => setMembers(!m)} />
        }
        label={`Copy ${season.memberCount} Memeber(s)`}
        labelPlacement="start"
      />
    </DialogContent>
  );
};

export default function NewSeasonConfirmDialog({
  league,
  year,
  handleClose,
}: NewSeasonConfirmDialogProps) {
  const membersState = useState(true);
  const teamState = useState(true);
  const { data } = useLeagueSettings(league?.id || 0);

  const { mutateAsync: createSeason, isLoading } = useCreateSeason();

  if (!data) {
    return null;
  }

  const prevSeason =
    data.years.length > 0 ? data.years[data.years.length - 1] : null;

  return (
    <Dialog
      open={Boolean(league)}
      aria-labelledby="alert-dialog-title"
      aria-describedby="alert-dialog-description">
      <DialogTitle id="alert-dialog-title">
        {prevSeason
          ? `Copy Teams and Members from ${prevSeason.year}?`
          : 'Start new Season?'}
      </DialogTitle>
      {prevSeason && (
        <CopySeasonContent
          season={prevSeason}
          members={membersState}
          teams={teamState}
        />
      )}
      <DialogActions>
        <Button onClick={() => handleClose('cancel')}>Cancel</Button>
        <LoadingButton
          onClick={async () => {
            const params: CreateSeasonParams = {
              leagueId: league?.id || 0,
              year: year,
            };
            if (prevSeason) {
              params.copyFrom = {
                season: prevSeason.year,
                members: membersState[0],
                teams: teamState[0],
              };
            }
            await createSeason(params);
            handleClose('ok');
          }}
          loading={isLoading}>
          OK
        </LoadingButton>
      </DialogActions>
    </Dialog>
  );
}
