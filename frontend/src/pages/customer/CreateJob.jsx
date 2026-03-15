import React, { useEffect, useState } from "react";
import {
  getCategories,
  getProvidersByCategory,
  createJob,
  getUserById,
} from "../../sevice/customerService";

const CreateJob = () => {
  const customer = JSON.parse(localStorage.getItem("customer"));
  const [job, setJob] = useState({
    customerId: customer.customerId,
    serviceCategoryId: 0,
    description: "",
    location: "",
    isEmergency: false,
    providerId: 0,
  });

  const [categories, setCategories] = useState([]);
  const [providers, setProviders] = useState([]);

  useEffect(() => {
    const fetchCategories = async () => {
      const data = await getCategories();
      setCategories(data);
    };

    fetchCategories();
  }, []);

  const handleCategoryChange = async (serviceCategoryId) => {
    setJob({ ...job, serviceCategoryId });

    const data = await getProvidersByCategory(serviceCategoryId);
    console.log("Prov: ", data);
    const providersWithNames = await Promise.all(
      data.map(async (provider) => {
        const user = await getUserById(provider.userId);
        return {
          ...provider,
          fullName: user.fullName,
        };
      }),
    );

    setProviders(providersWithNames);
  };

  const handleSubmit = async () => {
    try {
      await createJob(job);
      alert("Job Created Successfully");

      setJob({
        customerId: customer.customerId,
        serviceCategoryId: 0,
        description: "",
        location: "",
        isEmergency: false,
        providerId: 0,
      });
    } catch (error) {
      console.error("Error creating job", error);
    }
  };

  return (
    <div className="card space-y-4 max-w-xl">
      <textarea
        placeholder="Description"
        className="border p-2 rounded-lg w-full"
        onChange={(e) => setJob({ ...job, description: e.target.value })}
      />

      <select
        className="border p-2 rounded-lg w-full"
        value={job.serviceCategoryId}
        onChange={(e) => {
          const val = e.target.value;
          setJob({ ...job, serviceCategoryId: val === "" ? "" : Number(val) });
          if (val !== "") {
            handleCategoryChange(Number(val));
          }
        }}
      >
        <option key="empty-category" value="">
          Select Category
        </option>
        {categories.map((c, index) => (
          <option
            key={c.serviceCategoryId ?? index}
            value={c.serviceCategoryId}
          >
            {c.name}
          </option>
        ))}
      </select>

      <select
        className="border p-2 rounded-lg w-full"
        value={job.providerId}
        onChange={(e) => setJob({ ...job, providerId: Number(e.target.value) })}
      >
        <option key="empty-provider" value="">
          Select Provider
        </option>
        {providers.map((p, index) => (
          <option key={p.providerId ?? index} value={p.providerId}>
            {p.fullName}
          </option>
        ))}
      </select>

      <input
        placeholder="Location (exp: Tripoli)"
        className="border p-2 rounded-lg w-full"
        onChange={(e) => setJob({ ...job, location: e.target.value })}
      />

      <div className="flex items-center space-x-2">
        <input
          type="checkbox"
          checked={job.isEmergency}
          onChange={(e) => setJob({ ...job, isEmergency: e.target.checked })}
        />
        <label>Emergency Job</label>
      </div>

      <button onClick={handleSubmit} className="btn-primary">
        Create Job
      </button>
    </div>
  );
};

export default CreateJob;
