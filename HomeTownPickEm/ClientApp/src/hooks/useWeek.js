import { useState } from "react";
import { useHistory, useLocation } from "react-router-dom";
import Cookies from "js-cookie";

export const useWeek = () => {
  const location = useLocation();
  const history = useHistory();
  var params = new URLSearchParams(location.search);
  console.log(params.toString());
  let week = params.get("week");
  if (week) {
    Cookies.set("currWeek", week, { expires: 180 });
  } else {
    week = Cookies.get("currWeek");
    if (week) {
      history.push({
        pathname: location.pathname,
        search: "?week=" + week,
      });
    }
  }

  return week;
};
