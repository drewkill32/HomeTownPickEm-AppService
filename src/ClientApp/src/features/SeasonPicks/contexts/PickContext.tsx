import { createContext, ReactNode } from 'react';
import { PickContextProps } from '../types';
import { useParamState } from '../../../hooks/useParamState';

export const PickContext = createContext<PickContextProps>({
  week: 1,
  setWeek: () => {},
});

export const PickProvider = ({ children }: { children: ReactNode }) => {
  const [param, setParam] = useParamState('week', '1');

  return (
    <PickContext.Provider
      value={{
        week: Number(param),
        setWeek: (v: number) => setParam(v.toString()),
      }}>
      {children}
    </PickContext.Provider>
  );
};
