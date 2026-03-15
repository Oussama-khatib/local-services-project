import React from "react";

const ReviewCard = ({ review }) => {
  return (
    <div className="card space-y-2">
      <div className="flex justify-between">
        <span className="text-yellow-500">⭐ {review.rating}</span>
      </div>

      <p className="text-gray-600 text-sm">{review.comment}</p>
    </div>
  );
};

export default ReviewCard;
