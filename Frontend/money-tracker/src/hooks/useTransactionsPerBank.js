import { api } from "../boot/axios";
import { getDatesByBudgetType } from "../utils/date";
import { notifyError } from "../utils/notify";
import { useCallback, useEffect, useState } from "react";

export function useTransactionsPerBank({ budgetTypeId }) {
    const [transactionsPerBank, setTransactionsPerBank] = useState([])
    const [data, setData] = useState([])
    const [labels, setLabels] = useState([])

    const getTransactionsPerBank = useCallback(() => {        
        const { startDate, endDate } = getDatesByBudgetType({ budgetTypeId })

        api.post('/Transaction/GroupByBank', {
            startDate: startDate,
            endDate: endDate
        })
            .then(({ data }) => {
                setTransactionsPerBank(data.response)
            })
            .catch(error => notifyError({ message: error }))
    }, [setTransactionsPerBank, budgetTypeId])

    useEffect(() => {
        getTransactionsPerBank()
    }, [getTransactionsPerBank])

    useEffect(() => {
        setData(transactionsPerBank.map((bank) => bank.totalAmount))
        setLabels(transactionsPerBank.map((bank) => bank.bankName))
    }, [transactionsPerBank])

    return {
        transactionsPerBankData: data,
        transactionsPerBankLabels: labels,
        getTransactionsPerBank
    }
}