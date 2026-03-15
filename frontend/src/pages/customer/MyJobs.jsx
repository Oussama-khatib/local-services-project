import React, { useEffect, useState } from "react";
import JobCard from "../../components/JobCard";
import JobDetails from "./JobDetails";
import {
  getMyJobs,
  getCategoryById,
  getProviderByJob,
  cancelJob,
} from "../../sevice/customerService";

const MyJobs = () => {
  const [selected, setSelected] = useState(null);

  const [jobs, setJobs] = useState([]);

  const handleCancelJob = async (jobId) => {
    try {
      await cancelJob(jobId);

      // update UI
      setJobs((prevJobs) =>
        prevJobs.map((job) =>
          job.jobId === jobId ? { ...job, status: "Cancelled" } : job,
        ),
      );
    } catch (error) {
      console.error("Error cancelling job:", error);
    }
  };

  useEffect(() => {
    const fetchJobs = async () => {
      try {
        const data = await getMyJobs();
        const jobsWithCategory = await Promise.all(
          data.map(async (job) => {
            const category = await getCategoryById(job.categoryId);
            const provider = await getProviderByJob(job.jobId);
            return {
              ...job,
              categoryTitle: category.name,
              providerName: provider.name,
              providerId: provider.providerId,
            };
          }),
        );

        setJobs(jobsWithCategory);
      } catch (error) {
        console.error("Error fetching jobs:", error);
      }
    };

    fetchJobs();
  }, []);

  if (selected)
    return <JobDetails job={selected} back={() => setSelected(null)} />;

  return (
    <div className="grid grid-cols-2 gap-6">
      {jobs.map((job) => (
        <div key={job.jobId} onClick={() => setSelected(job)}>
          <JobCard
            job={job}
            onCancel={() => handleCancelJob(job.jobId)}
            onClick={() => setSelected(job)}
          />
        </div>
      ))}
    </div>
  );
};

export default MyJobs;
