import React, { useState, useEffect } from "react";
import {
  getUsers,
  activateUser,
  deactivateUser,
} from "../../sevice/adminService";

const Users = () => {
  const [users, setUsers] = useState([]);

  useEffect(() => {
    loadUsers();
  }, []);

  const loadUsers = async () => {
    try {
      const data = await getUsers();
      setUsers(data);
    } catch (err) {
      console.error(err);
    }
  };

  const toggleStatus = async (user) => {
    try {
      if (user.isActive === "yes") {
        await deactivateUser(user.userId);
      } else {
        await activateUser(user.userId);
      }

      setUsers((prev) =>
        prev.map((u) =>
          u.userId === user.userId
            ? { ...u, isActive: u.isActive === "yes" ? "no" : "yes" }
            : u,
        ),
      );
    } catch (err) {
      console.error("Error updating user", err);
    }
  };

  return (
    <div className="card">
      <h2 className="text-xl font-bold mb-6">Manage Users</h2>

      <table className="w-full text-left">
        <thead>
          <tr className="border-b text-gray-500 text-sm">
            <th className="py-3">Name</th>
            <th>Role</th>
            <th>Status</th>
            <th>Action</th>
          </tr>
        </thead>

        <tbody>
          {users.map((user) => (
            <tr key={user.userId} className="border-b hover:bg-gray-50">
              <td className="py-3">{user.fullName}</td>
              <td>{user.role}</td>

              <td>
                <span
                  className={
                    user.isActive === "yes" ? "badge-success" : "badge-danger"
                  }
                >
                  {user.isActive === "yes" ? "Active" : "Inactive"}
                </span>
              </td>

              <td>
                <button
                  onClick={() => toggleStatus(user)}
                  className="btn-secondary text-sm"
                >
                  {user.isActive === "yes" ? "Deactivate" : "Activate"}
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default Users;
