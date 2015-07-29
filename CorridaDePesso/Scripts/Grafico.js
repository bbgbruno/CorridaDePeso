function CarregaGrafico(resultado) {
    var index;
    var categorias = [];
    var series = [];
    for (index in resultado.Data) {
        series.push({ name: resultado.categories[index], data: resultado.Data[index].Valor });
        categorias.push(resultado.Data[index].Chave);
        
    }

    $('#GraficoDePeso').InnerHTML = "";
    $('#GraficoDePeso').highcharts({
        chart: {
            type: 'spline'
        },
        title: {
            text: 'Resultado Parcial Da Competição de Perda de Peso',
            zoomType: 'xy'
        },
        subtitle: {
            text: 'Corredor de Peso'
        },
        xAxis: {
            categories: resultado.Data[0].Chave,
            showLastLabel: true,
            
        },
        yAxis:[ {
            title: {
                text: 'Pesagem Semanal em Kg'
            },
            labels: {
                formatter: function () {
                    return this.value + 'kg';
                },
            },
        },
        {
            title: {
                text: 'Peso perdido em Kg'
            },
   
            min:0,
            labels: {
                formatter: function () {
                    return this.value + 'kg';
                }

            },
           
            minRange:20
                
        },
        ],
        tooltip: {
            crosshairs: true,
            shared: true
        },
        plotOptions: {
            spline: {
                marker: {
                    radius: 4,
                    lineColor: '#666666',
                    lineWidth: 1
                }
            }
        },
        series: series

    });

}

function RankingPerdaDePeso(resultado) {
    var TextoTitulo = 'Ranking de Perda de Peso';
    $('#GraficoDePeso').InnerHTML = "";
    $('#GraficoDePeso').highcharts({
        chart: {
            type: 'column'
        },
        credits: {
            enabled: 0
        },
        title: {
            text: TextoTitulo
        },
        subtitle: {
            text: 'Corrida de Peso'
        },
        xAxis: {
            categories: resultado.Data.Chave
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Peso Perdido (Kg)'
            },
          },
        tooltip: {
            headerFormat: '<span style="font-size:10px">{point.key}</span><br />',
            pointFormat: '<span style="color:{series.color};padding:0">Peso: </span>' +
                '<span style="padding:0"><b>{point.y:.2f} kg</b></span>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0,
                stacking: 'percent'
            },
            series: {
                cursor: 'pointer',
                point: {
                    events: {
                        click: function () {
                            if (TipoGrafico == "GERAL")
                                CarregaChartClickColunaVendaUltimoAno(this.category);
                        }
                    }
                }
            }
        },
        legend: {
            enabled: false
        },
        series: [
            {
                data: resultado.Data.Dado,
                color:'blue'
            },
            {
                data: resultado.Data.Valor,
                color:'Green'
            }
        ]

    });
}


$(document).ready(function () {

  
    $.get("/Corredor/GetCorredorPeso", {}, RankingPerdaDePeso, 'json');

    $(".LinkCorredor").on('click', function (e) {
        var id = $(this).attr('id');
        $.get("/Corredor/GetPesagemCorredorGeral/"+id, {}, CarregaGrafico, 'json');
    });

});



