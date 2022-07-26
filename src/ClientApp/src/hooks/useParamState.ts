import { useEffect, useState } from 'react';
import { useSearchParams } from 'react-router-dom';

export function useParamState(
  key: string,
  initialValue: string | null
): [string | null, (value: string | null) => void] {
  const [searchParams, setSearchParams] = useSearchParams();

  const [value, setValue] = useState(() => {
    const value = searchParams.get(key);
    if (value != null) return value;
    return initialValue;
  });

  useEffect(() => {
    if (value) {
      searchParams.set(key, value);
      setSearchParams(searchParams);
    } else {
      searchParams.delete(key);
      setSearchParams(searchParams);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [key, value]);

  return [value, setValue];
}
