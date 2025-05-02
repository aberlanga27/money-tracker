import { currency } from "../utils/formatters";

export function InfoCard({ title, value }) {
    return (
        <div className="grid grid-rows-2 bg-secondary/30 px-3 py-2 rounded-lg">
            <h2 className="text-md font-bold text-gray-900">{title}</h2>
            <span className="text-md font-bold bg-white text-primary p-1 rounded">{currency(value)}</span>
        </div>
    );
}