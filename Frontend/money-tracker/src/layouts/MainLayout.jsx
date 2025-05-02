import { Outlet } from "react-router-dom";
import { TransactionModal } from "../components/modals/TransactionModal";
import { Button} from "../components/common/Button";
import { useState } from "react";
import Menu from "../components/Menu";

export default function MainLayout() {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const toggleModal = () => setIsModalOpen(prev => !prev);

    const menuOptions = [
        { label: "Home", path: "/", icon: "home" },
        { label: "Calendar", path: "/calendar", icon: "calendar_month" },
        { label: "Stadistics", path: "/stats", icon: "leaderboard" },
        { label: "Accounts", path: "/accounts", icon: "switch_account" },
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

            <Button
                className="fixed bottom-16 right-1"
                onClick={toggleModal}
            >
                <span className="material-icons">add</span>
            </Button>

            <TransactionModal show={isModalOpen} onOk={toggleModal} onClose={toggleModal} />
        </div>
    );
}