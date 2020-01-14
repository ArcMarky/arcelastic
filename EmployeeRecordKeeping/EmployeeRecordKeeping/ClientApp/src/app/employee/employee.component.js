"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var employee_1 = require("../models/employee");
var EmployeeComponent = /** @class */ (function () {
    function EmployeeComponent(_employeeService, toastr) {
        this._employeeService = _employeeService;
        this.toastr = toastr;
        this.model = new employee_1.EmployeeList();
        this.searchFilterModel = new employee_1.EmployeeListFilter();
        this.searchResults = new employee_1.searchResultsSummary();
        this.listOfFirstNames = ["Adam", "Alex", "Aaron", "Ben", "Carl", "Dan", "David", "Edward", "Fred", "Frank", "George", "Hal", "Hank", "Ike", "John", "Jack", "Joe", "Larry", "Monte", "Matthew", "Mark", "Nathan", "Otto", "Paul", "Peter", "Roger", "Roger", "Steve", "Thomas", "Tim", "Ty", "Victor", "Walter"];
        this.listOfLastNames = ["Anderson", "Ashwoon", "Aikin", "Bateman", "Bongard", "Bowers", "Boyd", "Cannon", "Cast", "Deitz", "Dewalt", "Ebner", "Frick", "Hancock", "Haworth", "Hesch", "Hoffman", "Kassing", "Knutson", "Lawless", "Lawicki", "Mccord", "McCormack", "Miller", "Myers", "Nugent", "Ortiz", "Orwig", "Ory", "Paiser", "Pak", "Pettigrew", "Quinn", "Quizoz", "Ramachandran", "Resnick", "Sagar", "Schickowski", "Schiebel", "Sellon", "Severson", "Shaffer", "Solberg", "Soloman", "Sonderling", "Soukup", "Soulis", "Stahl", "Sweeney", "Tandy", "Trebil", "Trusela", "Trussel", "Turco", "Uddin", "Uflan", "Ulrich", "Upson", "Vader", "Vail", "Valente", "Van Zandt", "Vanderpoel", "Ventotla", "Vogal", "Wagle", "Wagner", "Wakefield", "Weinstein", "Weiss", "Woo", "Yang", "Yates", "Yocum", "Zeaser", "Zeller", "Ziegler", "Bauer", "Baxster", "Casal", "Cataldi", "Caswell", "Celedon", "Chambers", "Chapman", "Christensen", "Darnell", "Davidson", "Davis", "DeLorenzo", "Dinkins", "Doran", "Dugelman", "Dugan", "Duffman", "Eastman", "Ferro", "Ferry", "Fletcher", "Fietzer", "Hylan", "Hydinger", "Illingsworth", "Ingram", "Irwin", "Jagtap", "Jenson", "Johnson", "Johnsen", "Jones", "Jurgenson", "Kalleg", "Kaskel", "Keller", "Leisinger", "LePage", "Lewis", "Linde", "Lulloff", "Maki", "Martin", "McGinnis", "Mills", "Moody", "Moore", "Napier", "Nelson", "Norquist", "Nuttle", "Olson", "Ostrander", "Reamer", "Reardon", "Reyes", "Rice", "Ripka", "Roberts", "Rogers", "Root", "Sandstrom", "Sawyer", "Schlicht", "Schmitt", "Schwager", "Schutz", "Schuster", "Tapia", "Thompson", "Tiernan", "Tisler"];
        this.listOfAge = ["20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59"];
        this.streetNumber = ["256", "232", "634", "714", "531", "934"];
        this.streetName = ["Colon street", "Escario street", "Gorordo street", "Sesame street", "Tabura street", "Almaciga street"];
        this.cityName = ["Cebu City", "Talisay City", "Mandaue City", "Naga City", "LapuLapu City", "Manila City", "Quezon City"];
        this.zipCode = ["6000", "6001", "6002", "6003", "6004", "6005"];
        this.listOfjobPosition = ["Computer Scientist", "IT Professional", "UX Designer & UI Developer", "SQL Developer", "Web Designer", "Web Developer", "Help Desk Worker/Desktop Support", "Software Engineer", "Data Entry", "DevOps Engineer", "Computer Programmer", "Network Administrator", "Information Security Analyst", "Artificial Intelligence Engineer", "Cloud Architect", "IT Manager", "Technical Specialist", "Application Developer", "Chief Technology Officer (CTO)", "Chief Information Officer (CIO)"];
        this.listOfDepartments = ["Office of the Vice President", "Housing and Urban Development Coordinating Council", "Executive Office", "Office of the Presidential Spokesperson", "Office of the Cabinet Secretary", "Department of Agrarian Reform", "Department of Agriculture", "Department of Budget and Management", "Department of Education", "Department of Energy", "Department of Environment and Natural Resources", "Department of Finance", "Department of Foreign Affairs", "Department of Health", "Department of the Interior and Local Government", "Department of Justice", "Department of Labor and Employment", "Department of National Defense", "Department of Public Works and Highways", "Department of Science and Technology", "Department of Social Welfare and Development", "Department of Tourism", "Department of Trade and Industry", "Department of Transportation and Communications"];
        this.listOfRandomNotes = ["anise ", "allspice", "basil", "bay leaves", "caraway seeds", "cardamom", "cayenne pepper", "chili powder", "chives", "cilantro", "cinnamon", "cloves", "coriander", "cumin", "curry powder", "dill weed", "dill seed", "fennel seed", "fenugreek", "garlic", "ginger", "ginseng", "mace", "marjoram", "mint", "mustard", "nutmeg", "oregano", "paprika", "parsley", "peppercorns", "poppy seeds", "red pepper flakes", "rosemary", "saffron", "sage", "savory", "tarragon", "thyme", "turmeric"];
    }
    EmployeeComponent.prototype.ngOnInit = function () {
        this.getEmployeeList();
        this.isAddButtonClicked = false;
    };
    EmployeeComponent.prototype.getEmployeeList = function () {
        var _this = this;
        this._employeeService.getEmployees().then(function (res) {
            if (res.isSuccess) {
                _this.employeeList = res.data;
            }
        });
    };
    EmployeeComponent.prototype.saveEmployee = function () {
        var _this = this;
        this._employeeService.saveEmployee(this.model).then(function (res) {
            if (res.isSuccess) {
                _this.toastr.success("Successfully created the employee.", "Success!");
                _this.getEmployeeList();
                _this.isAddButtonClicked = false;
                _this.model = null;
                _this.model = new employee_1.EmployeeList();
            }
            else {
                _this.toastr.error(res.message);
                _this.isAddButtonClicked = true;
            }
        });
    };
    EmployeeComponent.prototype.generateRandomData = function () {
        this.generateFirstName();
        this.generateAge();
        this.generateAddress();
        this.generatePosition();
        this.generateDepartment();
        this.generateNotes();
    };
    EmployeeComponent.prototype.generateFirstName = function () {
        this.model.fullName = this.listOfFirstNames[Math.floor(Math.random() * this.listOfFirstNames.length)] + " " + this.listOfLastNames[Math.floor(Math.random() * this.listOfLastNames.length)];
    };
    EmployeeComponent.prototype.generateAge = function () {
        this.model.age = this.listOfAge[Math.floor(Math.random() * this.listOfAge.length)];
    };
    EmployeeComponent.prototype.generateAddress = function () {
        var template = [this.streetNumber, " ", this.streetName, ", ", this.cityName, ", ", this.zipCode];
        this.model.address = template.map(getRandomElement).join("");
        function getRandomElement(array) {
            if (array instanceof Array)
                return array[Math.floor(Math.random() * array.length)];
            else
                return array;
        }
    };
    EmployeeComponent.prototype.generatePosition = function () {
        this.model.position = this.listOfjobPosition[Math.floor(Math.random() * this.listOfjobPosition.length)];
    };
    EmployeeComponent.prototype.generateDepartment = function () {
        this.model.department = this.listOfDepartments[Math.floor(Math.random() * this.listOfDepartments.length)];
    };
    EmployeeComponent.prototype.generateNotes = function () {
        this.model.notes = this.listOfRandomNotes[Math.floor(Math.random() * this.listOfRandomNotes.length)];
        ;
    };
    EmployeeComponent.prototype.searchEmployees = function () {
    };
    EmployeeComponent = __decorate([
        core_1.Component({
            selector: 'app-employee',
            templateUrl: './employee.component.html',
            styleUrls: ['./employee.component.css']
        })
    ], EmployeeComponent);
    return EmployeeComponent;
}());
exports.EmployeeComponent = EmployeeComponent;
//# sourceMappingURL=employee.component.js.map