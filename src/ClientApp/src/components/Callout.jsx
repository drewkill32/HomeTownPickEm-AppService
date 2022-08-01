import axios from 'axios';
import { useQuery } from 'react-query';
import { useWeek } from '../features/SeasonPicks/hooks/useWeek';

const Callout = () => {
  const week = useWeek();
  const { isLoading, data } = useQuery('weekPicks', () =>
    axios
      .get(`/api/picks/st-pete-pick-em/weeklypicks/week/${week}`)
      .then((res) => res.data)
  );
  if (isLoading) {
    return null;
  }
  return (
    <table className="table table-striped" aria-labelledby="tabelLabel">
      <thead>
        <tr>
          <th>User</th>
          <th>Selected Picks</th>
        </tr>
      </thead>
      <tbody>
        {data.map((member, index) => (
          <tr key={index}>
            <td>
              <img
                src={member.profileImg}
                alt={`${member.userFirstName} ${member.userFirstName}`}
                width="20"
                style={{ marginInline: '10px' }}
              />
              {member.userFirstName} {member.userLastName}
            </td>
            <td>{`${member.totalPicks - member.unselectedPicks}/${
              member.totalPicks
            }`}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};

export default Callout;
