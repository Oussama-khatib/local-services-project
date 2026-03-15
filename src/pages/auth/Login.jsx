// src/auth/Login.jsx

import { useState, useEffect } from "react";
import { login } from "./authService";
import { useNavigate, Link } from "react-router-dom";

const Login = () => {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  /*useEffect(() => {
    initializeMockUsers();
  }, []);
  */

  const handleLogin = async (e) => {
    e.preventDefault();
    const res = await login(email, password);
    console.log("res:", res.user.role);
    if (res.success) {
      if (res.user.role === "Admin") navigate("/Admin");
      if (res.user.role === "Customer") navigate("/Customer");
      if (res.user.role === "Service Provider") navigate("/Service Provider");
    } else {
      setError(res.message);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <div className="bg-white p-8 rounded-2xl shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 text-center">
          Marketplace Login
        </h2>

        {error && (
          <div className="bg-red-100 text-red-600 p-2 rounded mb-4 text-sm">
            {error}
          </div>
        )}

        <form onSubmit={handleLogin} className="space-y-4">
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

          <button type="submit" className="btn-primary w-full">
            Login
          </button>
        </form>

        <p className="text-sm text-center mt-4">
          Don’t have an account?{" "}
          <Link to="/signup" className="text-indigo-600 font-medium">
            Sign up
          </Link>
        </p>

        <div className="text-xs text-gray-400 mt-6">
          <p>Admin: example@mail.com / password</p>
          <p>Customer: example1@mail.com / password1</p>
          <p>Provider: example2@mail.com / password2</p>
        </div>
      </div>
    </div>
  );
};

export default Login;
