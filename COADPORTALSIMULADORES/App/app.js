//app.js
(
    function () {
        'use strict';

        angular
            .module('app', ['ngRoute', 'ngCookies','ngSanitize'])
            .config(config)
            .run(run);

        config.$inject = ['$routeProvider', '$locationProvider', '$sceDelegateProvider'];
        function config($routeProvider, $locationProvider, $sceDelegateProvider) {
            $routeProvider
                .when('/', {
                    controller: 'LoginController',
                    templateUrl: 'App/Components/login/login.html',
                    controllerAs: 'vm',
                    css: ['Assets/css/teste.css']
                })
                .when('/Home', {
                    controller: 'HomeController',
                    templateUrl: 'App/Components/home/home.html',
                    controllerAs: 'vm'
                })
                .when('/SimuladoresTrabalhistas', {
                    controller: 'SimuladoresTrabalhistasController',
                    templateUrl: 'App/components/home/home.html',
                    controllerAs: 'vm'
                })
                .when('/HomeTrabalhista', {
                    controller: 'HomeTrabalhistaController',
                    templateUrl: 'App/components/HomeTrabalhista/HomeTrabalhista.html',
                    controllerAs: 'vm'
                })
                .when('/TrabPatroEmp', {
                    templateUrl: 'App/shared/SimuladoresTrabalhistas/Template/TrabPatroEmp.html'
                })
                .when('/TrabProfLib', {
                    templateUrl: 'App/shared/SimuladoresTrabalhistas/Template/TrabProfLib.html'
                })
                .when('/TrabEmpRur', {
                    templateUrl: 'App/shared/SimuladoresTrabalhistas/Template/TrabEmpRur.html'
                })
                .when('/TrabEmpreg', {
                    templateUrl: 'App/shared/SimuladoresTrabalhistas/Template/TrabEmpreg.html'
                })
                .when('/TrabCalcContrReco', {
                    templateUrl: 'App/shared/SimuladoresTrabalhistas/Template/TrabCalcContrReco.html'
                })
                .when('/TrabAliqContrPatrPrev', {
                    templateUrl: 'App/shared/SimuladoresTrabalhistas/Template/TrabAliqContrPatrPrev.html'
                })
                .when('/TrabCronDetEsoc', {
                    templateUrl: 'App/shared/SimuladoresTrabalhistas/Template/TrabCronDetEsoc.html'
                })
                .when('/TrabCalcQuantMaxEstag', {
                    templateUrl: 'App/shared/SimuladoresTrabalhistas/Template/TrabCalcQuantMaxEstag.html'
                })
                .when('/TrabIdentAliqAcordoCnae', {
                    templateUrl: 'App/shared/SimuladoresTrabalhistas/Template/TrabIdentAliqAcordoCnae.html'
                })
                .when('/TrabRetPrevInss', {
                    templateUrl: 'App/shared/SimuladoresTrabalhistas/Template/TrabRetPrevInss.html'
                })
                .when('/TrabSalAprend', {
                    templateUrl: 'App/shared/SimuladoresTrabalhistas/Template/TrabSalAprend.html'
                })
                .when('/TrabSalFamil', {
                    templateUrl: 'App/shared/SimuladoresTrabalhistas/Template/TrabSalFamil.html'
                })
                .when('/TrabSegDesemp', {
                    templateUrl: 'App/shared/SimuladoresTrabalhistas/Template/TrabSegDesemp.html'
                })



                .when('/HomeFiscal', {
                    controller: 'HomeFiscalController',
                    templateUrl: 'App/components/homeFiscal/HomeFiscal.html',
                    controllerAs: 'vm'
                })
                .when('/FiscalPis', {
                    templateUrl: 'App/shared/simuladoresFiscais/template/FiscalPis.html'
                })
                .when('/FiscalRet', {
                    templateUrl: 'App/shared/simuladoresFiscais/template/FiscalRet.html'
                })
                .when('/FiscalCest', {
                    templateUrl: 'App/shared/simuladoresFiscais/template/FiscalCest.html'
                })
                .when('/FiscalCefop', {
                    templateUrl: 'App/shared/simuladoresFiscais/template/FiscalCefop.html'
                })
                .when('/FiscalCst', {
                    templateUrl: 'App/shared/simuladoresFiscais/template/FiscalCst.html'
                })
                .when('/FiscalIcmsAliqInt', {
                    templateUrl: 'App/shared/simuladoresFiscais/templateFiscalIcmsAliqInt.html'
                })
                .when('/FiscalIcmsDif', {
                    templateUrl: 'App/shared/simuladoresFiscais/template/FiscalIcmsDif.html'
                })
                .when('/FiscalEnq', {
                    templateUrl: 'App/shared/simuladoresFiscais/template/FiscalEnq.html'
                })
                .when('/FiscalFatR', {
                    templateUrl: 'App/shared/simuladoresFiscais/template/FiscalFatR.html'
                })
                .when('/FiscalSimlRep', {
                    templateUrl: 'App/shared/simuladoresFiscais/template/FiscalSimlRep.html'
                })
                .otherwise({ redirectTo: '/' });
        }

        run.$inject = ['$rootScope', '$location', '$cookies', '$http'];

        function run($rootScope, $location, $cookies, $http) {

            // keep user logged in after page refresh
            /*$rootScope.globals = $cookies.get('globals') || {};

            if ($rootScope.globals.currentUser) {
                $http.defaults.headers.common['Authorization'] = 'Basic ' + JSON.parse(sessionStorage.getItem("autenticado")).authdata; // jshint ignore:line
            }

            $rootScope.$on('$locationChangeStart', function (event, next, current) {
                
                // redirect to login page if not logged in and trying to access a restricted page
                var restrictedPage = $.inArray($location.path(), ['/login']) === -1;
                console.log(restrictedPage);
                console.log($location.path());

                var loggedIn = $rootScope.globals.currentUser;
                console.log("currentUser = " + loggedIn);

                console.log("Current User" + JSON.parse(sessionStorage.getItem("autenticado")).login);
                console.log(loggedIn);
                if (restrictedPage && !loggedIn) {
                    $location.path('/');
                }
            });
                */

            $rootScope.$on('$locationChangeStart', function (event, next, current) {

                if (sessionStorage.getItem("autenticado")) {
                    var restrictedPage = $.inArray($location.path(), ['/Login']) === -1;
                    var loggedIn = JSON.parse(sessionStorage.getItem("autenticado")).login;
                    if (restrictedPage && !loggedIn) {
                        $location.path('/');
                    }

                } else {
                    $location.path('/');
                }
            });
        }
    }
)();
