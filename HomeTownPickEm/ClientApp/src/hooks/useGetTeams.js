import axios from "axios";
import {useQuery} from "react-query";

export default function useGetTeams() {
  return useQuery("teams", () =>
      axios.get("/api/teams").then((res) => res.data)
  );
}
