using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalAccount.Constants;
using PersonalAccount.Services.Account;
using PersonalAccount.Services.Cabinet;
using PersonalAccount.Services.Email;
using PersonalAccount.Types;
using PersonalAccount.Utils;
using PersonalAccount.ViewModels;

namespace PersonalAccount.Controllers;

[Authorize(Roles = AccountRoleConstants.Administrator)]
public class AdminCabinetController(
    IAdminCabinetService cabinetService,
    IAccountService accountService,
    IEmailSenderService emailSenderService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var accounts = await cabinetService.GetAllStudentAccountsAsync();
        var accountDictionary = accounts.ToDictionary(account => account.Id);

        var groups = await cabinetService.GetAllGroupsAsync();
        var groupDictionary = groups.ToDictionary(group => group.Id);

        var studentProfiles = await cabinetService.GetAllStudentProfilesAsync();

        var groupIdsOrder = groups
            .OrderByDescending(group => group.Name)
            .Select(group => group.Id)
            .ToList();

        var groupInfos = groupDictionary
            .ToDictionary(
                group => group.Key,
                group => new AdminCabinetGroupInfoViewModel
                {
                    Name = group.Value.Name,
                    Description = group.Value.Description,
                    ImageUrl = group.Value.ImageUrl?.ToString()
                }
            );

        var studentInfos = studentProfiles.GroupBy(profile => profile.GroupId)
            .ToDictionary(
                group => group.Key,
                group => group.Select(student => new AdminCabinetStudentInfoViewModel
                    {
                        AccountId = student.AccountId,
                        GroupId = student.GroupId,
                        Email = accountDictionary[student.AccountId].Email,
                        FullName = student.FullName,
                        PhotoUrl = student.PhotoUrl?.ToString()
                    })
                    .ToList()
            );

        var subjects = await cabinetService.GetAllSubjectsAsync();

        return View(new AdminCabinetViewModel
        {
            GroupIdsOrder = groupIdsOrder,
            GroupInfos = groupInfos,
            StudentInfos = studentInfos,
            Subjects = subjects
        });
    }

    [HttpGet]
    public IActionResult RegisterStudent()
    {
        return View(new RegisterStudentViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> RegisterStudent(RegisterStudentViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var hasAccountWithEmail = await accountService.HasAccountWithEmailAsync(model.Email);
        if (hasAccountWithEmail)
        {
            ModelState.AddModelError(string.Empty, $"Account with same email \"{model.Email}\" already exists");
            return View(model);
        }

        var password = await accountService.AddAccountWithGeneratedPasswordAsync(model.Email, AccountRoles.Student);
        await cabinetService.AddStudentProfileAsync(model.Email, model.FullName);

        var appUrl = Url.Action("Index", "Cabinet", Request.Scheme);

        await emailSenderService.SendEmailAsync(model.ContactEmail, "Данные для входа в аккаунт", $"""
             <head></head>
             <body>
                <a href="{appUrl}">Вход в аккаунт</a>
                <p>{model.Email}</p>
                <p>{password}</p>
             </body>
             """);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AddGroup()
    {
        return View(new AddGroupViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddGroup(AddGroupViewModel model)
   {
        if (!ModelState.IsValid) return View(model);

        await cabinetService.AddGroupAsync(model.Name, model.Description?? string.Empty, model.ImageUrl?.ToUri());

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult AddSubject()
    {
        return View(new AddSubjectViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSubject(AddSubjectViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await cabinetService.AddSubjectAsync(model.Name);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStudentGroup(int studentAccountId, int groupId)
    {
        var isSuccess = await cabinetService.ChangeStudentGroupAsync(studentAccountId, groupId);

        if (!isSuccess)
        {
            TempData["Error"] = "В группе нет мест!";
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteGroup(int groupId)
    {
        await cabinetService.DeleteGroupAsync(groupId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSubject(int subjectId)
    {
        await cabinetService.DeleteSubjectAsync(subjectId);
        return RedirectToAction("Index");
    }
}