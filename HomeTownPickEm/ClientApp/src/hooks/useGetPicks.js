import { useQuery } from "react-query";
import { useAuth } from "./useAuth";
import { useWeek } from "./useWeek";

export default function useGetPicks() {
  const { getToken, user } = useAuth();
  const week = useWeek();
  const token = getToken();
  const id = user.id;
  return useQuery(["picks", user.id, week], () =>
    fetch(`api/picks/1/${id}/week/${week}`, {
      headers: { Authorization: `Bearer ${token}` },
    }).then((res) => res.json())
  );
}
