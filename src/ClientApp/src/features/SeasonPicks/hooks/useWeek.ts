import { useContext } from 'react';
import { PickContext } from '../contexts/PickContext';

export const useWeek = () => {
  return useContext(PickContext);
};
