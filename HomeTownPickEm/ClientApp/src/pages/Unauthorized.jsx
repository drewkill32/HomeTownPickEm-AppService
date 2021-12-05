import React from 'react';

import {makeStyles} from '@material-ui/core/styles';
import {Typography} from '@material-ui/core';
import {useLocation} from 'react-router';

const useStyles = makeStyles({
    root: {
        border: 'solid 1px #fc2a0e',
        borderRadius: 3,
        boxShadow: '0 3px 5px 2px #ff69764c',
        padding: '0 30px',
        display: 'flex',
        alignItems: 'center',
        flexDirection: 'column',
        justifyContent: 'space-around',
        paddingTop: '10px',
        minHeight: '10rem',
    },
});

const Unauthorized = () => {
    const classes = useStyles();
    const location = useLocation();
    var params = new URLSearchParams(location.search);
    const pageTitle = params.get('pageTitle');

    return (
        <div className={classes.root}>
            <Typography variant="h3">Unauthorized</Typography>
            <Typography>You are not authorized to view {pageTitle}</Typography>
        </div>
    );
};

export default Unauthorized;
