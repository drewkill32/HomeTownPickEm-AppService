import React, {Fragment, useState} from 'react';
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Divider,
    IconButton,
    Typography,
} from '@material-ui/core';

import InfoOutlinedIcon from '@material-ui/icons/InfoOutlined';

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
                  way to play it safe by making sure you get at least one point
                  instead of losing two.
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

export default Head2HeadFooter;
