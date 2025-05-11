using System.ComponentModel.DataAnnotations;

public class LoginViewModelCustom
{
    [Required(ErrorMessage = "Имя пользователя или Email обязательны")]
    [Display(Name = "Имя пользователя или Email")]
    public string UsernameOrEmail { get; set; }
    [Required(ErrorMessage = "Пароль обязателен")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    
}