import React from "react";

const StatusBadge = ({ status }) => {
  const getClass = () => {
    switch (status) {
      case "Pending":
        return "badge-warning";
      case "Accepted":
        return "badge-info";
      case "Completed":
        return "badge-success";
      default:
        return "badge-danger";
    }
  };

  return <span className={getClass()}>{status}</span>;
};

export default StatusBadge;
