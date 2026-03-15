import React, { useEffect, useState } from "react";
import { getPlatformEarning } from "../../sevice/adminService";

const Earnings = () => {
  const [earnings, setEarnings] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchEarnings = async () => {
      try {
        const res = await getPlatformEarning();
        setEarnings(res.platformEarnings);
      } catch (err) {
        console.error("Error fetching earnings:", err);
      } finally {
        setLoading(false);
      }
    };
    fetchEarnings();
  }, []);

  if (loading) return <p>Loading earnings...</p>;

  return (
    <div className="card p-6">
      <h2 className="text-xl font-bold mb-4">Platform Earnings</h2>
      <p className="text-3xl font-semibold">${earnings}</p>
    </div>
  );
};
export default Earnings;
