using Company.G02.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeController(IEmployeeRepository _employeeRepository)
        {
            employeeRepository = _employeeRepository;
        }
        public IActionResult Index()
        {
            var employees = employeeRepository.GetAll();
            return View(employees);
        }
    }
}
