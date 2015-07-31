/*
$(".telefone").unmask();
$(".telefone").mask("(99)9999-9999");

$(".data").unmask();
$(".data").mask("99/99/9999");

$(".diaMes").unmask();
$(".diaMes").mask("99/99");

$(".mesAno").unmask();
$(".mesAno").mask("99/9999");

$(".CPF").unmask();
$(".CPF").mask("999.999.999-99");

$(".CNPJ").unmask();
$(".CNPJ").mask("99.999.999/9999-99");

$(".CEP").unmask();
$(".CEP").mask("99999-999");

$(".placa").unmask();
$(".placa").mask("aaa-9999");

$(".ano").unmask();
$(".ano").mask("9999");

$(".2numeros").unmask();
$(".2numeros").mask("99");

*/

$(document).ready(function () {
    $(".telefone").inputmask("(99)9999-9999");
    $(".data").inputmask("99/99/9999");
    $(".diaMes").inputmask("99/99");
    $(".mesAno").inputmask("99/9999");
    $(".CPF").inputmask("999.999.999-99");
    $(".CNPJ").inputmask("99.999.999/9999-99");
    $(".CEP").inputmask("99999-999");
    $(".placa").inputmask("aaa-9999");
    $(".ano").inputmask("9999");
    $(".2numeros").inputmask("99");
    $(".UF").inputmask("aa");
    $(".mes").inputmask("99");

    $(".numero").maskMoney({ showSymbol: false, symbol: "", decimal: ",", thousands: "", precision: 0, allowZero: true });
    $(".numero2Decimais").maskMoney({ showSymbol: false, symbol: "", decimal: ",", thousands: "", precision: 2, allowZero: true });
    $(".numero3Decimais").maskMoney({ showSymbol: false, symbol: "", decimal: ",", thousands: "", precision: 3, allowZero: true });
    $(".numero4Decimais").maskMoney({ showSymbol: false, symbol: "", decimal: ",", thousands: "", precision: 4, allowZero: true });

    $(".moeda-real input[type='text']").maskMoney({ showSymbol: true, symbol: "", decimal: ",", thousands: "", allowZero: true });
});
