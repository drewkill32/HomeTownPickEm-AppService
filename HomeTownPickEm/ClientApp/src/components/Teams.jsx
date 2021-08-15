import React, { useState, useEffect } from "react";
import authService from "./api-authorization/AuthorizeService";

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
                loading={index < 15 ? "eager" : "lazy"}
                src={team.logos && team.logos.length > 0 && team.logos[0]}
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

  useEffect(() => {
    populateTeams();
  }, []);

  const populateTeams = async () => {
    const token = await authService.getAccessToken();
    const response = await fetch("api/teams/all", {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
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
