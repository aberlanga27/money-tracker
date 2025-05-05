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
        colors: ['#008FFB', '#00E396', '#FEB019', '#FF4560', '#775DD0', '#546E7A', '#26a69a', '#D10CE8', '#FF9800', '#FF5722', '#00BCD4', '#607D8B'],
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
                    width: 350
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