import { createContext, useContext } from 'react';
import useLocalStorage from '../../../hooks/useLocalStorage';

export interface League {
  slug: string;
  season: string;
}

const leagueContext = createContext<
  [League | null, (league: League | null) => void]
>([null, () => {}]);

export const useLeague = () => {
  return useContext(leagueContext);
};

type Props = {
  children: JSX.Element;
};

export function LeagueProvider({ children }: Props) {
  const [league, setLeague] = useLocalStorage<League>('league', null);

  return (
    <leagueContext.Provider value={[league, setLeague]}>
      {children}
    </leagueContext.Provider>
  );
}
