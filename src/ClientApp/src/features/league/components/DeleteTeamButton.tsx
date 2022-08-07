import { LeagueAdminTeam } from '../types';
import React, { useState } from 'react';
import { useMutation, useQueryClient } from 'react-query';
import { useLeague } from '../contexts/LeagueProvider';
import { leagueAgent } from '../utils/leagueAgent';
import { LeagueKeys } from '../utils/queryKeys';
import {
  Alert,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  IconButton,
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import { red } from '@mui/material/colors';
import { LoadingButton } from '@mui/lab';

export function DeleteTeamButton({ team }: { team: LeagueAdminTeam }) {
  const [openDialog, setOpenDialog] = useState(false);
  const queryClient = useQueryClient();
  const [league] = useLeague();

  const { mutateAsync, isLoading } = useMutation(
    (team: LeagueAdminTeam) => {
      return leagueAgent.removeTeam({
        leagueId: league!.id,
        teamId: team.id,
        season: league!.season,
      });
    },
    {
      onSuccess: async () => {
        await Promise.all([
          queryClient.invalidateQueries(LeagueKeys.AvailableTeams),
          queryClient.invalidateQueries(LeagueKeys.LeagueAdmin),
        ]);
      },
    }
  );

  return (
    <>
      <IconButton onClick={() => setOpenDialog(true)}>
        <DeleteIcon sx={{ color: red[500] }} />
      </IconButton>
      <Dialog open={openDialog} onClose={() => setOpenDialog(false)}>
        <DialogTitle>{`Delete ${team.name} from the league?`}</DialogTitle>
        <DialogContent>
          <DialogContentText>
            <Alert severity="error" icon={false}>
              <p>
                {`Are you sure you want to remove ${team.name} from the league?`}
              </p>
              <p>
                This will remove the team and picks for that team. This action
                cannot be undone.
              </p>
            </Alert>
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button variant="outlined" onClick={() => setOpenDialog(false)}>
            Cancel
          </Button>
          <LoadingButton
            color="error"
            loading={isLoading}
            variant="contained"
            onClick={async () => {
              await mutateAsync(team);
              setOpenDialog(false);
            }}
            autoFocus>
            Delete
          </LoadingButton>
        </DialogActions>
      </Dialog>
    </>
  );
}
