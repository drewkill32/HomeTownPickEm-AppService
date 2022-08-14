import { useState } from 'react';
import { AddTeamData, AvailableTeamsResult } from '../types';
import { useLeague } from '../contexts/LeagueProvider';
import { useMutation, useQuery, useQueryClient } from 'react-query';
import { LeagueKeys } from '../utils/queryKeys';
import { leagueAgent } from '../utils/leagueAgent';
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  IconButton,
  SxProps,
  TextField,
} from '@mui/material';
import AddCircleIcon from '@mui/icons-material/AddCircle';
import { green } from '@mui/material/colors';
import { LoadingButton } from '@mui/lab';
import { TeamAutoComplete } from './TeamAutoComplete';

export const AddNewTeamButton = ({ sx }: { sx?: SxProps }) => {
  const [openDialog, setOpenDialog] = useState(false);
  const [team, setTeam] = useState<AvailableTeamsResult | null>(null);
  const league = useLeague();
  const queryClient = useQueryClient();

  const { data } = useQuery(
    LeagueKeys.AvailableTeams,
    () => leagueAgent.availableTeams(league),
    {
      enabled: openDialog,
    }
  );

  const teams = data ? data : [];
  const { mutateAsync: addTeam, isLoading } = useMutation(
    (data: AddTeamData) => leagueAgent.addTeam(data),
    {
      onSuccess: async () => {
        await queryClient.invalidateQueries(LeagueKeys.LeagueAdmin);
      },
    }
  );

  const handleClose = () => {
    setOpenDialog(false);
    setTeam(null);
  };

  const handleSubmit = async () => {
    if (team) {
      const teamId = team.id;
      await addTeam({
        leagueId: league!.id,
        teamId: teamId,
        season: league!.season,
      });
      handleClose();
      await queryClient.invalidateQueries(LeagueKeys.AvailableTeams);
    }
  };

  return (
    <>
      <IconButton
        sx={sx}
        onClick={(e) => {
          e.stopPropagation();
          setOpenDialog(true);
        }}>
        <AddCircleIcon sx={{ color: green[500] }} />
      </IconButton>
      <Dialog fullWidth maxWidth="sm" open={openDialog} onClose={handleClose}>
        <DialogTitle>Add New Team</DialogTitle>
        <DialogContent>
          <TeamAutoComplete
            options={teams}
            value={team}
            onChange={(e, t) => {
              setTeam(t);
            }}
            renderInput={(params) => (
              <TextField
                variant="standard"
                {...params}
                label="Pick a team"
                inputProps={{
                  ...params.inputProps,
                }}
              />
            )}
          />
        </DialogContent>
        <DialogActions>
          <Button variant="outlined" onClick={handleClose}>
            Cancel
          </Button>
          <LoadingButton
            color="primary"
            disabled={!team}
            loading={isLoading}
            variant="contained"
            onClick={handleSubmit}
            autoFocus>
            Save
          </LoadingButton>
        </DialogActions>
      </Dialog>
    </>
  );
};
