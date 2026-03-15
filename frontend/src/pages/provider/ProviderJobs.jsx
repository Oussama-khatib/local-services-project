import React, { useEffect, useState } from "react";
import Modal from "../../components/Modal";
import ProviderJobCard from "../../components/ProviderJobCard";
import {
  getProviderJobs,
  acceptJob,
  cancelJob,
  completeJob,
  getCategoryById,
} from "../../sevice/providerService";

const ProviderJobs = () => {
  const [jobs, setJobs] = useState([]);
  const [selectedJob, setSelectedJob] = useState(null);
  const [amount, setAmount] = useState("");

  const loadJobs = async () => {
    try {
      const data = await getProviderJobs();
      const jobsWithCategory = await Promise.all(
        data.map(async (job) => {
          const category = await getCategoryById(job.categoryId);

          return {
            ...job,
            categoryTitle: category.name,
          };
        }),
      );
      setJobs(jobsWithCategory);
    } catch (error) {
      console.error("Error loading jobs", error);
    }
  };

  useEffect(() => {
    loadJobs();
  }, []);

  const handleAccept = async (jobId) => {
    await acceptJob(jobId);
    loadJobs();
  };

  const handleCancel = async (jobId) => {
    await cancelJob(jobId);
    loadJobs();
  };

  const openCompleteModal = (job) => {
    setSelectedJob(job);
  };

  const handleComplete = async () => {
    await completeJob(selectedJob.jobId, amount);
    setSelectedJob(null);
    setAmount("");
    loadJobs();
  };

  return (
    <>
      <div className="grid grid-cols-2 gap-6">
        {jobs.map((job) => (
          <ProviderJobCard
            key={job.jobId}
            job={job}
            onAccept={handleAccept}
            onCancel={handleCancel}
            onComplete={openCompleteModal}
          />
        ))}
      </div>

      <Modal isOpen={!!selectedJob} onClose={() => setSelectedJob(null)}>
        <h3 className="font-semibold mb-3">Complete Job</h3>

        <input
          type="number"
          placeholder="Enter earned amount ($)"
          className="border p-2 rounded w-full"
          value={amount}
          onChange={(e) => setAmount(e.target.value)}
        />

        <button
          onClick={handleComplete}
          className="bg-blue-600 text-white w-full mt-4 py-2 rounded"
        >
          Confirm Completion
        </button>
      </Modal>
    </>
  );
};

export default ProviderJobs;
