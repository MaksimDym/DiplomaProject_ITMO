using System.ComponentModel.DataAnnotations;

namespace DiplomaProject_ITMO.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [StringLength(100)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Адрес электронной почты обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат адреса электронной почты")]
        [StringLength(255)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Хеш пароля обязателен")]
        public string PasswordHash { get; set; }
        

    }
}
