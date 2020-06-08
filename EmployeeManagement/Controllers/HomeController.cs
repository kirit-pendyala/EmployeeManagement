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
using Microsoft.AspNetCore.Http;

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
            Employee employee = _employeeRepo.GetEmployee(id.Value);
            if(employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
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

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepo.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                Mobile = employee.Mobile,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);

        }




        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepo.GetEmployee(model.Id);

                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                employee.Mobile = model.Mobile;
                if(model.Photo != null)
                {
                    if(model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);
                }
                
                _employeeRepo.Update(employee);
                //Employee newemployee = _employeeRepo.Add(employee);
                return RedirectToAction("index");
            }
            else
            {
                return View();
            }
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniquefilename = null;
            if (model.Photo != null)
            {
                string upLoadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniquefilename = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;

                string filePath = Path.Combine(upLoadsFolder, uniquefilename);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }
            return uniquefilename;
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniquefilename = null;
                if (model.Photo != null )
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
