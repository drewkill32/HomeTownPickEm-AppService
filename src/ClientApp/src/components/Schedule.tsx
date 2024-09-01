import { useWeek } from '../features/SeasonPicks/hooks/useWeek';
import {
  Card,
  CardContent,
  Divider,
  IconButton,
  Skeleton,
  Box,
  Typography,
  SxProps,
  Theme,
} from '@mui/material';
import ArrowBackIosIcon from '@mui/icons-material/ArrowBackIos';
import ArrowForwardIosIcon from '@mui/icons-material/ArrowForwardIos';
import format from 'date-fns/format';
import { useSchedule, CalendarItem } from '../hooks/useSchedule';
import { useEffect, useState } from 'react';

const Schedule = ({ sx }: { sx?: SxProps<Theme> }) => {
  const { week, setWeek } = useWeek();
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

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
        ...sx,
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
              'MMM do',
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
