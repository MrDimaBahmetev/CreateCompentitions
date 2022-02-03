using System.ComponentModel.DataAnnotations;

namespace ExamenWeb.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        public string SurName { get; set; }
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
        [Display(Name = "Почта")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[A-Za-z0-9]+@[A-Za-z0-9]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Gmail { get; set; }
        [Display(Name = "Номер телефона")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Не указан номер телефона")]
        public string PhoneNumber { get; set; }
        public bool VipStatus { get; set; }
    }
}
