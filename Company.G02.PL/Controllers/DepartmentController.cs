using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Threading.Tasks;

namespace Company.G02.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentRepositry;
        private readonly IMapper _mapper;

        // Ask CLR Create Object From DepartmentRepositry
        public DepartmentController(/*IDepartmentRepository departmentRepositry*/
                                      IUnitOfWork unitOfWork, 
                                      IMapper mapper)
        {
            //_departmentRepositry = departmentRepositry;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet] // GET: /Department/Index
        public async Task<IActionResult> Index()
        { 
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if(ModelState.IsValid)   // Server Side Validation
            {

                var department = _mapper.Map<Department>(model);

                 await _unitOfWork.DepartmentRepository.AddAsync(department);
                var count = await _unitOfWork.CompleteAsync();
                if(count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Details(int? id,string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id: {id} Is Not Found!" });
            
            return View(viewName, department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id");

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id: {id} Is Not Found!" });

            #region MM
            //var DeotDto = new CreateDepartmentDto()
            //{
            //    Code = department.Code,
            //    Name = department.Name,
            //    CreateAt = department.CreateAt
            //}; 
            #endregion

            var DeptDto = _mapper.Map<CreateDepartmentDto>(department);

            return View(DeptDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateDepartmentDto dept)
        {
            if (ModelState.IsValid)
            {
                #region MM
                //var department = new Department()
                //{
                //    Id = id,
                //    Code = dept.Code,
                //    Name = dept.Name,
                //    CreateAt = dept.CreateAt
                //}; 
                #endregion
                var department = _mapper.Map<Department>(dept);
                department.Id = id;

                 _unitOfWork.DepartmentRepository.Update(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(dept);
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
        public async Task<IActionResult> Delete(int? id)
        {
            #region ..
            //if (id is null) return BadRequest("Invalid Id");

            //var department = _departmentRepositry.GetById(id.Value);
            //if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id: {id} Is Not Found!" });

            //return View(department); 
            #endregion

            return await Details(id, "Delete");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, Department department)
        {
            //if (id != department.Id)
            //    return BadRequest();
            var dept = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (dept is null)
                return NotFound(new { StatusCode = 404, Message = $"Department with ID {id} was not found." });

            _unitOfWork.DepartmentRepository.Delete(dept);
            var count = await _unitOfWork.CompleteAsync();
            if (count > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

    }
}  
