import axios from 'axios';
import { useCurrentSeason } from '../features/league/hooks/useCurrentSeason';
import { useLocalQuery } from './useLocalQuery';

export interface CalendarItem {
  season: string;
  week: number;
  seasonType: 'regular';
  firstGameStart: Date;
  lastGameStart: Date;
}

const convertData = (data: any): CalendarItem[] => {
  return data.map((item: any) => {
    return {
      ...item,
      firstGameStart: new Date(item.firstGameStart),
      lastGameStart: new Date(item.lastGameStart),
    };
  });
};

export function useSchedule() {
  const { data: season } = useCurrentSeason();
  const queryKey = `schedule-${season}`;
  return useLocalQuery<CalendarItem[]>(
    queryKey,
    () =>
      axios.get(`api/calendar?season=2022`).then((res) => {
        const data = res.data as CalendarItem[];
        return convertData(data);
      }),
    {
      enabled: Boolean(season),
    },
    (data) => convertData(data)
  );
}
