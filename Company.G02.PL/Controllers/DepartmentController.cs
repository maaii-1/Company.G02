using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Company.G02.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepositry;

        // Ask CLR Create Object From DepartmentRepositry
        public DepartmentController(IDepartmentRepository departmentRepositry)
        {
            _departmentRepositry = departmentRepositry;
        }
        [HttpGet] // GET: /Department/Index
        public IActionResult Index()
        { 
            var departments = _departmentRepositry.GetAll();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if(ModelState.IsValid)   // Server Side Validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };

                var count = _departmentRepositry.Add(department);
                if(count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");

            var department = _departmentRepositry.GetById(id.Value);
            if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id: {id} Is Not Found!" });
            
            return View(department);
        }
    }
}
