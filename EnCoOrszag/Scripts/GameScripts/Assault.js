(function () {
    var app = angular.module('AssaultApp', []);
    var armyStore = [];
    var countryStore = [];
    var assaultStore = [];

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

    app.controller('Test', ['$scope','$window', function ($scope,$window) {
        this.armyStore = $window.armyModel;
        this.canSend = function () {

            var enoughArcher = $scope.archer > 10; //This can't be static.
           // var enoughKnight = $scope.val > 10;
            //var enoughElite = $scope.val > 10;
           
            return enoughArcher;// || enoughKnight || enoughElite;
        };
    }]);

})();
