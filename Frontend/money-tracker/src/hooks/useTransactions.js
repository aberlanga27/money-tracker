import { useContext } from "react";
import { TransactionsContext } from "../context/transactions";

export function useTransactions() {
    const { transactions, addTransaction } = useContext(TransactionsContext)

    const addNewTransaction = (transaction) => {
        addTransaction(transaction)
    }

    return {
        transactions,
        addNewTransaction
    }
}