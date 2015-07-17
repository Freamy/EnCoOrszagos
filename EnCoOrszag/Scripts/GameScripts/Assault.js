(function () {
    var app = angular.module('AssaultApp', []);
    var $scope.armyStore = []; //Army
    var countryStore = []; //Countries
    var assaultStore = []; //Assaults

    app.controller('ArmyController', ['$scope', '$window', function ($scope, $window) {
        
        this.armyStore = $window.armyModel;
    }]);

    app.controller('CountryController', ['$scope', '$window', function ($scope, $window) {
        this.armyStore = $window.armyModel;
        this.countryStore = $window.countries;
    }]);

    app.controller('AssaultController', ['$scope', '$window', function ($scope, $window) {

        this.assaultStore = $window.assaults;
    }]);

    app.controller('SendAssaultController', ['$scope','$window', function ($scope,$window) {
        this.armyStore = $window.armyModel;
        this.assaultStore = $window.assaults;
        this.canSend = function () {

            var enoughArcher = $scope.archer > $window.archerAmount; 
            var enoughKnight = $scope.knight > $window.knightAmount;
            var enoughElite = $scope.elite > $window.eliteAmount;
          
            return enoughArcher || enoughKnight || enoughElite;
        };

        this.sendArmy = function () {
            this.assaultStore.push({
                assault: { Country: { 'Name': "Fream" }, Thing: {'Mas':"Mas"}}
            });
            //assault["Country"]["Name"]
            
        }
    }]);

})();
