import React, { useMemo } from 'react';
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Avatar,
  Badge,
  Grid,
  Typography,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { useQuery } from 'react-query';
import axios from 'axios';
import { makeStyles } from '@mui/styles';
import { useLeague } from '../../features/league';

const useStyles = makeStyles((theme) => ({
  avatar: {
    [theme.breakpoints.down('md')]: {
      width: '2rem',
      height: '2rem',
      fontSize: '0.8rem',
    },
    background: (props) =>
      `linear-gradient(30deg, ${props.color} 10%, ${props.altColor} 100%)`,
  },
  badge: {
    top: '-3px',
  },
  details: {
    paddingRight: '0',
    marginRight: '-10px',
  },
}));

const getNumberSuffix = (i) => {
  var j = i % 10,
    k = i % 100;
  if (j === 1 && k !== 11) {
    return 'st';
  }
  if (j === 2 && k !== 12) {
    return 'nd';
  }
  if (j === 3 && k !== 13) {
    return 'rd';
  }
  return 'th';
};

const UserAvatar = ({ pick, direction = 'left' }) => {
  const classes = useStyles({
    color: pick.teamColor,
    altColor: pick.teamAltColor || '#fff',
  });
  return (
    <div
      style={{
        display: 'flex',
        alignItems: 'center',
        gap: '0.5rem',
        flexDirection: direction === 'left' ? 'row' : 'row-reverse',
      }}>
      <Badge
        anchorOrigin={{ horizontal: direction, vertical: 'top' }}
        color="primary"
        className={classes.badge}
        badgeContent={
          <div>
            {pick.rank}
            <sup>{getNumberSuffix(pick.rank)}</sup>
          </div>
        }>
        <Avatar className={classes.avatar}>{pick.initials}</Avatar>
      </Badge>
      <Typography align={direction}>{pick.name}</Typography>
    </div>
  );
};

const UserPicks = ({ gameId, homeId, awayId }) => {
  const classes = useStyles();
  const [expanded, setExpanded] = React.useState(false);
  const league = useLeague();
  const { data } = useQuery(
    ['gamePicks', gameId],
    () =>
      axios
        .get(`/api/league/${league.id}/${league.season}/game/${gameId}`)
        .then((res) => res.data),
    {
      enabled: expanded,
    }
  );

  const homePicks = useMemo(
    () => (data ? data.filter((pick) => pick.selectedTeamId === homeId) : []),
    [data, homeId]
  );

  const awayPicks = useMemo(
    () => (data ? data.filter((pick) => pick.selectedTeamId === awayId) : []),
    [data, awayId]
  );

  return (
    <Accordion onChange={(_, exp) => setExpanded(exp)} expanded={expanded}>
      <AccordionSummary expandIcon={<ExpandMoreIcon />}>Picks</AccordionSummary>
      <AccordionDetails className={classes.details}>
        <Grid container>
          <Grid
            container
            item
            xs={6}
            spacing={3}
            alignContent="flex-start"
            alignItems="flex-start"
            direction="column"
            justifyContent="flex-start"
            className={classes.grid}>
            {awayPicks.map((pick) => (
              <Grid item key={pick.userId}>
                <UserAvatar pick={pick} direction="left" />
              </Grid>
            ))}
          </Grid>
          <Grid
            container
            item
            xs={6}
            spacing={3}
            alignContent="flex-end"
            alignItems="flex-end"
            direction="column"
            justifyContent="flex-start"
            className={classes.grid}>
            {homePicks.map((pick) => (
              <Grid item key={pick.userId}>
                <UserAvatar pick={pick} direction="right" />
              </Grid>
            ))}
          </Grid>
        </Grid>
      </AccordionDetails>
    </Accordion>
  );
};

export default UserPicks;
