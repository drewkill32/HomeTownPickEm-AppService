import React from "react";

const LeaderboardItem = ({ rank, index }) => {
  return (
    <tr key={index}>
      <td>
        <img
          onError={(e) => (e.target.src = "https://placehold.jp/50x50.png")}
          loading={index < 15 ? "eager" : "lazy"}
          src={rank.teamLogo}
          alt={rank.teamName}
          width="25"
          height="25"
          style={{ marginInline: "0.5rem" }}
        />
        {rank.user}
      </td>
      <td>{rank.totalPoints}</td>
    </tr>
  );
};

export default LeaderboardItem;
