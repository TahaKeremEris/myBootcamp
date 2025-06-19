using Microsoft.AspNetCore.Identity;
using BootcampDay1.Models; // ApplicationUser ve ProfileViewModel burada olmalı
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;

namespace BootcampDay1.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public ProfileController(UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? editMode)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            var model = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.Name,
                LastName = user.Surname,
                Profession = user.Job,
                CurrentImagePath = user.ProfileImagePath
            };

            ViewBag.EditMode = !string.IsNullOrEmpty(editMode) && editMode == "true";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileViewModel model, IFormFile? ProfileImage, string? editMode)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Challenge();

            bool isEditMode = !string.IsNullOrEmpty(editMode) && editMode == "true";

            if (isEditMode && ModelState.IsValid)
            {
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ProfileImage.CopyToAsync(fileStream);
                    }

                    user.ProfileImagePath = "/uploads/" + uniqueFileName;
                }

                user.UserName = model.UserName;
                user.Email = model.Email;
                user.Name = model.FirstName;
                user.Surname = model.LastName;
                user.Job = model.Profession;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    foreach (var err in result.Errors)
                        ModelState.AddModelError("", err.Description);

                    ViewBag.EditMode = true;
                    return View(model);
                }

                TempData["Message"] = "Profil başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            // Düzenleme butonuna basıldıysa veya validation hatası varsa düzenleme moduna geç
            ViewBag.EditMode = true;
            return View(model);
        }
    }
}
