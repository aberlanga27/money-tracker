import { InfoCard } from "../components/InfoCard";
import { PolarAreaChart } from "../components/charts/PolarAreaChart";
import { TransactionTable } from "../components/tables/TransactionTable";
import { useTransactions } from "../hooks/useTransactions";
import { useBudget } from "../hooks/useBudget";
import { IndexedSelect } from "../components/common/IndexedSelect";

export default function HomePage() {
    const {
        transactions,
        transactionsPerCategory,
        transactionsPerCategoryData,
        transactionsPerCategoryLabels
    } = useTransactions()

    const {
        overallBudget,
        freeBudget,
        usedBudget
    } = useBudget({ transactionsPerCategory })

    return (
        <>
            <section id="budgetting-sneak-peak">
                <h2 className="text-primary font-bold pb-1">Budget</h2>

                <div className="grid grid-cols-3 gap-1">
                    <InfoCard title={'Budget'} value={overallBudget} />
                    <InfoCard title={'Used'} value={usedBudget} />
                    <InfoCard title={'Available'} value={freeBudget} />
                </div>
            </section>

            <IndexedSelect
                    endpoint={'Bank'} label={'Banks'} optionLabel={'bankName'} optionValue={'bankId'}
                    defaultValue={100}
                    onChange={({ value, label, option }) => {}}
                    onClear={() => {}}
            />

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-1">
                <section className="budgetting-chart">
                    <h2 className="text-primary font-bold py-1">By category</h2>

                    <div className="flex justify-center items-center">
                        <PolarAreaChart
                            data={transactionsPerCategoryData}
                            labels={transactionsPerCategoryLabels}
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