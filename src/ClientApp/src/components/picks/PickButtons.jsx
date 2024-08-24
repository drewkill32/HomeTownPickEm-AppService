import React from 'react';
import { ButtonBase, Paper, Typography } from '@mui/material';
import clsx from 'clsx';
import { makeStyles } from '@mui/styles';

const useStyles = makeStyles((theme) => ({
  logo: {
    width: '7.4vmin',
    minWidth: '40px',
    minHeight: '40px',
    height: '7.4vmin',
    display: 'block',
    margin: '0 auto',
  },
  splitLogo: {
    minWidth: '25px',
    minHeight: '25px',
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
    [theme.breakpoints.down('md')]: {
      width: '40vmin',
      minHeight: '130px',
    },
    width: '40vmin',
    minHeight: '180px',
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
      [theme.breakpoints.down('md')]: {
        fontSize: ({ noWrap }) => (noWrap ? '0.75rem' : '0.85rem'),
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
    [theme.breakpoints.down('md')]: {
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
      {...rest}>
      <Paper>
        <div className={classes.teamButtonBg}>
          <img
            className={clsx(classes.logo, classes.splitLogo)}
            src={teams[0].logo}
            alt={teams[0].name}
          />
          <img
            className={clsx(classes.logo, classes.splitLogo)}
            src={teams[1].logo}
            alt={teams[1].name}
          />
        </div>
        <Typography>Split points</Typography>
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
  noWrap,
  ...rest
}) => {
  const classes = useStyles({
    color: team.color,
    altColor: team.altColor,
    noWrap,
  });

  return (
    <ButtonBase
      onClick={onClick}
      disabled={disabled}
      {...rest}
      data-testid={`pick-button-${team.id}`}
      className={clsx(classes.teamButton, { [classes.selected]: selected })}>
      <Paper elevation={3} sx={{ mb: 6 }}>
        <div className={classes.teamButtonBg}>
          <img className={classes.logo} src={team.logo} alt={team.name} />
        </div>
        <Typography className={classes.buttonText} noWrap={noWrap}>
          {team.name}
        </Typography>
        {children}
      </Paper>
    </ButtonBase>
  );
};
