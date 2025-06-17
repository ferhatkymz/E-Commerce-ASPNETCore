using EF_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EF_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;
        public CategoryController(DataContext context)
        {
            _dataContext = context;
        }
        public ActionResult Index()
        {
            var Categories = _dataContext.Categories.Select
            (
                c => new CategoryGetModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Url = c.URL,
                    Count = c.Products.Count
                }
            ).ToList();
            return View(Categories);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CategoryCreateModel model)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Categories.Add(new Category
                {
                    Name = model.Name,
                    URL = model.Url
                });
                _dataContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var selectCategory = _dataContext.Categories.Select(c => new CategoryEditModel
            {
                Id = c.Id,
                Name = c.Name,
                Url = c.URL
            }).FirstOrDefault(c => c.Id == id);
            return View(selectCategory);
        }

        [HttpPost]
        public ActionResult Edit(int id, CategoryEditModel model)
        {
            if (id != model.Id)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {

                var entity = _dataContext.Categories.FirstOrDefault(c => c.Id == id);
                if (entity != null)
                {
                    entity.Name = model.Name;
                    entity.URL = model.Url;
                    _dataContext.SaveChanges();
                    TempData["message"] = $"{entity.Id} numaralı Kategorinin adı {entity.Name} olarak güncellendi";
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var category = _dataContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {
                return View(category);
                // _dataContext.Categories.Remove(category);
                // _dataContext.SaveChanges();
                // TempData["message"] = $"{id} Numaralıya ait Kategori Silindi";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]

        public ActionResult DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var category = _dataContext.Categories.FirstOrDefault(c => c.Id == id);
            if (category != null)
            {

                _dataContext.Categories.Remove(category);
                _dataContext.SaveChanges();
                TempData["message"] = $"{id} Numaralıya ait Kategori Silindi";
            }

            return RedirectToAction("Index");
        }
    }


}