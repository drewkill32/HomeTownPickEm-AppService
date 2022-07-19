import React from 'react';
import axios from 'axios';
import { useQuery } from 'react-query';
import { useHistory } from 'react-router';
import { useLocation } from 'react-router-dom';
import { useWeek } from '../hooks/useWeek';
import { makeStyles } from '@mui/styles';
import { Card, Divider, IconButton, Typography } from '@mui/material';
import ArrowBackIosIcon from '@mui/icons-material/ArrowBackIos';
import ArrowForwardIosIcon from '@mui/icons-material/ArrowForwardIos';
import format from 'date-fns/format';

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: theme.spacing(1),
    backgroundColor: '#fafafa',
    position: 'sticky',
    top: 0,
    zIndex: 50,
    paddingBottom: theme.spacing(1),
    paddingTop: theme.spacing(2),
  },
  card: {
    minWidth: 'clamp( 150px, 450px, 70vw)',
    minHeight: 'max( 50px, 7vmin)',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    flexDirection: 'column',
    '& > h6': {
      fontSize: 'clamp(0.7rem, 1rem + 6vmin,1rem)',
    },
  },

  weekRange: {
    paddingInline: theme.spacing(2),
    marginBottom: theme.spacing(0.7),
  },
}));

const Schedule = () => {
  const classes = useStyles();
  const history = useHistory();
  const { pathname } = useLocation();
  const week = useWeek();

  const {
    data: schedule,
    status,
    error,
  } = useQuery('schedule', () =>
    axios.get('api/calendar/st-pete-pick-em').then((res) =>
      res.data.map((d) => ({
        ...d,
        firstGameStart: new Date(d.firstGameStart),
        lastGameStart: new Date(d.lastGameStart),
        cutoffDate: new Date(d.cutoffDate),
      }))
    )
  );

  const handleClick = (direction) => {
    if (direction === 'next') {
      history.push({
        pathname: pathname,
        search: `?week=${parseInt(week, 10) + 1}`,
      });
    } else {
      history.push({
        pathname: pathname,
        search: `?week=${parseInt(week, 10) - 1}`,
      });
    }
  };
  if (status === 'loading') {
    return null;
  }
  if (status === 'success') {
    const maxWeek = Math.max(...schedule.map((s) => s.week));
    const currentWeek = schedule.find((s) => s.week === parseInt(week, 10)) || {
      week: week,
    };
    return (
      <div className={classes.root}>
        <IconButton
          disabled={status === 'loading' || week <= 1}
          onClick={() => handleClick('prev')}
          color="primary"
          size="large"
        >
          <ArrowBackIosIcon />
        </IconButton>
        <Card className={classes.card}>
          <Typography variant="h6">Week {currentWeek.week}</Typography>
          <Divider flexItem variant="fullWidth" />
          <Typography variant="subtitle1" className={classes.weekRange}>
            {`${format(currentWeek.firstGameStart, 'MMM do')} - ${format(
              currentWeek.lastGameStart,
              'MMM do'
            )}`}
          </Typography>
        </Card>
        <IconButton
          disabled={status === 'loading' || week >= maxWeek}
          onClick={() => handleClick('next')}
          color="primary"
          size="large"
        >
          <ArrowForwardIosIcon />
        </IconButton>
      </div>
    );
  }
  console.error(error);
  return null;
};

export default Schedule;
