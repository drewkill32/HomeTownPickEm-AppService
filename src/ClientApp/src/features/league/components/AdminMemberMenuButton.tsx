import { LeagueAdminMember } from '../types';
import { useAuth } from '../../authentication';
import { useState } from 'react';
import { useRemoveMember } from '../hooks/useRemoveMember';
import { useMakeCommissioner } from '../hooks/useMakeCommissioner';
import { useRemoveCommissioner } from '../hooks/useRemoveCommissioner';
import {
  Alert,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  IconButton,
  Menu,
  MenuItem,
} from '@mui/material';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import { LoadingButton } from '@mui/lab';

export const AdminMemberMenuButton = ({
  member,
}: {
  member: LeagueAdminMember;
}) => {
  const { user } = useAuth();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [openDialog, setOpenDialog] = useState(false);
  const { mutateAsync, isLoading } = useRemoveMember();
  const { mutateAsync: makeCommissioner } = useMakeCommissioner();
  const { mutateAsync: removeCommissioner } = useRemoveCommissioner();

  const open = Boolean(anchorEl);

  const handleClose = () => {
    setAnchorEl(null);
  };

  if (!user) {
    return null;
  }
  return (
    <>
      <IconButton onClick={(e) => setAnchorEl(e.currentTarget)}>
        <MoreVertIcon />
      </IconButton>
      <Menu
        id="basic-menu"
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
        MenuListProps={{
          'aria-labelledby': 'basic-button',
        }}>
        {user.claims['admin'] === 'true' && member.isCommissioner ? (
          <MenuItem
            onClick={async () => {
              await removeCommissioner(member.id);
              handleClose();
            }}>
            Remove Commissioner Role
          </MenuItem>
        ) : (
          <MenuItem
            onClick={async () => {
              await makeCommissioner(member.id);
              handleClose();
            }}>
            Make Commissioner
          </MenuItem>
        )}

        <MenuItem
          onClick={() => {
            setOpenDialog(true);
            handleClose();
          }}>
          Remove Member
        </MenuItem>
      </Menu>
      <Dialog open={openDialog} onClose={() => setOpenDialog(false)}>
        <DialogTitle>
          {`Delete ${member.fullName} from the league?`}
        </DialogTitle>
        <DialogContent>
          <DialogContentText>
            <Alert severity="error" icon={false}>
              <p>
                {`Are you sure you want to remove ${member.fullName} from the league?`}
              </p>
              <p>
                This will remove the member all of their picks. This action
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
              await mutateAsync(member.id);
              setOpenDialog(false);
            }}
            autoFocus>
            Delete
          </LoadingButton>
        </DialogActions>
      </Dialog>
    </>
  );
};
