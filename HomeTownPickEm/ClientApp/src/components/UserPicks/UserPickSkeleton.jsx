import React, {Fragment} from 'react';
import {Accordion, AccordionDetails, AccordionSummary, Divider, Grid, Paper,} from '@material-ui/core';
import {makeStyles} from '@material-ui/core/styles';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import {Skeleton} from '@material-ui/lab';

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
  root: {
    width: '100%',
    minWidth: '200px',
  },
  paper: {
    padding: theme.spacing(2),
    paddingRight: theme.spacing(4),
    width: '100%',
  },
  teamName: {
    marginLeft: theme.spacing(1),
  },
  score: {
    width: '25px',
  },
}));

const TeamPickSkelton = () => {
  const classes = useStyles();
  return (
      <Fragment>
        <Grid item xs={1}>
          <Skeleton animation="wave" className={classes.small} variant="circle"/>
        </Grid>
        <Grid item xs={9}>
          <Skeleton className={classes.teamName}/>
        </Grid>
        <Grid item xs={1}>
          <Skeleton className={classes.score}/>
        </Grid>
        <Grid item xs={1}></Grid>
      </Fragment>
  );
};

const GamePickSkeleton = () => {
  const classes = useStyles();
  return (
      <Grid item>
        <Paper className={classes.paper}>
          <Grid container spacing={2}>
            <TeamPickSkelton/>
            <Grid item xs={12}>
              <Divider variant="fullWidth"/>
            </Grid>
            <TeamPickSkelton/>
          </Grid>
        </Paper>
      </Grid>
  );
};

const UserSkeleton = ({expanded}) => {
  const classes = useStyles();
  const skeletons = [0, 1, 2];
  return (
      <Accordion defaultExpanded={expanded}>
        <AccordionSummary
            expandIcon={<ExpandMoreIcon/>}
            aria-controls="panel1a-content"
            id="panel1a-header"
        >
          <div className={classes.summary}>
            <Skeleton
                animation="wave"
                className={classes.small}
                variant="circle"
            />
            <Skeleton className={classes.root}/>
          </div>
        </AccordionSummary>
        <AccordionDetails>
          <Grid container direction="column" spacing={4}>
            {skeletons.map((i) => (
                <GamePickSkeleton key={i}/>
            ))}
          </Grid>
        </AccordionDetails>
      </Accordion>
  );
};

const UserPickSkeleton = () => {
  const skeletons = [0, 1, 2, 3, 4];

  return (
      <div>
        {skeletons.map((i) => (
            <UserSkeleton key={i} expanded={i === 0}/>
        ))}
      </div>
  );
};

export default UserPickSkeleton;
