import { BrowserRouter, Routes, Route } from "react-router-dom";
import MainLayout from "./layouts/MainLayout";
import HomePage from "./pages/HomePage";
import { TransactionsProvider } from "./context/transactions";

export default function App() {
    return (
        <TransactionsProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<MainLayout />}>
                        <Route index element={<HomePage />} />
                        <Route path="/calendar" element={<div>Calendar Page</div>} />
                    </Route>
                </Routes>
            </BrowserRouter>
        </TransactionsProvider>
    );
}
