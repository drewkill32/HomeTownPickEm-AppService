import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import {
  createTheme,
  CssBaseline,
  ThemeProvider,
  StyledEngineProvider,
} from '@mui/material';
import App from './App';

import { setupAxios } from './utils/agent';

setupAxios();

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');

const theme = createTheme();

const root = ReactDOM.createRoot(document.getElementById('root'));

root.render(
  <BrowserRouter basename={baseUrl}>
    <StyledEngineProvider injectFirst>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <App />
      </ThemeProvider>
    </StyledEngineProvider>
  </BrowserRouter>
);
