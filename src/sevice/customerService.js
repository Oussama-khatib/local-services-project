import api from "./api";

export const getMyJobs = async () => {
  const res = await api.get("/Jobs/my-jobs");
  return res.data;
};

export const getMyWallet = async () => {
  const res = await api.get("/Wallet/my-wallet");
  return res.data;
};

export const getCategoryById = async (id) => {
  const res = await api.get(`/ServiceCategory/${id}`);
  return res.data;
};

export const getProviderByJob = async (jobId) => {
  const res = await api.get(`/Jobs/provider/${jobId}`);
  return res.data;
};

export const cancelJob = async (jobId) => {
  const res = await api.post(`/Jobs/${jobId}/cancel`);
};

export const getReviewByJobId = async (jobId) => {
  const res = await api.get(`/Reviews/job/${jobId}`);
  return res.data;
};

export const addReview = async (newReview) => {
  const res = await api.post(
    `/Reviews?jobId=${newReview.jobId}&providerId=${newReview.providerId}&rating=${newReview.rating}&comment=${encodeURIComponent(newReview.comment)}`,
  );
};

export const getCategories = async () => {
  const res = await api.get("/ServiceCategory");
  return res.data;
};

export const getProvidersByCategory = async (categoryId) => {
  const res = await api.get(`/ServiceProviders/category/${categoryId}`);
  return res.data;
};

export const createJob = async (jobData) => {
  const res = await api.post(
    "/Jobs",
    {},
    {
      params: {
        customerId: jobData.customerId,
        categoryId: jobData.serviceCategoryId,
        description: jobData.description,
        location: jobData.location,
        isEmergency: jobData.isEmergency,
        providerId: jobData.providerId,
      },
    },
  );
};

export const getUserById = async (id) => {
  const res = await api.get(`/Users/${id}`);
  return res.data;
};

export const getProviders = async () => {
  const res = await api.get("/ServiceProviders");
  return res.data;
};

export const getCategoriesByProvider = async (providerId) => {
  const res = await api.get(`/ProviderServices/provider/${providerId}`);
  return res.data;
};

export const getProviderSummary = async (providerId) => {
  const res = await api.get(`/ReviewsSummary/summary`, {
    params: { providerId: providerId },
  });
  return res.data;
};

export const getWallet = async () => {
  const res = await api.get("/Wallet/my-wallet");
  return res.data;
};

export const deposit = async (amount) => {
  const res = await api.post("/Wallet/deposit", null, {
    params: { amount },
  });
};

export const getTransactions = async () => {
  const res = await api.get("/Transaction/my-transactions");
  return res.data;
};

export const getUserNameByWalletId = async (id) => {
  const res = await api.get(`/Wallet/userName/${id}`);
  return res.data;
};

export const askAssistant = async (question) => {
  const res = await api.post("/Chat", {
    message: question,
  });

  return res.data;
};
