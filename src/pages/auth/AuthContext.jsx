import React, { createContext, useState } from "react";

export const AuthContext = createContext();

const mockUsers = [
  { email: "admin@mail.com", password: "1234", role: "admin", name: "Admin" },
  {
    email: "customer@mail.com",
    password: "1234",
    role: "customer",
    name: "Customer",
  },
  {
    email: "provider@mail.com",
    password: "1234",
    role: "provider",
    name: "Provider",
  },
];

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  const login = (email, password) => {
    const found = mockUsers.find(
      (u) => u.email === email && u.password === password,
    );
    if (found) {
      setUser(found);
      return true;
    }
    return false;
  };

  const logout = () => setUser(null);

  return (
    <AuthContext.Provider value={{ user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
