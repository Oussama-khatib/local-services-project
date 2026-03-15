import React from "react";

const DashboardCard = ({ title, value, icon }) => {
  return (
    <div className="card flex items-center justify-between">
      <div>
        <p className="text-sm text-gray-500">{title}</p>
        <h2 className="text-2xl font-bold mt-2">{value}</h2>
      </div>
      <div className="text-indigo-600 text-3xl">{icon}</div>
    </div>
  );
};

export default DashboardCard;
