import { Outlet } from "react-router-dom";
import Menu from "../components/Menu";

export default function MainLayout() {
    const menuOptions = [
        { label: "Home", path: "/", icon: "home" },
        { label: "Budget", path: "/budget", icon: "receipt_long" },
        { label: "Stadistics", path: "/stats", icon: "leaderboard" },
        { label: "Banks", path: "/banks", icon: "switch_account" },
        { label: "Transactions", path: "/transactions", icon: "receipt" },
    ]

    return (
        <div className="flex flex-col min-h-screen">
            <header className="bg-primary flex items-center justify-center p-1">
                <div className="text-pink-400 flex items-center justify-center bg-amber-300 p-1 rounded-2xl">
                    <span className="material-icons">savings</span>
                </div>
            </header>

            <main className="flex-grow p-2.5">
                <Outlet />
            </main>

            <footer className="bg-primary text-white p-2.5">
                <Menu options={menuOptions} />
            </footer>
        </div>
    );
}