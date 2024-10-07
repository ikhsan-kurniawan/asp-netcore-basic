$(function () {
    $.ajax({
        type: "GET",
        url: "https://localhost:7180/accounts/education",
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        headers: {
            "Authorization": "Bearer " + localStorage.getItem("userToken")
        },
    }).done(function (hasil) {
        dataEducation = hasil.data;

        //- DONUT CHART -
        var donutChartCanvas = $('#donutChart').get(0).getContext('2d')
        var donutData = {
            labels: Object.keys(dataEducation),
            datasets: [
                {
                    data: Object.values(dataEducation),
                    backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef'],
                }
            ]
        }
        var donutOptions = {
            maintainAspectRatio: false,
            responsive: true,
        }

        new Chart(donutChartCanvas, {
            type: 'doughnut',
            data: donutData,
            options: donutOptions
        })

        //- BAR CHART -
        var barChartCanvas = $('#barChart').get(0).getContext('2d');

        var barChartData = {
            labels: Object.keys(dataEducation), // Use the same labels for consistency
            datasets: [
                {
                    label: 'Education Data',
                    data: Object.values(dataEducation), // Ensure you are using the right data here
                    backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef'],
                }
            ]
        };

        var barChartOptions = {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero:true,
                    }
                }]
            }
        };

        new Chart(barChartCanvas, {
            type: 'bar',
            data: barChartData,
            options: barChartOptions
        });

    }).fail(function (jqXHR) {
        console.log(jqXHR.responseJSON.message);
    });    


    $.ajax({
        type: "GET",
        url: "https://localhost:7180/accounts/",
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        headers: {
            "Authorization": "Bearer " + localStorage.getItem("userToken")
        }
    }).done(function (hasil) {
        const label = ['D3', 'D4', 'S1', 'S2','S3'];
        const degrees = hasil.data.map(item => item.degree);

        const orderedDegrees = label.map(degree => {
            return degrees.filter(d => d === degree).length
        })


        //- DONUT CHART -
        var donutChartCanvas = $('#donutChart2').get(0).getContext('2d')
        var donutData = {
            labels: label,
            datasets: [
                {
                    data: orderedDegrees,
                    backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef'],
                }
            ]
        }
        var donutOptions = {
            maintainAspectRatio: false,
            responsive: true,
        }

        new Chart(donutChartCanvas, {
            type: 'doughnut',
            data: donutData,
            options: donutOptions
        })

        //- BAR CHART -
        var barChartCanvas = $('#barChart2').get(0).getContext('2d');

        var barChartData = {
            labels: label, // Use the same labels for consistency
            datasets: [
                {
                    label: 'Education Data',
                    data: orderedDegrees, // Ensure you are using the right data here
                    backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef'],
                }
            ]
        };

        var barChartOptions = {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true,
                    }
                }]
            }
        };

        new Chart(barChartCanvas, {
            type: 'bar',
            data: barChartData,
            options: barChartOptions
        });

    }).fail(function (jqXHR) {
        console.log(jqXHR.responseJSON.message);
    });    

  })


