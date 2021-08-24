import axios from "axios";
import {useQuery} from "react-query";

export default function useGetTeams() {
  return useQuery("teams", () =>
      axios.get("/api/league/1/availableteams").then((res) => res.data)
  );
}
