import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { useEffect, useState } from 'react';
import { useQueryClient } from 'react-query';
import { useAuth } from '../features/authentication';

interface SignalRProviderProps {
  children: JSX.Element;
}

export const SignalRProvider = (props: SignalRProviderProps) => {
  const [connection, setConnection] = useState<HubConnection>();
  const queryClient = useQueryClient();
  const { isAuthenticated } = useAuth();

  useEffect(() => {
    if (!isAuthenticated) {
      return;
    }
    const newConnection = new HubConnectionBuilder()
      .withUrl(`${process.env.REACT_APP_API_URL}/hubs/cache`)
      .withAutomaticReconnect()
      .build();
    newConnection.on('RefreshCache', async () => {
      await queryClient.invalidateQueries();
    });
    setConnection(newConnection);
    return () => {
      newConnection
        .stop()
        .then(() => console.log('stopping SignalR'))
        .catch((e) => console.error(e));
    };
  }, [isAuthenticated, queryClient]);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => console.log('starting SignalR'))
        .catch((e) => console.error('Connection failed: ', e));
    }
  }, [connection]);
  return <div>{props.children}</div>;
};
