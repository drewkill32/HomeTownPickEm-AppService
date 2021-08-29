import React from "react";
import axios from "axios";
import { useQuery } from "react-query";
import LeaderboardTable from "../components/leaderboard/LeaderboardTable";

const Leaderboard = () => {
  const { isLoading, data, isSuccess } = useQuery("leaderboard", () =>
    axios.get("/api/leaderboard/st-pete-pick-em").then((res) => res.data)
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
