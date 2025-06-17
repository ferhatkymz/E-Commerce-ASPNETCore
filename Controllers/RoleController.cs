using System.Threading.Tasks;
using EF_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EF_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        public RoleController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public ActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new Role { Name = model.RoleName });
                if (result.Succeeded)
                {
                    TempData["message"] = $"Yeni bir rol olan {model.RoleName} eklendi";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            var entity = await _roleManager.FindByIdAsync(id);

            if (entity != null)
            {
                return View(new RoleEditModel { Id = entity.Id, RoleName = entity.Name! });
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult> Edit(RoleEditModel model, string id)
        {
            if (ModelState.IsValid)
            {
                var entity = await _roleManager.FindByIdAsync(id);
                if (entity != null)
                {
                    entity.Name = model.RoleName;

                    var result = await _roleManager.UpdateAsync(entity);
                    if (result.Succeeded)
                    {
                        TempData["message"] = $"{entity.Id} numaralı Id sahip rolün adı {model.RoleName} olarak güncellendi";
                        return RedirectToAction("Index");
                    }
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View();
        }

        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }
            var entity = await _roleManager.FindByIdAsync(id);
            if (entity != null)
            {
                ViewBag.Users = await _userManager.GetUsersInRoleAsync(entity.Name!);

                return View(entity);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirm(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }
            var entity = await _roleManager.FindByIdAsync(id);
            if (entity != null)
            {
                await _roleManager.DeleteAsync(entity);
                TempData["message"] = $"{entity.Name} rölü silindi";
            }
            return RedirectToAction("Index");
        }
    }
}