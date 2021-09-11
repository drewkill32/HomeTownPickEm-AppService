import React, {useMemo} from 'react';
import {Accordion, AccordionDetails, AccordionSummary, Avatar, Badge, Grid, Typography,} from '@material-ui/core';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import {useQuery} from 'react-query';
import axios from 'axios';
import {makeStyles} from '@material-ui/styles';

const useStyles = makeStyles((theme) => ({
  avatar: {
    background: (props) =>
        `linear-gradient(30deg, ${props.color} 10%, ${props.altColor} 100%)`,
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

const UserPicks = ({gameId, homeId, awayId}) => {
  const [expanded, setExpanded] = React.useState(false);

  const {data} = useQuery(
      ['gamePicks', gameId],
      () =>
          axios
              .get(`/api/league/st-pete-pick-em/game/${gameId}`)
              .then((res) => res.data),
      {
        enabled: expanded,
      }
  );

  const homePicks = useMemo(
      () => (data ? data.filter((pick) => pick.selectedTeamId === homeId) : []),
      [data]
  );

  const awayPicks = useMemo(
      () => (data ? data.filter((pick) => pick.selectedTeamId === awayId) : []),
      [data]
  );

  const splitPicks = useMemo(
      () => (data ? data.filter((pick) => pick.selectedTeamId === -1) : []),
      [data]
  );

  const UserAvatar = ({pick, direction = 'left'}) => {
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
            }}
        >
          <Badge
              color="primary"
              badgeContent={
                <div>
                  {pick.rank}
                  <sup>{getNumberSuffix(pick.rank)}</sup>
                </div>
              }
          >
            <Avatar className={classes.avatar}>{pick.initials}</Avatar>
          </Badge>
          <Typography>{pick.name}</Typography>
        </div>
    );
  };

  return (
      <Accordion onChange={(_, exp) => setExpanded(exp)} expanded={expanded}>
        <AccordionSummary expandIcon={<ExpandMoreIcon/>}>Picks</AccordionSummary>
        <AccordionDetails>
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
            >
              {awayPicks.map((pick) => (
                  <Grid item key={pick.userId}>
                    <UserAvatar pick={pick} direction="left"/>
                  </Grid>
              ))}
            </Grid>
            <Grid item>
              {splitPicks.map((pick) => (
                  <Grid item key={pick.userId}>
                    <UserAvatar pick={pick} direction="right"/>
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
            >
              {homePicks.map((pick) => (
                  <Grid item key={pick.userId}>
                    <UserAvatar pick={pick} direction="right"/>
                  </Grid>
              ))}
            </Grid>
          </Grid>
        </AccordionDetails>
      </Accordion>
  );
};

export default UserPicks;
