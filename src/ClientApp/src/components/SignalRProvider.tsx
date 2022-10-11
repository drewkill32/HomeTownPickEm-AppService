import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { useEffect, useState } from 'react';
import { useQueryClient } from 'react-query';
import { useAuth } from '../features/authentication';
import { fromUnixTime } from 'date-fns/esm';
import { isPast } from 'date-fns';

interface SignalRProviderProps {
  children: JSX.Element;
}

export const SignalRProvider = (props: SignalRProviderProps) => {
  const [, setConnection] = useState<HubConnection>();
  const queryClient = useQueryClient();
  const { isAuthenticated, token, refreshToken } = useAuth();

  useEffect(() => {
    if (!isAuthenticated) {
      return;
    }
    const newConnection = new HubConnectionBuilder()
      .withUrl(`${process.env.REACT_APP_API_URL}/hubs/cache`, {
        accessTokenFactory: async () => {
          const t = token;
          const exp = fromUnixTime(t?.decoded.exp || 0);
          console.log({ exp });
          if (isPast(exp)) {
            console.log({ message: 'refreshing token' });
            const t = await refreshToken();
            return t?.jwt || '';
          }
          return t?.jwt || '';
        },
      })
      .withAutomaticReconnect()
      .build();
    newConnection.on('RefreshCache', async () => {
      await queryClient.invalidateQueries();
    });
    newConnection
      .start()
      .then(() => console.log('starting SignalR'))
      .catch((e) => console.error('Connection failed: ', e));
    setConnection(newConnection);
    return () => {
      newConnection
        .stop()
        .then(() => console.log('stopping SignalR'))
        .catch((e) => console.error(e));
    };
  }, [queryClient, isAuthenticated, token, refreshToken]);

  return <div>{props.children}</div>;
};
