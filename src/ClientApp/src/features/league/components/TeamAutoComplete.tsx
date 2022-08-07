import { AvailableTeamsResult } from '../types';
import { SyntheticEvent } from 'react';
import { Autocomplete, Box } from '@mui/material';

export function TeamAutoComplete(props: {
  options: AvailableTeamsResult[];
  value: AvailableTeamsResult | null;
  onChange: (e: SyntheticEvent, team: AvailableTeamsResult | null) => void;
  renderInput: (params: any) => JSX.Element;
}) {
  return (
    <Autocomplete
      options={props.options}
      value={props.value}
      isOptionEqualToValue={(option, value) => option.id === value?.id}
      onChange={(e, t) => props.onChange(e, t)}
      renderOption={(params, option) => (
        <Box
          component="li"
          sx={{ '& > img': { mr: 2, flexShrink: 0 } }}
          // @ts-ignore
          {...params}>
          <img
            loading="lazy"
            width="20"
            src={option.logo || '/img/helmet.png'}
            alt={option.name}
            onError={(e) => {
              // @ts-ignore
              e.target.src = '/img/helmet.png';
            }}
          />
          {option.name}
        </Box>
      )}
      getOptionLabel={(option) => option.name}
      renderInput={props.renderInput}
    />
  );
}
