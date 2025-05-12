import { config } from "../config/entity-management";
import { DoughnutChart } from "../components/charts/DoughnutChart";
import { EntityManagement } from "../components/tables/EntityManagement";
import { IndexedSelect } from "../components/common/IndexedSelect";
import { InfoCard } from "../components/InfoCard";
import { useBudget } from "../hooks/useBudget";
import { useState } from "react";
import { useTransactionsPerBank } from "../hooks/useTransactionsPerBank";
import { useTransactionsPerCategory } from "../hooks/useTransactionsPerCategory";
import { TabPanels } from "../components/common/TabPanels";

export default function HomePage() {
    const [budgetTypeId, setBudgetTypeId] = useState(new Date().getDate() > 15 ? 2 : 1)

    const {
        transactionsPerCategory,
        transactionsPerCategoryData,
        transactionsPerCategoryLabels,
        getTransactionsPerCategory
    } = useTransactionsPerCategory()

    const {
        transactionsPerBankData,
        transactionsPerBankLabels,
        getTransactionsPerBank
    } = useTransactionsPerBank()

    const {
        overallBudget,
        freeBudget,
        usedBudget
    } = useBudget({ transactionsPerCategory, budgetTypeId })

    const updateCharts = () => {
        getTransactionsPerCategory()
        getTransactionsPerBank()
    }

    return (
        <>
            <section id="budgetting-sneak-peak">
                <h2 className="text-primary font-bold pb-1">Budget</h2>

                <IndexedSelect
                    endpoint={'BudgetType'} label={'Type'} optionLabel={'budgetTypeName'} optionValue={'budgetTypeId'}
                    defaultValue={budgetTypeId}
                    onChange={({ value }) => setBudgetTypeId(value)}
                />

                <div className="grid grid-cols-3 gap-1 pt-1">
                    <InfoCard title={'Budget'} value={overallBudget} />
                    <InfoCard title={'Used'} value={usedBudget} />
                    <InfoCard title={'Available'} value={freeBudget} />
                </div>
            </section>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-1">
                <section className="budgetting-chart">
                    <h2 className="text-primary font-bold py-1">Charts</h2>

                    <TabPanels sections={['By Category', 'By Bank', 'Budget vs Real']} className="flex flex-col gap-1">
                        <div className="flex justify-center items-center h-[350px]">
                            <DoughnutChart
                                data={transactionsPerCategoryData}
                                labels={transactionsPerCategoryLabels}
                            />
                        </div>

                        <div className="flex justify-center items-center h-[350px]">
                            <DoughnutChart
                                data={transactionsPerBankData}
                                labels={transactionsPerBankLabels}
                            />
                        </div>

                        <div className="flex justify-center items-center h-[350px]">
                            tmp
                        </div>
                    </TabPanels>
                </section>

                <section id="transactions-list">
                    <h2 className="text-primary font-bold py-1">Resent Transactions</h2>

                    <EntityManagement {...config[0].value} onRecordsModified={updateCharts} />
                </section>
            </div>
        </>
    );
}