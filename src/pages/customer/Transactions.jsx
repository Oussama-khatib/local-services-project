import { React, useEffect, useState } from "react";
import TableComponent from "../../components/TableComponent";
import {
  getTransactions,
  getUserNameByWalletId,
} from "../../sevice/customerService";
const Transactions = () => {
  const columns = [
    "Job Id",
    "Receiver",
    "Sender (You)",
    "Amount",
    "Type",
    "Date",
  ];

  const [transactions, setTransactions] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchTransactions = async () => {
      try {
        const data = await getTransactions();

        const enriched = await Promise.all(
          data.map(async (t) => {
            const customerRes = await getUserNameByWalletId(t.toWalletId);
            const providerRes = await getUserNameByWalletId(t.fromWalletId);

            return {
              job: t.jobId,
              customer: customerRes,
              provider: providerRes,
              amount: `$${t.amount}`,
              type: `${t.type}`,
              date: t.date || "-",
            };
          }),
        );

        setTransactions(enriched);
        setLoading(false);
      } catch (err) {
        console.error("Error fetching transactions:", err);
        setLoading(false);
      }
    };

    fetchTransactions();
  }, []);

  return (
    <>
      {loading ? (
        <p>Loading transactions...</p>
      ) : (
        <TableComponent columns={columns} data={transactions} />
      )}
    </>
  );
};

export default Transactions;
