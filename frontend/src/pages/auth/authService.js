/*
const USERS_KEY = "mock_users";
const SESSION_KEY = "mock_session";

// Initialize with default users
export const initializeMockUsers = () => {
  const existing = localStorage.getItem(USERS_KEY);
  if (!existing) {
    const defaultUsers = [
      { id: 1, email: "admin@mail.com", password: "123456", role: "admin" },
      {
        id: 2,
        email: "customer@mail.com",
        password: "123456",
        role: "customer",
      },
      {
        id: 3,
        email: "provider@mail.com",
        password: "123456",
        role: "provider",
      },
    ];
    localStorage.setItem(USERS_KEY, JSON.stringify(defaultUsers));
  }
};

export const login = (email, password) => {
  const users = JSON.parse(localStorage.getItem(USERS_KEY)) || [];
  const user = users.find((u) => u.email === email && u.password === password);

  if (user) {
    localStorage.setItem(SESSION_KEY, JSON.stringify(user));
    return { success: true, user };
  }

  return { success: false, message: "Invalid credentials" };
};

export const signup = (email, password, role) => {
  const users = JSON.parse(localStorage.getItem(USERS_KEY)) || [];

  const exists = users.find((u) => u.email === email);
  if (exists) {
    return { success: false, message: "Email already exists" };
  }

  const newUser = {
    id: Date.now(),
    email,
    password,
    role,
  };

  users.push(newUser);
  localStorage.setItem(USERS_KEY, JSON.stringify(users));
  localStorage.setItem(SESSION_KEY, JSON.stringify(newUser));

  return { success: true, user: newUser };
};

export const logout = () => {
  localStorage.removeItem(SESSION_KEY);
};

export const getCurrentUser = () => {
  return JSON.parse(localStorage.getItem(SESSION_KEY));
};
*/
import api from "../../sevice/api";

export const login = async (email, password) => {
  try {
    const res = await api.post("/Users/login", {
      email,
      password,
    });
    localStorage.setItem("user", JSON.stringify(res.data.user));
    localStorage.setItem("session", JSON.stringify(res.data));
    if (res.data.user.role === "Customer") {
      const customer = await api.get(
        `/Customers/by-user/${res.data.user.userId}`,
      );
      localStorage.setItem("customer", JSON.stringify(customer.data));
    }

    if (res.data.user.role === "Service Provider") {
      const provider = await api.get(
        `/ServiceProviders/by-user/${res.data.user.userId}`,
      );
      localStorage.setItem("provider", JSON.stringify(provider.data));
    }

    return { success: true, user: res.data.user };
  } catch (error) {
    return { success: false, message: "Invalid credentials" };
  }
};

export const signup = async (fullName, email, phoneNumber, password, role) => {
  try {
    const res = await api.post("/Users/register", {
      fullName,
      email,
      phoneNumber,
      password,
      role,
    });

    return { success: true, user: res.data.user };
  } catch (error) {
    return { success: false, message: "Signup failed" };
  }
};
