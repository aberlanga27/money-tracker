import { useContext, useState } from "react";
import { BaseModal } from "./BaseModal";
import { TransactionsContext } from "../../context/transactions";


export function TransactionModal({ show, onClose, onOk }) {
    const { addTransaction } = useContext(TransactionsContext)

    const [description, setDescription] = useState("");
    const [amount, setAmount] = useState("");
    const [date, setDate] = useState("");
    const [account, setAccount] = useState("");
    const [category, setCategory] = useState("");

    const handleAddTransaction = () => {
        if (description === "" || amount === "" || date === "" || account === "" || category === "") {
            alert("Please fill in all fields");
            return;
        }

        addTransaction({
            date,
            category,
            account,
            description,
            amount: parseFloat(amount),
        });

        if (onOk) onOk();
    };

    return (
        <BaseModal
            show={show} onOk={handleAddTransaction} onClose={onClose}
            title="Add Transaction" okLegend="Add"
        >
            <div className="flex flex-col gap-2">
                <input
                    type="text"
                    placeholder="Description"
                    className="p-2 border border-gray-300 rounded-lg"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                />

                <input
                    type="number"
                    placeholder="Amount"
                    className="p-2 border border-gray-300 rounded-lg"
                    value={amount}
                    onChange={(e) => setAmount(e.target.value)}
                />

                <input
                    type="date"
                    placeholder="Date"
                    className="p-2 border border-gray-300 rounded-lg"
                    value={date}
                    onChange={(e) => setDate(e.target.value)}
                />

                <select
                    className="p-2 border border-gray-300 rounded-lg"
                    value={account}
                    onChange={(e) => setAccount(e.target.value)}
                >
                    <option value="">Select Account</option>
                    <option value="1">Account 1</option>
                    <option value="2">Account 2</option>
                    <option value="3">Account 3</option>
                </select>

                <select
                    className="p-2 border border-gray-300 rounded-lg"
                    value={category}
                    onChange={(e) => setCategory(e.target.value)}
                >
                    <option value="">Select Category</option>
                    <option value="1">Category 1</option>
                    <option value="2">Category 2</option>
                    <option value="3">Category 3</option>
                </select>
            </div>
        </BaseModal>
    );
}