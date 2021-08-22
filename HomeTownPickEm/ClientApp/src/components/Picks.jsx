import React, {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import authService from "./api-authorization/AuthorizeService";
import {Spinner} from "reactstrap";

const PicksTable = ({picks}) => {
    return (
        <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
            <tr>
                <th>Away</th>
                <th>Home</th>
                <th></th>
            </tr>
            </thead>
            <tbody>
            {picks.map((pick, index) => {
                return <PickRow key={pick.id} pick={pick} index={index}/>;
            })}
            </tbody>
        </table>
    );
};

const PickRow = ({pick, index}) => {
    const [loading, setLoading] = useState(false);
    const [selectedId, setSelectedId] = useState(
        pick.selectedTeam && pick.selectedTeam.id
    );

    useEffect(() => {
    }, [pick]);

    const handleChange = async (teamId) => {
        setLoading(true);
        try {
            const token = await authService.getAccessToken();
            const response = await fetch(`api/pick/${pick.id}`, {
                method: "PUT",
                headers: !token
                    ? {"Content-Type": "application/json"}
                    : {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                body: JSON.stringify({selectedTeam: teamId}),
            });
            await response.json();
            setSelectedId(teamId);
        } catch (error) {
      console.error(error);
    }
    setLoading(false);
  };

  return (
      <tr key={pick.id}>
          <td>
        <span>
          <input
              class="form-check-input"
              type="radio"
              value=""
              id="flexCheckDefault"
              checked={selectedId === pick.game.awayTeam.id}
              style={{marginInline: "5px"}}
              onChange={() => handleChange(pick.game.awayTeam.id)}
          ></input>
          <img
              onError={(e) => (e.target.src = "https://placehold.jp/50x50.png")}
              loading={index < 15 ? "eager" : "lazy"}
              src={pick.game.awayTeam.logo}
              alt={pick.game.awayTeam.name}
              width="25"
              height="25"
              style={{marginInline: "5px"}}
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
              style={{marginInline: "5px"}}
              onChange={() => handleChange(pick.game.homeTeam.id)}
          ></input>
          <img
              onError={(e) => (e.target.src = "https://placehold.jp/50x50.png")}
              loading={index < 15 ? "eager" : "lazy"}
              src={pick.game.homeTeam.logo}
              alt={pick.game.homeTeam.name}
              width="25"
              height="25"
              style={{marginInline: "5px"}}
          />
          <span>{pick.game.homeTeam.name}</span>
        </span>
          </td>
          <td>{loading && <Spinner color="primary"> </Spinner>}</td>
      </tr>
  );
};

const Picks = () => {
    const [picks, setTeams] = useState([]);
    const [loading, setLoading] = useState(true);
    const {week} = useParams();
    useEffect(() => {
        populatePicks();
    }, []);

    const populatePicks = async () => {
        const token = await authService.getAccessToken();
        const user = await authService.getUser();
        if (!user) {
            throw new Error("User not found");
        }
    const id = user.sub;
    const response = await fetch(`api/picks/1/${id}/week/${week}`, {
        headers: !token ? {} : {Authorization: `Bearer ${token}`},
    });
    const data = await response.json();
    setTeams(data);
    setLoading(false);
  };
  return (
      <div>
          <h1 id="tabelLabel">Picks</h1>
          {loading ? (
              <p>
                  <em>Loading...</em>
              </p>
          ) : (
              <PicksTable picks={picks}/>
          )}
      </div>
  );
};

export default Picks;
