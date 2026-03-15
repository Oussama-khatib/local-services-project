import React, { useEffect, useState } from "react";
import {
  getCategories,
  addProviderService,
} from "../../sevice/providerService";
const Services = () => {
  const provider = JSON.parse(localStorage.getItem("provider"));

  const [services, setServices] = useState([]);
  const [form, setForm] = useState({
    serviceCategoryId: "",
    minPrice: "",
    maxPrice: "",
    description: "",
  });

  const loadServices = async () => {
    try {
      const data = await getCategories();
      setServices(data);
    } catch (err) {
      console.error("Error loading services", err);
    }
  };
  useEffect(() => {
    loadServices();
  }, []);
  const handleChange = (e) => {
    setForm({
      ...form,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async () => {
    try {
      await addProviderService(
        provider.providerId,
        form.serviceCategoryId,
        form.minPrice,
        form.maxPrice,
        form.description,
      );
      alert("Service added successfully");

      setForm({
        serviceCategoryId: "",
        minPrice: "",
        maxPrice: "",
        description: "",
      });
    } catch (err) {
      console.error(err);
      alert("Error adding service");
    }
  };

  return (
    <div className="card max-w-md space-y-4">
      <h3 className="font-semibold text-lg">Add Service</h3>

      {/* Service Select */}
      <select
        name="serviceCategoryId"
        value={form.serviceCategoryId}
        onChange={handleChange}
        className="border p-2 rounded-lg w-full"
      >
        <option value="">Select Service</option>
        {services.map((s, k) => (
          <option key={k} value={s.serviceCategoryId}>
            {s.name}
          </option>
        ))}
      </select>

      {/* Min Price */}
      <input
        type="number"
        name="minPrice"
        placeholder="Minimum Price ($)"
        value={form.minPrice}
        onChange={handleChange}
        className="border p-2 rounded-lg w-full"
      />

      {/* Max Price */}
      <input
        type="number"
        name="maxPrice"
        placeholder="Maximum Price ($)"
        value={form.maxPrice}
        onChange={handleChange}
        className="border p-2 rounded-lg w-full"
      />

      {/* Description */}
      <textarea
        name="description"
        placeholder="Service Description"
        value={form.description}
        onChange={handleChange}
        className="border p-2 rounded-lg w-full"
      />

      <button onClick={handleSubmit} className="btn-primary w-full">
        Add Service
      </button>
    </div>
  );
};

export default Services;
