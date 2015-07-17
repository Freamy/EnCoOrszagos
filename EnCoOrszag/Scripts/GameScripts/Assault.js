(function () {
    var app = angular.module('AssaultApp', []);
    var armyStore = []; //Army
    var countryStore = []; //Countries
    var assaultStore = []; //Assaults

    app.controller('MainEnCoController', ['$scope', '$window', '$http','$location', function ($scope, $window, $http, $location) {
        this.armyStore = $window.armyModel;
        this.assaultStore = $window.assaults;
        this.countryStore = $window.countries;

        $scope.archer = '0';
        $scope.knight = '0';
        $scope.elite = '0';

        this.canSend = function () {

            var enoughArcher = $scope.archer > $window.archerAmount;
            var enoughKnight = $scope.knight > $window.knightAmount;
            var enoughElite = $scope.elite > $window.eliteAmount;

            return enoughArcher || enoughKnight || enoughElite;
        };

        this.sendArmy = function () {

            
            var send = {
               'Name': $scope.target,
                'Archers': $scope.archer,
                'Knights': $scope.knight,
                'Elites': $scope.elite
            }

            $scope.archer = '0';
            $scope.knight = '0';
            $scope.elite = '0';

            var config = {
                headers : {
                    'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                }
            }
           // var res = $http.post('/Assault/SendAssault', { data: send });
            $http.post('/Assault/SendAssault', send, config).then(function (result) {
             
                $window.assaults.push(
                    
                );
            });
            
           
        }


    }]);
})();
