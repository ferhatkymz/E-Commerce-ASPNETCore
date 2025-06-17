using System.Threading.Tasks;
using EF_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EF_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<ActionResult> Index(string role)
        {
            if (!string.IsNullOrEmpty(role))
            {
                ViewBag.roles = new SelectList(_roleManager.Roles, "Name", "Name", role);
                return View(await _userManager.GetUsersInRoleAsync(role));
            }
            var users = await _userManager.Users.ToListAsync();
            ViewBag.roles = new SelectList(_roleManager.Roles, "Name", "Name", role);
            return View(users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, FullName = model.FullName, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    TempData["message"] = "Yeni bir Kullanıcı Eklendi";
                    return RedirectToAction("Index");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {

            var result = await _userManager.FindByIdAsync(id);
            if (result == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.roles = await _roleManager.Roles.Select(i => i.Name).ToListAsync();
            return View(new UserEditModel { FullName = result.FullName, Email = result.Email!, SelectedRoles = await _userManager.GetRolesAsync(result) });
        }
        [HttpPost]
        public async Task<ActionResult> Edit(string id, UserEditModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.FullName = model.FullName;
                    user.Email = model.Email;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded && !string.IsNullOrEmpty(model.Password))
                    {
                        //parolayıda güncelle
                        await _userManager.RemovePasswordAsync(user);
                        await _userManager.AddPasswordAsync(user, model.Password);
                    }
                    if (result.Succeeded)
                    {
                        await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                        if (model.SelectedRoles != null)
                        {
                            await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                        }
                        return RedirectToAction("Index");
                    }

                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            ViewBag.roles = await _roleManager.Roles.Select(i => i.Name).ToListAsync();
            return View(model);
        }


        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirm(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["message"] = "Kullanıcı Başarılı bir şekilde silindi";
                    return RedirectToAction("Index");
                }

            }
            return RedirectToAction("Index");
        }
    }
}