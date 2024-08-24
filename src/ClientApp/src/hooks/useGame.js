import { useMemo } from 'react';



export const useGame = (game) => {

  const homeSelected = useMemo(
    () => game.picks.length > 0 && game.picks.every((p) => p.selectedTeamId === game.home.id),
    [game]
  );

  const awaySelected = useMemo(
    () => game.picks.length > 0 && game.picks.every((p) => p.selectedTeamId === game.away.id),
    [game]
  );

  const pastCutoff = game.isPastCutoff;

  const splitSelected = useMemo(
    () =>
      game.picks.length === 2 &&
      game.picks[0].selectedTeamId === game.away.id &&
      game.picks[1].selectedTeamId === game.home.id,
    [game]
  );


  return {
    game: game,
    homeSelected,
    awaySelected,
    splitSelected,
    pastCutoff
  };
};
