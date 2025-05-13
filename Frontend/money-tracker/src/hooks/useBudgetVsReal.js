import { useEffect, useState } from "react";

export function useBudgetVsReal({ transactionsPerCategory, budgetsPerCategory }) {
    const [budgetVsRealData, setBudgetVsRealData] = useState([])
    const [budgetVsRealLabels, setBudgetVsRealLabels] = useState([])

    useEffect(() => {
        if (transactionsPerCategory.length === 0 || budgetsPerCategory.length === 0) {
            setBudgetVsRealData([])
            setBudgetVsRealLabels([])
            return
        }

        const data = {}
        budgetsPerCategory.forEach((budget) => {
            const category = transactionsPerCategory.find((category) => category.transactionCategoryId === budget.transactionCategoryId)
            if (category) {
                data[budget.transactionCategoryName] = {
                    budget: budget.budgetAmount,
                    used: category.totalAmount
                }
            }
        })

        setBudgetVsRealData([Object.values(data).map((item) => item.budget), Object.values(data).map((item) => item.used)])
        setBudgetVsRealLabels(Object.keys(data))

    }, [transactionsPerCategory, budgetsPerCategory])

    return {
        budgetVsRealData,
        budgetVsRealLabels
    }
}