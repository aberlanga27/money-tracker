import { createContext, useEffect, useState } from "react";
import { api } from "../boot/axios";
import { notifyError } from "../utils/notify";

export const TransactionsContext = createContext()

export function TransactionsProvider({ children }) {
    const [records, setRecords] = useState(0)
    const [transactions, setTransactions] = useState([])

    const getTransactions = () => {
        api.get('/Transaction?pageSize=5&offsetSize=0')
            .then(({ data }) => {
                if (!data.status) {
                    notifyError(data.message)    
                    return
                }

                setTransactions(data.response)
                setRecords(data.totalRecords)
            })
            .catch(error => notifyError(error.message))
    }

    const getPaginatedTransactions = ({ page, itemsPerPage }) => {
        api.get(`/Transaction?pageSize=${itemsPerPage}&offsetSize=${(page - 1) * itemsPerPage}`)
            .then(({ data }) => {
                if (!data.status) {
                    notifyError(data.message)    
                    return
                }

                setTransactions(data.response)
                setRecords(data.totalRecords)
            })
            .catch(error => notifyError(error.message))
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
        <TransactionsContext.Provider value={{ transactions, records, getPaginatedTransactions, addTransaction }}>
            {children}
        </TransactionsContext.Provider>
    )
}