import React, { useMemo, useReducer } from 'react';
import {
  Container,
  Typography,
  Paper,
  Divider,
  ButtonBase,
} from '@material-ui/core';
import clsx from 'clsx';
import { makeStyles } from '@material-ui/core/styles';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import format from 'date-fns/format';

import formatDistanceToNow from 'date-fns/formatDistanceToNow';
import isBefore from 'date-fns/isBefore';
import grey from '@material-ui/core/colors/grey';

const useStyles = makeStyles((theme) => ({
  root: {
    display: 'flex',
    flexDirection: 'column',
    marginBottom: theme.spacing(3),
  },
  details: {
    display: 'flex',
    flexDirection: 'column',
  },
  content: {
    flex: '1 0 auto',
  },
  logo: {
    width: 50,
    height: 50,
    display: 'block',
    margin: '0 auto',
  },
  controls: {
    display: 'flex',
    alignItems: 'center',
    paddingLeft: theme.spacing(1),
    paddingBottom: theme.spacing(1),
  },
  playIcon: {
    height: 38,
    width: 38,
  },
  subtitle: {
    color: grey[500],
    fontSize: '0.7rem',
  },
  subtitleLock: {
    color: grey[500],
    fontSize: '0.7rem',
    marginLeft: theme.spacing(1),
  },
  buttonContainer: {
    display: 'flex',
    justifyContent: 'space-between',
    paddingInline: theme.spacing(4),
    marginBottom: theme.spacing(1),
  },
  teamButton: {
    display: 'flex',
    margin: theme.spacing(0.25),
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
    width: '80px',
    height: '80px',
    borderRadius: '10px',
    overflow: 'hidden',
    '& p': {
      fontSize: '0.6rem',
      fontAlign: 'center',
      fontWeight: 'bold',
      padding: theme.spacing(1),
    },
  },
  teamButtonBg: {
    width: '80px',
    height: '80px',
    display: 'flex',
    alignItems: 'center',
    background: (props) =>
      `linear-gradient(50deg, ${props.color} 0%, ${props.altColor} 100%)`,
    borderTopLeftRadius: '10px',
    borderTopRightRadius: '10px',
  },
  selected: {
    border: `2px solid ${theme.palette.primary.main}`,
    borderRadius: '10px',
  },
}));

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

const PickLayout = ({ game }) => {
  const classes = useStyles();
  const [state, dispatch] = useReducer(reducer, game);

  const homeSelected = useMemo(
    () => state.picks.every((p) => p.selectedTeamId === state.home.id),
    [state]
  );

  const awaySelected = useMemo(
    () => state.picks.every((p) => p.selectedTeamId === state.away.id),
    [state]
  );

  const splitSelected = useMemo(
    () =>
      state.picks.length === 2 &&
      state.picks[0].selectedTeamId === state.away.id &&
      state.picks[1].selectedTeamId === state.home.id,
    [state]
  );

  return (
    <Paper className={classes.root}>
      {isBefore(new Date(), state.cutoffDate) && (
        <Typography gutterBottom className={classes.subtitle} align="center">
          Pick will lock in {formatDistanceToNow(state.cutoffDate)}
          <LockOutlinedIcon className={classes.subtitleLock} />
        </Typography>
      )}
      <Divider />
      <Typography gutterBottom align="center">
        {format(state.startDate, 'E MMM do h:mm a')}
      </Typography>
      <div className={classes.buttonContainer}>
        <PickButton
          team={state.away}
          onClick={() => dispatch({ type: ACTIONS.SELECT_AWAY })}
          selected={awaySelected}
        />
        {state.head2Head && (
          <SplitButton
            selected={splitSelected}
            onClick={() => dispatch({ type: ACTIONS.SELECT_SPLIT })}
          />
        )}
        <PickButton
          team={state.home}
          onClick={() => dispatch({ type: ACTIONS.SELECT_HOME })}
          selected={homeSelected}
        />
      </div>
    </Paper>
  );
};

const PickButton = ({ team, onClick, selected, ...rest }) => {
  const classes = useStyles({ color: team.color, altColor: team.altColor });

  return (
    <ButtonBase
      onClick={onClick}
      {...rest}
      className={clsx({ [classes.selected]: selected })}
    >
      <Paper className={classes.teamButton}>
        <div className={classes.teamButtonBg}>
          <img className={classes.logo} src={team.logo} alt={team.name} />
        </div>
        <Typography noWrap>{team.name}</Typography>
      </Paper>
    </ButtonBase>
  );
};

const SplitButton = ({ onClick, selected, ...rest }) => {
  const classes = useStyles();
  return (
    <ButtonBase
      onClick={onClick}
      className={clsx({ [classes.selected]: selected })}
      {...rest}
    >
      <Paper className={classes.teamButton}>
        <div>
          <Typography>Hello</Typography>
        </div>
        <Typography>Split the picks</Typography>
      </Paper>
    </ButtonBase>
  );
};

const PicksHome = () => {
  const games = [
    {
      id: 1,
      home: {
        id: 152,
        points: 45,
        name: 'NC State Wolfpack',
        logo: 'https://a.espncdn.com/i/teamlogos/ncaa/500/152.png',
        color: '#EF1216',
        altColor: '#231f20',
      },
      away: {
        id: 58,
        points: 0,
        name: 'South Florida Bulls',
        logo: 'https://a.espncdn.com/i/teamlogos/ncaa/500/58.png',
        color: '#004A36',
        altColor: '#237F20',
      },
      winnerId: 152,
      winner: 'home',
      status: 'Complete',
      startDate: new Date('2021-09-02T23:30:00+00:00'),
      startTimeTbd: false,
      head2Head: false,
      cutoffDate: new Date('2021-09-02T00:00:00+00:00'),
      picks: [
        {
          id: 1,
          gameId: 1,
          userId: '22c5c226-c3f8-4caa-9eba-72eb242067d9',
          selectedTeamId: 58,
        },
      ],
    },
    {
      id: 2,
      away: {
        id: 57,
        points: null,
        name: 'Florida Gators',
        logo: 'https://a.espncdn.com/i/teamlogos/ncaa/500/57.png',
        color: '#004A36',
        altColor: '#231f20',
      },
      home: {
        id: 58,
        points: null,
        name: 'South Florida Bulls',
        logo: 'https://a.espncdn.com/i/teamlogos/ncaa/500/58.png',
        color: '#004A36',
        altColor: '#237F20',
      },
      winnerId: null,
      winner: null,
      status: 'Pending',
      startDate: new Date('2021-09-11T17:00:00+00:00'),
      startTimeTbd: false,
      head2Head: true,
      cutoffDate: new Date('2021-09-09T00:00:00+00:00'),
      picks: [
        {
          id: 2,
          gameId: 2,
          userId: '22c5c226-c3f8-4caa-9eba-72eb242067d9',
          selectedTeamId: null,
        },
        {
          id: 3,
          gameId: 2,
          userId: '22c5c226-c3f8-4caa-9eba-72eb242067d9',
          selectedTeamId: null,
        },
      ],
    },
  ];

  return (
    <Container maxWidth="md">
      {games.map((game) => (
        <PickLayout key={game.id} game={game} />
      ))}
    </Container>
  );
};

export default PicksHome;
