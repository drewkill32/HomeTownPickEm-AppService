import { useEffect, useState } from 'react';

export default function useLocalStorage<T>(
  key: string,
  initialValue: T | null
): [T, (value: T | null) => void] {
  const [value, setValue] = useState(() => {
    const jsonValue = localStorage.getItem(key);
    if (jsonValue != null) return JSON.parse(jsonValue);
    return initialValue as T;
  });

  useEffect(() => {
    if (value === null) {
      localStorage.removeItem(key);
    } else {
      localStorage.setItem(key, JSON.stringify(value));
    }
  }, [key, value]);

  return [value, setValue];
}
