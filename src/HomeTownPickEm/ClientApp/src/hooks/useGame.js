import {isAfter} from 'date-fns';
import {useMemo, useReducer} from 'react';

const ACTIONS = {
    SELECT_HOME: 'SELECT_HOME',
    SELECT_AWAY: 'SELECT_AWAY',
    SELECT_SPLIT: 'SELECT_SPLIT',
};

const reducer = (state, action) => {
    switch (action.type) {
        case ACTIONS.SELECT_HOME:
            return {
                ...state,
                picks: state.picks.map((p) => ({
                    ...p,
                    selectedTeamId: state.home.id,
                })),
            };
        case ACTIONS.SELECT_AWAY:
            return {
                ...state,
        picks: state.picks.map((p) => ({
          ...p,
          selectedTeamId: state.away.id,
        })),
      };
    case ACTIONS.SELECT_SPLIT:
      var pickOne = { ...state.picks[0], selectedTeamId: state.away.id };
      var pickTwo = { ...state.picks[1], selectedTeamId: state.home.id };
      return {
        ...state,
        picks: [pickOne, pickTwo],
      };
    default:
      return state;
  }
};

export const useGame = (game) => {
    const [state, dispatch] = useReducer(reducer, game);

    const homeSelected = useMemo(
        () => state.picks.every((p) => p.selectedTeamId === state.home.id),
        [state]
    );

    const awaySelected = useMemo(
        () => state.picks.every((p) => p.selectedTeamId === state.away.id),
        [state]
    );

    const pastCutoff = useMemo(() => {
        var date = new Date();
        var now_utc = Date.UTC(
            date.getUTCFullYear(),
            date.getUTCMonth(),
            date.getUTCDate(),
            date.getUTCHours(),
            date.getUTCMinutes(),
            date.getUTCSeconds()
        );

        return isAfter(new Date(now_utc), game.cutoffDate);
    }, [state]);

    const splitSelected = useMemo(
        () =>
            state.picks.length === 2 &&
            state.picks[0].selectedTeamId === state.away.id &&
            state.picks[1].selectedTeamId === state.home.id,
        [state]
    );

    const selectHome = () => dispatch({type: ACTIONS.SELECT_HOME});
    const selectAway = () => dispatch({type: ACTIONS.SELECT_AWAY});
    const selectSplit = () => dispatch({type: ACTIONS.SELECT_SPLIT});

    return {
        game: state,
        homeSelected,
        awaySelected,
        splitSelected,
    pastCutoff,
    selectHome,
    selectAway,
    selectSplit,
  };
};
