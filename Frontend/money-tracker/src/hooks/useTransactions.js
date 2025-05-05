import { useContext } from "react";
import { TransactionsContext } from "../context/transactions";

export function useTransactions() {
    const { transactions, noRecords, getPaginatedTransactions, addTransaction } = useContext(TransactionsContext)

    return {
        transactions,
        noRecords,
        getPaginatedTransactions,
        addTransaction
    }
}