using System.Threading.Tasks;
using EF_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EF_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly DataContext _context;

        public SliderController(DataContext db)
        {
            _context = db;
        }
        public ActionResult Index()
        {
            var sliders = _context.Sliders.Select(s => new SliderGetModel
            {
                Id = s.Id,
                Title = s.Title,
                Img = s.img,
                isActive = s.isActive,
                Index = s.Index
            }).ToList();
            return View(sliders);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(SliderCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var fileName = Path.GetRandomFileName() + ".jpg";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.ImgFile!.CopyToAsync(stream);
                }

                _context.Sliders.Add(new Slider
                {
                    Title = model.Title,
                    Description = model.Description,
                    img = fileName,
                    Index = model.Index,
                    isActive = model.isActive

                });
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var slider = _context.Sliders.Select(s => new SliderDeleteModel
            {
                Id = s.Id,
                Title = s.Title
            }).FirstOrDefault(s => s.Id == id);

            if (slider != null)
            {
                return View(slider);
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
            var slider = _context.Sliders.Select(s => new SliderDeleteModel
            {
                Id = s.Id,
                Title = s.Title
            }).FirstOrDefault(s => s.Id == id);

            if (slider != null)
            {
                _context.Sliders.Remove(_context.Sliders.FirstOrDefault(s => s.Id == id)!);
                _context.SaveChanges();
                TempData["message"] = $"{id} Numaralı Slider Silindi";
            }
            return RedirectToAction("Index");

        }

        public ActionResult Edit(int id)
        {
            var slider = _context.Sliders.Select(s => new SliderEditModel
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                isActive = s.isActive,
                Index = s.Index,
                Img = s.img,
                EditFile = null
            }).FirstOrDefault(s => s.Id == id);

            if (slider == null)
                return RedirectToAction("Index");

            return View(slider);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(int id, SliderEditModel model)
        {
            if (ModelState.IsValid || model.EditFile == null)
            {

                var slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
                if (slider != null)
                {
                    if (model.EditFile != null)//eğer yerelde bir resim seçilmişse
                    {
                        var fileName = Path.GetRandomFileName() + ".jpg";//adı rastgele bir isim olan ve uzantısı jpg olan bir isim tanımla 
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);//ilgili proje altındaki wwwroot altındaki img konumlandır

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.EditFile!.CopyToAsync(stream);//resmi ilgili konuma asenkron bir şekilde oluştur
                        }

                        slider.img = fileName;
                    }

                    slider.Title = model.Title;
                    slider.Description = model.Description;
                    slider.Index = model.Index;
                    slider.isActive = model.isActive;

                    _context.SaveChanges();
                    TempData["message"] = $"{slider.Id} numaralı Id'ye sahip sliderın bilgileri yönetici tarafından güncellendi";
                }
                return RedirectToAction("Index");
            }
            return View(model);

        }
    }
}