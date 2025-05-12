import { useCallback, useEffect, useState } from "react";
import { notifyError } from "../utils/notify";
import { api } from "../boot/axios";

export function useTransactionsPerBank() {
    const [transactionsPerBank, setTransactionsPerBank] = useState([])
    const [data, setData] = useState([])
    const [labels, setLabels] = useState([])

    const getTransactionsPerBank = useCallback(() => {        
        const today = new Date()
        const startDate = new Date(Date.UTC(today.getFullYear(), today.getMonth(), 1, 0, 0, 0))
        const endDate = new Date(Date.UTC(today.getFullYear(), today.getMonth() + 1, 0, 23, 59, 59))

        api.post('/Transaction/GroupByBank', {
            startDate: startDate,
            endDate: endDate
        })
            .then(({ data }) => {
                setTransactionsPerBank(data.response)
            })
            .catch(error => notifyError({ message: 'Error fetching transactions by bank:', error }))
    }, [setTransactionsPerBank])

    useEffect(() => {
        getTransactionsPerBank()
    }, [getTransactionsPerBank])

    useEffect(() => {
        setData(transactionsPerBank.map((bank) => bank.totalAmount))
        setLabels(transactionsPerBank.map((bank) => bank.bankName))
    }, [transactionsPerBank])

    return {
        transactionsPerBankData: data,
        transactionsPerBankLabels: labels
    }
}