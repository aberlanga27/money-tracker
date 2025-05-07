import { useEffect, useId } from "react";
import Chart from 'chart.js/auto';
import { currency } from "../../utils/formatters";

const defaultColors = [
    '#FF6384',
    '#36A2EB',
    '#FFCE56',
    '#4BC0C0',
    '#9966FF',
    '#FF9F40',
];

export function DoughnutChart({ data = [], labels = [], colors }) {
    const uniqueId = useId();
    const chartId = `doughnut-chart-${uniqueId}`;

    useEffect(() => {
        const config = {
            type: 'doughnut',
            data: {
                datasets: [{
                    data,
                    backgroundColor: colors ?? defaultColors,
                }],
                labels
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        position: 'top',
                    },
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                const label = context.label || '';
                                const value = context.raw || 0;
                                const total = context.dataset.data.reduce((a, b) => a + b, 0);
                                const percentage = ((value / total) * 100).toFixed(2);
                                return `${label}: ${currency(value)} (${percentage}%)`;
                            }
                        }
                    }
                }  
            }
        };

        const ctx = document.getElementById(chartId).getContext('2d');
        const myChart = new Chart(ctx, config);
        return () => {
            myChart.destroy();
        };

    }, [data, labels, colors, chartId]);

    return (
        <canvas id={chartId}></canvas>
    );
}