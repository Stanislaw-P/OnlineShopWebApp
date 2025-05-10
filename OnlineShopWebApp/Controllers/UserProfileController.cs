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

        public async Task<IActionResult> OrderAsync()
        {
            var currentUser = usersManager.GetUserAsync(HttpContext.User).Result;
            var currentUserId = Guid.Parse(currentUser.Id);
            var ordersCurrentUser = await ordersRepository.TryGetUserOrdersAsync(currentUserId);

            if (ordersCurrentUser == null)
                return NotFound();

            var ordersViewModels = _mapper.Map<List<OrderViewModel>>(ordersCurrentUser);
            return View(ordersViewModels);
        }

        public async Task<IActionResult> OrderDetailsAsync(Guid orderId)
        {
            var existingOrder = await ordersRepository.TryGetByIdAsync(orderId);
            if (existingOrder == null)
                return NotFound();
            return View(_mapper.Map<OrderViewModel>(existingOrder));
        }

        public async Task<IActionResult> Edit()
        {
            // TODO: Подобное вытягивание аватарок оч не нравится, двойное обращение к БД!
            var currentUserId = usersManager.GetUserId(User);
            var currentUser = await usersManager.Users
                .Include(us => us.Avatar)
                .FirstOrDefaultAsync(us => us.Id == currentUserId);
            return View(_mapper.Map<EditUserProfileViewModel>(currentUser));
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EditUserProfileViewModel editUserProfile)
        {
            if (!ModelState.IsValid)
                return View(editUserProfile);

            var addedAvatarPath = await _imagesProvider.SafeFileAsync(editUserProfile.UploadedFile, ImageFolders.Profiles);
            editUserProfile.AvatarImgPath = addedAvatarPath;

            User? existingUser = await usersManager.FindByIdAsync(editUserProfile.Id);
            existingUser.UserName = editUserProfile.UserName;
            existingUser.UserSurname = editUserProfile.UserSurname;
            existingUser.PhoneNumber = editUserProfile.PhoneNumber;
            existingUser.Avatar = new AvatarImage { UserId = editUserProfile.Id, URL = editUserProfile.AvatarImgPath };

            var result =await usersManager.UpdateAsync(existingUser);
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
