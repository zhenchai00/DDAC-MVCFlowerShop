// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVCFlowerShop.Areas.Identity.Data;

namespace MVCFlowerShop.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<MVCFlowerShopUser> _userManager;
        private readonly SignInManager<MVCFlowerShopUser> _signInManager;

        public IndexModel(
            UserManager<MVCFlowerShopUser> userManager,
            SignInManager<MVCFlowerShopUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "You must enter the name first before submitting your form!")]
            [StringLength(256, ErrorMessage = "You must enter the value between 3 - 256 chars", MinimumLength = 3)]
            [Display(Name = "Your Full Name")] //label
            public string CustomerFullName { get; set; }

            [Required]
            [Display(Name = "Your DOB")]
            [DataType(DataType.Date)]
            public DateTime CustomerDoB { get; set; }

            [Required(ErrorMessage = "You must enter the age first before submitting your form!")]
            [Range(18, 100, ErrorMessage = "You must be 18 years old when register this member!")]
            [Display(Name = "Your Age")] //label
            public int CustomerAge { get; set; }

            [Required]
            [DataType(DataType.MultilineText)]
            [Display(Name = "Your Address")]
            public string CustomerAddress { get; set; }

        }

        private async Task LoadAsync(MVCFlowerShopUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                CustomerFullName = user.CustomerFullName,
                CustomerDoB = user.CustomerDoB,
                CustomerAge = user.CustomerAge,
                CustomerAddress = user.CustomerAddress,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (Input.CustomerFullName != user.CustomerFullName)
            {
                user.CustomerFullName = Input.CustomerFullName;
            }
            if (Input.CustomerDoB != user.CustomerDoB)
            {
                user.CustomerDoB = Input.CustomerDoB;
            }
            if (Input.CustomerAddress != user.CustomerAddress)
            {
                user.CustomerAddress = Input.CustomerAddress;
            }
            if (Input.CustomerAge != user.CustomerAge)
            {
                user.CustomerAge = Input.CustomerAge;
            }
            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
