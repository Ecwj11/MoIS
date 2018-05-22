/*****Ready function start*****/
$(document).ready(function () {
    if ($('#chart_6').length > 0) {
        var ctx6 = document.getElementById("chart_6").getContext("2d");
        var data6 = {
            labels: [
                "organic",
                "Referral",
                "Other"
            ],
            datasets: [
                {
                    data: [200, 50, 250],
                    backgroundColor: [
                        "#2879ff",
                        "#01c853",
                        "#fec107",
                    ],
                    hoverBackgroundColor: [
                        "#2879ff",
                        "#01c853",
                        "#fec107",
                    ]
                }]
        };

        var pieChart = new Chart(ctx6, {
            type: 'pie',
            data: data6,
            options: {
                animation: {
                    duration: 3000
                },
                responsive: true,
                maintainAspectRatio: false,
                legend: {
                    display: false
                },
                tooltip: {
                    backgroundColor: 'rgba(33,33,33,1)',
                    cornerRadius: 0,
                    footerFontFamily: "'Roboto'"
                },
                elements: {
                    arc: {
                        borderWidth: 0
                    }
                }
            }
        });
    }
});