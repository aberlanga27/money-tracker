import { api } from "../boot/axios";
import { notifyError } from "../utils/notify";
import { useCallback, useEffect, useState } from "react";

export function useTransactionsPerCategory() {
    const [transactionsPerCategory, setTransactionsPerCategory] = useState([])
    const [data, setData] = useState([])
    const [labels, setLabels] = useState([])

    const getTransactionsPerCategory = useCallback(() => {        
        const today = new Date()
        const startDate = new Date(Date.UTC(today.getFullYear(), today.getMonth(), 1, 0, 0, 0))
        const endDate = new Date(Date.UTC(today.getFullYear(), today.getMonth() + 1, 0, 23, 59, 59))

        api.post('/Transaction/GroupByCategory', {
            startDate: startDate,
            endDate: endDate
        })
            .then(({ data }) => {
                setTransactionsPerCategory(data.response)
            })
            .catch(error => notifyError({ message: 'Error fetching transactions by category:', error }))
    }, [setTransactionsPerCategory])

    useEffect(() => {
        getTransactionsPerCategory()
    }, [getTransactionsPerCategory])

    useEffect(() => {
        setData(transactionsPerCategory.map((category) => category.totalAmount))
        setLabels(transactionsPerCategory.map((category) => category.transactionCategoryName))
    }, [transactionsPerCategory])

    return {
        transactionsPerCategory,
        transactionsPerCategoryData: data,
        transactionsPerCategoryLabels: labels,
        getTransactionsPerCategory
    }
}