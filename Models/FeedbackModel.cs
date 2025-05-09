using System.ComponentModel.DataAnnotations;

namespace DiplomaProject_ITMO.Models
{
    public class FeedbackModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите ваш email")]
        [EmailAddress(ErrorMessage = "Некорректный email адрес")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите ваше сообщение")]
        [StringLength(500, ErrorMessage = "Сообщение не должно превышать 500 символов")]
        public string Message { get; set; }
    }
}
