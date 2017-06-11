using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Models;
using Core.Models.AccountViewModels;
using Core.Services;
using Core.Data;

namespace Core.Controllers
{
	[Authorize(Roles="Admin")]
	public class RoleController : Controller
    {
		ApplicationDbContext context;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

		public RoleController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
		{
            _context = context;
		    _userManager = userManager;
            _signInManager = signInManager;
        
		}

		/// <summary>
		/// Get All Roles
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> Index()
		{

			if (User.Identity.IsAuthenticated)
			{


				// if (await isAdminUser()==false)
				// {
				// 	return RedirectToAction("Index", "Home");
				// }
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
			

			if (_context.Roles!=null){
				var Roles = _context.Roles.ToList();
				return View(Roles);
			}else{
				
				List<string> Roles = new List<string>();
            	Roles.Add("FakeAdmin");
            	Roles.Add("FakeUser");

				return View(Roles);

			}
			

		}
		public async Task<Boolean>  isAdminUser()
		{
			if (User.Identity.IsAuthenticated)
			{
      
                
                //var user2 =  _userManager.FindByNameAsync(User.Identity.Name);
                //var userGuid = Guid.Parse(user2.Id);
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

				//var user = User.Identity;
				var UserManager = _userManager;
				var  s = await UserManager.GetRolesAsync(user);
				if (s.Count()==0){return false;}
				if ( s[0].ToString() == "Admin")
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			return false;
		}
		/// <summary>
		/// Create  a New role
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> Create()
		{
			if (User.Identity.IsAuthenticated)
			{


				// if ( await isAdminUser()==false)
				// {
				// 	return RedirectToAction("Index", "Home");
				// }
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}

			var Role = new IdentityRole();
			return View(Role);
		}

		/// <summary>
		/// Create a New Role
		/// </summary>
		/// <param name="Role"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Create(IdentityRole Role)
		{
			if (User.Identity.IsAuthenticated)
			{
				// if (await isAdminUser()==false)
				// {
				// 	return RedirectToAction("Index", "Home");
				// }
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}

			_context.Roles.Add(Role);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}