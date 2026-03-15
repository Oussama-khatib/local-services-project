import React from "react";

const Sidebar = ({ menu, active, setActive }) => {
  return (
    <div className="w-64 h-screen bg-white shadow-md p-6 fixed">
      <h1 className="text-xl font-bold text-indigo-600 mb-10">ServiceHub</h1>

      <ul className="space-y-4">
        {menu.map((item) => (
          <li
            key={item}
            onClick={() => setActive(item)}
            className={`cursor-pointer p-2 rounded-lg ${
              active === item
                ? "bg-indigo-100 text-indigo-600 font-semibold"
                : "hover:bg-gray-100"
            }`}
          >
            {item}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Sidebar;
