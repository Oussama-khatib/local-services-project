import React, { useEffect, useState } from "react";
import ReviewCard from "../../components/ReviewCard";
import { getReviews, getUserByCustomerId } from "../../sevice/providerService";
const Reviews = () => {
  const [reviews, setReviews] = useState([]);

  useEffect(() => {
    const fetchReviews = async () => {
      try {
        const reviewsData = await getReviews();

        const reviewsWithUsers = await Promise.all(
          reviewsData.map(async (r) => {
            const userRes = await getUserByCustomerId(r.customerId);

            return {
              user: userRes.fullName,
              rating: r.rating,
              comment: r.comment,
            };
          }),
        );

        setReviews(reviewsWithUsers);
      } catch (error) {
        console.error("Error fetching reviews:", error);
      }
    };

    fetchReviews();
  }, []);

  return (
    <div className="space-y-6">
      {reviews.map((r, i) => (
        <div key={i} className="bg-white shadow rounded-lg p-4">
          {/* Customer name */}
          <p className="font-semibold text-gray-800 mb-2">{r.user}</p>

          {/* Review card */}
          <ReviewCard review={r} />
        </div>
      ))}
    </div>
  );
};

export default Reviews;
