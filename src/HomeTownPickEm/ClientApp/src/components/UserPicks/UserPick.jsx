import React from 'react';
import { makeStyles } from '@mui/styles';
import Accordion from '@mui/material/Accordion';
import AccordionSummary from '@mui/material/AccordionSummary';
import AccordionDetails from '@mui/material/AccordionDetails';
import Typography from '@mui/material/Typography';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { Avatar, Grid } from '@mui/material';
import GamePick from './GamePick';

const useStyles = makeStyles((theme) => ({
  heading: {
    fontSize: theme.typography.pxToRem(15),
    fontWeight: theme.typography.fontWeightRegular,
  },
  small: {
    width: theme.spacing(3),
    height: theme.spacing(3),
    padding: '3px',
    objectFit: 'contain',
  },
  summary: {
    display: 'flex',
    alignItems: 'center',
    alignContent: 'center',
    '& > *': {
      margin: theme.spacing(1),
    },
  },
}));

export default function UserPick({ pickCollection: { user, picks } }) {
  const classes = useStyles();

  return (
    <Accordion>
      <AccordionSummary
        expandIcon={<ExpandMoreIcon />}
        aria-controls="panel1a-content"
        id="panel1a-header"
      >
        <div className={classes.summary}>
          <Avatar
            className={classes.small}
            alt={user.fullName}
            src={user.profileImg}
          />
          <Typography className={classes.heading}>
            {user.fullName} ({user.totalPoints} pts)
          </Typography>
        </div>
      </AccordionSummary>
      <AccordionDetails>
        <Grid container direction="column" spacing={4}>
          {picks.map((p) => (
            <GamePick key={p.id} pick={p} />
          ))}
        </Grid>
      </AccordionDetails>
    </Accordion>
  );
}
