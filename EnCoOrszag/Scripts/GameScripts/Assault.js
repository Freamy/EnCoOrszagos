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

                var enoughArcher = $scope.archer <= this.armyStore[0].Size && $scope.archer >= 0; 

                var enoughKnight = $scope.knight <= this.armyStore[1].Size && $scope.knight >= 0;

                var enoughElite = $scope.elite <= this.armyStore[2].Size && $scope.elite >= 0;

                var moreThenZero = ($scope.archer + $scope.knight + $scope.elite) != 0;

                var cantAttackOwn = $scope.target != $window.ownName;

            return !(enoughArcher && enoughKnight && enoughElite && cantAttackOwn && moreThenZero);
        };

        this.sendArmy = function () {
            
            var archers = parseInt($scope.archer);
            var knight = parseInt($scope.knight);
            var elite = parseInt($scope.elite);

            
            var send = {
               'Name': $scope.target,
                'Archers': archers,
                'Knights': knight,
                'Elites': elite
            }
         
                this.armyStore[0].Size -= archers;
                this.armyStore[1].Size -= knight;
                this.armyStore[2].Size -= elite;

           

            var config = {
                headers : {
                    'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                }
            }

            $http.post('/Assault/SendAssault', send, config).then(function (result) {
                    $window.assaults.push(
                        {
                            "Country": { "Id": null, "User": null, "StandingForce": null, "Assaults": null, "Construction": null, "Buildings": null, "Researching": null, "Researches": null, "Name": $scope.target, "Gold": 0, "Potato": 0, "Population": 0, "Score": 0 },
                            "Forces": [{ "Id": 0, "Size": archers, "Type": { "Id": 1, "Name": "Archer", "Description": null, "Attack": 0, "Defense": 0, "Cost": 0, "Upkeep": 0, "Payment": 0, "Score": 0 } },
                                { "Id": 0, "Size": knight, "Type": { "Id": 3, "Name": "Knight", "Description": null, "Attack": 0, "Defense": 0, "Cost": 0, "Upkeep": 0, "Payment": 0, "Score": 0 } },
                                { "Id": 0, "Size": elite, "Type": { "Id": 4, "Name": "Elite", "Description": null, "Attack": 0, "Defense": 0, "Cost": 0, "Upkeep": 0, "Payment": 0, "Score": 0 } }]
                        }
                        );
                    $scope.archer = '0';
                    $scope.knight = '0';
                    $scope.elite = '0';
            });
        
           
        }
    }]);
})();
