import React from 'react';
import { ButtonBase, Paper, Typography } from '@material-ui/core';
import clsx from 'clsx';
import { makeStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme) => ({
  logo: {
    width: '7.4vmin',
    minWidth: '40px',
    minHeight: '40px',
    height: '7.4vmin',
    display: 'block',
    margin: '0 auto',
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
    [theme.breakpoints.down('sm')]: {
      width: '40vmin',
      height: '125px',
    },
    width: '40vmin',
    height: '180px',
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
        fontSize: '0.75rem',
        paddingBlock: '1px',
      },
      paddingInline: '0.5rem',
      fontAlign: 'center',
      fontWeight: 500,
    },
  },
  teamButtonBg: {
    width: '100%',
    height: '60%',
    [theme.breakpoints.down('sm')]: {
      maxHeight: '62px',
    },
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

export const SplitButton = ({
  onClick,
  selected,
  teams,
  disabled,
  ...rest
}) => {
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
        <Typography noWrap>Split the points</Typography>
      </Paper>
    </ButtonBase>
  );
};

export const PickButton = ({
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
        <Typography className={classes.buttonText} noWrap>
          {team.name}
        </Typography>
        {children}
      </Paper>
    </ButtonBase>
  );
};
