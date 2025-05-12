import { currency } from "../../utils/formatters";
import { defaultColors } from "../../utils/colors";
import { useEffect, useId } from "react";
import Chart from 'chart.js/auto';

export function BarChart({ legends = [], data = [], labels = [], colors }) {
    const uniqueId = useId();
    const chartId = `bar-chart-${uniqueId}`;

    useEffect(() => {
        const config = {
            type: 'bar',
            data: {
                datasets: [{
                    label: legends[0] || 'First',
                    data: data.length > 0 ? data[0] : [],
                    backgroundColor: colors ?? defaultColors,
                }, {
                    label: legends[1] || 'Second',
                    data: data.length > 1 ? data[1] : [],
                    backgroundColor: colors?.map((color) => color + '80') ?? defaultColors.map((color) => color + '80'),
                }],
                labels
            },
            options: {
                responsive: true,
                plugins: {
                    tooltip: {
                        callbacks: ({
                            label: function (context) {
                                const value = context.raw || 0;
                                return currency(value);
                            }
                        })
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