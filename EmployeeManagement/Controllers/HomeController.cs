using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {

        private readonly IEmployeeRepo _employeeRepo;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepo employeeRepo,
                                IHostingEnvironment hostingEnvironment)
        {
            _employeeRepo = employeeRepo;
            this.hostingEnvironment = hostingEnvironment;
        }

        private readonly ILogger<HomeController> _logger;


        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public ViewResult Index()
        {
            //return View();

            var model = _employeeRepo.GetAllEmployee();
            return View(model);
        }

        
        public ViewResult Details(int ?id)
        {

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepo.GetEmployee(id??1),
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);

            //return View();
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();

        }


        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniquefilename = null;
                if (model.Photo != null)
                {
                    string upLoadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniquefilename = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;

                    string filePath = Path.Combine(upLoadsFolder, uniquefilename);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }


                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email= model.Email,
                    Department = model.Department,
                    PhotoPath = uniquefilename
                };
                _employeeRepo.Add(newEmployee);
                //Employee newemployee = _employeeRepo.Add(employee);
                return RedirectToAction("Details", new { id = newEmployee.Id });
            }
            else
            {
                return View();
            }
        }
    }
}
