import { useState } from 'react';
import {
  Alert,
  Avatar,
  Badge,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Divider,
  IconButton,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
  Menu,
  MenuItem,
  Stack,
} from '@mui/material';
import { Navigate, useLocation } from 'react-router-dom';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import { useAuth } from '../../authentication';
import { useLeagueAdmin } from '../hooks/useLeagueAdmin';
import { blueGrey } from '@mui/material/colors';
import { LoadingButton } from '@mui/lab';
import { LeagueAdminMember } from '../types';
import { useRemoveMember } from '../hooks/useRemoveMember';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import { useMakeCommissioner } from '../hooks/useMakeCommissioner';
import { useRemoveCommissioner } from '../hooks/useRemoveCommissioner';

function AdminMemberListItem({ member }: { member: LeagueAdminMember }) {
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
    <ListItem
      secondaryAction={
        <IconButton onClick={(e) => setAnchorEl(e.currentTarget)}>
          <MoreVertIcon />
        </IconButton>
      }>
      <ListItemAvatar>
        <Badge
          badgeContent={
            member.isCommissioner && (
              <CheckCircleIcon
                color="primary"
                sx={{ fontSize: '15px', marginLeft: '-10px', marginTop: '5px' }}
              />
            )
          }>
          <Avatar
            sx={{
              bgcolor: member.color || blueGrey[500],
              '& img': {
                width: '25px',
                height: '25px',
              },
            }}
            src={member.profileImg}>
            {member.initials}
          </Avatar>
        </Badge>
      </ListItemAvatar>
      <ListItemText primary={member.fullName}></ListItemText>
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
    </ListItem>
  );
}

export const LeagueAdmin = () => {
  const { user } = useAuth();
  let location = useLocation();

  const { data } = useLeagueAdmin();

  if (!user || !data) {
    return null;
  }
  if (user.claims['admin'] !== 'true') {
    return <Navigate to={'/unauthorized'} state={{ from: location }} />;
  }
  return (
    <Stack spacing={2} justifyContent="center" direction="row">
      <List sx={{ width: '100%' }}>
        {data.members.map((member) => (
          <AdminMemberListItem key={member.id} member={member} />
        ))}
      </List>
      <Divider variant="middle" orientation="vertical" flexItem />

      <List sx={{ width: '100%' }}>
        {data.teams.map((team) => (
          <ListItem
            key={team.id}
            secondaryAction={
              <IconButton>
                <MoreVertIcon />
              </IconButton>
            }>
            <ListItemAvatar>
              <Avatar
                sx={{
                  bgcolor: team.altColor || blueGrey[500],
                  '& img': {
                    width: '25px',
                    height: '25px',
                  },
                }}
                src={team.logo}
                alt={team.name}
              />
            </ListItemAvatar>
            <ListItemText primary={team.name}></ListItemText>
          </ListItem>
        ))}
      </List>
    </Stack>
  );
};
