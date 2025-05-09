import { BrowserRouter, Routes, Route } from "react-router-dom";
import MainLayout from "./layouts/MainLayout";
import HomePage from "./pages/HomePage";
import BudgetPage from "./pages/BudgetPage";

export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<MainLayout />}>
                    <Route index element={<HomePage />} />
                    <Route path="/budget" element={<BudgetPage />} />
                    <Route path="/stats" element={<div>Stadistics Page</div>} />
                    <Route path="/accounts" element={<div>Accounts Page</div>} />
                    <Route path="/transactions" element={<div>Transactions Page</div>} />
                </Route>
            </Routes>
        </BrowserRouter>
    );
}
