using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Company.G02.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Company.G02.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUSer> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUSer> userManager)
        {

            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToReturnDto> roles;
            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManager.Roles.Select(R => new RoleToReturnDto()
                {
                    Id = R.Id,
                    Name = R.Name,

                });
            }
            else
            {
                roles = _roleManager.Roles.Select(R => new RoleToReturnDto()
                {
                    Id = R.Id,
                    Name = R.Name,

                }).Where(R => R.Name.ToLower().Contains(SearchInput.ToLower()));
            }

            return View(roles);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturnDto roleDto)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(roleDto.Name);
                if(role is null)
                {
                    role = new IdentityRole() 
                    {
                        Name = roleDto.Name,
                    };
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(roleDto);
        }



        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var role = await _roleManager.FindByIdAsync(id);
            if (role is null) return NotFound(new { StatusCode = 404, message = $"Role with Id {id} does not exist. " });

            var dto = new RoleToReturnDto()
            {
                Id = role.Id,
                Name = role.Name,
            };

            return View(viewName, dto);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturnDto roleDto)
        {
            if (ModelState.IsValid)
            {
                if (id != roleDto.Id)
                    return BadRequest("Invalid Id!");
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null)
                    return BadRequest("Invalid Id!");

                var roleResult = await _roleManager.FindByNameAsync(roleDto.Name);
                if (roleResult is null)
                {
                    role.Name = roleDto.Name;

                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                ModelState.AddModelError("", "Invalid Operation");

            }
            return View(roleDto);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDto roleDto)
        {
            if (id != roleDto.Id)
                return BadRequest("Invalid Id!");
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return BadRequest("Invalid Id!");

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            
            ModelState.AddModelError("", "Invalid Operation");

            return View(roleDto);
        }


        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string id )
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();

            ViewData["RoleId"] = id;


            var usersInRole = new List<UsersInRoleDto>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userInRole = new UsersInRoleDto()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };

                if( await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }
                usersInRole.Add(userInRole);

            }
            return View(usersInRole);

        }


        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string id, List<UsersInRoleDto> users)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if(appUser is not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                    }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                }

                return RedirectToAction(nameof(Edit), new { id = id });

            }
            return View(users);

        }

    }
}
