import React from 'react';
import { Accordion, AccordionSummary } from '@material-ui/core';
import { makeStyles } from '@material-ui/core/styles';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import { Skeleton } from '@material-ui/lab';

const useStyles = makeStyles((theme) => ({
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
}));

const UserSkeleton = () => {
  const classes = useStyles();
  return (
    <Accordion>
      <AccordionSummary
        expandIcon={<ExpandMoreIcon />}
        aria-controls="panel1a-content"
        id="panel1a-header"
      >
        <div className={classes.summary}>
          <Skeleton
            animation="wave"
            className={classes.small}
            variant="circle"
          />
          <Skeleton className={classes.root} />
        </div>
      </AccordionSummary>
    </Accordion>
  );
};

const UserPickSkeleton = () => {
  const skeletons = [0, 1, 2, 3, 4, 5, 6, 7];

  return (
    <div>
      {skeletons.map((i) => (
        <UserSkeleton key={i} />
      ))}
    </div>
  );
};

export default UserPickSkeleton;
