import React, { useEffect } from "react";
import authService from "./api-authorization/AuthorizeService";

const ForecastsTable = ({ forecasts }) => {
  return (
    <table className="table table-striped" aria-labelledby="tabelLabel">
      <thead>
        <tr>
          <th>Date</th>
          <th>Temp. (C)</th>
          <th>Temp. (F)</th>
          <th>Summary</th>
        </tr>
      </thead>
      <tbody>
        {forecasts.map((forecast) => (
          <tr key={forecast.date}>
            <td>{forecast.date}</td>
            <td>{forecast.temperatureC}</td>
            <td>{forecast.temperatureF}</td>
            <td>{forecast.summary}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
};

const FetchData = () => {
  const [forecasts, setForecasts] = React.useState([]);
  const [loading, setLoading] = React.useState(true);

  useEffect(() => {
    populateWeatherData();
  }, []);

  const populateWeatherData = async () => {
    const token = await authService.getAccessToken();
    const response = await fetch("weatherforecast", {
      headers: !token ? {} : { Authorization: `Bearer ${token}` },
    });
    const data = await response.json();
    setForecasts(data);
    setLoading(false);
  };

  return (
    <div>
      <h1 id="tabelLabel">Weather forecast</h1>
      <p>This component demonstrates fetching data from the server.</p>
      {loading ? (
        <p>
          <em>Loading...</em>
        </p>
      ) : (
        <ForecastsTable forecasts={forecasts} />
      )}
    </div>
  );
};

export default FetchData;
