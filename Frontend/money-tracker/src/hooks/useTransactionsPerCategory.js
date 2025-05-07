import { useEffect, useState } from "react";
import { notifyError } from "../utils/notify";
import { api } from "../boot/axios";

export function useTransactionsPerCategory() {
    const [transactionsPerCategory, setTransactionsPerCategory] = useState([])
    const [transactionsPerCategoryData, setTransactionsPerCategoryData] = useState([])
    const [transactionsPerCategoryLabels, setTransactionsPerCategoryLabels] = useState([])

    const getTransactionsPerCategory = () => {        
        const today = new Date()
        let startDate = new Date(Date.UTC(today.getFullYear(), today.getMonth(), 1, 0, 0, 0))
        let endDate = new Date(Date.UTC(today.getFullYear(), today.getMonth() + 1, 0, 23, 59, 59))

        api.post('/Transaction/GroupByCategory', {
            startDate: startDate,
            endDate: endDate
        })
            .then(({ data }) => {
                setTransactionsPerCategory(data.response)
            })
            .catch(error => notifyError({ message: 'Error fetching transactions by category:', error }))
    }

    useEffect(() => {
        getTransactionsPerCategory()
    }, [])

    useEffect(() => {
        setTransactionsPerCategoryData(transactionsPerCategory.map((category) => category.totalAmount))
        setTransactionsPerCategoryLabels(transactionsPerCategory.map((category) => category.transactionCategoryName))
    }, [transactionsPerCategory])

    return {
        transactionsPerCategory,
        transactionsPerCategoryData,
        transactionsPerCategoryLabels,
        getTransactionsPerCategory
    }
}