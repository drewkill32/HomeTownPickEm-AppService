import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Box,
  Paper,
  Stack,
  Typography,
  useMediaQuery,
  useTheme,
} from '@mui/material';
import React, { useState } from 'react';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

interface AdminMemberListProps {
  addButton?: React.ReactNode;
  children?: React.ReactNode;
  title: string;
}

export const AdminList = ({
  addButton,
  children,
  title,
}: AdminMemberListProps) => {
  const theme = useTheme();
  const xs = useMediaQuery(theme.breakpoints.down('sm'));
  const [open, setOpen] = useState(false);
  return (
    <Paper>
      <Accordion
        expanded={open || !xs}
        onChange={(e) => {
          console.log({ e });
          //don't do anything if the user clicked on a button in the header
          if (e.target instanceof Element && e.target.tagName !== 'BUTTON') {
            setOpen(!open);
          }
        }}>
        <AccordionSummary
          expandIcon={xs ? <ExpandMoreIcon /> : null}
          sx={{
            cursor: 'default',
            '&.MuiAccordionSummary-root:hover': {
              cursor: { xs: 'pointer', sm: 'default' },
            },
          }}>
          <Stack sx={{ width: '100%' }}>
            <Box
              sx={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'space-between',
                width: '100%',
              }}>
              <Typography>{title}</Typography>
              {addButton}
            </Box>
          </Stack>
        </AccordionSummary>
        <AccordionDetails>{children}</AccordionDetails>
      </Accordion>
    </Paper>
  );
};
