import { Container } from '@mui/material';
import NavMenu from './NavMenu';

const Layout = ({ children }) => {
  return (
    <div>
      <NavMenu />
      <Container>{children}</Container>
    </div>
  );
};

export default Layout;
