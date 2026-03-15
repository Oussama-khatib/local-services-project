import React, { useState, useContext } from "react";
import Sidebar from "../../components/Sidebar";
import Navbar from "../../components/Navbar";
import ChatWidget from "../../components/ChatWidget";
import { AuthContext } from "../auth/AuthContext";

import Dashboard from "./Dashboard";
import MyJobs from "./MyJobs";
import CreateJob from "./CreateJob";
import Providers from "./Providers";
import Wallet from "./Wallet";
import Transactions from "./Transactions";
import AIAssistant from "./AIAssistant";
import { useNavigate } from "react-router-dom";

const CustomerLayout = () => {
  const navigate = useNavigate();
  const logout = () => {
    localStorage.removeItem("session");
    navigate("/");
  };

  const user = JSON.parse(localStorage.getItem("user"));
  const [active, setActive] = useState("Dashboard");

  const menu = [
    "Dashboard",
    "My Jobs",
    "Create Job",
    "Providers",
    "Wallet",
    "Transactions",
    "AI Assistant",
  ];

  const renderPage = () => {
    switch (active) {
      case "Dashboard":
        return <Dashboard />;
      case "My Jobs":
        return <MyJobs />;
      case "Create Job":
        return <CreateJob />;
      case "Providers":
        return <Providers />;
      case "Wallet":
        return <Wallet />;
      case "Transactions":
        return <Transactions />;
      case "AI Assistant":
        return <AIAssistant />;
      default:
        return <Dashboard />;
    }
  };

  return (
    <>
      <Sidebar menu={menu} active={active} setActive={setActive} />
      <Navbar user={user} onLogout={logout} />
      <div className="ml-64 mt-16 p-8">{renderPage()}</div>
      <ChatWidget />
    </>
  );
};

export default CustomerLayout;
