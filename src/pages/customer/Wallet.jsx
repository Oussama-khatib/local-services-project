import React, { useEffect, useState } from "react";
import WalletCard from "../../components/WalletCard";
import { getWallet } from "../../sevice/customerService";

const Wallet = () => {
  const [balance, setBalance] = useState(0);

  useEffect(() => {
    const fetchBalance = async () => {
      try {
        const data = await getWallet();
        setBalance(data.balance);
      } catch (err) {
        console.error("Failed to fetch wallet balance:", err);
      }
    };

    fetchBalance();
  }, []);

  return <WalletCard balance={balance} />;
};

export default Wallet;
