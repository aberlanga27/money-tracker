import { config } from "../config/entity-management";
import { EntityManagement } from "../components/tables/EntityManagement";

export default function BudgetPage() {
    return (
        <>
            <section id="transactions-list">
                <h2 className="text-primary font-bold py-1">Budgets</h2>

                <EntityManagement {...config[5].value} defaultItemsPerPage={10}/>
                <EntityManagement {...config[4].value} defaultItemsPerPage={10}/>
                <EntityManagement {...config[1].value} defaultItemsPerPage={10}/>
            </section>
        </>
    );
}