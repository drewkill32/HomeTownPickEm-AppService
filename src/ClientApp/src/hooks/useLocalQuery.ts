import { useQuery, UseQueryOptions } from 'react-query';
import { RequestErrorType } from '../zod';

export function useLocalQuery<T>(
  queryKey: string,
  queryFnc: () => Promise<T>,
  options?: UseQueryOptions<T, RequestErrorType>,
  postProcessor?: (data: any) => T
) {
  return useQuery<T, RequestErrorType>(queryKey, queryFnc, {
    ...options,
    initialData: () => {
      //get from local storage
      const data = localStorage.getItem(queryKey);
      if (data) {
        const json = JSON.parse(data);
        if (postProcessor) {
          return postProcessor(json);
        }
        return json;
      }
    },
    onSuccess: (data) => {
      if (data) {
        localStorage.setItem(queryKey, JSON.stringify(data));
      } else {
        localStorage.removeItem(queryKey);
      }
    },
    onError: () => {
      localStorage.removeItem(queryKey);
    },
  });
}
