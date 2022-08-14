import axios from 'axios';
import { useQuery } from 'react-query';
import { useWeek } from '../features/SeasonPicks/hooks/useWeek';
import { useLeague } from '../features/league';

export default function useGetPicks() {
  const { week } = useWeek();
  const league = useLeague();
  return useQuery(
    ['picks', league.id, league.season, week],
    () =>
      axios
        .get(
          `/api/picks/league/${league.id}/season/${league.season}/week/${week}/user`
        )
        .then((res) =>
          res.data.map((g) => ({
            ...g,
            cutoffDate: new Date(g.cutoffDate),
            startDate: new Date(g.startDate),
            currentDateTime: new Date(g.currentDateTime),
          }))
        ),
    {
      enabled: Boolean(league) && Boolean(week),
    }
  );
}
