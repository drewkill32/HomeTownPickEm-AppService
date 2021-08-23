import React, { useEffect, useState, Fragment } from "react";
import { useAuth } from "../hooks/useAuth";
import { Progress } from "reactstrap";
import useGetPicks from "../hooks/useGetPicks";

const PicksTable = ({ picks }) => {
  return (
    <table className="table table-striped" aria-labelledby="tabelLabel">
      <thead>
        <tr>
          <th>Away</th>
          <th>Home</th>
        </tr>
      </thead>
      <tbody>
        {picks.map((pick, index) => {
          return <PickRow key={pick.id} pick={pick} index={index} />;
        })}
      </tbody>
    </table>
  );
};

const PickRow = ({ pick, index }) => {
  const { getToken } = useAuth();
  const [loading, setLoading] = useState(false);
  const [selectedId, setSelectedId] = useState(
    pick.selectedTeam && pick.selectedTeam.id
  );

  useEffect(() => {}, [pick]);

  const handleChange = async (teamId) => {
    setLoading(true);
    try {
      const token = await getToken();
      const response = await fetch(`api/pick/${pick.id}`, {
        method: "PUT",
        headers: !token
          ? { "Content-Type": "application/json" }
          : {
              Authorization: `Bearer ${token}`,
              "Content-Type": "application/json",
            },
        body: JSON.stringify({ selectedTeam: teamId }),
      });
      await response.json();
      setSelectedId(teamId);
    } catch (error) {
      console.error(error);
    }
    setLoading(false);
  };

  return (
    <Fragment>
      <tr key={pick.id}>
        <td>
          <span>
            <input
              className="form-check-input"
              type="radio"
              value=""
              id="flexCheckDefault"
              checked={selectedId === pick.game.awayTeam.id}
              style={{ marginInline: "5px" }}
              onChange={() => handleChange(pick.game.awayTeam.id)}
            ></input>
            <img
              onError={(e) => (e.target.src = "https://placehold.jp/50x50.png")}
              loading={index < 15 ? "eager" : "lazy"}
              src={pick.game.awayTeam.logo}
              alt={pick.game.awayTeam.name}
              width="25"
              height="25"
              style={{ marginInline: "5px" }}
            />
            <span>{pick.game.awayTeam.name}</span>
          </span>
        </td>
        <td>
          <span>
            <span>{pick.head2Head}</span>
            <input
              className="form-check-input"
              type="radio"
              checked={selectedId === pick.game.homeTeam.id}
              id="flexCheckDefault"
              style={{ marginInline: "5px" }}
              onChange={() => handleChange(pick.game.homeTeam.id)}
            ></input>
            <img
              onError={(e) => (e.target.src = "https://placehold.jp/50x50.png")}
              loading={index < 15 ? "eager" : "lazy"}
              src={pick.game.homeTeam.logo}
              alt={pick.game.homeTeam.name}
              width="25"
              height="25"
              style={{ marginInline: "5px" }}
            />
            <span>{pick.game.homeTeam.name}</span>
          </span>
        </td>
      </tr>
      <tr>
        <td colSpan="2">
          {loading && <Progress animated color="primary" value="100" />}
        </td>
      </tr>
    </Fragment>
  );
};

const Picks = () => {
  const { status, data: picks, error } = useGetPicks();

  if (status === "loading") {
    return <Progress animated color="primary" value="100" />;
  }
  if (status === "success") {
    return (
      <div>
        <h1 id="tabelLabel">Picks</h1>
        <PicksTable picks={picks} />
      </div>
    );
  }
  return <div>Error Loading Picks: {error}</div>;
};

export default Picks;
