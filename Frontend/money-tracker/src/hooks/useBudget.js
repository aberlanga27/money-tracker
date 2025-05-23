import { api } from "../boot/axios"
import { notifyError } from "../utils/notify"
import { useCallback, useEffect, useState } from "react"

export function useBudget({ transactionsPerCategory, budgetTypeId = 0 }) {
    const [budgetsPerCategory, setBudgetsPerCategory] = useState([])

    const [overallBudget, setOverallBudget] = useState(0)
    const [freeBudget, setFreeBudget] = useState(0)
    const [usedBudget, setUsedBudget] = useState(0)

    const getBudgets = useCallback(() => {
        api.post('/Budget/Find', { budgetTypeId })
            .then(({ data }) => {
                setBudgetsPerCategory(data)
            })
            .catch(error => notifyError({ message: error }))
    }, [budgetTypeId, setBudgetsPerCategory])

    useEffect(() => {
        getBudgets()
    }, [getBudgets])


    useEffect(() => {
        const usedOnTransactions = transactionsPerCategory.reduce((acc, category) => acc + category.totalAmount, 0)
        setUsedBudget(usedOnTransactions)

        const totalBudget = budgetsPerCategory.reduce((acc, category) => acc + category.budgetAmount, 0)
        setOverallBudget(totalBudget)

        const totalExpenses = transactionsPerCategory.reduce((acc, category) => acc + category.totalAmount, 0)
        setFreeBudget(totalBudget - totalExpenses)
    }, [transactionsPerCategory, budgetsPerCategory])

    return {
        budgetsPerCategory,
        overallBudget,
        freeBudget,
        usedBudget
    }
}