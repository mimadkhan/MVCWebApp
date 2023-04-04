using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvcWebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MvcWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext, IMapper mapper)
        {
            _logger = logger;
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MappingObjects()
        {
            // Map List of objects
            /* List<Users> user = new List<Users>()
             {
                 new Users() { Id = 1, FirstName = "Saleem", LastName = "khan", Email = "saleem@yopmail.com", PhoneNumber = "03156446551"},
                 new Users() { Id = 2, FirstName = "Saqib", LastName = "khan", Email = "saqib@yopmail.com", PhoneNumber = "035866551"},
                 new Users() { Id = 3, FirstName = "Sajid", LastName = "Khan",Email = "sajid@yopmail.com", PhoneNumber = "031586946551"},
                 new Users() { Id = 4, FirstName = "John", LastName = "Doe", Email = "johndoe@gmail.com", PhoneNumber = "555-1234"},
                 new Users() { Id = 5, FirstName = "Jane", LastName = "Doe", Email = "janedoe@yahoo.com", PhoneNumber = "555-5678" }
             };
             var userViewModel = _mapper.Map<List<UserViewModel>>(user);*/
            // Map Single object
            var user = new Users() { Id = 1, FirstName = "Saleem", LastName = "khan", Email = "saleem@yopmail.com", PhoneNumber = "03156446551" };
            var userViewModel = _mapper.Map<UserViewModel>(user);
            return View(userViewModel);
        }

        public async Task<IActionResult> UserList()
        {
            List<User> users = await _appDbContext.Users.ToListAsync();
            List<UsersDetail> usersDetail = await _appDbContext.UsersDetails.ToListAsync();
            var userVMs = from u in users
                          join ud in usersDetail
                          on u.Id equals ud.UserId
                          select new UserVM
                          {
                              Id = u.Id,
                              Name = u.Name,
                              Email = u.Email,
                              Address = ud.Address,
                              City = ud.City,
                              PhoneNumber = ud.PhoneNumber,

                          };

            return View(userVMs);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userExist = await _appDbContext.Users.Include(x => x.UsersDetails).FirstOrDefaultAsync(u => u.Id == id);
            UserVM userDetail = new UserVM
            {
                Name = userExist.Name,
                Email = userExist.Email,
                City = userExist.UsersDetails.FirstOrDefault().City,
                Address = userExist.UsersDetails.FirstOrDefault().Address,
                PhoneNumber = userExist.UsersDetails.FirstOrDefault().PhoneNumber,

            };
            if (userDetail == null)
            {
                return NotFound();
            }
            return View(userDetail);
        }
        public IActionResult SaveData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveData([Bind("Id,Name,Email,PhoneNumber,Address,City,UserId")] UserVM model)
        {
            if (ModelState.IsValid)
            {
                var userObj = new User
                {
                    Name = model.Name,
                    Email = model.Email
                };
                _appDbContext.Users.Add(userObj);
                await _appDbContext.SaveChangesAsync();
                var userId = _appDbContext.Users.OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();
                var userDetailObj = new UsersDetail
                {
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    City = model.City,
                    UserId = userId
                };
                _appDbContext.UsersDetails.Add(userDetailObj);
                await _appDbContext.SaveChangesAsync();
                return RedirectToAction("UserList");
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
