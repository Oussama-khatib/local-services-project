import React, { useState, useEffect } from "react";
import ProviderCard from "../../components/ProviderCard";
import { getProviders } from "../../sevice/customerService";

const Providers = () => {
  const [filter, setFilter] = useState("");
  const [providers, setProviders] = useState([]);
  const [loading, setLoading] = useState(true);

  // Fetch all providers
  useEffect(() => {
    const fetchProviders = async () => {
      try {
        const data = await getProviders();
        //const data = await response.json();
        setProviders(data);
        setLoading(false);
      } catch (error) {
        console.error("Failed to fetch providers:", error);
        setLoading(false);
      }
    };

    fetchProviders();
  }, []);

  // Filter providers
  const filtered = providers.filter(
    (p) =>
      p.fullName.toLowerCase().includes(filter.toLowerCase()) ||
      p.location.toLowerCase().includes(filter.toLowerCase()),
  );

  if (loading) return <p>Loading providers...</p>;

  return (
    <div>
      <input
        placeholder="Filter by Name or Location"
        className="border p-2 rounded-lg mb-6 w-64"
        onChange={(e) => setFilter(e.target.value)}
      />

      <div className="grid grid-cols-2 gap-6">
        {filtered.map((p, i) => (
          <ProviderCard key={i} provider={p} />
        ))}
      </div>
    </div>
  );
};

export default Providers;
