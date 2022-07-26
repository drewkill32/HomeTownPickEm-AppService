import { useWeek } from '../hooks/useWeek';
import {
  Card,
  CardContent,
  Divider,
  IconButton,
  Typography,
} from '@mui/material';
import ArrowBackIosIcon from '@mui/icons-material/ArrowBackIos';
import ArrowForwardIosIcon from '@mui/icons-material/ArrowForwardIos';
import format from 'date-fns/format';
import { useSchedule, CalendarItem } from '../hooks/useSchedule';
import { Skeleton } from '@mui/lab';
import { Box } from '@mui/system';
import { useEffect, useState } from 'react';
//
// const useStyles = makeStyles((theme) => ({
//   root: {
//     display: 'flex',
//     flexDirection: 'row',
//     justifyContent: 'space-between',
//     alignItems: 'center',
//     marginBottom: theme.spacing(1),
//     backgroundColor: '#fafafa',
//     position: 'sticky',
//     top: 0,
//     zIndex: 50,
//     paddingBottom: theme.spacing(1),
//     paddingTop: theme.spacing(2),
//   },
//   card: {
//     minWidth: 'clamp( 150px, 450px, 70vw)',
//     minHeight: 'max( 50px, 7vmin)',
//     display: 'flex',
//     justifyContent: 'center',
//     alignItems: 'center',
//     flexDirection: 'column',
//     '& > h6': {
//       fontSize: 'clamp(0.7rem, 1rem + 6vmin,1rem)',
//     },
//   },
//
//   weekRange: {
//     paddingInline: theme.spacing(2),
//     marginBottom: theme.spacing(0.7),
//   },
// }));

const Schedule = () => {
  const [week, setWeek] = useWeek();
  const [curWeek, setCurWeek] = useState<CalendarItem>();

  const { data: schedule } = useSchedule();

  useEffect(() => {
    if (schedule) {
      const i = schedule.findIndex((x) => x.week === week);
      setCurWeek(schedule[i]);
    }
  }, [schedule, week]);

  const maxWeek = schedule ? Math.max(...schedule.map((x) => x.week)) : 0;

  const handleClick = (direction: 'next' | 'prev') => {
    if (!schedule) {
      return;
    }
    switch (direction) {
      case 'next':
        const nextIndex = schedule.findIndex((x) => x.week === week) + 1;
        if (nextIndex < schedule.length) {
          setWeek(schedule[nextIndex].week);
        }
        break;
      case 'prev':
        const prevIndex = schedule.findIndex((x) => x.week === week) - 1;
        if (prevIndex >= 0) {
          setWeek(schedule[prevIndex].week);
        }
        break;
    }
  };

  if (!schedule) {
    return <Skeleton />;
  }

  if (!curWeek) {
    return <Skeleton />;
  }
  console.log(curWeek);
  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
      }}>
      <IconButton
        disabled={week <= 1}
        onClick={() => handleClick('prev')}
        color="primary"
        size="large">
        <ArrowBackIosIcon />
      </IconButton>
      <Card sx={{ flexGrow: 1, padding: 2 }}>
        <CardContent sx={{ textAlign: 'center' }}>
          <Typography variant="h6">Week {curWeek.week}</Typography>
          <Divider flexItem variant="fullWidth" sx={{ my: 2 }} />
          <Typography variant="subtitle1">
            {`${format(curWeek.firstGameStart, 'MMM do')} - ${format(
              curWeek.lastGameStart,
              'MMM do'
            )}`}
          </Typography>
        </CardContent>
      </Card>
      <IconButton
        disabled={week >= maxWeek}
        onClick={() => handleClick('next')}
        color="primary"
        size="large">
        <ArrowForwardIosIcon />
      </IconButton>
    </Box>
  );
};

export default Schedule;
