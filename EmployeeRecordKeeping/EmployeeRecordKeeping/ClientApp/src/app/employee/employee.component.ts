import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EmployeeService } from '../services/employee.service';
import { EmployeeList} from '../models/employee';
import { ToastrService } from 'ngx-toastr';
import { datatable} from 'datatables'
import { elasticListFilter, elasticSearchResultsSummary } from '../models/elasticsearch';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})
export class EmployeeComponent implements OnInit {
  public employeeList: any = [];
  public model: EmployeeList = new EmployeeList();
  public searchFilterModel: elasticListFilter = new elasticListFilter();
  public searchResults: any = [];
  public isAddButtonClicked: boolean;
  public dataToDisplay: any;
  public $employeeDataTable: any;
  public hasNoResults: boolean = false;

  public filterType = [
    {
      "id": "0",
      "value": "All"
    },
    {
      "id": "2",
      "value": "Smart search"
    },
    {
      "id": "3",
      "value": "Highlight match keywords"
    },
    {
      "id": "4",
      "value": "Must not match"
    }] 

  public listOfFirstNames = ["Adam", "Alex", "Aaron", "Ben", "Carl", "Dan", "David", "Edward", "Fred", "Frank", "George", "Hal", "Hank", "Ike", "John", "Jack", "Joe", "Larry", "Monte", "Matthew", "Mark", "Nathan", "Otto", "Paul", "Peter", "Roger", "Roger", "Steve", "Thomas", "Tim", "Ty", "Victor","Walter"];
  public listOfLastNames = ["Anderson", "Ashwoon", "Aikin", "Bateman", "Bongard", "Bowers", "Boyd", "Cannon", "Cast", "Deitz", "Dewalt", "Ebner", "Frick", "Hancock", "Haworth", "Hesch", "Hoffman", "Kassing", "Knutson", "Lawless", "Lawicki", "Mccord", "McCormack", "Miller", "Myers", "Nugent", "Ortiz", "Orwig", "Ory", "Paiser", "Pak", "Pettigrew", "Quinn", "Quizoz", "Ramachandran", "Resnick", "Sagar", "Schickowski", "Schiebel", "Sellon", "Severson", "Shaffer", "Solberg", "Soloman", "Sonderling", "Soukup", "Soulis", "Stahl", "Sweeney", "Tandy", "Trebil", "Trusela", "Trussel", "Turco", "Uddin", "Uflan", "Ulrich", "Upson", "Vader", "Vail", "Valente", "Van Zandt", "Vanderpoel", "Ventotla", "Vogal", "Wagle", "Wagner", "Wakefield", "Weinstein", "Weiss", "Woo", "Yang", "Yates", "Yocum", "Zeaser", "Zeller", "Ziegler", "Bauer", "Baxster", "Casal", "Cataldi", "Caswell", "Celedon", "Chambers", "Chapman", "Christensen", "Darnell", "Davidson", "Davis", "DeLorenzo", "Dinkins", "Doran", "Dugelman", "Dugan", "Duffman", "Eastman", "Ferro", "Ferry", "Fletcher", "Fietzer", "Hylan", "Hydinger", "Illingsworth", "Ingram", "Irwin", "Jagtap", "Jenson", "Johnson", "Johnsen", "Jones", "Jurgenson", "Kalleg", "Kaskel", "Keller", "Leisinger", "LePage", "Lewis", "Linde", "Lulloff", "Maki", "Martin", "McGinnis", "Mills", "Moody", "Moore", "Napier", "Nelson", "Norquist", "Nuttle", "Olson", "Ostrander", "Reamer", "Reardon", "Reyes", "Rice", "Ripka", "Roberts", "Rogers", "Root", "Sandstrom", "Sawyer", "Schlicht", "Schmitt", "Schwager", "Schutz", "Schuster", "Tapia", "Thompson", "Tiernan", "Tisler" ];
  public listOfAge = ["20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59"];
  public streetNumber = ["256", "232", "634", "714", "531", "934"];
  public streetName = ["Colon street", "Escario street", "Gorordo street", "Sesame street", "Tabura street", "Almaciga street"];
  public cityName = ["Cebu City", "Talisay City", "Mandaue City", "Naga City", "LapuLapu City", "Manila City", "Quezon City"];
  public zipCode = ["6000", "6001", "6002", "6003", "6004", "6005"];
  public listOfjobPosition = ["Computer Scientist", "IT Professional", "UX Designer & UI Developer", "SQL Developer", "Web Designer", "Web Developer", "Help Desk Worker/Desktop Support", "Software Engineer", "Data Entry", "DevOps Engineer", "Computer Programmer", "Network Administrator", "Information Security Analyst", "Artificial Intelligence Engineer", "Cloud Architect", "IT Manager", "Technical Specialist", "Application Developer", "Chief Technology Officer (CTO)", "Chief Information Officer (CIO)"];
  public listOfDepartments = ["Office of the Vice President", "Housing and Urban Development Coordinating Council", "Executive Office", "Office of the Presidential Spokesperson", "Office of the Cabinet Secretary", "Department of Agrarian Reform", "Department of Agriculture", "Department of Budget and Management", "Department of Education", "Department of Energy", "Department of Environment and Natural Resources", "Department of Finance", "Department of Foreign Affairs", "Department of Health", "Department of the Interior and Local Government", "Department of Justice", "Department of Labor and Employment", "Department of National Defense", "Department of Public Works and Highways", "Department of Science and Technology", "Department of Social Welfare and Development", "Department of Tourism", "Department of Trade and Industry", "Department of Transportation and Communications"];
  public listOfRandomNotes = ["anise ", "allspice", "basil", "bay leaves", "caraway seeds", "cardamom","cayenne pepper", "chili powder", "chives", "cilantro", "cinnamon","cloves", "coriander", "cumin", "curry powder", "dill weed", "dill seed","fennel seed", "fenugreek", "garlic", "ginger", "ginseng", "mace","marjoram", "mint", "mustard", "nutmeg", "oregano", "paprika", "parsley","peppercorns", "poppy seeds", "red pepper flakes", "rosemary", "saffron","sage", "savory", "tarragon", "thyme", "turmeric"];

  constructor(private _employeeService: EmployeeService, private toastr: ToastrService) {
   
  }
  ngOnInit(): void {
    this.getEmployeeList();
    this.isAddButtonClicked = false;
    this.searchResults = new elasticSearchResultsSummary();
    this.searchFilterModel.filterTypeId = "0";
    //$('#dtBasicExample').dataTable();
  }

  getEmployeeList() {
    this._employeeService.getEmployees().then((res) => {
      if (res.isSuccess) {
        this.employeeList = res.data;
        this.loadDataTable();
      }
    });
  } 
  loadDataTable() {
    this.$employeeDataTable = $('#employeeDataTable').DataTable({
      columns: [
        { data: 'employeeid', title: 'Employee ID' },
        { data: 'fullName', title: 'Full Name' },
        { data: 'address', title: 'Address' },
        { data: 'notes', title: 'Notes' },
        { data: 'age', title: 'Age' },
        { data: 'position', title: 'Position' },
        { data: 'department', title: 'Department' },
      ],
      processing: true,
      data: this.employeeList,
      responsive: true,
      paging: true,
      info: true,
      searching: false,
      scrollX: true
      
    });
  }
  destroyTable() {
    this.$employeeDataTable.destroy();
  }
  saveEmployee() {
    this._employeeService.saveEmployee(this.model).then((res) => {
      if (res.isSuccess) {
        this.toastr.success("Successfully created the employee.", "Success!");
        this.destroyTable();
        this.getEmployeeList();
        this.isAddButtonClicked = false;
        this.model = null;
        this.model = new EmployeeList();
      }
      else {
        this.toastr.error(res.message);
        this.isAddButtonClicked = true;
      }
    });
  }

  generateRandomData() {
    this.generateFirstName();
    this.generateAge();
    this.generateAddress();
    this.generatePosition();
    this.generateDepartment();
    this.generateNotes();
  }

  generateFirstName() {
    this.model.fullName = this.listOfFirstNames[Math.floor(Math.random() * this.listOfFirstNames.length)] + " " + this.listOfLastNames[Math.floor(Math.random() * this.listOfLastNames.length)];
  }

  generateAge() {
    this.model.age = this.listOfAge[Math.floor(Math.random() * this.listOfAge.length)];
  }

  generateAddress() {
    let template = [this.streetNumber, " ", this.streetName, ", ", this.cityName, ", ", this.zipCode];
    this.model.address = template.map(getRandomElement).join("");
    function getRandomElement(array) {
      if (array instanceof Array) return array[Math.floor(Math.random() * array.length)];
      else return array;
    }

  }

  generatePosition() {
    this.model.position = this.listOfjobPosition[Math.floor(Math.random() * this.listOfjobPosition.length)];
  }

  generateDepartment() {
    this.model.department = this.listOfDepartments[Math.floor(Math.random() * this.listOfDepartments.length)];
  }
  generateNotes() {
    this.model.notes = this.listOfRandomNotes[Math.floor(Math.random() * this.listOfRandomNotes.length)];;
  }

  searchEmployees() {
    this._employeeService.searchIndexedEmployee(this.searchFilterModel).then((res) => {
      if (res.isSuccess) {
        this.searchResults = res.data as elasticSearchResultsSummary;
        if (res.data.searchOutPut == null) {
          this.hasNoResults = true;
        }
        else {
          this.hasNoResults = false;
        }
      }
      else {
        this.toastr.error(res.message);
        this.hasNoResults = false;
      }
    });
  }


}

