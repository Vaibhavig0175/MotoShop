const revenueCtx = document.getElementById('revenueChart');

if (revenueCtx) {

    new Chart(revenueCtx, {

        type: 'line',

        data: {

            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],

            datasets: [{

                label: 'Revenue',

                data: [15, 20, 30, 22, 45, 60],

                borderWidth: 3,

                fill: false,

                tension: .4

            }]

        }

    });

}

const auctionCtx = document.getElementById('auctionChart');

if (auctionCtx) {

    new Chart(auctionCtx, {

        type: 'doughnut',

        data: {

            labels: ['Live', 'Completed', 'Scheduled'],

            datasets: [{

                data: [12, 28, 6]

            }]

        }

    });

}