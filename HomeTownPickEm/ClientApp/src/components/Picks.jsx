import React, {useEffect, useState} from "react";
import {useParams} from "react-router-dom";
import authService from "./api-authorization/AuthorizeService";

// "id": 109,
// "leagueId": 1,
// "gameId": 401282607,
// "game": {
//   "id": 401282607,
//   "season": "2021",
//   "week": 1,
//   "seasonType": "regular",
//   "startDate": "2021-09-02T23:30:00+00:00",
//   "startTimeTbd": false,
//   "homePoints": null,
//   "homeTeam": {
//     "id": 152,
//     "school": "NC State",
//     "mascot": "Wolfpack",
//     "abbreviation": "NCST",
//     "conference": "ACC",
//     "division": "Atlantic",
//     "color": "#EF1216",
//     "altColor": "#231f20",
//     "logo": "http://a.espncdn.com/i/teamlogos/ncaa/500/152.png",
//     "name": "NC State Wolfpack"
//   },
//   "awayTeam": {
//     "id": 58,
//     "school": "South Florida",
//     "mascot": "Bulls",
//     "abbreviation": "USF",
//     "conference": "American Athletic",
//     "division": "East",
//     "color": "#004A36",
//     "altColor": "#231f20",
//     "logo": "http://a.espncdn.com/i/teamlogos/ncaa/500/58.png",
//     "name": "South Florida Bulls"
//   },
//   "awayPoints": null,
//   "gameFinal": false,
//   "winner": null
// },
// "userId": "cce50321-35f8-4a51-85c4-dc249e4ad1ea",
// "points": 0,
// "teamsPicked": []

const PicksTable = ({picks}) => {
  return (
      <table className="table table-striped" aria-labelledby="tabelLabel">
        <thead>
        <tr>
          <th>Start Date</th>
          <th>Away</th>
          <th>Home</th>
        </tr>
        </thead>
        <tbody>
        {picks.map((pick, index) => {
          console.log("isHeadToHead", pick.head2Head);
          return (
              <tr key={pick.id}>
                <td>{pick.game.startDate}</td>
                <td>
                <span>
                  <input
                      class="form-check-input"
                      type="checkbox"
                      value=""
                      id="flexCheckDefault"
                  ></input>
                  <img
                      onError={(e) =>
                          (e.target.src = "https://placehold.jp/50x50.png")
                      }
                      loading={index < 15 ? "eager" : "lazy"}
                      src={pick.game.awayTeam.logo}
                      alt={pick.game.awayTeam.name}
                      width="25"
                      height="25"
                      style={{marginRight: "10px"}}
                  />
                  <span>{pick.game.awayTeam.name}</span>
                </span>
                </td>
                <td>
                <span>
                  <span>{pick.head2Head}</span>
                  <input
                      class="form-check-input"
                      type="checkbox"
                      value=""
                      id="flexCheckDefault"
                  ></input>
                  <img
                      onError={(e) =>
                          (e.target.src = "https://placehold.jp/50x50.png")
                      }
                      loading={index < 15 ? "eager" : "lazy"}
                      src={pick.game.homeTeam.logo}
                      alt={pick.game.homeTeam.name}
                      width="25"
                      height="25"
                      style={{marginRight: "10px"}}
                  />
                  <span>{pick.game.homeTeam.name}</span>
                </span>
                </td>
              </tr>
          );
        })}
        </tbody>
      </table>
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
