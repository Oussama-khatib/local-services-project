import React, { useEffect, useState } from "react";
import DashboardCard from "../../components/DashboardCard";
import { getMyJobs, getMyWallet } from "../../sevice/customerService";

const Dashboard = () => {
  const [totalJobs, setTotalJobs] = useState(0);
  const [activeJobs, setActiveJobs] = useState(0);
  const [walletBalance, setWalletBalance] = useState(0);
  useEffect(() => {
    const fetchStats = async () => {
      try {
        const myJobs = await getMyJobs();
        setTotalJobs(myJobs.length);
        setActiveJobs(myJobs.filter((job) => job.status === "Active").length);

        const wallet = await getMyWallet();
        setWalletBalance(wallet.balance);
      } catch (error) {
        console.error("Error fetching dashboard stats:", error);
      }
    };

    fetchStats();
  }, []);

  return (
    <div className="grid grid-cols-3 gap-6">
      <DashboardCard title="Total Jobs" value={totalJobs} />
      <DashboardCard title="Active Jobs" value={activeJobs} />
      <DashboardCard title="Wallet Balance" value={`$${walletBalance}`} />
    </div>
  );
};

export default Dashboard;
