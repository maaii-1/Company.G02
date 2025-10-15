using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Company.G02.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.G02.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(/*IEmployeeRepository employeeRepository,*/ 
                                  //IDepartmentRepository departmentRepository, 
                                  IUnitOfWork unitOfWork,
                                  IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
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
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateEmployeeDto emp)
        {
            if (ModelState.IsValid)  
            {
                #region Manual Mapping
                //var employee = new Employee()
                //{
                // Name = emp.Name,
                // Age = emp.Age, 
                // Email = emp.Email,
                // Phone = emp.Phone,
                // Address = emp.Address, 
                // Salary = emp.Salary,
                // CreateAt = emp.CreateAt,
                // HiringDate = emp.HiringDate,
                // IsActive = emp.IsActive,
                // IsDeleted = emp.IsDeleted,
                // WorkForId = emp.WorkForId,
                //}; 
                #endregion

                if (emp.Image is not null)
                {
                    emp.ImageName = DocumentSettings.UploadFile(emp.Image, "images");
                }

                var employee = _mapper.Map<Employee>(emp);
                await _unitOfWork.EmployeeRepository.AddAsync(employee);

                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {

                    TempData["Message"] = "Employee created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(emp);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, message = $"Employee with Id {id} does not exist. " });

            return View(viewName, employee);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //var departments = _departmentRepository.GetAll();
            //ViewData["departments"] = departments;

            if (id is null) return BadRequest("Invalid Id");
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, message = $"Employee with Id {id} does not exist. " });
            #region MM
            //var employeeDto = new CreateEmployeeDto()
            //{

            //    Name = employee.Name,
            //    Age = employee.Age,
            //    Email = employee.Email,
            //    Phone = employee.Phone,
            //    Address = employee.Address,
            //    Salary = employee.Salary,
            //    CreateAt = employee.CreateAt,
            //    HiringDate = employee.HiringDate,
            //    IsActive = employee.IsActive,
            //    IsDeleted = employee.IsDeleted,

            //}; 
            #endregion
            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(employeeDto);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto empDto)
        {
            if (ModelState.IsValid)
            {
                if(empDto.ImageName is not null && empDto.Image is not null)
                {
                    DocumentSettings.DeleteFile(empDto.ImageName, "images");
                }
                if (empDto.Image is not null)
                {
                    empDto.ImageName = DocumentSettings.UploadFile(empDto.Image, "images");
                }

                #region MM
                //var employee = new Employee()
                //{
                //    Id = id,
                //    Name = empDto.Name,
                //    Age = empDto.Age,
                //    Email = empDto.Email,
                //    Phone = empDto.Phone,
                //    Address = empDto.Address,
                //    Salary = empDto.Salary,
                //    CreateAt = empDto.CreateAt,
                //    HiringDate = empDto.HiringDate,
                //    IsActive = empDto.IsActive,
                //    IsDeleted = empDto.IsDeleted,
                //    WorkForId = empDto.WorkForId,
                //}; 
                #endregion
                var employee = _mapper.Map<Employee>(empDto);
                employee.Id = id;
                 _unitOfWork.EmployeeRepository.Update(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(empDto);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();

            var emp = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (emp is null)
                return NotFound();

            _unitOfWork.EmployeeRepository.Delete(emp);
            var count = await _unitOfWork.CompleteAsync();
            if (count > 0)
            {
                if (employee.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(employee.ImageName, "images");
                }
                return RedirectToAction(nameof(Index));
            }

            return View(emp);
        }

    }
}
