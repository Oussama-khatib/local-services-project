// src/auth/Signup.jsx

import { useState } from "react";
import { signup } from "./authService";
import { useNavigate, Link } from "react-router-dom";

const Signup = () => {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [fullName, setFullName] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState("Customer");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [error, setError] = useState("");

  const handleSignup = async (e) => {
    e.preventDefault();
    const res = await signup(fullName, email, phoneNumber, password, role);

    if (res.success) {
      navigate(`/${role}`);
    } else {
      setError(res.message);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <div className="bg-white p-8 rounded-2xl shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 text-center">Create Account</h2>

        {error && (
          <div className="bg-red-100 text-red-600 p-2 rounded mb-4 text-sm">
            {error}
          </div>
        )}

        <form onSubmit={handleSignup} className="space-y-4">
          <input
            type="text"
            placeholder="Enter your full name"
            className="w-full border p-2 rounded-xl"
            value={fullName}
            onChange={(e) => setFullName(e.target.value)}
            required
          />

          <input
            type="tel"
            placeholder="Enter your phone number"
            className="w-full border p-2 rounded-xl"
            value={phoneNumber}
            onChange={(e) => setPhoneNumber(e.target.value)}
            pattern="[0-9]{8}"
            required
          />

          <input
            type="email"
            placeholder="Email"
            className="w-full border p-2 rounded-xl"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />

          <input
            type="password"
            placeholder="Password"
            className="w-full border p-2 rounded-xl"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />

          <select
            className="w-full border p-2 rounded-xl"
            value={role}
            onChange={(e) => setRole(e.target.value)}
          >
            <option value="Customer">Customer</option>
            <option value="Service Provider">Service Provider</option>
            <option value="Admin">Admin</option>
          </select>

          <button type="submit" className="btn-primary w-full">
            Sign Up
          </button>
        </form>

        <p className="text-sm text-center mt-4">
          Already have an account?{" "}
          <Link to="/" className="text-indigo-600 font-medium">
            Login
          </Link>
        </p>
      </div>
    </div>
  );
};

export default Signup;
