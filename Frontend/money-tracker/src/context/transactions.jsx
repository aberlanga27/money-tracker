import { createContext, useEffect, useState } from "react";
import { api } from "../boot/axios";

export const TransactionsContext = createContext()

export function TransactionsProvider({ children }) {
    const [transactions, setTransactions] = useState([])

    const getTransactions = () => {
        api.get('/Transaction')
            .then(({ data }) => {
                setTransactions(data.response)
            })
            .catch(error => console.error('Error fetching transactions:', error))
    }
    
    const addTransaction = (transaction) => {
        setTransactions((prevTransactions) => [
            transaction,
            ...prevTransactions,
        ])
    }

    useEffect(() => {
        getTransactions()
    }, [])

    return (
        <TransactionsContext.Provider value={{ transactions, addTransaction }}>
            {children}
        </TransactionsContext.Provider>
    )
}