import React, { useEffect, useState } from "react";
import WalletCard from "../../components/WalletCard";
import { getMyWallet } from "../../sevice/providerService";
const Wallet = () => {
  const [balance, setBalance] = useState(0);

  useEffect(() => {
    const fetchBalance = async () => {
      try {
        const data = await getMyWallet();
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
