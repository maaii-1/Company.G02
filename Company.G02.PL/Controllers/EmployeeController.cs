using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }


        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                 employees = _employeeRepository.GetAll();
            }
            else
            {
                 employees = _employeeRepository.GetByName(SearchInput);
            }
            #region Dictionary
            // Dictionary: 3 Properties
            // 1. ViewData : Transfer Extra Info From COntroller (Action) To View

            //ViewData["Message"] = "Hello From ViewData";

            // 2. ViewBag  : Transfer Extra Info From COntroller (Action) To View

            //ViewBag.Message = "Hello From ViewBag ";

            // 3. TempData : 
            #endregion
            return View(employees);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;
            return View();
        }


        [HttpPost]
        public IActionResult Create(CreateEmployeeDto emp)
        {
            if (ModelState.IsValid)  
            {
                var employee = new Employee()
                {
                 Name = emp.Name,
                 Age = emp.Age, 
                 Email = emp.Email,
                 Phone = emp.Phone,
                 Address = emp.Address, 
                 Salary = emp.Salary,
                 CreateAt = emp.CreateAt,
                 HiringDate = emp.HiringDate,
                 IsActive = emp.IsActive,
                 IsDeleted = emp.IsDeleted,
                 WorkForId = emp.WorkForId,
                };

                var count = _employeeRepository.Add(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(emp);
        }


        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var employee = _employeeRepository.GetById(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, message = $"Employee with Id {id} does not exist. " });

            return View(viewName, employee);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;

            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepository.GetById(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, message = $"Employee with Id {id} does not exist. " });
            var employeeDto = new CreateEmployeeDto()
            {
                
                Name = employee.Name,
                Age = employee.Age,
                Email = employee.Email,
                Phone = employee.Phone,
                Address = employee.Address,
                Salary = employee.Salary,
                CreateAt = employee.CreateAt,
                HiringDate = employee.HiringDate,
                IsActive = employee.IsActive,
                IsDeleted = employee.IsDeleted,
                
            };
            return View(employeeDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto empDto)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    Id = id,
                    Name = empDto.Name,
                    Age = empDto.Age,
                    Email = empDto.Email,
                    Phone = empDto.Phone,
                    Address = empDto.Address,
                    Salary = empDto.Salary,
                    CreateAt = empDto.CreateAt,
                    HiringDate = empDto.HiringDate,
                    IsActive = empDto.IsActive,
                    IsDeleted = empDto.IsDeleted,
                    WorkForId = empDto.WorkForId,
                };
                var count = _employeeRepository.Update(employee);
                if(count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(empDto);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (id == employee.Id)
                {
                    var count = _employeeRepository.Delete(employee);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            return View(employee);
        }

    }
}
