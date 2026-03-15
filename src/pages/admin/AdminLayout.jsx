import React, { useState, useContext } from "react";
import Sidebar from "../../components/Sidebar";
import Navbar from "../../components/Navbar";
import { AuthContext } from "../auth/AuthContext";
import { useNavigate } from "react-router-dom";

import Users from "./Users";
import Transactions from "./Transactions";
import Earnings from "./Earnings";

const AdminLayout = () => {
  const navigate = useNavigate();
  const logout = () => {
    localStorage.removeItem("session");
    navigate("/");
  };

  const user = JSON.parse(localStorage.getItem("user"));
  const [active, setActive] = useState("Users");

  const menu = ["Users", "Transactions", "Earnings"];

  const renderPage = () => {
    switch (active) {
      case "Users":
        return <Users />;

      case "Transactions":
        return <Transactions />;
      case "Earnings":
        return <Earnings />;
      default:
        return <Users />;
    }
  };

  return (
    <>
      <Sidebar menu={menu} active={active} setActive={setActive} />
      <Navbar user={user} onLogout={logout} />
      <div className="ml-64 mt-16 p-8">{renderPage()}</div>
    </>
  );
};

export default AdminLayout;
