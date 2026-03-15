import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import Login from "./pages/auth/Login";
import Signup from "./pages/auth/Signup";
import ProtectedRoute from "./pages/auth/ProtectedRoute";
import CustomerLayout from "./pages/customer/CustomerLayout";
import ProviderLayout from "./pages/provider/ProviderLayout";
import AdminLayout from "./pages/admin/AdminLayout";

const App = () => {
  return (
    <Router>
      <Routes>
        {/* Login */}
        <Route path="/" element={<Login />} />
        <Route path="/signup" element={<Signup />} />

        {/* Customer Protected Route */}
        <Route path="/Customer" element={<CustomerLayout />} />

        <Route path="/Service Provider" element={<ProviderLayout />} />
        <Route path="/Admin" element={<AdminLayout />} />
        {/* Temporary fallback */}
        <Route path="*" element={<Login />} />
      </Routes>
    </Router>
  );
};

export default App;
