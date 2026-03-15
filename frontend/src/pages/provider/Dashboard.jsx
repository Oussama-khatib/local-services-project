import React, { useEffect, useState } from "react";
import DashboardCard from "../../components/DashboardCard";
import { getProviderJobs } from "../../sevice/providerService";

const Dashboard = () => {
  const [availableJobs, setAvailableJobs] = useState(0);
  const [assignedJobs, setAssignedJobs] = useState(0);
  const [emergencyJobs, setEmergencyJobs] = useState(0);
  const provider = JSON.parse(localStorage.getItem("provider"));

  useEffect(() => {
    const fetchJobs = async () => {
      try {
        const jobs = await getProviderJobs();

        const available = jobs.filter((j) => j.status === "Open").length;
        const assigned = jobs.filter((j) => j.status === "Accepted").length;

        const emergency = jobs.filter(
          (j) =>
            (j.status === "Open" || j.status === "Accepted") && j.isEmergency,
        ).length;

        setAvailableJobs(available);
        setAssignedJobs(assigned);
        setEmergencyJobs(emergency);
      } catch (error) {
        console.error("Error loading jobs", error);
      }
    };

    fetchJobs();
  }, []);

  return (
    <div className="grid grid-cols-3 gap-6">
      <DashboardCard title="Available Jobs" value={availableJobs} />
      <DashboardCard title="Assigned Jobs" value={assignedJobs} />
      <DashboardCard title="Emergency Jobs" value={emergencyJobs} />
    </div>
  );
};

export default Dashboard;
