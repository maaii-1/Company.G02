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

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");

            var department = _departmentRepositry.GetById(id.Value);
            if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id: {id} Is Not Found!" });

            return View(department);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id == department.Id)
                {
                    var count = _departmentRepositry.Update(department);
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
            return View(department);
        }

        #region Update Dto
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, UpdateDepartmentDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var department = new Department()
        //        {
        //            Id = id,
        //            Name = model.Name,
        //            Code = model.Code,
        //            CreateAt = model.CreateAt
        //        };
        //        var count = _departmentRepositry.Update(department);
        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }

        //    }
        //    return View(model);
        //} 
        #endregion


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");

            var department = _departmentRepositry.GetById(id.Value);
            if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id: {id} Is Not Found!" });

            return View(department);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id == department.Id)
                {
                    var count = _departmentRepositry.Delete(department);
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
            return View(department);
        }

    }
}  
