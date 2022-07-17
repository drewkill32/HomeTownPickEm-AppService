import axios from "axios";
import {getUserCookie} from "./cookies";

const getUrl = () => {
    const url = `${window.location.protocol}//${window.location.hostname}`;
    if (window.location.port) {
        return `${url}:${window.location.port}`;
    }
    return url + "/";
};

export function setupAxios() {
    axios.defaults.baseURL = process.env.REACT_APP_API_URL || getUrl();
    axios.defaults.headers.common["Content-Type"] = "application/json";
    axios.defaults.headers.post["Content-Type"] = "application/json";
    axios.defaults.headers.put["Content-Type"] = "application/json";
    axios.defaults.headers.patch["Content-Type"] = "application/json";
    axios.defaults.headers.delete["Content-Type"] = "application/json";

    axios.interceptors.request //add bearer token to all requests
        .use((config) => {
            const user = getUserCookie();
            if (user) {
                config.headers.Authorization = `Bearer ${user.token}`;
            }
            return config;
        });
}