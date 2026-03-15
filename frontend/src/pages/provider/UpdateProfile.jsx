import React, { useState, useEffect } from "react";
import { getProviderById, updateProvider } from "../../sevice/providerService";

const UpdateProfile = () => {
  const provider = JSON.parse(localStorage.getItem("provider"));

  const [profile, setProfile] = useState({
    biography: "",
    location: "",
    yearOfExperience: "",
  });

  useEffect(() => {
    const fetchProvider = async () => {
      try {
        const data = await getProviderById(provider.providerId);

        setProfile({
          biography: data.biography || "",
          location: data.location || "",
          yearOfExperience: data.yearOfExperience || "",
        });
      } catch (err) {
        console.error("Error loading provider", err);
      }
    };

    fetchProvider();
  }, []);

  const update = async () => {
    try {
      await updateProvider(provider.providerId, profile);
      alert("Profile updated successfully");
    } catch (err) {
      console.error("Update failed", err);
      alert("Error updating profile");
    }
  };

  return (
    <div className="card max-w-xl space-y-4">
      <textarea
        placeholder="Biography"
        value={profile.biography}
        className="border p-2 rounded-lg w-full"
        onChange={(e) => setProfile({ ...profile, biography: e.target.value })}
      />

      <input
        placeholder="Location"
        value={profile.location}
        className="border p-2 rounded-lg w-full"
        onChange={(e) => setProfile({ ...profile, location: e.target.value })}
      />

      <input
        type="number"
        placeholder="Years of Experience"
        value={profile.yearOfExperience}
        className="border p-2 rounded-lg w-full"
        onChange={(e) =>
          setProfile({ ...profile, yearOfExperience: e.target.value })
        }
      />

      <button onClick={update} className="btn-primary w-full">
        Update Profile
      </button>
    </div>
  );
};

export default UpdateProfile;
