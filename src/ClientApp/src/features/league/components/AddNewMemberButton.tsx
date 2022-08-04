import * as yup from 'yup';
import { useState } from 'react';
import { AddMemberData, AvailableTeamsResult } from '../types';
import { useLeague } from '../contexts/LeagueProvider';
import { useMutation, useQuery, useQueryClient } from 'react-query';
import { LeagueKeys } from '../utils/queryKeys';
import { leagueAgent } from '../utils/leagueAgent';
import { useFormik } from 'formik';
import {
  Autocomplete,
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  IconButton,
  SxProps,
  TextField,
} from '@mui/material';
import AddCircleIcon from '@mui/icons-material/AddCircle';
import { green } from '@mui/material/colors';
import { LoadingButton } from '@mui/lab';

const validationSchema = yup.object({
  email: yup
    .string()
    .email('Enter a valid email')
    .required('Email is required'),
  firstName: yup.string().required('First Name is required'),
  lastName: yup.string().required('Last Name is required'),
  teamId: yup.number(),
});

export const AddNewMemberButton = ({ sx }: { sx?: SxProps }) => {
  const [openDialog, setOpenDialog] = useState(false);
  const [team, setTeam] = useState<AvailableTeamsResult | null>(null);
  const [league] = useLeague();
  const queryClient = useQueryClient();

  const { data } = useQuery(
    LeagueKeys.AvailableTeams,
    () => leagueAgent.availableTeams(league),
    {
      enabled: openDialog,
    }
  );

  const teams = data ? data : [];
  const { mutateAsync: addMember } = useMutation(
    (data: AddMemberData) => leagueAgent.addMember(data),
    {
      onSuccess: async () => {
        await Promise.all([
          queryClient.invalidateQueries(LeagueKeys.AvailableTeams),
          queryClient.invalidateQueries(LeagueKeys.LeagueAdmin),
        ]);
      },
    }
  );

  const formik = useFormik({
    initialValues: {
      email: '',
      firstName: '',
      lastName: '',
      teamId: 0,
    },
    validationSchema,
    onSubmit: async (values) => {
      await addMember({
        ...values,
        leagueId: league!.id,
        season: league!.season,
        teamId: values.teamId || null,
      });
      handleClose();
    },
  });

  const handleClose = () => {
    setOpenDialog(false);
    formik.resetForm();
  };

  return (
    <>
      <IconButton
        sx={sx}
        onClick={(e) => {
          e.stopPropagation();
          setOpenDialog(true);
        }}>
        <AddCircleIcon sx={{ color: green[500] }} />
      </IconButton>
      <Dialog open={openDialog} onClose={handleClose}>
        <DialogTitle>Add New Member</DialogTitle>
        <DialogContent>
          <DialogContentText>
            <TextField
              autoFocus
              margin="dense"
              id="email"
              label="Email Address"
              type="email"
              fullWidth
              variant="standard"
              value={formik.values.email}
              onChange={formik.handleChange}
              error={formik.touched.email && Boolean(formik.errors.email)}
              helperText={formik.touched.email && formik.errors.email}
            />
            <TextField
              margin="dense"
              id="firstName"
              label="First Name"
              fullWidth
              variant="standard"
              value={formik.values.firstName}
              onChange={formik.handleChange}
              error={
                formik.touched.firstName && Boolean(formik.errors.firstName)
              }
              helperText={formik.touched.firstName && formik.errors.firstName}
            />
            <TextField
              margin="dense"
              id="lastName"
              label="Last Name"
              fullWidth
              variant="standard"
              value={formik.values.lastName}
              onChange={formik.handleChange}
              error={formik.touched.lastName && Boolean(formik.errors.lastName)}
              helperText={formik.touched.lastName && formik.errors.lastName}
            />

            <Autocomplete
              options={teams}
              value={team}
              onChange={(e, team) => {
                setTeam(team);
                formik.touched.teamId = true;
                formik.setFieldValue('teamId', team?.id || 0);
              }}
              renderOption={(params, option) => (
                <Box
                  component="li"
                  sx={{ '& > img': { mr: 2, flexShrink: 0 } }}
                  // @ts-ignore
                  {...params}>
                  <img
                    loading="lazy"
                    width="20"
                    src={option.logo || '/img/helmet.png'}
                    alt={option.name}
                    onError={(e) => {
                      // @ts-ignore
                      e.target.src = '/img/helmet.png';
                    }}
                  />
                  {option.name}
                </Box>
              )}
              getOptionLabel={(option) => option.name}
              renderInput={(params) => (
                <TextField
                  variant="standard"
                  {...params}
                  label="Pick a team"
                  error={formik.touched.teamId && Boolean(formik.errors.teamId)}
                  helperText={formik.touched.teamId && formik.errors.teamId}
                  inputProps={{
                    ...params.inputProps,
                  }}
                />
              )}
            />
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button variant="outlined" onClick={handleClose}>
            Cancel
          </Button>
          <LoadingButton
            color="primary"
            loading={false}
            variant="contained"
            onClick={async () => {
              await formik.submitForm();
            }}
            autoFocus>
            Save
          </LoadingButton>
        </DialogActions>
      </Dialog>
      {/*<Dialog open={openDialog} onClose={handleClose}>*/}
      {/*  <DialogTitle>Add new Member</DialogTitle>*/}
      {/*  <DialogContent>*/}
      {/*    <DialogContentText>*/}
      {/*      <TextField*/}
      {/*        autoFocus*/}
      {/*        margin="dense"*/}
      {/*        id="email"*/}
      {/*        label="Email Address"*/}
      {/*        type="email"*/}
      {/*        fullWidth*/}
      {/*        variant="standard"*/}
      {/*        value={formik.values.email}*/}
      {/*        onChange={formik.handleChange}*/}
      {/*        error={formik.touched.email && Boolean(formik.errors.email)}*/}
      {/*        helperText={formik.touched.email && formik.errors.email}*/}
      {/*      />*/}
      {/*      <TextField*/}
      {/*        margin="dense"*/}
      {/*        id="firstName"*/}
      {/*        label="First Name"*/}
      {/*        fullWidth*/}
      {/*        variant="standard"*/}
      {/*        value={formik.values.firstName}*/}
      {/*        onChange={formik.handleChange}*/}
      {/*        error={*/}
      {/*          formik.touched.firstName && Boolean(formik.errors.firstName)*/}
      {/*        }*/}
      {/*        helperText={formik.touched.firstName && formik.errors.firstName}*/}
      {/*      />*/}
      {/*      <TextField*/}
      {/*        margin="dense"*/}
      {/*        id="lastName"*/}
      {/*        label="Last Name"*/}
      {/*        fullWidth*/}
      {/*        variant="standard"*/}
      {/*        value={formik.values.lastName}*/}
      {/*        onChange={formik.handleChange}*/}
      {/*        error={formik.touched.lastName && Boolean(formik.errors.lastName)}*/}
      {/*        helperText={formik.touched.lastName && formik.errors.lastName}*/}
      {/*      />*/}

      {/*      <Autocomplete*/}
      {/*        options={teams}*/}
      {/*        value={team}*/}
      {/*        onChange={(e, team) => {*/}
      {/*          setTeam(team);*/}
      {/*          formik.touched.teamId = true;*/}
      {/*          formik.setFieldValue('teamId', team?.id || 0);*/}
      {/*        }}*/}
      {/*        renderOption={(params, option) => (*/}
      {/*          <Box*/}
      {/*            component="li"*/}
      {/*            sx={{ '& > img': { mr: 2, flexShrink: 0 } }}*/}
      {/*            // @ts-ignore*/}
      {/*            {...params}>*/}
      {/*            <img*/}
      {/*              loading="lazy"*/}
      {/*              width="20"*/}
      {/*              src={option.logo || '/img/helmet.png'}*/}
      {/*              alt={option.name}*/}
      {/*              onError={(e) => {*/}
      {/*                // @ts-ignore*/}
      {/*                e.target.src = '/img/helmet.png';*/}
      {/*              }}*/}
      {/*            />*/}
      {/*            {option.name}*/}
      {/*          </Box>*/}
      {/*        )}*/}
      {/*        getOptionLabel={(option) => option.name}*/}
      {/*        renderInput={(params) => (*/}
      {/*          <TextField*/}
      {/*            variant="standard"*/}
      {/*            {...params}*/}
      {/*            label="Pick a team"*/}
      {/*            error={formik.touched.teamId && Boolean(formik.errors.teamId)}*/}
      {/*            helperText={formik.touched.teamId && formik.errors.teamId}*/}
      {/*            inputProps={{*/}
      {/*              ...params.inputProps,*/}
      {/*            }}*/}
      {/*          />*/}
      {/*        )}*/}
      {/*      />*/}
      {/*    </DialogContentText>*/}
      {/*  </DialogContent>*/}

      {/*</Dialog>*/}
    </>
  );
};
