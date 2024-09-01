import LeaderboardTable from '../components/LeaderboardTable';
import { Paper, Tab } from '@mui/material';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { useState } from 'react';
import { Box } from '@mui/system';
import WeeklyLeaderboard from './WeeklyLeaderboard';
import { PickProvider } from '../../SeasonPicks/contexts/PickContext';

const Leaderboard = () => {
  const [value, setValue] = useState('1');

  const handleChange = (_: React.SyntheticEvent, newValue: string) => {
    setValue(newValue);
  };
  return (
    <Paper>
      <TabContext value={value}>
        <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
          <TabList onChange={handleChange}>
            <Tab label="Season" value="1" />
            <Tab label="Weekly" value="2" />
          </TabList>
        </Box>
        <TabPanel value="1">
          <LeaderboardTable />
        </TabPanel>
        <TabPanel value="2">
          <PickProvider>
            <WeeklyLeaderboard />
          </PickProvider>
        </TabPanel>
      </TabContext>
    </Paper>
  );
};

export default Leaderboard;
