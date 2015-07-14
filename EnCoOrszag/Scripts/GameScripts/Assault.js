(function () {
    var app = angular.module('AssaultApp', []).v;

    

    app.controller('ArmyStoreController', function () {
        this.loadArmy = function (army) {
            this.army = army;
        }
    });


    app.controller('TestController', function () {
        this.setData = function (val) {
            this.dmg = val;
        }
        this.minus = function (val) {
            this.dmg -= val;
        }
    });

})();
