import axios from 'axios';
import { useQuery } from 'react-query';
import LeaderboardTable from '../components/LeaderboardTable';
import { useParams } from 'react-router-dom';

const Leaderboard = () => {
  let { league, season } = useParams();
  const { isLoading, data, isSuccess } = useQuery('leaderboard', () =>
    axios
      .get(`/api/league/${league}/${season}/leaderboard`)
      .then((res) => res.data)
  );
  if (isLoading)
    return (
      <p>
        <em>Loading...</em>
      </p>
    );
  if (isSuccess) {
    return <LeaderboardTable ranks={data} />;
  }
  return null;
};

export default Leaderboard;
