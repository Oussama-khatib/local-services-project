import React, { useState } from "react";
import { deposit } from "../sevice/customerService";
const WalletCard = ({ balance }) => {
  const [amount, setAmount] = useState("");

  const handleDeposit = async () => {
    if (!amount || amount <= 0) {
      alert("Enter a valid amount");
      return;
    }

    try {
      await deposit(amount);
      alert("Deposit successful");
      window.location.reload();
    } catch (err) {
      console.error("Deposit failed:", err);
    }
  };

  return (
    <div className="bg-white shadow-md rounded-xl p-6 w-96">
      <h2 className="text-xl font-bold mb-4">Wallet</h2>

      <p className="text-3xl font-semibold text-green-600 mb-6">${balance}</p>

      {/* User enters amount here */}
      <input
        type="number"
        placeholder="Enter deposit amount"
        className="border p-2 rounded-lg w-full mb-3"
        value={amount}
        onChange={(e) => setAmount(e.target.value)}
      />

      <button
        onClick={handleDeposit}
        className="bg-indigo-600 text-white px-4 py-2 rounded-lg w-full hover:bg-indigo-700"
      >
        Deposit
      </button>
    </div>
  );
};

export default WalletCard;
