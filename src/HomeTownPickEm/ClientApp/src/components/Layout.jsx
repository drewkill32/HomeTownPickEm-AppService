import NavMenu from './NavMenu';

const Layout = ({ children }) => {
  return (
    <div>
      <NavMenu />
      <div>{children}</div>
    </div>
  );
};

export default Layout;
