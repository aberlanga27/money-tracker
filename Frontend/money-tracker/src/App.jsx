import { BrowserRouter, Routes, Route } from "react-router-dom";
import BanksPage from "./pages/BanksPage";
import BudgetPage from "./pages/BudgetPage";
import HomePage from "./pages/HomePage";
import MainLayout from "./layouts/MainLayout";
import StatsPage from "./pages/StatsPage";
import TransactionsPage from "./pages/TransactionsPage";

export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<MainLayout />}>
                    <Route index element={<HomePage />} />
                    <Route path="/budget" element={<BudgetPage />} />
                    <Route path="/stats" element={<StatsPage />} />
                    <Route path="/banks" element={<BanksPage />} />
                    <Route path="/transactions" element={<TransactionsPage />} />
                </Route>
            </Routes>
        </BrowserRouter>
    );
}
