import { useContext } from "react";
import { TransactionsContext } from "../context/transactions";

export function useTransactions() {
    const { transactions, records, getPaginatedTransactions, addTransaction } = useContext(TransactionsContext)

    return {
        transactions,
        records,
        getPaginatedTransactions,
        addTransaction
    }
}