import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Avatar,
  Badge,
  Box,
  Grid,
  IconButton,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
  Paper,
  Stack,
  Typography,
  useMediaQuery,
  useTheme,
} from '@mui/material';
import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../../authentication';
import { useLeagueAdmin } from '../hooks/useLeagueAdmin';
import { blueGrey, red } from '@mui/material/colors';
import { LeagueAdminMember } from '../types';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import { AddNewMemberButton } from '../components/AddNewMemberButton';
import { AdminMemberMenuButton } from '../components/AdminMemberMenuButton';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import DeleteIcon from '@mui/icons-material/Delete';
import React, { useState } from 'react';

function AdminMemberListItem({ member }: { member: LeagueAdminMember }) {
  return (
    <ListItem secondaryAction={<AdminMemberMenuButton member={member} />}>
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
    </ListItem>
  );
}

interface AdminMemberListProps {
  addButton: React.ReactNode;
  children?: React.ReactNode;
  title: string;
}

const AdminList = ({ addButton, children, title }: AdminMemberListProps) => {
  const theme = useTheme();
  const xs = useMediaQuery(theme.breakpoints.down('sm'));
  const [open, setOpen] = useState(false);
  return (
    <Paper>
      <Accordion
        expanded={open || !xs}
        onChange={(e) => {
          //don't do anything if the user clicked on a button in the header
          if (e.target instanceof Element && e.target.tagName !== 'BUTTON') {
            setOpen(!open);
          }
        }}>
        <AccordionSummary
          expandIcon={xs ? <ExpandMoreIcon /> : null}
          sx={{
            cursor: 'default',
            '&.MuiAccordionSummary-root:hover': {
              cursor: { xs: 'pointer', sm: 'default' },
            },
          }}>
          <Stack sx={{ width: '100%' }}>
            <Box
              sx={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'space-between',
                width: '100%',
              }}>
              <Typography>{title}</Typography>
              {addButton}
            </Box>
          </Stack>
        </AccordionSummary>
        <AccordionDetails>{children}</AccordionDetails>
      </Accordion>
    </Paper>
  );
};

export const LeagueAdmin = () => {
  const { user } = useAuth();
  const location = useLocation();

  const { data } = useLeagueAdmin();

  if (!user || !data) {
    return null;
  }
  if (user.claims['admin'] !== 'true') {
    return <Navigate to={'/unauthorized'} state={{ from: location }} />;
  }
  return (
    <Grid container spacing={2} justifyContent="center" direction="row">
      <Grid item xs={12} sm={6}>
        <AdminList
          title="Members"
          addButton={
            <AddNewMemberButton sx={{ marginRight: { xs: 3, sm: 0 } }} />
          }>
          <List sx={{ width: '100%' }}>
            {data.members.map((member) => (
              <AdminMemberListItem key={member.id} member={member} />
            ))}
          </List>
        </AdminList>
      </Grid>

      <Grid item xs={12} sm={6}>
        <AdminList
          title="Teams"
          addButton={
            <AddNewMemberButton sx={{ marginRight: { xs: 3, sm: 0 } }} />
          }>
          <List sx={{ width: '100%' }}>
            {data.teams.map((team) => (
              <ListItem
                key={team.id}
                secondaryAction={
                  <IconButton>
                    <DeleteIcon sx={{ color: red[500] }} />
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
        </AdminList>
      </Grid>
    </Grid>
  );
};
