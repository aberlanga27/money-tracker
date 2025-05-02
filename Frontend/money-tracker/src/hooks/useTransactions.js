import { useContext, useEffect, useState } from "react";
import { TransactionsContext } from "../context/transactions";
import { api } from "../boot/axios";

export function useTransactions() {
    const { transactions, setInitialTransactions } = useContext(TransactionsContext)
    const [transactionsPerCategory, setTransactionsPerCategory] = useState([])

    const [transactionsPerCategoryData, setTransactionsPerCategoryData] = useState([])
    const [transactionsPerCategoryLabels, setTransactionsPerCategoryLabels] = useState([])

    useEffect(() => {
        api.get('/Transaction')
            .then(({ data }) => {
                setInitialTransactions(data.response)
            })
            .catch(error => console.error('Error fetching transactions:', error))

        // ...

        const today = new Date()
        const startDate = new Date(today.getFullYear(), today.getMonth(), 1)
        const endDate = new Date(today.getFullYear(), today.getMonth() + 1, 0)

        api.post('/Transaction/GroupByCategory', {
            startDate,
            endDate,
        })
            .then(({ data }) => {
                setTransactionsPerCategory(data.response)
            })
            .catch(error => console.error('Error fetching transactions by category:', error))
    }, [])

    useEffect(() => {
        setTransactionsPerCategoryData(transactionsPerCategory.map((category) => category.totalAmount))
        setTransactionsPerCategoryLabels(transactionsPerCategory.map((category) => category.transactionCategoryName))
    }, [transactionsPerCategory])

    return {
        transactions,
        transactionsPerCategory,
        transactionsPerCategoryData,
        transactionsPerCategoryLabels,
    }
}