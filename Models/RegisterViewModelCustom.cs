using System.ComponentModel.DataAnnotations;

public class RegisterViewModelCustom
{
    [Required(ErrorMessage = "Имя пользователя обязательно")]
    [Display(Name = "Имя пользователя")]
    public string Username { get; set; }
    [Required(ErrorMessage = "Адрес электронной почты обязателен")]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Пароль обязателен")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    [StringLength(100, ErrorMessage = "{0} должен содержать от {2} до {1} символов.", MinimumLength = 6)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Подтвердите пароль")]
    [Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
    public string ConfirmPassword { get; set; }
}
