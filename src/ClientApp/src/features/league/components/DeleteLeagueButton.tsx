import { useNavigate } from 'react-router-dom';
import { useRemoveLeague } from '../hooks/useRemoveLeague';
import { useState } from 'react';
import {
  Box,
  Button,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from '@mui/material';

export function DeleteLeagueButton() {
  const navigate = useNavigate();
  const { mutateAsync, isLoading: isDeleting } = useRemoveLeague();
  const [open, setOpen] = useState(false);
  const handleClose = async (save: boolean) => {
    if (!save) {
      setOpen(false);
      return;
    }
    await mutateAsync();
    navigate('/leagues');
  };
  return (
    <>
      <Button color="error" variant="contained" onClick={() => setOpen(true)}>
        Delete League
      </Button>
      <Dialog onClose={() => handleClose(false)} open={open}>
        <DialogTitle>
          Are you sure you want to delete this season of the league?
        </DialogTitle>
        <DialogContent>
          {isDeleting && (
            <Box
              sx={{
                width: '100%',
                paddingBlock: 2,
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
              }}>
              <CircularProgress />
            </Box>
          )}

          <DialogContentText>This action cannot be undone.</DialogContentText>
          <DialogActions>
            <Button disabled={isDeleting} onClick={() => setOpen(false)}>
              No
            </Button>
            <Button
              disabled={isDeleting}
              onClick={() => handleClose(true)}
              autoFocus>
              Yes
            </Button>
          </DialogActions>
        </DialogContent>
      </Dialog>
    </>
  );
}
