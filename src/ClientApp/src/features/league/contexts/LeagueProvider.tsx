import { createContext, useContext } from 'react';
import { League } from '../types';
import { useParams } from 'react-router-dom';
import { useLocalQuery } from '../../../hooks/useLocalQuery';
import { leagueAgent } from '../utils/leagueAgent';
import { Paper } from '@mui/material';

const LeagueContext = createContext<League>({} as League);

export const useLeague = () => {
  return useContext(LeagueContext);
};

type Props = {
  children: JSX.Element;
};

export function LeagueProvider({ children }: Props) {
  const { league: slug, season } = useParams();
  if (!slug || !season) {
    throw new Error('LeagueProvider requires league and season');
  }

  const query = useLocalQuery<League>(
    `current-league`,
    () => {
      return leagueAgent.getLeague(slug, season);
    },
    {
      enabled: Boolean(slug) && Boolean(season),
    }
  );
  if (query.isLoading) {
    return <Paper>Loading...</Paper>;
  }
  if (query.data) {
    console.log({ data: query.data });
    return (
      <LeagueContext.Provider value={query.data}>
        {children}
      </LeagueContext.Provider>
    );
  }
  return <div>Error {query.isError}</div>;
}
