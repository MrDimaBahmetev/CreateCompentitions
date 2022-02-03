using Microsoft.Graph;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExamenWeb.Models
{
    public class Participant
    {
        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }
        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указана фамилия")]
        public string SerName { get; set; }
        [Display(Name = "Отчество")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указано отчество")]
        public string Patronymic { get; set; }
        [Display(Name = "Страна")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указана Страна")]
        public string Country { get; set; }
        [Display(Name = "Вид спорта")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указан вид спорта")]
        public string TypeSport { get; set; }
        [Display(Name = "Время")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указана дата рождения")]
        public string Birth { get; set; }
        [Display(Name = "Вес")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указан вес")]
        public float Weight { get; set; }
        [Display(Name = "ИИН")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указан ИИН")]
        public string YIN { get; set; }
        [Display(Name = "Город")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указан город")]
        public string City { get; set; }
        [Display(Name = "Лет тренировок")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указано лет тренировок")]
        public int YearsOfTraining { get; set; }
        [Display(Name = "Количество боев")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указано количесвто боев")]
        public int CountOfFights { get; set; }
        [Display(Name = "Побед")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указано количесвто побед")]
        public int Wins { get; set; }
        [Display(Name = "Лучший результат")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указан лучший результат")]
        public string BestResult { get; set; }
        [Display(Name = "ФИО тренера")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Не указано ФИО тренера")]
        public string FullNameTrainer { get; set; }
    }
}
