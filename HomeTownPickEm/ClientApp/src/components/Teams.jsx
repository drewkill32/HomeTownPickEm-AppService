import React, {useEffect, useState} from "react";
import {useAuth} from "../hooks/useAuth";

const TeamsTable = ({ teams }) => {
  return (
    <table className="table table-striped" aria-labelledby="tabelLabel">
      <thead>
        <tr>
          <th>Logo</th>
          <th>Name</th>
          <th>Conference</th>
          <th>Division</th>
        </tr>
      </thead>
      <tbody>
        {teams.map((team, index) => (
          <tr key={team.id}>
            <td>
              <img
                  onError={(e) =>
                      (e.target.src = "https://placehold.jp/50x50.png")
                  }
                  loading={index < 15 ? "eager" : "lazy"}
                  src={team.logo}
                  alt={team.name}
                  width="25"
                  height="25"
              />
            </td>
            <td>{team.name}</td>
            <td>{team.conference}</td>
            <td>{team.division}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};

const Teams = () => {
  const [teams, setTeams] = useState([]);
  const [loading, setLoading] = useState(true);
  const {getToken} = useAuth();
  useEffect(() => {
    populateTeams();
  }, []);

  const populateTeams = async () => {
    const token = await getToken();
    const response = await fetch("api/teams", {
      headers: !token ? {} : {Authorization: `Bearer ${token}`},
    });
    const data = await response.json();
    setTeams(data);
    setLoading(false);
  };
  return (
    <div>
      <h1 id="tabelLabel">All Teams</h1>
      {loading ? (
        <p>
          <em>Loading...</em>
        </p>
      ) : (
        <TeamsTable teams={teams} />
      )}
    </div>
  );
};

export default Teams;
