const axios = require("axios");

function defaultFactory(token) {
  const headers = {};

  if (token) {
    headers.Authorization = `Bearer ${token}`;
  }

  return axios.create({
    baseURL: process.env.API_BASE_URL || "http://localhost:5000/api",
    timeout: 10000,
    headers
  });
}

let activeFactory = defaultFactory;

function createApiClient(token) {
  return activeFactory(token);
}

createApiClient.__setFactoryForTests = (factory) => {
  activeFactory = factory;
};

createApiClient.__resetFactoryForTests = () => {
  activeFactory = defaultFactory;
};

module.exports = createApiClient;
