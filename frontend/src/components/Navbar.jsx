import React from "react";

const Navbar = ({ user, onLogout }) => {
  return (
    <div className="h-16 bg-white shadow-sm flex justify-between items-center px-8 ml-64">
      <h2 className="font-semibold text-gray-600">Welcome, {user.fullName}</h2>

      <button onClick={onLogout} className="btn-secondary">
        Logout
      </button>
    </div>
  );
};

export default Navbar;
