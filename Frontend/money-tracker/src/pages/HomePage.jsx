import { useContext, useEffect, useState } from "react";
import { InfoCard } from "../components/InfoCard";
import { PolarAreaChart } from "../components/charts/PolarAreaChart";
import { TransactionTable } from "../components/tables/TransactionTable";
import { TransactionsContext } from "../context/transactions";
import { api } from "../boot/axios";

export default function HomePage() {
    const { transactions, setInitialTransactions } = useContext(TransactionsContext)
    const [transactionsPerCategory, setTransactionsPerCategory] = useState([])
    const [budgetsPerCategory, setBudgetsPerCategory] = useState([])

    const [data, setData] = useState([])
    const [labels, setLabels] = useState([])
    
    const [overallBudget, setOverallBudget] = useState(0)
    const [freeBudget, setFreeBudget] = useState(0)
    const [usedBudget, setUsedBudget] = useState(0)

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

        api.get('/Budget')
            .then(({ data }) => {
                setBudgetsPerCategory(data.response)
            })
            .catch(error => console.error('Error fetching budgets by category:', error))
    }, [])

    useEffect(() => {
        setData(transactionsPerCategory.map((category) => category.totalAmount))
        setLabels(transactionsPerCategory.map((category) => category.transactionCategoryName))

        const usedOnTransactions = transactionsPerCategory.reduce((acc, category) => acc + category.totalAmount, 0)
        setUsedBudget(usedOnTransactions)
    }, [transactionsPerCategory])

    useEffect(() => {
        const totalBudget = budgetsPerCategory.reduce((acc, category) => acc + category.budgetAmount, 0)
        setOverallBudget(totalBudget)

        const totalExpenses = transactionsPerCategory.reduce((acc, category) => acc + category.totalAmount, 0)
        setFreeBudget(totalBudget - totalExpenses)
    }, [transactionsPerCategory, budgetsPerCategory])

    return (
        <>
            <section id="budgetting-sneak-peak">
                <h2 className="text-primary font-bold pb-1">Budget</h2>

                <div className="grid grid-cols-3 gap-1">
                    <InfoCard title={'Budget'} value={overallBudget} />
                    <InfoCard title={'Available'} value={freeBudget} />
                    <InfoCard title={'Used'} value={usedBudget} />
                </div>
            </section>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-1">
                <section className="budgetting-chart">
                    <h2 className="text-primary font-bold py-1">By category</h2>

                    <div className="flex justify-center items-center">
                        <PolarAreaChart
                            data={data}
                            labels={labels}
                        />
                    </div>
                </section>

                <section id="transactions-list">
                    <h2 className="text-primary font-bold py-1">Last transactions</h2>

                    <TransactionTable transactions={transactions} />
                </section>
            </div>
        </>
    );
}