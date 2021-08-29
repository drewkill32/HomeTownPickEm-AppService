import React, {Fragment, useMemo} from 'react';
import {Alert, Progress} from 'reactstrap';
import useGetPicks from '../../hooks/useGetPicks';
import isAfter from 'date-fns/isAfter';
import isThisWeek from 'date-fns/isThisWeek';
import PickRow from './PickRow';

const PicksTable = () => {
  const {status, data, error} = useGetPicks();

  const cutoffDate = useMemo(
      () => (data ? new Date(data.cutoffDate) : new Date()),
      [data]
  );
  if (status === 'loading') {
    return <Progress animated color="primary" value="100"/>;
  }
  if (status === 'success') {
    return (
        <Fragment>
          {isThisWeek(cutoffDate) && (
              <Alert className="mt-2" color="warning">
                The picks will be locked on {cutoffDate.toLocaleDateString()} at
                midnight. Get your picks in before then!
              </Alert>
          )}
          <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
            <tr>
              <th>Away</th>
              <th>Home</th>
            </tr>
            </thead>
            <tbody>
            {data.picks.map((pick) => (
                <PickRow
                    key={pick.id}
                    pick={pick}
                    isAfter={isAfter(new Date(), cutoffDate)}
                />
            ))}
            </tbody>
          </table>
        </Fragment>
    );
  }
  return <div>{error.message}</div>;
};

export default PicksTable;
