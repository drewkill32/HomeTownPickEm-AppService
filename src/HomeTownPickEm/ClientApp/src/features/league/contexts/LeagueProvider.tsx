import { createContext, useContext } from 'react';
import useLocalStorage from '../../../hooks/useLocalStorage';

export interface League {
  slug: string;
  season: string;
}

const LeagueContext = createContext<
  [League | null, (league: League | null) => void]
>([null, () => {}]);

export const useLeague = () => {
  return useContext(LeagueContext);
};

type Props = {
  children: JSX.Element;
};

export function LeagueProvider({ children }: Props) {
  const [league, setLeague] = useLocalStorage('league', null);

  return (
    <LeagueContext.Provider value={[league, setLeague]}>
      {children}
    </LeagueContext.Provider>
  );
}
