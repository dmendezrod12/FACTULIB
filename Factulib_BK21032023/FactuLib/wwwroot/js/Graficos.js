import { Chart } from "./chart";

var controlBar = document.getElementById("myBarChart");

var graficoBar = new Chart(controlBar, {
    type: 'bar',
    data: {
        labels: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio"],
        datasets: [
            {
                label: "reporte",
                backgroundColor: "#BB2100",
                data: [4215, 5312, 6251, 7841, 9821, 14984],
            }
        ]
    },
    options: {
        scales: {
            xAxes: [{
                gridLines: {
                    display: true
                }
            }],
            yAxes: [{
                gridLines: {
                    display: true
                }
            }]
        }
    }
})