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
  const [connection, setConnection] = useState<HubConnection>();
  const queryClient = useQueryClient();
  const auth = useAuth();

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl(`${process.env.REACT_APP_API_URL}/hubs/cache`, {
        accessTokenFactory: async () => {
          const token = auth.token;
          const exp = fromUnixTime(token?.decoded.exp || 0);
          if (isPast(exp)) {
            const t = await auth.refreshToken();
            return t?.jwt || '';
          }
          return token?.jwt || '';
        },
      })
      .withAutomaticReconnect()
      .build();
    newConnection.on('RefreshCache', async () => {
      await queryClient.invalidateQueries();
    });
    setConnection(newConnection);
  }, [auth, queryClient]);

  useEffect(() => {
    if (connection) {
      connection.start().catch((e) => console.error('Connection failed: ', e));
    }
  }, [connection]);
  return <div>{props.children}</div>;
};
