using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
	public class UserProfileController : Controller
	{
		readonly UserManager<User> usersManager;
		readonly IOrdersRepository ordersRepository;
		readonly IMapper _mapper;
		readonly IWebHostEnvironment webAppEnvironment;
		readonly ImagesProvider _imagesProvider;
		public UserProfileController(UserManager<User> usersManager, IOrdersRepository ordersRepository, IMapper mapper, IWebHostEnvironment webAppEnvironment)
		{
			this.usersManager = usersManager;
			this.ordersRepository = ordersRepository;
			_mapper = mapper;
			this.webAppEnvironment = webAppEnvironment;
			_imagesProvider = new ImagesProvider(this.webAppEnvironment);
		}

		public IActionResult Index()
		{
			var currentUserId = usersManager.GetUserId(User);
			var currentUser = usersManager.Users
				.Include(us => us.Avatar)
				.FirstOrDefaultAsync(us => us.Id == currentUserId)
				.Result;
			return View(_mapper.Map<UserViewModel>(currentUser));
		}

		public IActionResult Order()
		{
			var currentUser = usersManager.GetUserAsync(HttpContext.User).Result;
			var currentUserId = Guid.Parse(currentUser.Id);
			var ordersCurrentUser = ordersRepository.TryGetUserOrders(currentUserId);

			if (ordersCurrentUser == null)
				return NotFound();

			var ordersViewModels = _mapper.Map<List<OrderViewModel>>(ordersCurrentUser);
			return View(ordersViewModels);
		}

		public IActionResult OrderDetails(Guid orderId)
		{
			var existingOrder = ordersRepository.TryGetById(orderId);
			if (existingOrder == null)
				return NotFound();
			return View(_mapper.Map<OrderViewModel>(existingOrder));
		}

		public IActionResult Edit()
		{
			var currentUserId = usersManager.GetUserId(User);
			var currentUser = usersManager.Users
				.Include(us => us.Avatar)
				.FirstOrDefaultAsync(us => us.Id == currentUserId)
				.Result;
			return View(_mapper.Map<EditUserProfileViewModel>(currentUser));
		}

		[HttpPost]
		public IActionResult Edit(EditUserProfileViewModel editUserProfile)
		{
			if (!ModelState.IsValid)
				return View(editUserProfile);

			var addedAvatarPath = _imagesProvider.SafeFile(editUserProfile.UploadedFile, ImageFolders.Profiles);
			editUserProfile.AvatarImgPath = addedAvatarPath;

			var existingUser = usersManager.FindByIdAsync(editUserProfile.Id).Result;
			existingUser.UserName = editUserProfile.UserName;
			existingUser.UserSurname = editUserProfile.UserSurname;
			existingUser.PhoneNumber = editUserProfile.PhoneNumber;
			existingUser.Avatar = new AvatarImage { UserId = editUserProfile.Id, URL = editUserProfile.AvatarImgPath };

			var result = usersManager.UpdateAsync(existingUser).Result;
			if (result.Succeeded)
				return RedirectToAction(nameof(Index));
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return View(editUserProfile);
		}
	}
}
