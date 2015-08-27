function CarregaGrafico(resultado) {
    var index;
    var categorias = [];
    var series = [];
    categorias.push(resultado.Data[0].Chave);
    for (index in resultado.Data) {
        series.push({ name: resultado.categories[index], data: resultado.Data[index].Valor });
    }

    $('#GraficoDePeso').highcharts({
        chart: {
            type: 'spline'
        },
        title: {
            text: 'Evolução do participante',
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
            pointFormat:  '<span style="color:{series.color};padding:0">Peso: </span>' +
                          '<span style="padding:0"><b>{point.y:.2f} kg</b></span>',
            shared: true,
            useHTML: true
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0

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

function CarregaListaPesagem(resultado) {
    console.log(resultado);
    $('#pesagem').html(resultado)
}


$(document).ready(function () {

    var corridaid = $("#Corrida_Id").val()
    console.log(corridaid);
    $.get("/DashboardCorrida/GetCorredorPeso/" + corridaid, {}, RankingPerdaDePeso, 'json');

    $(".LinkCorredor").on('click', function (e) {
        var corredorId = $(this).attr('id');
        var corridaId = $(this).attr('Value');
        $.get("/DashboardCorrida/GetPesagemCorredorGeral/", { id: corredorId, corridaId: corridaId }, CarregaGrafico, 'json');

        $.get("/Pesagem/ListarPesagem/" + corredorId, {}, CarregaListaPesagem, 'html');
    });

});



