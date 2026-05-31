using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Models.ViewModels;
using PersonalAccount.Services.Auth;
using PersonalAccount.Services.Cabinet;
using PersonalAccount.Types;
using PersonalAccount.Utils;

namespace PersonalAccount.Controllers;

[Authorize]
public class CabinetController(
    IStudentCabinetService studentCabinet,
    IAdminCabinetService adminCabinet,
    IConfirmationTokenService confirmations) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var accountId = User.GetId();
        var role = User.GetRole();

        if (accountId == null || role == null)
            return RedirectToAction("Error", "Home");

        switch (role)
        {
            case AccountRole.Student:
                return RedirectToAction("Student");
            case AccountRole.Administrator:
                return RedirectToAction("Admin");
            case AccountRole.Teacher:
            default:
                return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Student()
    {
        var accountEmail = User.GetEmail();
        if (accountEmail is null)
            return RedirectToAction("Error", "Home");

        var accountId = User.GetId();
        if(accountId is null)
            return RedirectToAction("Error", "Home");

        var student = await studentCabinet.GetStudentAsync(accountId.Value);
        if (student is null) return RedirectToAction("Error", "Home");

        var isEmailConfirmed = await confirmations.HasConfirmedTokensAsync(student.Id);

        return View(new StudentCabinetViewModel
        {
            Email = accountEmail,
            GroupName = student.GroupName,
            FullName = student.FullName,
            IsEmailConfirmed = isEmailConfirmed,
            PhotoUrl = student.PhotoUrl?.ToString(),
        });
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Admin()
    {
        var accounts = await adminCabinet.GetAllStudentAccountsAsync();
        var profiles = await adminCabinet.GetAllStudentProfilesAsync();

        var studentInfos = new List<StudentInfoViewModel>();

        foreach (var profile in profiles)
        {
            var isConfirmed = await confirmations.HasConfirmedTokensAsync(profile.AccountId);

            studentInfos.Add(new StudentInfoViewModel
            {
                AccountId = profile.AccountId,
                Email = accounts[profile.AccountId].Email,
                FullName = profile.FullName,
                GroupName = profile.GroupName,
                PhotoUrl = profile.PhotoUrl?.ToString(),
                IsEmailConfirmed = isConfirmed
            });
        }

        return View(new AdminCabinetViewModel
        {
            StudentInfos = studentInfos
        });
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmStudentEmail(int accountId)
    {
        await adminCabinet.ConfirmStudentEmailAsync(accountId); 
        return RedirectToAction("Admin");
    }

}