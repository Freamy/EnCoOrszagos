(function () {
    //komment: használjunk strict módot angularhoz
    "use strict";

    var app = angular.module('AssaultApp', []);

    app.controller('MainEnCoController', ['$scope', '$window', '$http', '$location', function ($scope, $window, $http, $location) {
        //komment: döntsük el, hogy pokakoljuk a modelre a dolgokat: this.myProp-pal vagy $scope.myProp-pal, de akkor az legyen egységes
        // egyébként a $scope-os a preferált, ennek is meg vannak az okai, tutorialban is ez van, a this-es csak legacy támogatás miatt él még
  

        $scope.countriesML = $window.countries;
        $scope.armiesML = $window.armyModel;

        $scope.archer = '0';
        $scope.knight = '0';
        $scope.elite = '0';

            $scope.target = $window.countries[0].Name;

            this.canSend = function () {

                $scope.armyML = $window.armyModel;

                var enoughArcher = $scope.archer <= $scope.armyML[0].Size && $scope.archer >= 0;

                var enoughKnight = $scope.knight <= $scope.armyML[1].Size && $scope.knight >= 0;

                var enoughElite = $scope.elite <= $scope.armyML[2].Size && $scope.elite >= 0;

                var atleastOne = ($scope.archer + $scope.knight + $scope.elite) > 0;

                var cantAttackOwn = $scope.target != $window.ownName;

             

            return !(enoughArcher && enoughKnight && enoughElite && cantAttackOwn && atleastOne);
        };

        this.sendArmy = function () {

            var archer = parseInt($scope.archer);
            var knight = parseInt($scope.knight);
            var elite = parseInt($scope.elite);
            
            var send = {
               'Name': $scope.target,
                'Archers': archer,
                'Knights': knight,
                'Elites': elite
            }
         
               $scope.armiesML[0].Size -= archer;
               $scope.armiesML[1].Size -= knight;
               $scope.armiesML[2].Size -= elite;

           
            //komment: az ilyeneket globálisan lehet állítani, ha szükség van rá, így nem kell minden postnak megadni configként.
            // itt egyébként nincs rá szükség, valahol máshol volt a hiba, ha enélkül nem kapta meg a json-t az MVC
            var config = {
                headers : {
                    'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
                }
            }

            $http.post('/Assault/SendAssault', send, config).then(function (result) {
                //komment: ez nagyon-nagyon-nagyon nem jó így. hol nézed, hogy mi jött válaszként a post-ra?
                // mi van, ha hiba volt, és nem is indította el az adott assaultot? ez akkora is be fogja pusholni a listába.
                // másrészt ez a hatalmas object kézzel bepusholása nagyon ronda, biztos lehetne ezt szebben is.
                    $window.assaults.push(
                        {
                            "Country": { "Id": null, "User": null, "StandingForce": null, "Assaults": null, "Construction": null, "Buildings": null, "Researching": null, "Researches": null, "Name": $scope.target, "Gold": 0, "Potato": 0, "Population": 0, "Score": 0 },
                            "Forces": [{ "Id": 0, "Size": archer, "Type": { "Id": 1, "Name": "Archer", "Description": null, "Attack": 0, "Defense": 0, "Cost": 0, "Upkeep": 0, "Payment": 0, "Score": 0 } },
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
