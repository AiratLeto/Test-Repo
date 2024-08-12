import axios from 'axios';

/** url апи приложения **/
export const appApiUrl = import.meta.env.VITE_APP_API_URL;

export const ApiConnection = axios.create({
  baseURL: `${appApiUrl ?? ''}/api`,
  headers: {
    'Cache-Control': 'no-cache',
    'Access-Control-Allow-Origin': '*',
    'Content-Type': 'application/json',
  },
});