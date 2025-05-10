import axios from "axios";
const axiosInstance = axios.create({
  baseURL: "http://localhost:5173/api",
  headers: {
    "Content-Type": "application/json",
  },
  timeout: 10000,
});

export default axiosInstance;
