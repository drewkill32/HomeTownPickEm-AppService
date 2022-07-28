import { User, UserLeague } from '../../../models';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Button,
  Card,
  CardActions,
  CardContent,
  CardHeader,
  CardMedia,
  Divider,
  IconButton,
  Menu,
  MenuItem,
  Tooltip,
  Typography,
} from '@mui/material';
import NewSeasonConfirmDialog from './NewSeasonConfirmDialog';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import { Box } from '@mui/system';
import { useLeague } from '../contexts/LeagueProvider';

interface LeagueCardParams {
  league: UserLeague;
  season: string;
  user: User;
}

interface PastYearsMenuParams {
  pastSeasons: string[];
  slug: string;
}

const PastYearsMenu = ({ pastSeasons, slug }: PastYearsMenuParams) => {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);
  const navigate = useNavigate();

  return (
    <div>
      <IconButton onClick={(e) => setAnchorEl(e.currentTarget)}>
        <MoreVertIcon />
      </IconButton>
      <Menu open={open} anchorEl={anchorEl} onClose={() => setAnchorEl(null)}>
        <MenuItem>Previous Seasons</MenuItem>
        <Divider />
        {pastSeasons.map((year) => (
          <MenuItem
            key={year}
            onClick={() => navigate(`/league/${slug}/${year}`)}>
            {year}
          </MenuItem>
        ))}
      </Menu>
    </div>
  );
};

export function LeagueCard({ league, season, user }: LeagueCardParams) {
  const [newLeague, setNewLeague] = useState<UserLeague>();
  const navigate = useNavigate();
  const [, setLeague] = useLeague();

  const pastYears = (league?.years || []).filter((x) => x !== season);
  const handleNewSeason = (league: UserLeague) => {
    setNewLeague(league);
  };
  return (
    <>
      <Card>
        <CardHeader title={league.name} />
        <CardMedia component="img" height="100" image={league.imageUrl} />
        <CardContent>
          <Typography>{league.name}</Typography>
        </CardContent>
        <CardActions>
          <Box sx={{ flexGrow: 1 }}>
            {league.years.includes(season) ? (
              <Button
                onClick={() => {
                  setLeague({
                    id: league.id,
                    name: league.name,
                    slug: league.slug,
                    imageUrl: league.imageUrl,
                    season: season,
                  });
                  navigate(`/league/${league.slug}/${season}`);
                }}>
                {`${season} Season`}
              </Button>
            ) : user.roles.includes(`commissioner:${league.id}`) ? (
              <Button
                disabled={Boolean(league)}
                onClick={() => handleNewSeason(league)}>
                Start new Season
              </Button>
            ) : (
              <Tooltip
                title={`A commissioner has not created started the ${season} season`}>
                <Button disabled>Go To Season</Button>
              </Tooltip>
            )}
          </Box>
          <PastYearsMenu pastSeasons={pastYears} slug={league.slug} />
        </CardActions>
      </Card>
      <NewSeasonConfirmDialog
        league={newLeague}
        year={season}
        handleClose={(result) => {
          setNewLeague(undefined);
          if (result === 'ok' && newLeague) {
            navigate(`/league/${newLeague.slug}/${season}`);
          }
        }}
      />
    </>
  );
}
