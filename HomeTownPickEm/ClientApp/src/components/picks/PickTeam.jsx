import React from 'react';
import {useMutation} from 'react-query';
import axios from 'axios';

const PickTeam = ({
                    pickId,
                    points,
                    team,
                    isAfter,
                    isSelected,
                    isWinner,
                    onChange,
                  }) => {
  const mutation = useMutation((selectedTeam) =>
      axios.put(`/api/pick/${pickId}`, {selectedTeam}).then((res) => res.data)
  );
  const handleChange = async (teamId) => {
    await mutation.mutate(teamId);
    onChange(teamId);
  };
  return (
      <span>
      {isAfter ? (
          <span style={{color: isWinner ? 'green' : 'red'}}>
          <strong>{points || 0}</strong>
        </span>
      ) : (
          <input
              className="form-check-input"
              type="radio"
              value=""
              id={`${team.id}-${pickId}`}
              checked={isSelected}
              style={{marginInline: '5px'}}
              onChange={() => handleChange(team.id)}
          ></input>
      )}
        <label htmlFor={`${team.id}-${pickId}`}>
        <img
            onError={(e) => (e.target.src = 'https://placehold.jp/50x50.png')}
            src={team.logo}
            alt={team.name}
            width="25"
            height="25"
            style={{marginInline: '5px'}}
        />
          {team.name}
      </label>
    </span>
  );
};

export default PickTeam;
