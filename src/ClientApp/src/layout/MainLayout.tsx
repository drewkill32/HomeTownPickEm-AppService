import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import IconButton from '@mui/material/IconButton';
import Typography from '@mui/material/Typography';
import Menu from '@mui/material/Menu';
import MenuIcon from '@mui/icons-material/Menu';
import Container from '@mui/material/Container';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import MenuItem from '@mui/material/MenuItem';
import SportsFootballIcon from '@mui/icons-material/SportsFootball';
import { useAuth } from '../features/authentication';
import { useNavigate } from 'react-router-dom';
import { z } from 'zod';
import { useState } from 'react';
import { LayoutContextProvider } from './LayoutContext';

const PageRoot = z.object({
  name: z.string(),
});

const ClickPage = PageRoot.extend({
  onClick: z.function().args().returns(z.void()),
  path: z.undefined(),
});

const PathPage = PageRoot.extend({
  path: z.string(),
  onClick: z.undefined(),
});

type Page = z.infer<typeof PathPage> | z.infer<typeof ClickPage>;

export interface MainLayoutProps {
  children?: JSX.Element;
  pages?: Page[];
  header?: string;
}

const MainLayout = ({ children, pages, header }: MainLayoutProps) => {
  const { user, signOut } = useAuth();
  const [paddingBottom, setPaddingBottom] = useState<string | number>(0);
  const navigate = useNavigate();

  const settings = [
    { name: 'Profile', path: '/profile' },
    {
      name: 'Logout',
      onClick: async () => {
        await signOut();
        navigate('/login');
      },
    },
  ];
  const [anchorElNav, setAnchorElNav] = React.useState<null | HTMLElement>(
    null,
  );
  const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(
    null,
  );

  const handleOpenNavMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElNav(event.currentTarget);
  };
  const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseNavMenu = () => {
    setAnchorElNav(null);
  };

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  const handlePageClick = (page: Page) => {
    if (page.onClick) {
      page.onClick();
    }
    if (page.path) {
      navigate(page.path);
    }
    handleCloseNavMenu();
  };

  return (
    <>
      <AppBar position="fixed" sx={{ overflowY: 'hidden', height: '64px' }}>
        <Container maxWidth="xl">
          <Toolbar disableGutters>
            <SportsFootballIcon
              sx={{ display: { xs: 'none', md: 'flex' }, mr: 1 }}
            />
            <Typography
              variant="h6"
              noWrap
              onClick={() => navigate('/')}
              sx={{
                mr: 2,
                display: { xs: 'none', md: 'flex' },
                fontFamily: 'monospace',
                fontWeight: 700,
                letterSpacing: '.3rem',
                color: 'inherit',
                textDecoration: 'none',
                cursor: 'pointer',
              }}>
              {`St.Pete Pick'em ${header || ''}`}
            </Typography>
            {pages && pages.length > 0 && (
              <Box sx={{ flexGrow: 1, display: { xs: 'flex', md: 'none' } }}>
                <IconButton
                  size="large"
                  aria-label="account of current user"
                  aria-controls="menu-appbar"
                  aria-haspopup="true"
                  onClick={handleOpenNavMenu}
                  color="inherit">
                  <MenuIcon />
                </IconButton>
                <Menu
                  id="menu-appbar"
                  anchorEl={anchorElNav}
                  anchorOrigin={{
                    vertical: 'bottom',
                    horizontal: 'left',
                  }}
                  keepMounted
                  transformOrigin={{
                    vertical: 'top',
                    horizontal: 'left',
                  }}
                  open={Boolean(anchorElNav)}
                  onClose={handleCloseNavMenu}
                  sx={{
                    display: { xs: 'block', md: 'none' },
                  }}>
                  {pages.map((page) => (
                    <MenuItem
                      key={page.name}
                      onClick={() => handlePageClick(page)}>
                      <Typography textAlign="center">{page.name}</Typography>
                    </MenuItem>
                  ))}
                </Menu>
              </Box>
            )}
            <SportsFootballIcon
              sx={{ display: { xs: 'flex', md: 'none' }, mr: 1 }}
            />
            <Typography
              variant="h5"
              noWrap
              onClick={() => navigate('/')}
              sx={{
                mr: 2,
                display: { xs: 'flex', md: 'none' },
                flexGrow: 1,
                fontFamily: 'monospace',
                fontWeight: 700,
                letterSpacing: '.1rem',
                color: 'inherit',
                fontSize: '1rem',
                cursor: 'pointer',
              }}>
              {`St.Pete Pick'em ${header || ''}`}
            </Typography>
            <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' } }}>
              {pages &&
                pages.map((page) => (
                  <Button
                    onClick={() => handlePageClick(page)}
                    key={page.name}
                    sx={{ my: 2, color: 'white', display: 'block' }}>
                    {page.name}
                  </Button>
                ))}
            </Box>

            <Box sx={{ flexGrow: 0 }}>
              <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                <Avatar
                  sizes="20px"
                  sx={{
                    bgcolor: user?.team?.color,
                    '& img': {
                      width: '25px',
                      height: '25px',
                    },
                  }}
                  alt={user?.lastName}
                  src={user?.team?.logo}
                />
              </IconButton>

              <Menu
                sx={{ mt: '45px' }}
                id="menu-appbar"
                anchorEl={anchorElUser}
                anchorOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                keepMounted
                transformOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                open={Boolean(anchorElUser)}
                onClose={handleCloseUserMenu}>
                {settings.map((setting) => (
                  <MenuItem
                    key={setting.name}
                    onClick={() => {
                      if (setting.path) {
                        navigate(setting.path);
                      }
                      if (typeof setting.onClick === 'function') {
                        setting.onClick();
                      }
                      handleCloseUserMenu();
                    }}>
                    <Typography textAlign="center">{setting.name}</Typography>
                  </MenuItem>
                ))}
              </Menu>
            </Box>
          </Toolbar>
        </Container>
      </AppBar>
      <LayoutContextProvider
        paddingBottom={paddingBottom}
        setPaddingBottom={setPaddingBottom}>
        <Box
          sx={{
            overflowY: 'auto',
            height: `calc(100vh - 64px - ${paddingBottom})`,
            mt: '64px',
          }}>
          <Container
            maxWidth="md"
            sx={{
              pt: 3,
              mb: '15px',
            }}>
            {children}
          </Container>
        </Box>
      </LayoutContextProvider>
    </>
  );
};
export default MainLayout;
