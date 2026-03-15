import React, { useState, useEffect } from "react";
import StatusBadge from "../../components/StatusBadge";
import ReviewCard from "../../components/ReviewCard";
import { getReviewByJobId, addReview } from "../../sevice/customerService";

const JobDetails = ({ job, back }) => {
  const reviews = [{ user: "John", rating: 5, comment: "Great client!" }];
  const [review, setReview] = useState(null);
  useEffect(() => {
    const getReview = async () => {
      try {
        const data = await getReviewByJobId(job.jobId);
        console.log("Review:", data.comment);

        if (data && data.comment != null) {
          setReview(data);
        } else {
          setReview(null);
        }
      } catch (error) {
        console.error("Error: ", error);
      }
    };

    getReview();
  }, []);

  const [rating, setRating] = useState(0);
  const [comment, setComment] = useState("");
  const [showForm, setShowForm] = useState(false);
  const handleSubmitReview = async (e) => {
    e.preventDefault();
    try {
      const newReview = {
        jobId: job.jobId,
        providerId: job.providerId,
        rating: rating,
        comment: comment,
      };
      await addReview(newReview);
      setReview(newReview);
      setShowForm(false);
    } catch (error) {
      console.error("Error submitting review:", error);
    }
  };

  return (
    <div className="space-y-6">
      <button onClick={back} className="btn-secondary">
        Back
      </button>

      <div className="card space-y-3">
        <h2 className="text-xl font-bold">{job.title}</h2>
        <StatusBadge status={job.status} />
        <p>Description: {job.description}</p>
        <p>Assigned Provider: {job.providerName}</p>
      </div>

      <div>
        <h3 className="font-semibold mb-4">Review</h3>
        {review ? (
          <ReviewCard review={review} />
        ) : job.status === "Completed" ? (
          showForm ? (
            <form onSubmit={handleSubmitReview} className="space-y-4">
              <div>
                <label className="block text-sm font-medium">Rating</label>
                <input
                  type="number"
                  min="1"
                  max="5"
                  value={rating}
                  onChange={(e) => setRating(Number(e.target.value))}
                  className="border rounded px-2 py-1 w-20"
                  required
                />
              </div>
              <div>
                <label className="block text-sm font-medium">Comment</label>
                <textarea
                  value={comment}
                  onChange={(e) => setComment(e.target.value)}
                  className="border rounded px-2 py-1 w-full"
                  required
                />
              </div>
              <button
                type="submit"
                className="bg-green-500 text-white px-4 py-2 rounded"
              >
                Submit Review
              </button>
              <button
                type="button"
                onClick={() => setShowForm(false)}
                className="ml-2 bg-gray-400 text-white px-4 py-2 rounded"
              >
                Cancel
              </button>
            </form>
          ) : (
            <button
              onClick={() => setShowForm(true)}
              className="bg-blue-500 text-white px-4 py-2 rounded"
            >
              Add Review
            </button>
          )
        ) : (
          <p className="text-gray-500">No review available</p>
        )}
      </div>
    </div>
  );
};

export default JobDetails;
