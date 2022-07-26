import { useParamState } from './useParamState';

export const useWeek = (): [number, (value: number) => void] => {
  const [param, setParam] = useParamState('week', '1');

  return [Number(param), (v: number) => setParam(v.toString())];
};
