import api from "./api";

export const getUsers = async () => {
  const res = await api.get("/Users");
  return res.data;
};

export const activateUser = async (id) => {
  const res = await api.put(`/Users/activate/${id}`);
  return res.data;
};

export const deactivateUser = async (id) => {
  const res = await api.put(`/Users/deactivate/${id}`);
  return res.data;
};

export const getTransactions = async () => {
  const res = await api.get("/Transaction/my-transactions");
  return res.data;
};

export const getUserNameByWalletId = async (id) => {
  const res = await api.get(`/Wallet/userName/${id}`);
  return res.data;
};

export const getPlatformEarning = async () => {
  const res = await api.get("/Transaction/platform-earnings");
  return res.data;
};
