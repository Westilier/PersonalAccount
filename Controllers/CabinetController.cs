using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using PersonalAccount.Models;
using PersonalAccount.Models.Students;
using PersonalAccount.Services;
using PersonalAccount.Services.Cabinet;
using PersonalAccount.Utils;

namespace PersonalAccount.Controllers;

[Authorize]
public class CabinetController(IStudentCabinetService cabinet, IStudentService studentService, IPasswordService password) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var id = User.GetId();
        if  (id is null) return RedirectToAction("Error", "Home"); 
        var student = await cabinet.GetStudentByIdAsync(id.Value);
        return View(student);
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var id = User.GetId();
        if (id is null) return RedirectToAction("Error", "Home");
        var student = await cabinet.GetStudentByIdAsync(id.Value);
        if (student is null) return RedirectToAction("Error", "Home");

        return View(new StudentEditViewModel
        {
            FullName = student.FullName,
            GroupName = student.GroupName,
            PhotoUrl = student.PhotoUrl?.ToString()
        });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(StudentEditViewModel model)
    {
        if (model == null) return RedirectToAction("Error", "Home");

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var id = User.GetId();
        if (id is null) return RedirectToAction("Error", "Home");

        await studentService.UpdateByIdAsync(id.Value, new StudentModel()
        {
            FullName = model.FullName,
            GroupName = model.GroupName,
            PhotoUrl = model.PhotoUrl?.ToUri()
        });

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View(new PasswordChangeViewModel());
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(PasswordChangeViewModel model)
    {
        var id = User.GetId();
        if (id is null) return RedirectToAction("Error", "Home");

        if (model == null) return RedirectToAction("Error", "Home");

        if (model.OldPassword == model.NewPassword)
            ModelState.AddModelError(string.Empty, "Новый пароль должен отличаться от старого пароля");

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!await password.ValidatePasswordAsync(id.Value, model.OldPassword))
        {
            ModelState.AddModelError(string.Empty, "Старый пароль введен неверно");
            return View(model);
        }

        await password.UpdatePasswordAsync(id.Value, model.NewPassword);
        return RedirectToAction("Index");
    }
}