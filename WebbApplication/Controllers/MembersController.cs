using Business.Interfaces;
using Data.Entities;
using Domain.Extensions;
using Domain.FormData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebbApplication.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebbApplication.Extensions;

namespace WebbApplication.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/members")]
public class MembersController(IMemberService memberService, UserManager<UserEntity> userManager) : Controller
{
    private readonly IMemberService _memberService = memberService;
    private readonly UserManager<UserEntity> _userManager = userManager;
    
    public async Task<IActionResult> Index(int page = 1, int pageSize = 4)
    {
        var membersResult = await _memberService.GetMembersAsync(page, pageSize);
        // Mappar member --> viewmodel för rätt typ av lista
        var members = membersResult.Result!.Select(m => m.MapToViewModel(pageSize)).ToList();
        var totalMembers = await _memberService.GetMembersCountAsync();
        var totalPages = (int)Math.Ceiling(totalMembers /(double) pageSize);

        var viewModel = new MembersIndexViewModel
        {
            Members = members,
            CurrentPage = page,
            TotalPages = totalPages,
            PageSize = pageSize
        };
        
        return View(viewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddMember(AddMemberViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", await GetMembersViewModel(addMember: model));

        // Filuppladdning
        if (model.MemberImage != null && model.MemberImage.Length > 0)
        {
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/members");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.MemberImage.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await model.MemberImage.CopyToAsync(stream);

            model.ImageUrl = $"/uploads/members/{fileName}";
        }
        
        var formData = model.MapTo<AddMemberFormData>();
        var result = await _memberService.CreateMemberAsync(formData);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "Failed to create member");
            return View("Index", await GetMembersViewModel());
        }
        return RedirectToAction("Index", new { pageSize = 4 });
    }
    
    [HttpPost("update")]
    public async Task<IActionResult> UpdateMember(UpdateMemberViewModel model)
    {
        if (!ModelState.IsValid)
            return View("Index", await GetMembersViewModel(pageSize: 4));

        // 🔵 Hantera bilduppladdning om en ny bild har valts
        if (model.MemberImage != null && model.MemberImage.Length > 0)
        {
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/members");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.MemberImage.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await model.MemberImage.CopyToAsync(stream);

            model.ImageUrl = $"/uploads/members/{fileName}";
        }

        // 🔵 Mappa till UpdateMemberFormData
        var formData = model.MapTo<UpdateMemberFormData>();

        // 🔵 Anropa din MemberService
        var response = await _memberService.UpdateMemberAsync(formData.Id, formData);

        if (!response.Succeeded)
        {
            ModelState.AddModelError(string.Empty, response.Error ?? "Failed to update member");
            return View("Index", await GetMembersViewModel());
        }

        return RedirectToAction("Index");
    }
    
    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeleteMember(string id, int pageSize = 4)
    {
        var response = await _memberService.DeleteMemberAsync(id);
        return RedirectToAction("Index", new { pageSize = pageSize });
    }
    
    /* Denna metod är genererad av Chat GPT 4.0 för att slippa duplicering av kod i CRUD metoderna.
     Den omvandlar all innehåll på Members sidan till en IndexViewModel som sedan skickas till vyn */
    private async Task<MembersIndexViewModel> GetMembersViewModel(
        AddMemberViewModel? addMember = null,
        UpdateMemberViewModel? updateMember = null,
        int page = 1,
        int pageSize = 6)
    {
        var membersResult = await _memberService.GetMembersAsync(page, pageSize);
        var members = membersResult.Result?.MapToViewModels(pageSize) ?? new();

        var totalMembers = await _memberService.GetMembersCountAsync();
        var totalPages = (int)Math.Ceiling(totalMembers / (double)pageSize);

        return new MembersIndexViewModel
        {
            Members = members,
            CurrentPage = page,
            TotalPages = totalPages,
            PageSize = pageSize,
            AddMemberFormData = addMember ?? new AddMemberViewModel
            {
                Roles = new List<SelectListItem>
                {
                    new() { Text = "Admin", Value = "Admin" },
                    new() { Text = "Member", Value = "Member" }
                }
            },
            UpdateMemberFormData = updateMember ?? new()
        };
    }

}