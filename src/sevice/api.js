import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:5102/api",
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.request.use((config) => {
  const session = JSON.parse(localStorage.getItem("session"));

  if (session?.token) {
    config.headers.Authorization = `Bearer ${session.token}`;
  }

  return config;
});

export default api;
