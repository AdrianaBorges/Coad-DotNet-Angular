//loginController.js
(function () {
    'use strict';
    angular
        .module('app')
        .controller('LoginController', LoginController);
 
    LoginController.$inject = ['$location','AuthenticationService'];
    function LoginController($location, AuthenticationService) {
        var vm = this;
        vm.fazerLogin = fazerLogin;
 
        (function initController() {
            sessionStorage.clear();
            AuthenticationService.ClearCredentials();
        })();
        
        function fazerLogin() {
            vm.dataLoading = true;
            AuthenticationService.Login(vm.login, vm.senha, function (response) {
                try {
                    if (response.data.success) {
                        AuthenticationService.SetCredentials(vm.login, vm.senha);
                        $location.path('/Home');
                    } else {
                        vm.errorMessage = "Usuário ou senha inválidos. Tente novamente.";
                    }
                }
                catch(err) {
                    vm.errorMessage = "Ocorreu um erro ao tentar se logar. Tente novamente mais tarde.";
                }
            });
        }
    }
})();