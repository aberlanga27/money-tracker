export const getDatesByBudgetType = ({ budgetTypeId }) => {
    const today = new Date()

    switch (budgetTypeId) {
        case 1: // First fortnight
            return {
                startDate: new Date(Date.UTC(today.getFullYear(), today.getMonth(), 1, 0, 0, 0)),
                endDate: new Date(Date.UTC(today.getFullYear(), today.getMonth(), 15, 23, 59, 59))
            }
        case 2: // Second fortnight
            return {
                startDate: new Date(Date.UTC(today.getFullYear(), today.getMonth(), 16, 0, 0, 0)),
                endDate: new Date(Date.UTC(today.getFullYear(), today.getMonth() + 1, 0, 23, 59, 59))
            }
        case 3: // Monthly
            return {
                startDate: new Date(Date.UTC(today.getFullYear(), today.getMonth(), 1, 0, 0, 0)),
                endDate: new Date(Date.UTC(today.getFullYear(), today.getMonth() + 1, 0, 23, 59, 59))
            }
        default:
            return {
                startDate: new Date(Date.UTC(today.getFullYear(), today.getMonth(), 1, 0, 0, 0)),
                endDate: new Date(Date.UTC(today.getFullYear(), today.getMonth() + 1, 0, 23, 59, 59))
            }
    }  
}