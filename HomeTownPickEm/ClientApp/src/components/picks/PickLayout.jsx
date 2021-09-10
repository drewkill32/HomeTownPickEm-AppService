import React, { Fragment, useState } from 'react';
import {
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

import { useMakePick } from '../../hooks/useMakePick';
import isAfter from 'date-fns/isAfter';

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
      [theme.breakpoints.up('med')]: {
        fontSize: '1rem',
        paddingBlock: '2px',
      },
      [theme.breakpoints.down('sm')]: {
        fontSize: '0.5rem',
        paddingTop: '1px',
      },
      fontAlign: 'center',
      fontWeight: 500,
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
    borderBottom: '1px solid #8f8f8f',
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

  const { mutateAsync } = useMakePick();

  const handleClick = async (picks, type) => {
    switch (type) {
      case 'home':
        selectHome();
        await mutateAsync(picks);
        break;
      case 'split':
        selectSplit();
        await mutateAsync(picks);
        break;
      case 'away':
        selectAway();
        await mutateAsync(picks);
        break;
      default:
        break;
    }
  };

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
          disabled={isAfter(new Date(), game.cutoffDate)}
          onClick={() =>
            handleClick(
              game.picks.map((p) => ({
                pickId: p.id,
                selectedTeamId: game.away.id,
              })),
              'away'
            )
          }
          selected={awaySelected}
        >
          <Typography>{game.away.points}</Typography>
        </PickButton>
        {game.head2Head && (
          <SplitButton
            disabled={isAfter(new Date(), game.cutoffDate)}
            selected={splitSelected}
            onClick={() =>
              handleClick(
                [
                  { pickId: game.picks[0].id, selectedTeamId: game.away.id },
                  { pickId: game.picks[1].id, selectedTeamId: game.home.id },
                ],
                'split'
              )
            }
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
          disabled={isAfter(new Date(), game.cutoffDate)}
          onClick={() =>
            handleClick(
              game.picks.map((p) => ({
                pickId: p.id,
                selectedTeamId: game.home.id,
              })),
              'home'
            )
          }
          selected={homeSelected}
        >
          <Typography>{game.home.points}</Typography>
        </PickButton>
      </div>
      {game.winner !== 'pending' && game.head2Head && <Head2HeadFooter />}
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

const PickButton = ({
  team,
  onClick,
  selected,
  disabled,
  children,
  ...rest
}) => {
  const classes = useStyles({ color: team.color, altColor: team.altColor });

  return (
    <ButtonBase
      onClick={onClick}
      disabled={disabled}
      {...rest}
      className={clsx(classes.teamButton, { [classes.selected]: selected })}
    >
      <Paper elevation={3}>
        <div className={classes.teamButtonBg}>
          <img className={classes.logo} src={team.logo} alt={team.name} />
        </div>
        <Typography class={classes.buttonText} noWrap>
          {team.name}
        </Typography>
        {children}
      </Paper>
    </ButtonBase>
  );
};

const SplitButton = ({ onClick, selected, teams, disabled, ...rest }) => {
  const classes = useStyles({
    color: teams[0].color,
    altColor: teams[1].color,
  });
  return (
    <ButtonBase
      onClick={onClick}
      disabled={disabled}
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
        <Typography class={classes.buttonText} noWrap>
          Split the points
        </Typography>
      </Paper>
    </ButtonBase>
  );
};

export default PickLayout;
