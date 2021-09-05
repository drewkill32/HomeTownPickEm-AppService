import React, {Fragment} from 'react';
import {Divider, Grid, Paper, Typography} from '@material-ui/core';
import {makeStyles} from '@material-ui/core/styles';
import CheckCircleOutlineRoundedIcon from '@material-ui/icons/CheckCircleOutlineRounded';
import HighlightOffRoundedIcon from '@material-ui/icons/HighlightOffRounded';
import RadioButtonCheckedOutlinedIcon from '@material-ui/icons/RadioButtonCheckedOutlined';
import {green, grey, red} from '@material-ui/core/colors';

const useStyles = makeStyles((theme) => ({
    small: {
        width: theme.spacing(3),
        height: theme.spacing(3),
        padding: '3px',
        objectFit: 'contain',
    },
    paper: {
        padding: theme.spacing(2),
        paddingRight: theme.spacing(4),
        width: '100%',
    },
    score: {
        fontWeight: 600,
    },
    teamName: {
        marginLeft: theme.spacing(1),
    },
}));

const TeamPick = ({team, status}) => {
    const classes = useStyles();
    return (
        <Fragment>
            <Grid item xs={1}>
                <img className={classes.small} alt={team.name} src={team.logo}/>
            </Grid>
            <Grid item xs={9}>
                <Typography noWrap className={classes.teamName}>
                    {team.name}
                </Typography>
            </Grid>
            <Grid item xs={1}>
                <Typography className={classes.score}>
                    {team.points ? team.points : '-'}
                </Typography>
            </Grid>
            <Grid item xs={1}>
                {getIcon(team.isSelected, status)}
            </Grid>
        </Fragment>
    );
};

const getIcon = (isSelected, status) => {
    if (!isSelected) {
        return null;
    }
    if (status === 'Pending') {
        return <RadioButtonCheckedOutlinedIcon style={{color: grey[600]}}/>;
    }
    if (status === 'Win') {
        return <CheckCircleOutlineRoundedIcon style={{color: green[400]}}/>;
    }
    return <HighlightOffRoundedIcon style={{color: red[400]}}/>;
};

const GamePick = ({pick}) => {
    const classes = useStyles();
    return (
        <Grid item>
            <Paper className={classes.paper}>
                <Grid container spacing={2}>
                    <TeamPick
                        team={{
                            points: pick.game.awayPoints,
                            isSelected: pick.selectedTeamId === pick.game.away.id,
                            ...pick.game.away,
                        }}
                        status={pick.status}
                    />
                    <Grid item xs={12}>
                        <Divider variant="fullWidth"/>
                    </Grid>
                    <TeamPick
                        team={{
                            points: pick.game.homePoints,
                            isSelected: pick.selectedTeamId === pick.game.home.id,
                            ...pick.game.home,
                        }}
                        status={pick.status}
                    />
                </Grid>
            </Paper>
        </Grid>
    );
};

export default GamePick;
