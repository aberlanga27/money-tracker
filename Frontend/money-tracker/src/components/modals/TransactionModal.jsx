import { useEffect, useState } from "react";
import { BaseModal } from "./BaseModal";
import { IndexedSelect } from "../common/IndexedSelect";
import { api } from "../../boot/axios";
import { notifyWarning } from "../../utils/notify";
import { useTransactions } from "../../hooks/useTransactions";

export function TransactionModal({ show, onClose, onOk }) {
    const { addTransaction } = useTransactions()

    const [description, setDescription] = useState("");
    const [amount, setAmount] = useState("");
    const [date, setDate] = useState("");
    const [bank, setBank] = useState(null);
    const [category, setCategory] = useState(null);

    const handleAddTransaction = async () => {
        if (description === "" || amount === "" || date === "" || bank === "" || category === "") {
            notifyWarning({ message: "Please fill all fields" });
            return;
        }

        let transactionPayload = {
            transactionCategoryId: category.transactionCategoryId,
            transactionCategoryName: category.transactionCategoryName,
            transactionTypeId: 2,
            transactionTypeName: "Expense",
            bankId: bank.bankId,
            bankName: bank.bankName,
            transactionAmount: parseFloat(amount),
            transactionDate: date,
            transactionDescription: description,
        }

        const { data } = await api.post("/Transaction", transactionPayload);
        addTransaction({ ...transactionPayload, transactionId: data.response.transactionId });

        if (onOk) onOk();
    };

    useEffect(() => {
        return () => {
            setDescription("");
            setAmount("");
            setDate("");
            setBank(null);
            setCategory(null);
        }
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

                <IndexedSelect
                    endpoint={'Bank'} label={'Banks'} optionLabel={'bankName'} optionValue={'bankId'}
                    onChange={({ option }) => setBank(option)}
                />

                <IndexedSelect
                    endpoint={'TransactionCategory'} label={'Categories'} optionLabel={'transactionCategoryName'} optionValue={'transactionCategoryId'}
                    onChange={({ option }) => setCategory(option)}
                />
            </div>
        </BaseModal>
    );
}