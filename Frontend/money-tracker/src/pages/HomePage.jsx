import { InfoCard } from "../components/InfoCard";
import { DoughnutChart } from "../components/charts/DoughnutChart";
import { useBudget } from "../hooks/useBudget";
import { useTransactionsPerCategory } from "../hooks/useTransactionsPerCategory";
import { EntityManagement } from "../components/tables/EntityManagement";
import { config } from "../config/entity-management";

export default function HomePage() {
    const {
        transactionsPerCategory,
        transactionsPerCategoryData,
        transactionsPerCategoryLabels,
        transactionsPerCategoryColors,
        getTransactionsPerCategory
    } = useTransactionsPerCategory()

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

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-1">
                <section className="budgetting-chart">
                    <h2 className="text-primary font-bold py-1">By category</h2>

                    <div className="flex justify-center items-center h-[350px]">
                        <DoughnutChart
                            data={transactionsPerCategoryData}
                            labels={transactionsPerCategoryLabels}
                            colors={transactionsPerCategoryColors}
                        />
                    </div>
                </section>

                <section id="transactions-list">
                    <h2 className="text-primary font-bold py-1">Last transactions</h2>

                    <EntityManagement {...config[0].value} onRecordsModified={getTransactionsPerCategory} />
                </section>
            </div>
        </>
    );
}