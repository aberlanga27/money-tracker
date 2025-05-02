import { useContext, useEffect, useState } from "react";
import { BaseModal } from "./BaseModal";
import { TransactionsContext } from "../../context/transactions";
import { api } from "../../boot/axios";

export function TransactionModal({ show, onClose, onOk }) {
    const { addTransaction } = useContext(TransactionsContext)

    const [categories, setCategories] = useState([]);
    const [banks, setBanks] = useState([]);

    const [description, setDescription] = useState("");
    const [amount, setAmount] = useState("");
    const [date, setDate] = useState("");
    const [bank, setBank] = useState("");
    const [category, setCategory] = useState("");

    const handleAddTransaction = () => {
        if (description === "" || amount === "" || date === "" || bank === "" || category === "") {
            alert("Please fill in all fields");
            return;
        }

        addTransaction({
            transactionDate: date,
            // transactionCategoryId: category.transactionCategoryId,
            transactionCategoryName: category,
            // bankId: bank.bankId,
            bankName: bank,
            transactionTypeId: 2,
            transactionTypeName: "Expense",
            transactionDescription: description,
            transactionAmount: parseFloat(amount),
        });

        if (onOk) onOk();
    };

    useEffect(() => {
        api.get('/Bank')
            .then(({ data }) => {
                console.log(data.response)
                setBanks(data.response)
            })
            .catch((error) => console.error("Error fetching banks:", error));

        api.get('/TransactionCategory')
            .then(({ data }) => {
                console.log(data.response)
                setCategories(data.response)
            })
            .catch((error) => console.error("Error fetching categories:", error));
    }, [])

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
                    value={bank}
                    onChange={(e) => setBank(e.target.value)}
                >
                    <option value="">Select Bank</option>
                    {
                        banks.map((bank) => (
                            <option key={bank.bankId} value={bank.bankId}>
                                {bank.bankName}
                            </option>
                        ))
                    }
                </select>

                <select
                    className="p-2 border border-gray-300 rounded-lg"
                    value={category}
                    onChange={(e) => setCategory(e.target.value)}
                >
                    <option value="">Select Category</option>
                    {
                        categories.map((category) => (
                            <option key={category.transactionCategoryId} value={category.transactionCategoryId}>
                                {category.transactionCategoryName}
                            </option>
                        ))
                    }
                </select>
            </div>
        </BaseModal>
    );
}