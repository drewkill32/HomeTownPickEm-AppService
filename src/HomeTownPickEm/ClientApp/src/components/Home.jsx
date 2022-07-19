import { Outlet } from 'react-router-dom';
import Layout from './Layout';

const Home = () => {
  return (
    <Layout>
      <Outlet />
    </Layout>
  );
};

export default Home;
