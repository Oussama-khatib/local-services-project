import React from "react";
import StatusBadge from "./StatusBadge";

const ProviderJobCard = ({ job, onAccept, onCancel, onComplete }) => {
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
            onClick={() => onAccept(job.jobId)}
            className="bg-green-500 text-white px-3 py-1 rounded"
          >
            Accept
          </button>
        )}

        {job.status === "Accepted" && (
          <>
            <button
              onClick={() => onCancel(job.jobId)}
              className="bg-red-500 text-white px-3 py-1 rounded"
            >
              Cancel
            </button>

            <button
              onClick={() => onComplete(job)}
              className="bg-blue-500 text-white px-3 py-1 rounded"
            >
              Complete
            </button>
          </>
        )}
      </div>
    </div>
  );
};

export default ProviderJobCard;
