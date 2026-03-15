import React, { useState, useContext } from "react";
import Sidebar from "../../components/Sidebar";
import Navbar from "../../components/Navbar";
import { AuthContext } from "../auth/AuthContext";

import Dashboard from "./Dashboard";
import ProviderJobs from "./ProviderJobs";
import Services from "./Services";
import Reviews from "./Reviews";
import Wallet from "./Wallet";
import AIAssistant from "./AIAssistant";
import UpdateProfile from "./UpdateProfile";
import { useNavigate } from "react-router-dom";

const ProviderLayout = () => {
  const navigate = useNavigate();
  const logout = () => {
    localStorage.removeItem("session");
    navigate("/");
  };

  const user = JSON.parse(localStorage.getItem("user"));
  const [active, setActive] = useState("Dashboard");

  const menu = [
    "Dashboard",
    "Jobs",
    "Services",
    "Reviews",
    "Wallet",
    "AI Assistant",
    "Update Profile",
  ];

  const renderPage = () => {
    switch (active) {
      case "Dashboard":
        return <Dashboard />;
      case "Jobs":
        return <ProviderJobs />;
      case "Assigned Jobs":
        return <AssignedJobs />;
      case "Completed Jobs":
        return <CompletedJobs />;
      case "Services":
        return <Services />;
      case "Reviews":
        return <Reviews />;
      case "Wallet":
        return <Wallet />;
      case "AI Assistant":
        return <AIAssistant />;
      case "Update Profile":
        return <UpdateProfile />;
      default:
        return <Dashboard />;
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

export default ProviderLayout;
