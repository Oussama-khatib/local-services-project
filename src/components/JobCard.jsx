import React from "react";
import StatusBadge from "./StatusBadge";

const JobCard = ({ job, onCancel }) => {
  return (
    <div className="card space-y-3">
      <div className="flex justify-between items-center">
        <h1 className="font-semibold text-lg">Category: {job.categoryTitle}</h1>
        <StatusBadge status={job.status} />
      </div>

      <p className="text-sm text-gray-500">Description: {job.description}</p>

      <div className="flex justify-between items-center pt-2">
        <span className="text-sm text-gray-600">Location: {job.location}</span>

        {job.status === "Open" && (
          <button
            onClick={() => onCancel(job)}
            className="bg-red-500 text-white px-3 py-1 rounded"
          >
            Cancel
          </button>
        )}
      </div>
    </div>
  );
};

export default JobCard;
