import { createContext, useState } from "react";

export const TransactionsContext = createContext()

export function TransactionsProvider({ children }) {
    const [transactions, setTransactions] = useState([])

    const setInitialTransactions = (initialTransactions) => {
        // console.log("Setting initial transactions:", initialTransactions)
        setTransactions(initialTransactions)
    }

    const addTransaction = (transaction) => {
        // console.log("Adding transaction:", transaction)
        setTransactions((prevTransactions) => [
            transaction,
            ...prevTransactions,
        ])
    }

    const clearTransactions = () => {
        // console.log("Clearing transactions")
        setTransactions([])
    }

    return (
        <TransactionsContext.Provider value={{ transactions, setInitialTransactions, addTransaction, clearTransactions }}>
            {children}
        </TransactionsContext.Provider>
    )
}