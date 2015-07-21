(function () {
    var app = angular.module('AssaultApp', []);
    var armyStore = []; //Army
    var countryStore = []; //Countries
    var assaultStore = []; //Assaults

    app.controller('MainEnCoController', ['$scope', '$window', '$http', '$location', function ($scope, $window, $http, $location) {

        this.armyStore = $window.armyModel;
        this.assaultStore = $window.assaults;
        this.countryStore = $window.countries;

        $scope.archer = '0';
        $scope.knight = '0';
        $scope.elite = '0';
        $scope.target = $window.countries[0].Name;

        this.canSend = function () {

            var enoughArcher = $scope.archer <= $window.archerAmount;
            var enoughKnight = $scope.knight <= $window.knightAmount;
            var enoughElite = $scope.elite <= $window.eliteAmount;
            var atleastOne = $scope.archer > 0 || $scope.knight > 0 || $scope.elite > 0;

            return !(enoughArcher && enoughKnight && enoughElite && atleastOne);
        };

        this.sendArmy = function () {

            
            var send = {
               'Name': $scope.target,
                'Archers': $scope.archer,
                'Knights': $scope.knight,
                'Elites': $scope.elite
            }
         
            this.armyStore[0].Size -= $scope.archer;
            this.armyStore[1].Size -= $scope.knight;
            this.armyStore[2].Size -= $scope.elite;

            $window.archerAmount -= $scope.archer;
            $window.knightAmount -= $scope.knight;
            $window.eliteAmount -= $scope.elite;

            var config = {
                headers : {
                    'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                }
            }

            $http.post('/Assault/SendAssault', send, config).then(function (result) {
                    $window.assaults.push(
                        {
                            "Country": { "Id": null, "User": null, "StandingForce": null, "Assaults": null, "Construction": null, "Buildings": null, "Researching": null, "Researches": null, "Name": $scope.target, "Gold": 0, "Potato": 0, "Population": 0, "Score": 0 },
                            "Forces": [{ "Id": 0, "Size": $scope.archer, "Type": { "Id": 1, "Name": "Archer", "Description": null, "Attack": 0, "Defense": 0, "Cost": 0, "Upkeep": 0, "Payment": 0, "Score": 0 } },
                                { "Id": 0, "Size": $scope.knight, "Type": { "Id": 3, "Name": "Knight", "Description": null, "Attack": 0, "Defense": 0, "Cost": 0, "Upkeep": 0, "Payment": 0, "Score": 0 } },
                                { "Id": 0, "Size": $scope.elite, "Type": { "Id": 4, "Name": "Elite", "Description": null, "Attack": 0, "Defense": 0, "Cost": 0, "Upkeep": 0, "Payment": 0, "Score": 0 } }]
                        }
                        );
                    $scope.archer = '0';
                    $scope.knight = '0';
                    $scope.elite = '0';
            });
        
           
        }
    }]);
})();
