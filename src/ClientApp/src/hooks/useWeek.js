import { useNavigate, useLocation } from 'react-router-dom';
import Cookies from 'js-cookie';

export const useWeek = () => {
  const location = useLocation();
  const navigate = useNavigate();
  var params = new URLSearchParams(location.search);
  let week = params.get('week');

  if (week) {
    Cookies.set('currWeek', week, { expires: 180 });
  } else {
    week = Cookies.get('currWeek');
    if (week) {
      navigate({
        pathname: location.pathname,
        search: '?week=' + week,
      });
    } else {
      navigate({
        pathname: location.pathname,
        search: '?week=1',
      });
    }
  }

  return week;
};
