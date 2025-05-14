const transactionCategories = {
    endpoint: 'TransactionCategory',
    indexKey: 'transactionCategoryId',
    displayName: 'Categories',
    properties: [
        { name: 'transactionCategoryId', display: 'ID', type: 'number', required: true },
        { name: 'transactionCategoryName', display: 'Name', type: 'text', min: 1, max: 100, required: true },
        { name: 'transactionCategoryDescription', display: 'Description', type: 'text', min: 1, max: 100, required: true },
        { name: 'transactionCategoryIcon', display: 'Icon', type: 'text', min: 1, max: 100, required: true },
        { name: 'transactionCategoryColor', display: 'Color', type: 'text', min: 1, max: 6, required: true }
    ],
    allowAdd: true,
    allowEdit: true,
    allowDelete: true,
    allowMultipleAdd: false
}

const transactionTypes = {
    endpoint: 'TransactionType',
    indexKey: 'transactionTypeId',
    displayName: 'Transaction Types',
    properties: [
        { name: 'transactionTypeId', display: 'ID', type: 'number', required: true },
        { name: 'transactionTypeName', display: 'Name', type: 'text', min: 1, max: 100, required: true },
        { name: 'transactionTypeDescription', display: 'Description', type: 'text', min: 1, max: 100, required: true }
    ],
    allowAdd: true,
    allowEdit: true,
    allowDelete: true,
    allowMultipleAdd: false
}

const banks = {
    endpoint: 'Bank',
    indexKey: 'bankId',
    displayName: 'Banks',
    properties: [
        { name: 'bankId', display: 'ID', type: 'number', required: true },
        { name: 'bankName', display: 'Name', type: 'text', min: 1, max: 100, required: true }
    ],
    allowAdd: true,
    allowEdit: true,
    allowDelete: true,
    allowMultipleAdd: false
}

const budgetTypes = {
    endpoint: 'BudgetType',
    indexKey: 'budgetTypeId',
    displayName: 'Budget Types',
    properties: [
        { name: 'budgetTypeId', display: 'ID', type: 'number', required: true },
        { name: 'budgetTypeName', display: 'Name', type: 'text', min: 1, max: 100, required: true },
        { name: 'budgetTypeDays', display: 'Days', type: 'number', min: 1, max: 31, required: true }
    ],
    allowAdd: true,
    allowEdit: true,
    allowDelete: true,
    allowMultipleAdd: false
}

const budgets = {
    endpoint: 'Budget',
    indexKey: 'budgetId',
    displayName: 'Budgets',
    properties: [
        { name: 'budgetId', display: 'ID', type: 'number', required: true },
        { name: 'transactionCategoryId', display: 'Category', type: 'select', option: { name: 'TransactionCategory', value: 'transactionCategoryId', label: 'transactionCategoryName' }, required: true },
        { name: 'budgetTypeId', display: 'Type', type: 'select', option: { name: 'BudgetType', value: 'budgetTypeId', label: 'budgetTypeName' }, required: true },
        { name: 'budgetAmount', display: 'Amount', type: 'number', format:'currency', min: 1, max: 100000, required: true },
    ],
    allowAdd: true,
    allowEdit: true,
    allowDelete: true,
    allowMultipleAdd: false
}

const transactions = {
    endpoint: 'Transaction',
    indexKey: 'transactionId',
    displayName: 'Transactions',
    properties: [
        { name: 'transactionId', display: 'ID', type: 'number', required: true },
        { name: 'transactionDate', display: 'Date', type: 'datetime-local', required: true },
        { name: 'bankId', display: 'Bank', type: 'select', option: { name: 'Bank', value: 'bankId', label: 'bankName' }, required: true },
        { name: 'transactionTypeId', display: 'Type', type: 'select', option: { name: 'TransactionType', value: 'transactionTypeId', label: 'transactionTypeName' }, required: true },
        { name: 'transactionCategoryId', display: 'Category', type: 'select', option: { name: 'TransactionCategory', value: 'transactionCategoryId', label: 'transactionCategoryName' }, required: true },
        { name: 'transactionAmount', display: 'Amount', type: 'number', format: 'currency', min: 1, max: 100000, required: true },
        { name: 'transactionDescription', display: 'Description', type: 'text', min: 1, max: 150, required: true }
    ],
    allowAdd: true,
    allowEdit: true,
    allowDelete: true,
    allowMultipleAdd: false
}

export const config = [
    {
        value: transactions,
        label: transactions.displayName
    },
    {
        value: transactionCategories,
        label: transactionCategories.displayName
    },
    {
        value: transactionTypes,
        label: transactionTypes.displayName
    },
    {
        value: banks,
        label: banks.displayName
    },
    {
        value: budgetTypes,
        label: budgetTypes.displayName
    },
    {
        value: budgets,
        label: budgets.displayName
    }
]