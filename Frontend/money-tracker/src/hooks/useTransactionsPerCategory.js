import { api } from "../boot/axios";
import { getDatesByBudgetType } from "../utils/date";
import { notifyError } from "../utils/notify";
import { useCallback, useEffect, useState } from "react";

export function useTransactionsPerCategory({ budgetTypeId }) {
    const [transactionsPerCategory, setTransactionsPerCategory] = useState([])
    const [data, setData] = useState([])
    const [labels, setLabels] = useState([])

    const getTransactionsPerCategory = useCallback(() => {        
        const { startDate, endDate } = getDatesByBudgetType({ budgetTypeId })

        api.post('/Transaction/GroupByCategory', {
            startDate: startDate,
            endDate: endDate
        })
            .then(({ data }) => {
                setTransactionsPerCategory(data.response)
            })
            .catch(error => notifyError({ message: error }))
    }, [setTransactionsPerCategory, budgetTypeId])

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