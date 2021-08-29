import React from "react";
import LeaderboardItem from "./LeaderboardItem";

const LeaderboardTable = ({ ranks }) => {
  return (
    <table className="table table-striped" aria-labelledby="tabelLabel">
      <thead>
        <tr>
          <th>Name</th>
          <th>Points</th>
        </tr>
      </thead>
      <tbody>
        {ranks.map((rank, index) => (
          <LeaderboardItem key={index} rank={rank} />
        ))}
      </tbody>
    </table>
  );
};

export default LeaderboardTable;
