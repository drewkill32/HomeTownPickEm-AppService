import React, {Fragment, useState} from 'react';

import PickTeam from './PickTeam';

const PickRow = ({pick, isAfter}) => {
  const [selectedId, setSelectedId] = useState(
      pick.selectedTeam && pick.selectedTeam.id
  );
  return (
      <Fragment>
        <tr>
          <td>
            <PickTeam
                pickId={pick.id}
                points={pick.game.awayPoints}
                isAfter={isAfter}
                team={pick.game.awayTeam}
                onChange={(teamId) => setSelectedId(teamId)}
                isSelected={selectedId === pick.game.awayTeam.id}
            />
          </td>
          <td>
            <PickTeam
                pickId={pick.id}
                points={pick.game.homePoints}
                isAfter={isAfter}
                team={pick.game.homeTeam}
                onChange={(teamId) => setSelectedId(teamId)}
                isSelected={selectedId === pick.game.homeTeam.id}
            />
          </td>
        </tr>
      </Fragment>
  );
};

export default PickRow;
