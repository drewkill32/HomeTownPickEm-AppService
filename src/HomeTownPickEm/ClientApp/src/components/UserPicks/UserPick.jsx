import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Accordion from '@material-ui/core/Accordion';
import AccordionSummary from '@material-ui/core/AccordionSummary';
import AccordionDetails from '@material-ui/core/AccordionDetails';
import Typography from '@material-ui/core/Typography';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import { Avatar, Grid } from '@material-ui/core';
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
