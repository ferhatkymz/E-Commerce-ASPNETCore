using EF_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EF_MVC.Controllers;

[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly DataContext Context;//Dependecy Injection => DI
    public ProductController(DataContext _Context)
    {
        this.Context = _Context;
    }

    public ActionResult Index(int? kategori, string q)
    {
        var query = Context.Products.AsQueryable();
        if (kategori != null)//eğer kategori seçilmişse
        {
            query = query.Where(c => c.CategoryId == kategori);
        }
        if (q != null)//eğer bir ürün aranmışsa 
        {
            query = query.Where(c => c.Name!.ToLower().Contains(q.ToLower()));
        }
        var products = query.Select
        (
            p => new ProductGetModel
            {
                Id = p.Id,
                img = p.img,
                Name = p.Name,
                Price = p.Price,
                Discount = p.Discount,
                isActive = p.isActive,
                isHome = p.isHome,
                CategoryName = p.Category.Name
            }
        ).ToList();
        ViewBag.Categories = new SelectList(Context.Categories.ToList(), "Id", "Name", kategori);
        ViewData["q"] = q;
        return View(products);
    }
    [AllowAnonymous]
    public ActionResult List(string url, string q)
    {
        var query = Context.Products.Where(i => i.isActive);
        if (!string.IsNullOrEmpty(url))
        {
            query = query.Where(i => i.Category.URL == url);
        }
        if (!string.IsNullOrEmpty(q))
        {
            query = query.Where(i => i.Name!.ToLower().Contains(q.ToLower()));
            ViewData["q"] = q;
        }

        return View(query.ToList());

    }

    [AllowAnonymous]

    public ActionResult Details(int id)
    {
        // var product=this.Context.Products.FirstOrDefault(p=>p.Id==id); eğer birden fazla kayıt içerisinden ilk kaydı istersem bu mantıklı
        var product = Context.Products.Find(id);
        if (product == null)
        {
            return RedirectToAction("List", "Product");
        }
        ViewData["SimilarProducts"] = Context.Products.Where(p => p.isActive == true && p.CategoryId == product.CategoryId && p.Id != id).Take(4).ToList();
        ViewData["productImages"] = Context.ProductDetails.Where(p => p.ProductId == id).ToList();
        return View(product);
    }

    [HttpGet]
    public ActionResult Create()
    {
        ViewBag.categories = new SelectList(Context.Categories.ToList(), "Id", "Name");
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(ProductCreateModel model)
    {
        if (model.ImgFile == null || model.ImgFile2 == null || model.ImgFile3 == null)
        {
            ModelState.AddModelError("", "En az 3 Resim Seçiniz");
        }
        if (ModelState.IsValid)
        {
            var fileName = Path.GetRandomFileName() + ".jpg";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.ImgFile!.CopyToAsync(stream);
            }
            var fileName2 = Path.GetRandomFileName() + ".jpg";
            var path2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName2);

            using (var stream = new FileStream(path2, FileMode.Create))
            {
                await model.ImgFile2!.CopyToAsync(stream);
            }

            var fileName3 = Path.GetRandomFileName() + ".jpg";
            var path3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName3);

            using (var stream = new FileStream(path3, FileMode.Create))
            {
                await model.ImgFile3!.CopyToAsync(stream);
            }




            Context.Products.Add
            (
                new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    isActive = model.isActive,
                    isHome = model.isHome,
                    img = fileName,
                    Discount = model.Discount,
                    CategoryId = model.CategoryId,
                    productDetails = new List<ProductDetail>
                    {
                        new ProductDetail{Img=fileName},
                        new ProductDetail{Img=fileName2},
                        new ProductDetail{Img=fileName3},

                     }


                }
            );
            Context.SaveChanges();
            return RedirectToAction("Index");
        }
        ViewBag.categories = new SelectList(Context.Categories.ToList(), "Id", "Name");
        return View(model);

    }

    public ActionResult Edit(int id)
    {
        var product = Context.Products.Select(p => new ProductEditModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            ImgName = p.img,
            Description = p.Description,
            isActive = p.isActive,
            Discount = p.Discount,
            isHome = p.isHome,
            CategoryId = p.CategoryId
        }).FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return RedirectToAction("Index");
        }
        ViewBag.categories = new SelectList(Context.Categories.ToList(), "Id", "Name");
        return View(product);
    }

    [HttpPost]
    public async Task<ActionResult> Edit(int id, ProductEditModel model)
    {
        if (id != model.Id)
        {
            return View("Index");
        }
        if (ModelState.IsValid)
        {

            var entity = Context.Products.FirstOrDefault(p => p.Id == model.Id);

            if (entity != null)
            {
                if (model.ImgFile != null)//eğer yerelde bir resim seçilmişse
                {
                    var fileName = Path.GetRandomFileName() + ".jpg";//adı rastgele bir isim olan ve uzantısı jpg olan bir isim tanımla 
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);//ilgili proje altındaki wwwroot altındaki img konumlandır

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImgFile!.CopyToAsync(stream);//resmi ilgili konuma asenkron bir şekilde oluştur
                    }

                    entity.img = fileName;
                }
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Price = model.Price;
                entity.isActive = model.isActive;
                entity.Discount = model.Discount;
                entity.isHome = model.isHome;
                entity.CategoryId = model.CategoryId;
                Context.SaveChanges();
                TempData["message"] = $"{entity.Id} numaralı Id'ye sahip ürünün bilgileri yönetici tarafından güncellendi";
                return RedirectToAction("Index");

            }
        }
        ViewBag.categories = new SelectList(Context.Categories.ToList(), "Id", "Name");
        return View(model);
    }

    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return RedirectToAction("Index");
        }
        var p = Context.Products.Select(p => new ProductDeleteModel
        {
            Id = p.Id,
            Name = p.Name
        }).FirstOrDefault(p => p.Id == id);
        if (p != null)
        {
            return View(p);
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
        var p = Context.Products.Select(p => new ProductDeleteModel
        {
            Id = p.Id,
            Name = p.Name
        }).FirstOrDefault(p => p.Id == id);
        if (p != null)
        {
            Context.Products.Remove(Context.Products.Where(p => p.Id == id).FirstOrDefault()!);
            Context.SaveChanges();
            TempData["message"] = $"{id} Numaralıya ait Ürün Silindi";
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }
}