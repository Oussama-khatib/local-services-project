import api from "./api";

// get jobs for provider
export const getProviderJobs = async () => {
  const res = await api.get(`/Jobs/provider`);
  return res.data;
};

// accept job
export const acceptJob = async (jobId) => {
  await api.post(`/Jobs/${jobId}/accept`);
};

// cancel job
export const cancelJob = async (jobId) => {
  await api.post(`/Jobs/${jobId}/cancel`);
};

// complete job
export const completeJob = async (jobId, amount) => {
  await api.post(`/Jobs/${jobId}/complete`, {}, { params: { price: amount } });
};

export const getCategoryById = async (id) => {
  const res = await api.get(`/ServiceCategory/${id}`);
  return res.data;
};

export const getCategories = async () => {
  const res = await api.get("/ServiceCategory");
  return res.data;
};

export const addProviderService = async (
  providerId,
  categoryId,
  priceMin,
  priceMax,
  description,
) => {
  const res = await api.post(`/ProviderServices/${providerId}`, {
    categoryId: categoryId,
    priceMin: priceMin,
    priceMax: priceMax,
    description: description,
  });
};

export const getReviews = async () => {
  const res = await api.get("/Reviews/my-provider-reviews");
  return res.data;
};

export const getUserByCustomerId = async (customerId) => {
  const res = await api.get(`/Users/Customer/${customerId}`);
  return res.data;
};

export const getMyWallet = async () => {
  const res = await api.get("/Wallet/my-wallet");
  return res.data;
};

export const askAssistant = async (question) => {
  const res = await api.post("/Chat", {
    message: question,
  });

  return res.data;
};

export const getProviderById = async (providerId) => {
  const res = await api.get(`/ServiceProviders/${providerId}`);
  return res.data;
};

export const updateProvider = async (providerId, profile) => {
  const res = await api.put(`/ServiceProviders/${providerId}`, {
    biography: profile.biography,
    yearOfExperience: profile.yearOfExperience,
    location: profile.location,
  });
};
