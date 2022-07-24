import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import {
  createTheme,
  CssBaseline,
  ThemeProvider,
  StyledEngineProvider,
  GlobalStyles,
  Theme,
} from '@mui/material';
import App from './App';

import { setupAxios } from './utils/agent';

setupAxios();

const theme = createTheme({
  palette: {
    background: {
      default: '#f6f6f6',
    },
  },
  components: {
    MuiTextField: {
      styleOverrides: {
        root: {
          paddingBottom: '1.05rem',
        },
      },
    },
    MuiFormHelperText: {
      styleOverrides: {
        root: {
          marginTop: 0,
          height: 0,
        },
      },
    },
  },
});

const root = ReactDOM.createRoot(document.getElementById('root')!);

export const styles = (theme: Theme) => {
  const mode = theme.palette.mode;
  return {
    '*::-webkit-scrollbar': {
      width: '0.6rem',
      height: '0.6rem',
    },
    '*::-webkit-scrollbar-track': {
      background: theme.palette.grey[300],
      margin: 3,
    },
    '*::-webkit-scrollbar-thumb': {
      borderRadius: '10px',
      background:
        mode === 'dark' ? theme.palette.grey[500] : theme.palette.primary.main,
    },
  };
};

root.render(
  <BrowserRouter>
    <StyledEngineProvider injectFirst>
      <ThemeProvider theme={theme}>
        <GlobalStyles styles={styles(theme)} />
        <CssBaseline />
        <App />
      </ThemeProvider>
    </StyledEngineProvider>
  </BrowserRouter>
);
