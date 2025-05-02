import './TransactionTable.css';
import { currency, date } from "../../utils/formatters";
import { PlainButton } from '../common/Button';

export function TransactionTable({ transactions }) {
    return (
        <div id="transaction-table" className='text-sm'>
            <table className="w-full table-auto bg-secondary/30 rounded-lg">
                <thead>
                    <tr className="bg-primary text-white">
                        <th className="p-2">Date</th>
                        <th className="p-2">Bank</th>
                        <th className="p-2">Category</th>
                        <th className="p-2">Description</th>
                        <th className="p-2">Amount</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        transactions.map((expense, index) => (
                            <tr key={index} className="border-b border-gray-300">
                                <td className="p-2">{date(expense.transactionDate)}</td>
                                <td className="p-2">{expense.bankName}</td>
                                <td className="p-2">{expense.transactionCategoryName}</td>
                                <td className="p-2">{expense.transactionDescription}</td>
                                <td className="p-2">{currency(expense.transactionAmount)}</td>
                            </tr>
                        ))
                    }
                </tbody>
            </table>

            <div className="flex justify-end mt-2">
                <PlainButton className='px-4 py-2'>
                    All transactions
                </PlainButton>
            </div>
        </div>
    );
}