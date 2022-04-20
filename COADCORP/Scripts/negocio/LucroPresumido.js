appModule.controller('LucroPresumidoController', function ($scope, $http) {

    $scope.dataLucroPresumido = {
        "listaAtividade": [
            { "id": 1, "nome": "Grupo 1" },
            { "id": 2, "nome": "Grupo 2" },
            { "id": 3, "nome": "Grupo 3" },
            { "id": 4, "nome": "Grupo 4" }
        ],
        "listaTrimestreAux":
            {
                "id": 1,
                "nome": "1º Trimestre",
                "meses": [
                    { "id": 1, "valor": "Janeiro" },
                    { "id": 2, "valor": "Fevereiro" },
                    { "id": 3, "valor": "Março" }
                ]
            },
        "listaTrimestre": [
            {
                "id": 1,
                "nome": "1º Trimestre",
                "meses": [
                    { "id": 1, "valor": "Janeiro" },
                    { "id": 2, "valor": "Fevereiro" },
                    { "id": 3, "valor": "Março" }
                ]
            },
            {
                "id": 2,
                "nome": "2º Trimestre",
                "meses": [
                    { "id": 4, "valor": "Abril" },
                    { "id": 5, "valor": "Maio" },
                    { "id": 6, "valor": "Junho" }
                ]
            },
            {
                "id": 3,
                "nome": "3º Trimestre",
                "meses": [
                    { "id": 7, "valor": "Julho" },
                    { "id": 8, "valor": "Agosto" },
                    { "id": 9, "valor": "Setembro" }
                ]
            },
            {
                "id": 4,
                "nome": "4º Trimestre",
                "meses": [
                    { "id": 10, "valor": "Outubro" },
                    { "id": 11, "valor": "Novembro" },
                    { "id": 12, "valor": "Dezembro" }
                ]
            }
        ]
    };

    $scope.lucroPresumidoModel = {
        "atividade": { "id": 1, "nome": "Grupo 1" },
        "trimestre": {
            "id": 1,
            "nome": "1º Trimestre",
            "meses": [
                { "id": 1, "valor": "Janeiro" }
            ]
        },
        "receitaMercadoInterno": [
            { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
        ],
        "receitaExportacao": [
            { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
        ],
        "totalFaturamento": [
            {"valor": 0 }, {"valor": 0 }, {"valor": 0 }
        ],
        "vendasCanceladasDescontosConcedidos": [
            { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
        ],
        "totalReceitaBruta": [
            { "valor": 0 }, { "valor": 0 }, { "valor": 0 }
        ],
        "receitasFinanceirasRendimentoAplicacoesEmRendaFixa": [
            { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
        ],
        "recuperacaoDePerdasNoRecebimentoDeCredito": [
            { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
        ],
        "ganhoCapital": [
            { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
        ],
        "irrfSemReceitasFinanceiras": [
            { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
        ],
        "irrfServicoPj": [
            { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
        ],
        "csllRetidoPj": [
            { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
        ],
        "pisRetidoServicosPj": [
            { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
        ],
        "cofinsRetidoPj": [
            { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
        ]
    };

    $scope.lucroPresumido = {
        "templateDOM": {
            "listaAtividade": [
                { "id": 1, "nome": "Grupo 1" },
                { "id": 2, "nome": "Grupo 2" },
                { "id": 3, "nome": "Grupo 3" },
                { "id": 4, "nome": "Grupo 4" }
            ],
            "listaTrimestreAux":
            {
                "id": 1,
                "nome": "1º Trimestre",
                "meses": [
                    { "id": 1, "valor": "Janeiro" },
                    { "id": 2, "valor": "Fevereiro" },
                    { "id": 3, "valor": "Março" }
                ]
            },
            "listaTrimestre": [
                {
                    "id": 1,
                    "nome": "1º Trimestre",
                    "meses": [
                        { "id": 1, "valor": "Janeiro" },
                        { "id": 2, "valor": "Fevereiro" },
                        { "id": 3, "valor": "Março" }
                    ]
                },
                {
                    "id": 2,
                    "nome": "2º Trimestre",
                    "meses": [
                        { "id": 4, "valor": "Abril" },
                        { "id": 5, "valor": "Maio" },
                        { "id": 6, "valor": "Junho" }
                    ]
                },
                {
                    "id": 3,
                    "nome": "3º Trimestre",
                    "meses": [
                        { "id": 7, "valor": "Julho" },
                        { "id": 8, "valor": "Agosto" },
                        { "id": 9, "valor": "Setembro" }
                    ]
                },
                {
                    "id": 4,
                    "nome": "4º Trimestre",
                    "meses": [
                        { "id": 10, "valor": "Outubro" },
                        { "id": 11, "valor": "Novembro" },
                        { "id": 12, "valor": "Dezembro" }
                    ]
                }
            ]
        },
        "model": {
            "atividade": { "id": 1, "nome": "Grupo 1" },
            "trimestre": {
                "id": 1,
                "nome": "1º Trimestre",
                "meses": [
                    { "id": 1, "valor": "Janeiro" }

                ]
            },
            "receitaMercadoInterno": [
                { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
            ],
            "receitaExportacao": [
                { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
            ],
            "totalFaturamento": [
                { "valor": 0 }, { "valor": 0 }, { "valor": 0 }
            ],
            "vendasCanceladasDescontosConcedidos": [
                { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
            ],
            "totalReceitaBruta": [
                { "valor": 0 }, { "valor": 0 }, { "valor": 0 }
            ],
            "receitasFinanceirasRendimentoAplicacoesEmRendaFixa": [
                { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
            ],
            "recuperacaoDePerdasNoRecebimentoDeCredito": [
                { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
            ],
            "ganhoCapital": [
                { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
            ],
            "irrfSemReceitasFinanceiras": [
                { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
            ],
            "irrfServicoPj": [
                { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
            ],
            "csllRetidoPj": [
                { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
            ],
            "pisRetidoServicosPj": [
                { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
            ],
            "cofinsRetidoPj": [
                { "id": 1, "valor": 0 }, { "id": 2, "valor": 0 }, { "id": 3, "valor": 0 }
            ]
            ,"baseCalcPisCofinsPriMesTri": 0
            ,"baseCalcPisCofinsSegMesTri": 0
            ,"baseCalcPisCofinsTerMesTri": 0
            ,"pisDevNoMesPriMesTri": 0
            ,"pisDevNoMesSegMesTri": 0
            , "pisDevNoMesTerMesTri": 0
            ,"compPisRetPriMesTri":0
            ,"compPisRetSegMesTri":0
            , "compPisRetTerMesTri": 0
            , "pisRecPriMesTri": 0
            , "pisRecSegMesTri": 0
            , "pisRecTerMesTri": 0
            , "cofinsDevPriMesTri": 0
            , "cofinsDevSegMesTri": 0
            , "cofinsDevTerMesTri": 0
            , "compCofinsRetidoPriMesTri": 0
            , "compCofinsRetidoSegMesTri": 0
            , "compCofinsRetidoTerMesTri": 0
            , "cofinsRecPriMesTri": 0
            , "cofinsRecSegMesTri": 0
            , "cofinsRecTerMesTri": 0
            , "baseCalculoLucroPresumidoPorAtividade": 0
            ,"baseCalculoLucroPresumidoAcordoAtividadeCsll":0

        }
    };

    $scope.lucroPresumidoLista = [];




    //Início - Variáveis do esultado do CALCULO PIS/COFINS (APURAÇÃO MENSAL)
    //Objeto de entrada de cada grupo
    $scope.baseCalcPisCofinsPriMesTriList = [];
    $scope.baseCalcPisCofinsSegMesTriList = [];
    $scope.baseCalcPisCofinsTerMesTriList = [];

    $scope.pisDevNoMesPriMesTriList = [];
    $scope.pisDevNoMesSegMesTriList = [];
    $scope.pisDevNoMesTerMesTriList = [];

    $scope.compPisRetPriMesTriList = [];
    $scope.compPisRetSegMesTriList = [];
    $scope.compPisRetTerMesTriList = [];

    $scope.pisRecPriMesTriList = [];
    $scope.pisRecSegMesTriList = [];
    $scope.pisRecTerMesTriList = [];

    $scope.cofinsDevPriMesTriList = [];
    $scope.cofinsDevSegMesTriList = [];
    $scope.cofinsDevTerMesTriList = [];

    $scope.compCofinsRetidoPriMesTriList = [];
    $scope.compCofinsRetidoSegMesTriList = [];
    $scope.compCofinsRetidoTerMesTriList = [];

    $scope.cofinsRecPriMesTriList = [];
    $scope.cofinsRecSegMesTriList = [];
    $scope.cofinsRecTerMesTriList = [];
    //Objeto de entrada de cada grupo

    //Resultado geral
    $scope.baseCalcPisCofinsPriMesTriResult = [];
    $scope.baseCalcPisCofinsSegMesTriResult = [];
    $scope.baseCalcPisCofinsTerMesTriResult = [];

    $scope.pisDevNoMesPriMesTriResult = 0;
    $scope.pisDevNoMesSegMesTriResult = 0;
    $scope.pisDevNoMesTerMesTriResult = 0;

    $scope.compPisRetPriMesTriResult = 0;
    $scope.compPisRetSegMesTriResult = 0;
    $scope.compPisRetTerMesTriResult = 0;

    $scope.pisRecPriMesTriResult = 0;
    $scope.pisRecSegMesTriResult = 0;
    $scope.pisRecTerMesTriResult = 0;

    $scope.cofinsDevPriMesTriResult = 0;
    $scope.cofinsDevSegMesTriResult = 0;
    $scope.cofinsDevTerMesTriResult = 0;

    $scope.compCofinsRetidoPriMesTriResult = 0;
    $scope.compCofinsRetidoSegMesTriResult = 0;
    $scope.compCofinsRetidoTerMesTriResult = 0;

    $scope.cofinsRecPriMesTriResult = 0;
    $scope.cofinsRecSegMesTriResult = 0;
    $scope.cofinsRecTerMesTriResult = 0;
    //Resultado geral

    //Fim - Variáveis do esultado do CALCULO PIS/COFINS (APURAÇÃO MENSAL)

    //Inicio - CALCULO IRPJ (APURAÇÃO TRIMESTRAL)

    $scope.baseCalculoLucroPresumidoPorAtividadeList = [];
    $scope.demaisReceitasList = [];

    $scope.baseCalculoLucroPresumidoPorAtividadeResult = [];
    $scope.demaisReceitasResult = 0;
    $scope.baseCalculoIrpjResult = 0;
    $scope.irpjQuinzePorCentoResult = 0;
    $scope.limiteAdicionalResult = 0;
    $scope.baseCalculoAdicionalResult = 0;
    $scope.adicionalDeIrpjResult = 0;
    $scope.irpjDevidoResult = 0;
    $scope.compensacaoIrRetidoResult = 0;
    $scope.irRecolherResult = 0;
    $scope.cotaUnicaResult = 0;
    $scope.primeiraCotaResult = 0;
    $scope.segundaCotaResult = 0;
    $scope.terceiraCotaResult = 0;

    //Fim - CALCULO IRPJ (APURAÇÃO TRIMESTRAL)

    //Inicio - CALCULO CSLL (APURAÇÃO TRIMESTRAL)

    $scope.baseCalculoLucroPresumidoAcordoAtividadeCsllList = [];
    $scope.demaisReceitasCsllList = [];

    //Fim - CALCULO CSLL (APURAÇÃO TRIMESTRAL)

    $scope.tabelaCalculo = false;

    $scope.teste = function () {
        $('.money-mask').maskMoney('destroy');
        $('.money-mask').maskMoney({ thousands: '', decimal: '.' });
        $('.money-mask').maskMoney('mask');
    }

    $scope.init = function () {
        for (var i = 0; i < 1; i++) {
            var lucroPresumido = Object.assign($scope.lucroPresumido);
            var obj = new Object();
            var lucroPresumidoObj = new Object();
            //var lucroPresumidoObj = new Object();
            obj["id"] = i;
            lucroPresumidoObj["lucroPresumido"] = lucroPresumido;
            //obj["data"] = {"lucroPresumido": lucroPresumido };
            obj["data"] = lucroPresumidoObj;
            lucroPresumido["dataId"] = i;
            $scope.lucroPresumidoLista.push(obj);
        };
        console.log($scope.lucroPresumidoLista);
    };

    $scope.addTabLucroPresumido = function () {
        var lucroPresumido = Object.assign($scope.lucroPresumido);
        var obj = new Object();
        var lucroPresumidoObj = new Object();
        var i = $scope.lucroPresumidoLista.length;
        obj["id"] = i;
        lucroPresumidoObj["lucroPresumido"] = lucroPresumido;
        //obj["data"] = {"lucroPresumido": lucroPresumido };
        obj["data"] = lucroPresumidoObj;
        lucroPresumido["dataId"] = i;
        $scope.lucroPresumidoLista.push(obj);
        console.log($scope.lucroPresumidoLista);
    }

    $scope.limpaTabelaLucroPresumido = function () {
        $scope.lucroPresumidoLista = [];
        $scope.init();
    }

    $scope.calcularLucroPresumido = function () {

        if (!$scope.validacaoCamposCalculo()) {
            return false;
        }


       $scope.tabelaCalculo = true;

        $scope.calculaPisCofinsApuracaoMensal();

    };

    $scope.validacaoCamposCalculo = function () {
        for (var i = 0; i < $scope.lucroPresumidoLista.length; i++) {
            if ($scope.lucroPresumidoLista[i].data.model.atividade == null) {
                alert("'Atividade' não preenchida.");
                return false;
            };
        };

        if ($scope.lucroPresumidoLista[0].data.lucroPresumido.model.listaTrimestreAux == null) {
            alert("'Trimestre' não preenchido.");
            return false;
        };

        if ($scope.lucroPresumidoLista[0].data.lucroPresumido.model.trimestre.meses == null) {
            alert("'Mês de início' não preenchido.");
            return false;
        };
        return true;
    };

    $scope.calculaPisCofinsApuracaoMensal = function () {

        var calcIrpjTodasDemaisReceitas = 0;
        var calcIrpjTodasBaseCalcIrpj = 0;

        var calcCsllbaseCalculoCsll = 0;
        var calcCsllCsllRetidoPj = 0;

        angular.forEach($scope.lucroPresumidoLista, function (value, index) {

            $scope.pisCofinsZeraValueItensLista(index);

            if ($scope.lucroPresumidoLista[index].data.model.atividade.id != 2) {

                //$scope.lucroPresumidoLista[index].data.model.baseCalcPisCofinsPriMesTri = $scope.lucroPresumidoLista[index].data.model.totalReceitaBruta[0].valor - $scope.lucroPresumidoLista[index].data.model.receitaExportacao[0].valor;
                //$scope.lucroPresumidoLista[index].data.model.baseCalcPisCofinsSegMesTri = $scope.lucroPresumidoLista[index].data.model.totalReceitaBruta[1].valor - $scope.lucroPresumidoLista[index].data.model.receitaExportacao[1].valor;
                //$scope.lucroPresumidoLista[index].data.model.baseCalcPisCofinsTerMesTri = $scope.lucroPresumidoLista[index].data.model.totalReceitaBruta[2].valor - $scope.lucroPresumidoLista[index].data.model.receitaExportacao[2].valor;

                $scope.pisCofinsCalculaBaseCalculoApuracaoMensal(index);

                //$scope.lucroPresumidoLista[index].data.model.pisDevNoMesPriMesTri = $scope.lucroPresumidoLista[index].data.model.baseCalcPisCofinsPriMesTri * 0.0065;
                //$scope.lucroPresumidoLista[index].data.model.pisDevNoMesSegMesTri = $scope.lucroPresumidoLista[index].data.model.baseCalcPisCofinsSegMesTri * 0.0065;
                //$scope.lucroPresumidoLista[index].data.model.pisDevNoMesTerMesTri = $scope.lucroPresumidoLista[index].data.model.baseCalcPisCofinsTerMesTri * 0.0065;

                $scope.pisCofinsPisDevidoMes(index);

               //$scope.lucroPresumidoLista[index].data.model.compPisRetPriMesTri = $scope.lucroPresumidoLista[index].data.model.pisRetidoServicosPj[0].valor;
               //$scope.lucroPresumidoLista[index].data.model.compPisRetSegMesTri = $scope.lucroPresumidoLista[index].data.model.pisRetidoServicosPj[1].valor;
               //$scope.lucroPresumidoLista[index].data.model.compPisRetTerMesTri = $scope.lucroPresumidoLista[index].data.model.pisRetidoServicosPj[2].valor;

                $scope.pisCofinsCompensacaoPisRetido(index);

                //$scope.lucroPresumidoLista[index].data.model.pisRecPriMesTri = $scope.lucroPresumidoLista[index].data.model.pisDevNoMesPriMesTri - $scope.lucroPresumidoLista[index].data.model.compPisRetPriMesTri;
                //$scope.lucroPresumidoLista[index].data.model.pisRecSegMesTri = $scope.lucroPresumidoLista[index].data.model.pisDevNoMesSegMesTri - $scope.lucroPresumidoLista[index].data.model.compPisRetSegMesTri;
                //$scope.lucroPresumidoLista[index].data.model.pisRecTerMesTri = $scope.lucroPresumidoLista[index].data.model.pisDevNoMesTerMesTri - $scope.lucroPresumidoLista[index].data.model.compPisRetTerMesTri;

                $scope.pisCofinsPisRecolher(index);

                //$scope.lucroPresumidoLista[index].data.model.cofinsDevPriMesTri = $scope.lucroPresumidoLista[index].data.model.baseCalcPisCofinsPriMesTri * 0.03;
                //$scope.lucroPresumidoLista[index].data.model.cofinsDevSegMesTri = $scope.lucroPresumidoLista[index].data.model.baseCalcPisCofinsSegMesTri * 0.03;
                //$scope.lucroPresumidoLista[index].data.model.cofinsDevTerMesTri = $scope.lucroPresumidoLista[index].data.model.baseCalcPisCofinsTerMesTri * 0.03;

                $scope.pisCofinsCofinsDevido(index);

                //$scope.lucroPresumidoLista[index].data.model.compCofinsRetidoPriMesTri = $scope.lucroPresumidoLista[index].data.model.cofinsRetidoPj[0].valor;
                //$scope.lucroPresumidoLista[index].data.model.compCofinsRetidoSegMesTri = $scope.lucroPresumidoLista[index].data.model.cofinsRetidoPj[0].valor;
                //$scope.lucroPresumidoLista[index].data.model.compCofinsRetidoTerMesTri = $scope.lucroPresumidoLista[index].data.model.cofinsRetidoPj[0].valor;

                $scope.pisCofinsCompensacaoCofinsRetido(index);

                //$scope.lucroPresumidoLista[index].data.model.cofinsRecPriMesTri = $scope.lucroPresumidoLista[index].data.model.cofinsDevPriMesTri - $scope.lucroPresumidoLista[index].data.model.compCofinsRetidoPriMesTri;
                //$scope.lucroPresumidoLista[index].data.model.cofinsRecSegMesTri = $scope.lucroPresumidoLista[index].data.model.cofinsDevSegMesTri - $scope.lucroPresumidoLista[index].data.model.compCofinsRetidoSegMesTri;
                //$scope.lucroPresumidoLista[index].data.model.cofinsRecTerMesTri = $scope.lucroPresumidoLista[index].data.model.cofinsDevTerMesTri - $scope.lucroPresumidoLista[index].data.model.compCofinsRetidoTerMesTri;

                $scope.pisCofinsCofinsRecolher(index);
            };

            $scope.calculoIrpjZeraLista(index);
            $scope.calculoCsllZeraLista(index);

            $scope.irrpjBaseCalculoLucroPresumido(index);

            $scope.irpjDemaisReceitas(index);

            $scope.csllBaseCalculoLucroPresumido(index);
            $scope.csllDemaisReceitasCsll(index);
            /*
            $scope.baseCalculoIrpjResult = $scope.lucroPresumidoLista[index].data.model.baseCalculoLucroPresumidoPorAtividadeResult + $scope.baseCalculoIrpjResult;

            calcIrpjTodasCompIrRetido = $scope.lucroPresumidoLista[index].data.model.irrfSemReceitasFinanceiras[0].valor +
                $scope.lucroPresumidoLista[index].data.model.irrfSemReceitasFinanceiras[1].valor +
                $scope.lucroPresumidoLista[index].data.model.irrfSemReceitasFinanceiras[2].valor +
                $scope.lucroPresumidoLista[index].data.model.model.irrfServicoPj[0].valor +
                $scope.lucroPresumidoLista[index].data.model.model.irrfServicoPj[1].valor +
                $scope.lucroPresumidoLista[index].data.model.model.irrfServicoPj[2].valor + 
                calcIrpjTodasCompIrRetido;

            $scope.lucroPresumidoLista[index].data.model.baseCalculoLucroPresumidoAcordoAtividadeCsll = ($scope.lucroPresumidoLista[index].data.model.baseCalcPisCofinsPriMesTri + $scope.lucroPresumidoLista[index].data.model.baseCalcPisCofinsSegMesTri + $scope.lucroPresumidoLista[index].data.model.baseCalcPisCofinsTerMesTri) * formulaCalcCsll;

            calcCsllbaseCalculoCsll = $scope.lucroPresumidoLista[index].data.model.baseCalculoLucroPresumidoAcordoAtividadeCsll + calcCsllbaseCalculoCsll;

            calcCsllCsllRetidoPj = $scope.lucroPresumidoLista[index].data.model.csllRetidoPj[0].valor +
                $scope.lucroPresumidoLista[index].data.model.csllRetidoPj[1].valor +
                $scope.lucroPresumidoLista[index].data.model.csllRetidoPj[2].valor +
                calcCsllCsllRetidoPj;
            */
        });            

        //Variáveis de auxílio PIS - Cofins
        var baseCalcPisCofinsPriMesTriResultAux = 0;
        var baseCalcPisCofinsSegMesTriResultAux = 0;
        var baseCalcPisCofinsTerMesTriResultAux = 0;
        var pisDevNoMesPriMesTriResultAux = 0;
        var pisDevNoMesSegMesTriResultAux = 0;
        var pisDevNoMesTerMesTriResultAux = 0;
        var compPisRetPriMesTriResultAux = 0;
        var compPisRetSegMesTriResultAux = 0;
        var compPisRetTerMesTriResultAux = 0;
        var pisRecPriMesTriResultAux = 0;
        var pisRecSegMesTriResultAux = 0;
        var pisRecTerMesTriResultAux = 0;
        var cofinsDevPriMesTriResultAux = 0;
        var cofinsDevSegMesTriResultAux = 0;
        var cofinsDevTerMesTriResultAux = 0;
        var compCofinsRetidoPriMesTriResultAux = 0;
        var compCofinsRetidoSegMesTriResultAux = 0;
        var compCofinsRetidoTerMesTriResultAux = 0;
        var cofinsRecPriMesTriResultAux = 0;
        var cofinsRecSegMesTriResultAux = 0;
        var cofinsRecTerMesTriResultAux = 0;
        //Variáveis de auxílio PIS - Cofins

        //Variáveis de auxílio IRPJ
        var baseCalculoLucroPresumidoPorAtividadeAux = 0;
        var demaisReceitasAux = 0;
        var compensacaoIrRetidoAux = 0;
        var irRecolherAux = 0;
        var cotaUnicaAux = 0;
        var primeiraCotaAux = 0;
        var segundaCotaAux = 0;
        var terceiraCotaAux = 0;
        //Variáveis de auxílio IRPJ

        //Variáveis de auxílio CSLL
        var csllBaseCalculoLucroPresumidoPorAtividadeAux = 0;
        var csllDemaisReceitasAux = 0;
        var csllCompensacaoCsllRetidaAux = 0;
                               
        //Variáveis de auxílio CSLL

        angular.forEach($scope.lucroPresumidoLista, function (value, index) {

            //Pis Cofins - Inclusão do resultado em uma variável auxiliar
            baseCalcPisCofinsPriMesTriResultAux = $scope.baseCalcPisCofinsPriMesTriList[index].value + baseCalcPisCofinsPriMesTriResultAux;
            baseCalcPisCofinsSegMesTriResultAux = $scope.baseCalcPisCofinsSegMesTriList[index].value + baseCalcPisCofinsSegMesTriResultAux;
            baseCalcPisCofinsTerMesTriResultAux = $scope.baseCalcPisCofinsTerMesTriList[index].value + baseCalcPisCofinsTerMesTriResultAux;

            pisDevNoMesPriMesTriResultAux = $scope.pisDevNoMesPriMesTriList[index].value + pisDevNoMesPriMesTriResultAux;
            pisDevNoMesSegMesTriResultAux = $scope.pisDevNoMesSegMesTriList[index].value + pisDevNoMesSegMesTriResultAux;
            pisDevNoMesTerMesTriResultAux = $scope.pisDevNoMesTerMesTriList[index].value + pisDevNoMesTerMesTriResultAux;

            compPisRetPriMesTriResultAux = $scope.compPisRetPriMesTriList[index].value + compPisRetPriMesTriResultAux;
            compPisRetSegMesTriResultAux = $scope.compPisRetSegMesTriList[index].value + compPisRetSegMesTriResultAux;
            compPisRetTerMesTriResultAux = $scope.compPisRetTerMesTriList[index].value + compPisRetTerMesTriResultAux;

            pisRecPriMesTriResultAux = $scope.pisRecPriMesTriList[index].value + pisRecPriMesTriResultAux
            pisRecSegMesTriResultAux = $scope.pisRecSegMesTriList[index].value + pisRecSegMesTriResultAux
            pisRecTerMesTriResultAux = $scope.pisRecTerMesTriList[index].value + pisRecTerMesTriResultAux

            cofinsDevPriMesTriResultAux = $scope.cofinsDevPriMesTriList[index].value + cofinsDevPriMesTriResultAux;
            cofinsDevSegMesTriResultAux = $scope.cofinsDevSegMesTriList[index].value + cofinsDevSegMesTriResultAux;
            cofinsDevTerMesTriResultAux = $scope.cofinsDevTerMesTriList[index].value + cofinsDevTerMesTriResultAux;

            compCofinsRetidoPriMesTriResultAux = $scope.compCofinsRetidoPriMesTriList[index].value + compCofinsRetidoPriMesTriResultAux;
            compCofinsRetidoSegMesTriResultAux = $scope.compCofinsRetidoSegMesTriList[index].value + compCofinsRetidoSegMesTriResultAux;
            compCofinsRetidoTerMesTriResultAux = $scope.compCofinsRetidoTerMesTriList[index].value + compCofinsRetidoTerMesTriResultAux;

            cofinsRecPriMesTriResultAux = $scope.cofinsRecPriMesTriList[index].value + cofinsRecPriMesTriResultAux;
            cofinsRecSegMesTriResultAux = $scope.cofinsRecSegMesTriList[index].value + cofinsRecSegMesTriResultAux;
            cofinsRecTerMesTriResultAux = $scope.cofinsRecTerMesTriList[index].value + cofinsRecTerMesTriResultAux;
            //Pis Cofins - Inclusão do resultado ta tabela para apresentar para o cliente

            //Irpj - Inclusão do resultado ta tabela para apresentar para o cliente

            demaisReceitasAux = $scope.demaisReceitasList[index].value + demaisReceitasAux;

            compensacaoIrRetidoAux = $scope.lucroPresumidoLista[index].data.model.irrfSemReceitasFinanceiras[0].valor +
                $scope.lucroPresumidoLista[index].data.model.irrfSemReceitasFinanceiras[1].valor +
                $scope.lucroPresumidoLista[index].data.model.irrfSemReceitasFinanceiras[2].valor +
                $scope.lucroPresumidoLista[index].data.model.model.irrfServicoPj[0].valor +
                $scope.lucroPresumidoLista[index].data.model.model.irrfServicoPj[1].valor +
                $scope.lucroPresumidoLista[index].data.model.model.irrfServicoPj[2].valor +
                compensacaoIrRetidoAux;

            baseCalculoLucroPresumidoPorAtividadeAux = $scope.baseCalculoLucroPresumidoPorAtividadeList[index].value + baseCalculoLucroPresumidoPorAtividadeAux;
            //Irpj - Inclusão do resultado ta tabela para apresentar para o cliente

            //CSLL
            csllBaseCalculoLucroPresumidoPorAtividadeAux = csllBaseCalculoLucroPresumidoPorAtividadeAux + $scope.baseCalculoLucroPresumidoAcordoAtividadeCsllList[index].value;

            csllCompensacaoCsllRetidaAux = $scope.lucroPresumidoLista[index].data.model.csllRetidoPj[0].valor +
                $scope.lucroPresumidoLista[index].data.model.csllRetidoPj[1].valor +
                $scope.lucroPresumidoLista[index].data.model.csllRetidoPj[2].valor +
                csllCompensacaoCsllRetidaAux;
            //CSLL

        });                              

        //Pis Cofins - Inclusão do resultado ta tabela para apresentar para o cliente
        $scope.baseCalcPisCofinsPriMesTriResult = baseCalcPisCofinsPriMesTriResultAux;
        $scope.baseCalcPisCofinsSegMesTriResult = baseCalcPisCofinsSegMesTriResultAux;
        $scope.baseCalcPisCofinsTerMesTriResult = baseCalcPisCofinsTerMesTriResultAux;

        $scope.pisDevNoMesPriMesTriResult = pisDevNoMesPriMesTriResultAux;
        $scope.pisDevNoMesSegMesTriResult = pisDevNoMesSegMesTriResultAux;
        $scope.pisDevNoMesTerMesTriResult = pisDevNoMesTerMesTriResultAux;

        $scope.compPisRetPriMesTriResult = compPisRetPriMesTriResultAux;
        $scope.compPisRetSegMesTriResult = compPisRetSegMesTriResultAux;
        $scope.compPisRetTerMesTriResult = compPisRetTerMesTriResultAux;

        $scope.pisRecPriMesTriResult = pisRecPriMesTriResultAux;
        $scope.pisRecSegMesTriResult = pisRecSegMesTriResultAux;
        $scope.pisRecTerMesTriResult = pisRecTerMesTriResultAux;

        $scope.cofinsDevPriMesTriResult = cofinsDevPriMesTriResultAux;
        $scope.cofinsDevSegMesTriResult = cofinsDevSegMesTriResultAux;
        $scope.cofinsDevTerMesTriResult = cofinsDevTerMesTriResultAux;

        $scope.compCofinsRetidoPriMesTriResult = compCofinsRetidoPriMesTriResultAux;
        $scope.compCofinsRetidoSegMesTriResult = compCofinsRetidoSegMesTriResultAux;
        $scope.compCofinsRetidoTerMesTriResult = compCofinsRetidoTerMesTriResultAux;

        $scope.cofinsRecPriMesTriResult = cofinsRecPriMesTriResultAux;
        $scope.cofinsRecSegMesTriResult = cofinsRecSegMesTriResultAux;
        $scope.cofinsRecTerMesTriResult = cofinsRecTerMesTriResultAux;
        //Pis Cofins - Inclusão do resultado ta tabela para apresentar para o cliente

        //IRPJ - Inclusão do resultado ta tabela para apresentar para o cliente
        $scope.demaisReceitas = demaisReceitasAux;

        $scope.baseCalculoIrpj = baseCalculoLucroPresumidoPorAtividadeAux + $scope.demaisReceitas;

        $scope.irpjQuinzePorCento = $scope.baseCalculoIrpj * 0.15;
        
        var auxLimiteAdicional = 0;
        if (($scope.lucroPresumidoLista[0].data.lucroPresumido.model.trimestre.meses.id % 3) == 1) {
            auxLimiteAdicional = 60000;
        } else if (($scope.lucroPresumidoLista[0].data.lucroPresumido.model.trimestre.meses.id % 3) == 2) {
            auxLimiteAdicional = 40000;
        } else if (($scope.lucroPresumidoLista[0].data.lucroPresumido.model.trimestre.meses.id % 3) == 0) {
            auxLimiteAdicional = 20000;
        } else {
            auxLimiteAdicional = 60000;
        }

        $scope.limiteAdicional = auxLimiteAdicional;

        if ($scope.baseCalculoIrpj <= $scope.limiteAdicional) {
            $scope.baseCalculoAdicional = 0;
            $scope.adicionalDeIrpj = 0;
        } else {
            $scope.baseCalculoAdicional = ($scope.baseCalculoIrpj - $scope.limiteAdicional);
            $scope.adicionalDeIrpj = $scope.baseCalculoAdicional * 0.1;
        }
        
        $scope.irpjDevido = $scope.irpjQuinzePorCento + $scope.adicionalDeIrpj;

        $scope.compensacaoIrRetido = compensacaoIrRetidoAux;

        $scope.irRecolher = $scope.irpjDevido - $scope.compensacaoIrRetido;

        $scope.cotaUnica = $scope.irRecolher;

        if (($scope.irRecolher / 3) > 1000) {
            $scope.primeiraCota = $scope.irRecolher / 3;
            $scope.segundaCota = $scope.irRecolher / 3;
            $scope.terceiraCota = $scope.irRecolher / 3;
        } else {
            $scope.primeiraCota = 0;
            $scope.segundaCota =  0;
            $scope.terceiraCota = 0;
        }


        //CSLL - Inclusão do resultado ta tabela para apresentar para o cliente
        $scope.demaisReceitasCsll = $scope.demaisReceitas;

        $scope.baseCalculoCsll = csllBaseCalculoLucroPresumidoPorAtividadeAux + $scope.demaisReceitasCsll;

        $scope.cssDevidaCsll = $scope.baseCalculoCsll * 0.09;

        $scope.compensacoesCsllRetida = csllCompensacaoCsllRetidaAux;

        $scope.cssRecolherCsll = $scope.cssDevidaCsll - $scope.compensacoesCsllRetida;

        $scope.cotaUnicaCsll = $scope.cssRecolherCsll;

        if (($scope.cssRecolherCsll / 3) > 1000) {
            $scope.primeiraCotaCsll = $scope.cssRecolherCsll / 3;
            $scope.segundaCotaCsll =  $scope.cssRecolherCsll / 3;
            $scope.terceiraCotaCsll = $scope.cssRecolherCsll / 3;
        } else {
            $scope.primeiraCotaCsll = 0;
            $scope.segundaCotaCsll = 0;
            $scope.terceiraCotaCsll = 0;
        }
        //CSLL - Inclusão do resultado ta tabela para apresentar para o cliente

    };


    $scope.pisCofinsCalculaBaseCalculoApuracaoMensal = function (index) {
        $scope.baseCalcPisCofinsPriMesTriList[index].value = $scope.lucroPresumidoLista[index].data.model.totalReceitaBruta[0].valor - $scope.lucroPresumidoLista[index].data.model.receitaExportacao[0].valor;
        $scope.baseCalcPisCofinsSegMesTriList[index].value = $scope.lucroPresumidoLista[index].data.model.totalReceitaBruta[1].valor - $scope.lucroPresumidoLista[index].data.model.receitaExportacao[1].valor;
        $scope.baseCalcPisCofinsTerMesTriList[index].value = $scope.lucroPresumidoLista[index].data.model.totalReceitaBruta[2].valor - $scope.lucroPresumidoLista[index].data.model.receitaExportacao[2].valor;
    };

    $scope.pisCofinsPisDevidoMes = function (index) {
        $scope.pisDevNoMesPriMesTriList[index].value = $scope.baseCalcPisCofinsPriMesTriList[index].value * 0.0065;
        $scope.pisDevNoMesSegMesTriList[index].value = $scope.baseCalcPisCofinsSegMesTriList[index].value * 0.0065;
        $scope.pisDevNoMesTerMesTriList[index].value = $scope.baseCalcPisCofinsTerMesTriList[index].value * 0.0065;
    };

    $scope.pisCofinsCompensacaoPisRetido = function (index) {
        $scope.compPisRetPriMesTriList[index].value = $scope.lucroPresumidoLista[index].data.model.pisRetidoServicosPj[0].valor;
        $scope.compPisRetSegMesTriList[index].value = $scope.lucroPresumidoLista[index].data.model.pisRetidoServicosPj[1].valor;
        $scope.compPisRetTerMesTriList[index].value = $scope.lucroPresumidoLista[index].data.model.pisRetidoServicosPj[2].valor;
    };

    $scope.pisCofinsPisRecolher = function (index) {
        $scope.pisRecPriMesTriList[index].value = $scope.pisDevNoMesPriMesTriList[index].value - $scope.compPisRetPriMesTriList[index].value;
        $scope.pisRecSegMesTriList[index].value = $scope.pisDevNoMesSegMesTriList[index].value - $scope.compPisRetSegMesTriList[index].value;
        $scope.pisRecTerMesTriList[index].value = $scope.pisDevNoMesTerMesTriList[index].value - $scope.compPisRetTerMesTriList[index].value;
    };

    $scope.pisCofinsCofinsDevido = function (index) {
        $scope.cofinsDevPriMesTriList[index].value = $scope.baseCalcPisCofinsPriMesTriList[index].value * 0.03;
        $scope.cofinsDevSegMesTriList[index].value = $scope.baseCalcPisCofinsSegMesTriList[index].value * 0.03;
        $scope.cofinsDevTerMesTriList[index].value = $scope.baseCalcPisCofinsTerMesTriList[index].value * 0.03;
    };

    $scope.pisCofinsCompensacaoCofinsRetido = function (index) {
        $scope.compCofinsRetidoPriMesTriList[index].value = $scope.lucroPresumidoLista[index].data.model.cofinsRetidoPj[0].valor;
        $scope.compCofinsRetidoSegMesTriList[index].value = $scope.lucroPresumidoLista[index].data.model.cofinsRetidoPj[0].valor;
        $scope.compCofinsRetidoTerMesTriList[index].value = $scope.lucroPresumidoLista[index].data.model.cofinsRetidoPj[0].valor;
    };

    $scope.pisCofinsCofinsRecolher = function (index) {
        $scope.cofinsRecPriMesTriList[index].value = $scope.cofinsDevPriMesTriList[index].value - $scope.cofinsDevPriMesTriList[index].value;
        $scope.cofinsRecSegMesTriList[index].value = $scope.cofinsDevSegMesTriList[index].value - $scope.cofinsDevSegMesTriList[index].value;
        $scope.cofinsRecTerMesTriList[index].value = $scope.cofinsDevTerMesTriList[index].value - $scope.cofinsDevTerMesTriList[index].value;
    };

    $scope.irrpjBaseCalculoLucroPresumido = function (index) {
        var formlaCalcLucroPresum = 1;
        if ($scope.lucroPresumidoLista[index].data.model.atividade.id == 1) {
            formlaCalcLucroPresum = 0.32;
        } else if ($scope.lucroPresumidoLista[index].data.model.atividade.id == 2) {
            formlaCalcLucroPresum = 0.016;
        } else if ($scope.lucroPresumidoLista[index].data.model.atividade.id == 3) {
            formlaCalcLucroPresum = 0.16;
        } else if ($scope.lucroPresumidoLista[index].data.model.atividade.id == 4) {
            formlaCalcLucroPresum = 0.08;
        } else {
            formlaCalcLucroPresum = 0.32;
        }
        $scope.baseCalculoLucroPresumidoPorAtividadeList[index].value = ($scope.lucroPresumidoLista[index].data.model.totalReceitaBruta[0].valor + $scope.lucroPresumidoLista[index].data.model.totalReceitaBruta[1].valor + $scope.lucroPresumidoLista[index].data.model.totalReceitaBruta[2].valor) * formlaCalcLucroPresum;
    };

    $scope.irpjDemaisReceitas = function (index) {
        $scope.demaisReceitasList[index].value = $scope.lucroPresumidoLista[index].data.model.receitasFinanceirasRendimentoAplicacoesEmRendaFixa[0].valor +
            $scope.lucroPresumidoLista[index].data.model.receitasFinanceirasRendimentoAplicacoesEmRendaFixa[1].valor +
            $scope.lucroPresumidoLista[index].data.model.receitasFinanceirasRendimentoAplicacoesEmRendaFixa[2].valor +
            $scope.lucroPresumidoLista[index].data.model.recuperacaoDePerdasNoRecebimentoDeCredito[0].valor +
            $scope.lucroPresumidoLista[index].data.model.recuperacaoDePerdasNoRecebimentoDeCredito[1].valor +
            $scope.lucroPresumidoLista[index].data.model.recuperacaoDePerdasNoRecebimentoDeCredito[2].valor +
            $scope.lucroPresumidoLista[index].data.model.ganhoCapital[0].valor +
            $scope.lucroPresumidoLista[index].data.model.ganhoCapital[1].valor +
            $scope.lucroPresumidoLista[index].data.model.ganhoCapital[2].valor;
    };

    $scope.csllBaseCalculoLucroPresumido = function (index) {
        var formulaCalcCsll = 1;
        if ($scope.lucroPresumidoLista[index].data.model.atividade.id == 1) {
            formulaCalcCsll = 0.32;
        } else if ($scope.lucroPresumidoLista[index].data.model.atividade.id == 2) {
            formulaCalcCsll = 0.12;
        } else if ($scope.lucroPresumidoLista[index].data.model.atividade.id == 3) {
            formulaCalcCsll = 0.12;
        } else if ($scope.lucroPresumidoLista[index].data.model.atividade.id == 4) {
            formulaCalcCsll = 0.12;
        } else {
            formulaCalcCsll = 0.32;
        }

        var baseCalculoLucroPresumidoAcordoAtividadeCsllAux = ($scope.lucroPresumidoLista[index].data.model.totalReceitaBruta[0].valor + $scope.lucroPresumidoLista[index].data.model.totalReceitaBruta[1].valor + $scope.lucroPresumidoLista[index].data.model.totalReceitaBruta[2].valor) * formulaCalcCsll;

        $scope.baseCalculoLucroPresumidoAcordoAtividadeCsllList[index].value = baseCalculoLucroPresumidoAcordoAtividadeCsllAux;
    };

    $scope.csllDemaisReceitasCsll = function (index) {
        $scope.demaisReceitasCsllList[index].value = $scope.lucroPresumidoLista[index].data.model.receitasFinanceirasRendimentoAplicacoesEmRendaFixa[0].valor +
            $scope.lucroPresumidoLista[index].data.model.receitasFinanceirasRendimentoAplicacoesEmRendaFixa[1].valor +
            $scope.lucroPresumidoLista[index].data.model.receitasFinanceirasRendimentoAplicacoesEmRendaFixa[2].valor +
            $scope.lucroPresumidoLista[index].data.model.recuperacaoDePerdasNoRecebimentoDeCredito[0].valor +
            $scope.lucroPresumidoLista[index].data.model.recuperacaoDePerdasNoRecebimentoDeCredito[1].valor +
            $scope.lucroPresumidoLista[index].data.model.recuperacaoDePerdasNoRecebimentoDeCredito[2].valor +
            $scope.lucroPresumidoLista[index].data.model.ganhoCapital[0].valor +
            $scope.lucroPresumidoLista[index].data.model.ganhoCapital[1].valor +
            $scope.lucroPresumidoLista[index].data.model.ganhoCapital[2].valor;
    };

    $scope.pisCofinsZeraValueItensLista = function (index) {
        var obj = new Object();
        obj["value"] = 0;

        $scope.baseCalcPisCofinsPriMesTriList.push(obj);
        $scope.baseCalcPisCofinsSegMesTriList.push(obj);
        $scope.baseCalcPisCofinsTerMesTriList.push(obj);

        $scope.pisDevNoMesPriMesTriList.push(obj);
        $scope.pisDevNoMesSegMesTriList.push(obj);
        $scope.pisDevNoMesTerMesTriList.push(obj);

        $scope.compPisRetPriMesTriList.push(obj);
        $scope.compPisRetSegMesTriList.push(obj);
        $scope.compPisRetTerMesTriList.push(obj);

        $scope.pisRecPriMesTriList.push(obj);
        $scope.pisRecSegMesTriList.push(obj);
        $scope.pisRecTerMesTriList.push(obj);

        $scope.cofinsDevPriMesTriList.push(obj);
        $scope.cofinsDevSegMesTriList.push(obj);
        $scope.cofinsDevTerMesTriList.push(obj);

        $scope.compCofinsRetidoPriMesTriList.push(obj);
        $scope.compCofinsRetidoSegMesTriList.push(obj);
        $scope.compCofinsRetidoTerMesTriList.push(obj);

        $scope.cofinsRecPriMesTriList.push(obj);
        $scope.cofinsRecSegMesTriList.push(obj);
        $scope.cofinsRecTerMesTriList.push(obj);
    }

    $scope.calculoIrpjZeraLista = function (index) {
        var obj = new Object();
        obj["value"] = 0;
        $scope.baseCalculoLucroPresumidoPorAtividadeList.push(obj);
        $scope.demaisReceitasList.push(obj);
    };

    $scope.calculoCsllZeraLista = function (index) {
        var obj = new Object();
        obj["value"] = 0;
        $scope.baseCalculoLucroPresumidoAcordoAtividadeCsllList.push(obj);
        $scope.demaisReceitasCsllList.push(obj);
    };

});