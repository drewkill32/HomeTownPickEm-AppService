import React, { Fragment, useMemo, useState } from 'react';
import {
  Container,
  Typography,
  Paper,
  Divider,
  ButtonBase,
  IconButton,
  Dialog,
  DialogTitle,
  DialogActions,
  Button,
  DialogContent,
  LinearProgress,
} from '@material-ui/core';
import clsx from 'clsx';
import { makeStyles } from '@material-ui/core/styles';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import format from 'date-fns/format';

import formatDistanceToNow from 'date-fns/formatDistanceToNow';
import isBefore from 'date-fns/isBefore';
import grey from '@material-ui/core/colors/grey';
import { useGame } from '../../hooks/useGame';
import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';
import { withStyles } from '@material-ui/styles';

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
    width: '8vmin',
    height: '8vmin',
    display: 'block',
    margin: '0 auto',
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
    gap: theme.spacing(2),
    paddingInline: theme.spacing(2),
    marginBottom: theme.spacing(1),
  },
  teamButton: {
    display: 'flex',
    margin: theme.spacing(0.25),
    flexDirection: 'column',
    justifyContent: 'center',
    alignItems: 'center',
    width: '40vmin',
    height: '20vmin',
    borderRadius: '10px',
    boxShadow: '0px 0px 5px rgba(0, 0, 0, 0.5)',
    '& > div': {
      width: '100%',
      height: '100%',
      padding: '1px',
      margin: 0,
    },
    overflow: 'hidden',
    '& p': {
      fontSize: '0.6rem',
      fontAlign: 'center',
      fontWeight: 'bold',
      padding: theme.spacing(1),
    },
  },
  teamButtonBg: {
    width: '100%',
    height: '60%',
    display: 'flex',
    alignItems: 'center',
    background: (props) =>
      `linear-gradient(50deg, ${props.color} 0%, ${props.altColor} 100%)`,
    borderTopLeftRadius: '10px',
    borderTopRightRadius: '10px',
  },
  selected: {
    boxSizing: 'border-box',
    border: `2px solid ${theme.palette.primary.main}`,

    transition: 'border 0.3s ease-in',
  },
}));

const PickLayout = ({ game: currentGame }) => {
  const classes = useStyles();

  const {
    game,
    homeSelected,
    awaySelected,
    splitSelected,
    selectHome,
    selectAway,
    selectSplit,
  } = useGame(currentGame);

  return (
    <Paper className={classes.root}>
      {isBefore(new Date(), game.cutoffDate) && (
        <Typography gutterBottom className={classes.subtitle} align="center">
          Pick will lock in {formatDistanceToNow(game.cutoffDate)}
          <LockOutlinedIcon className={classes.subtitleLock} />
        </Typography>
      )}
      <Divider />
      <Typography gutterBottom align="center">
        {format(game.startDate, 'E MMM do h:mm a')}
      </Typography>
      <div className={classes.buttonContainer}>
        <PickButton
          team={game.away}
          onClick={selectAway}
          selected={awaySelected}
        />
        {game.head2Head && (
          <SplitButton
            selected={splitSelected}
            onClick={selectSplit}
            teams={[
              {
                ...game.away,
              },
              {
                ...game.home,
              },
            ]}
          />
        )}
        <PickButton
          team={game.home}
          onClick={selectHome}
          selected={homeSelected}
        />
      </div>
      {game.head2Head && <Head2HeadFooter />}
    </Paper>
  );
};

const Head2HeadFooter = () => {
  const [open, setOpen] = useState(false);
  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };
  return (
    <Fragment>
      <Divider />
      <div
        style={{
          display: 'flex',
          justifyContent: 'flex-end',
          alignItems: 'center',
        }}
      >
        <Typography align="center" color="primary">
          Head 2 Head
        </Typography>
        <IconButton
          color="primary"
          aria-label="add to shopping cart"
          size="small"
          onClick={handleClickOpen}
        >
          <InfoOutlinedIcon />
        </IconButton>
        <Dialog open={open} onClose={handleClose}>
          <DialogTitle>Head 2 Head Matchup</DialogTitle>
          <DialogContent dividers>
            <Typography gutterBottom>
              Head to head games are worth two points!
            </Typography>
            <Typography gutterBottom>
              Head to head games are when two teams that are both in the league
              are playing against each other.
            </Typography>
            <Typography gutterBottom>
              You have to option of picking one team to win the game or split
              the points between the two teams. Splitting the points is a good
              way to play it safe by making sure you get at least point instead
              of losing two.
            </Typography>
          </DialogContent>
          <DialogActions>
            <Button autoFocus onClick={handleClose} color="primary">
              OK
            </Button>
          </DialogActions>
        </Dialog>
      </div>
    </Fragment>
  );
};

const PickButton = ({ team, onClick, selected, ...rest }) => {
  const classes = useStyles({ color: team.color, altColor: team.altColor });

  return (
    <ButtonBase
      onClick={onClick}
      {...rest}
      className={clsx(classes.teamButton, { [classes.selected]: selected })}
    >
      <Paper elevation={3}>
        <div className={classes.teamButtonBg}>
          <img className={classes.logo} src={team.logo} alt={team.name} />
        </div>
        <Typography noWrap>{team.name}</Typography>
      </Paper>
    </ButtonBase>
  );
};

const SplitButton = ({ onClick, selected, teams, ...rest }) => {
  const classes = useStyles({
    color: teams[0].color,
    altColor: teams[1].color,
  });
  return (
    <ButtonBase
      onClick={onClick}
      className={clsx(classes.teamButton, { [classes.selected]: selected })}
      {...rest}
    >
      <Paper>
        <div className={classes.teamButtonBg}>
          <img
            className={classes.logo}
            src={teams[0].logo}
            alt={teams[0].name}
          />
          <img
            className={classes.logo}
            src={teams[1].logo}
            alt={teams[1].name}
          />
        </div>
        <Typography noWrap>Split the picks</Typography>
      </Paper>
    </ButtonBase>
  );
};

const usePickStyles = makeStyles((theme) => ({
  root: {
    position: 'relative',
  },
  footer: {
    position: 'fixed',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingInline: '2rem',
    bottom: 0,
    left: 0,
    right: 0,
    height: '60px',
    background: 'linear-gradient(14deg, #f5f5f5545, #353535444, 100%)',
    borderTop: '1px solid #353535',
  },
  progress: {},
}));

const PicksHome = () => {
  const classes = usePickStyles();

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
      startDate: new Date('2021-10-11T17:00:00+00:00'),
      startTimeTbd: false,
      head2Head: true,
      cutoffDate: new Date('2021-10-09T00:00:00+00:00'),
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

  const gameCount = useMemo(() => games.length, [games]);
  const selCount = useMemo(
    () =>
      games.filter((g) => g.picks.every((p) => p.selectedTeamId !== null))
        .length,
    [games]
  );
  console.log({ selectedGames: selCount });
  const normalise = (value) => ((value - 0) * 100) / (gameCount - 0);
  return (
    <div className={classes.root}>
      <Container maxWidth="sm">
        {games.map((game) => (
          <PickLayout key={game.id} game={game} />
        ))}
      </Container>
      <div className={classes.footer}>
        <p>All Changes will autosave</p>
        <div className={classes.progress}>
          <p>1/1 Picks Made</p>
          <BorderLinearProgress
            variant="determinate"
            value={normalise(selCount)}
          />
        </div>
      </div>
    </div>
  );
};

const BorderLinearProgress = withStyles((theme) => ({
  root: {
    height: 10,
    borderRadius: 5,
  },
  colorPrimary: {
    backgroundColor: '#c2c0c0',
  },
  bar: {
    borderRadius: 5,
    backgroundColor: '#1a90ff',
  },
}))(LinearProgress);

export default PicksHome;
