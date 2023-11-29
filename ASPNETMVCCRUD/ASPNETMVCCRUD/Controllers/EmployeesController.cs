using ASPNETMVCCRUD.Data;
using ASPNETMVCCRUD.Models;
using ASPNETMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ASPNETMVCCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext _context;
        public EmployeesController(MVCDemoDbContext mVCDemoDbContext)
        {
            this._context = mVCDemoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeViewModel)
        {
            if(addEmployeeViewModel == null)
            {
                return Ok("Has No Data");
            }
            else
            {
                var employee = new Employee()
                {
                    Id = Guid.NewGuid(),
                    Name = addEmployeeViewModel.Name,
                    Salary = addEmployeeViewModel.Salary,
                    Email = addEmployeeViewModel.Email,
                    Department = addEmployeeViewModel.Department,
                    DateOfBirth = addEmployeeViewModel.DateOfBirth,
                };
                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Add");
            }
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id) 
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if(employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Salary = employee.Salary,
                    Email = employee.Email,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };
                return await Task.Run(() => View("View", viewModel));
                // await Task.Run(() => 
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel updateEmployeeViewModel)
        {
            var employee = await _context.Employees.FindAsync(updateEmployeeViewModel.Id);
            if (employee != null)
            {
                employee.Name = updateEmployeeViewModel.Name;
                employee.Email = updateEmployeeViewModel.Email;
                employee.Salary = updateEmployeeViewModel.Salary;
                employee.DateOfBirth = updateEmployeeViewModel.DateOfBirth;
                employee.Department = updateEmployeeViewModel.Department;

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel updateEmployeeViewModel)
        {
            var employee = await _context.Employees.FindAsync(updateEmployeeViewModel.Id);
            if(employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
