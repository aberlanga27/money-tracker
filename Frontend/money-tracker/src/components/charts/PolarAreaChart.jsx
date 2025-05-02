import { currency } from "../../utils/formatters";
import { useEffect, useState } from "react";
import ReactApexChart from "react-apexcharts";

export function PolarAreaChart({ data = [], labels = [] }) {
    const [options, setOptions] = useState({
        chart: {
            type: 'polarArea'
        },
        labels,
        stroke: {
            colors: ['#fff']
        },
        fill: {
            opacity: 0.8
        },
        legend: {
            show: true,
            position: 'bottom'
        },
        tooltip: {
            y: {
                formatter: currency
            }
        },
        responsive: [{
            breakpoint: 2000,
            options: {
                chart: {
                    width: 400
                }
            }
        }]
    });

    useEffect(() => {
        setOptions((prevState) => ({
            ...prevState,
            labels,
            legend: {
                ...prevState.legend,
                show: labels.length > 0
            }
        }));
    }, [labels]);

    return (
        <ReactApexChart id="apex-chart-polar" type="polarArea" options={options} series={data} />
    );
}