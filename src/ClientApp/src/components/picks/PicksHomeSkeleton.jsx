import { Container, Paper, Skeleton, Stack } from '@mui/material';

import React from 'react';
import { styled } from '@mui/styles';

const StyledSkeleton = styled(Skeleton)(() => ({
  height: '125px',
  borderRadius: '10px',
  width: ' 35vw',
}));

export const PickLayoutSkeleton = () => {
  const items = [1, 2, 3];
  return (
    <Container
      maxWidth="xs"
      sx={{
        marginTop: '30px',
        paddingBottom: '40px',
      }}>
      <Stack
        sx={{ maxWidth: '520px', marginTop: '20px', marginInline: 'auto' }}
        alignItems="center"
        spacing={2}
        justifyContent="center">
        {items.map((item) => (
          <Paper key={item}>
            <Skeleton
              variant="rectangular"
              sx={{
                marginInline: 'auto',
                borderRadius: '10px',
                marginTop: '10px',
                height: '25px',
                width: ' 200px',
              }}
            />
            <Stack
              spacing={3}
              sx={{ marginBlock: '20px', marginInline: '15px' }}
              direction="row"
              justifyContent="center">
              <StyledSkeleton variant="rectangular" />
              <StyledSkeleton variant="rect" />
            </Stack>
          </Paper>
        ))}
      </Stack>
    </Container>
  );
};
