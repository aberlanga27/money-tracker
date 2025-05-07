import { useEffect, useState } from "react";
import { notifyError } from "../utils/notify";
import { api } from "../boot/axios";

export function useTransactionsPerCategory() {
    const [transactionsPerCategory, setTransactionsPerCategory] = useState([])
    const [data, setData] = useState([])
    const [labels, setLabels] = useState([])
    const [colors, setColors] = useState([])

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
        setData(transactionsPerCategory.map((category) => category.totalAmount))
        setLabels(transactionsPerCategory.map((category) => category.transactionCategoryName))
        setColors(transactionsPerCategory.map((category) => `#${category.transactionCategoryColor}`))
    }, [transactionsPerCategory])

    return {
        transactionsPerCategory,
        transactionsPerCategoryData: data,
        transactionsPerCategoryLabels: labels,
        transactionsPerCategoryColors: colors,
        getTransactionsPerCategory
    }
}