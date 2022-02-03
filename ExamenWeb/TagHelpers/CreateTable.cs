using ExamenWeb.Controllers;
using ExamenWeb.Models;
using Microsoft.AspNetCore.Html;
using System.Data.SqlClient;
using static ExamenWeb.Controllers.HomeController;

namespace ExamenWeb.TagHelpers
{
    public class CreateTable
    {
        public static HtmlString ShowParticipants(string connStr, User UserProp)
        {

            using (var conn = new SqlConnection(connStr))
            {
                // Инициализация строки для комманды. 
                string sqlShowParticipants = $"USE [Olympiads] SELECT p.name as 'Имя', p.ser_name as 'Фамилия',p.patronymic as 'Отчество', co.name as 'Страна', p.city as 'Город',p.birth as 'Дата рождения',p.weight as 'Вес', p.Full_name_trainer as 'Имя тренера', s.name as 'Вид спорта' FROM Participant p JOIN [Country] co ON co.id = p.country_id JOIN Sport s ON s.id = p.type_sport_id JOIN users u ON u.phone_number = '{UserProp.PhoneNumber}'";
                string result = "<table border='1'<tr><th> Имя </th><th> Фамилия </th><th> Отчество </th><th> Страна </th><th> Город </th><th> Дата рождения </th><th> Вес </th><th> Имя тренера </th><th> Вид спорта </th></tr> ";
                // Соединение с базой
                logger.Trace("Соединение с базой");
                conn.Open();
                // Инициализация команды
                logger.Trace("Инициализация команды");
                SqlCommand cmd = new SqlCommand(sqlShowParticipants, conn);
                // Инициализируется класс чтения команды
                logger.Trace("Инициализируется класс чтения команды");
                SqlDataReader reader = cmd.ExecuteReader();
                // Считывание данных
                logger.Trace("Считывание данных");
                while (reader.Read())
                {
                    result += $"<tr><td>{reader[0]}</td><td>{reader[1]}</td><td>{reader[2]}</td><td>{reader[3]}</td><td>{reader[4]}</td><td>{reader[5]}</td><td>{reader[6]} кг</td><td>{reader[7]}</td><td>{reader[8]}</td></tr>";
                }
                result += "</table>";
                return new HtmlString(result);
            }
        }
        public static HtmlString ShowParticipants(string connStr)
        {
            using (var conn = new SqlConnection(connStr))
            {
                string result = "<table border='1'<tr><th> Имя </th><th> Фамилия </th><th> Отчество </th><th> Страна </th><th> Город </th><th> Дата рождения </th><th> Вес </th><th> Имя тренера </th><th> Вид спорта </th></tr> ";
                // Соединение с базой
                logger.Trace("Соединение с базой");
                conn.Open();
                // Инициализация строки для комманды.
                string sqlShowParticipants = $"USE [Olympiads] SELECT TOP(50) p.name as 'Имя', p.ser_name as 'Фамилия',p.patronymic as 'Отчество', co.name as 'Страна', p.city as 'Город',p.birth as 'Дата рождения',p.weight as 'Вес', p.Full_name_trainer as 'Имя тренера', s.name as 'Вид спорта' FROM Participant p JOIN [Country] co ON co.id = p.country_id JOIN Sport s ON s.id = p.type_sport_id";
                // Инициализация команды
                logger.Trace("Инициализация команды");
                SqlCommand cmd = new SqlCommand(sqlShowParticipants, conn);
                // Инициализируется класс чтения команды
                logger.Trace("Инициализируется класс чтения команды");
                SqlDataReader reader = cmd.ExecuteReader();
                // Считывание данных
                logger.Trace("Считывание данных");
                while (reader.Read())
                {
                    result += $"<tr><td>{reader[0]}</td><td>{reader[1]}</td><td>{reader[2]}</td><td>{reader[3]}</td><td>{reader[4]}</td><td>{reader[5]}</td><td>{reader[6]} кг</td><td>{reader[7]}</td><td>{reader[8]}</td></tr>";
                }
                result += "</table>";
                return new HtmlString(result);
            }
        }

        public static HtmlString ShowCompetitions(string connStr, User UserProp)
        {
            using (var conn = new SqlConnection(connStr))
            {
                // Инициализация строки для комманды. Для дабавления олимпиады
                string sqlShowOlympiads = $"USE [Olympiads] SELECT c.date_competitions,co.name, c.host_sity_competitions, COUNT(p.yin) as 'Количество участников' FROM[Competitions] c LEFT JOIN Country co ON co.id = c.host_country_competitions_id LEFT JOIN [Top] t ON t.id_competitions = c.id FULL JOIN Participant p ON p.id = t.id_participant JOIN users u ON u.id = c.id_user WHERE u.phone_number = '{UserProp.PhoneNumber}' GROUP BY c.date_competitions, co.name, c.host_sity_competitions";
                string result = "<table border='1'<tr><th> Дата </th><th> Страна </th><th> Город </th><th> Количество участников </th></tr> ";
                // Соединение с базой
                logger.Trace("Соединение с базой");
                conn.Open();
                // Инициализация команды
                logger.Trace("Инициализация команды");
                SqlCommand cmd = new SqlCommand(sqlShowOlympiads, conn);
                // Инициализируется класс чтения команды
                logger.Trace("Инициализируется класс чтения команды");
                SqlDataReader reader = cmd.ExecuteReader();
                // Считывание данных
                logger.Trace("Считывание данных");
                while (reader.Read())
                {
                    result += $"<tr><td>{reader[0]}</td><td>{reader[1]}</td><td>{reader[2]}</td><td>{reader[3]}</td></tr>";
                }
                result += "</table>";
                return new HtmlString(result);
            }
        }
        public static HtmlString ShowCompetitions(string connStr)
        {
            using (var conn = new SqlConnection(connStr))
            {
                string result = "<table border='1'<tr><th> Дата </th><th> Страна </th><th> Город </th><th> Количество участников </th></tr> ";
                // Соединение с базой
                logger.Trace("Соединение с базой");
                conn.Open();
                // Инициализация строки для комманды.
                string sqlShowOlympiads = $"USE [Olympiads] SELECT TOP(50) c.date_competitions,co.name, c.host_sity_competitions, COUNT(p.yin) as 'Количество участников' FROM[Competitions] c LEFT JOIN Country co ON co.id = c.host_country_competitions_id LEFT JOIN [Top] t ON t.id_competitions = c.id FULL JOIN Participant p ON p.id = t.id_participant GROUP BY c.date_competitions, co.name, c.host_sity_competitions";
                // Инициализация команды
                logger.Trace("Инициализация команды");
                SqlCommand cmd = new SqlCommand(sqlShowOlympiads, conn);
                // Инициализируется класс чтения команды
                logger.Trace("Инициализируется класс чтения команды");
                SqlDataReader reader = cmd.ExecuteReader();
                // Считывание данных
                logger.Trace("Считывание данных");
                while (reader.Read())
                {
                    result += $"<tr><td>{reader[0]}</td><td>{reader[1]}</td><td>{reader[2]}</td><td>{reader[3]}</td></tr>";
                }
                result += "</table>";
                return new HtmlString(result);
            }
        }


        public static HtmlString ShowTypesSport(string connStr)
        {
            using (var conn = new SqlConnection(connStr))
            {
                string result = "<table border='1'<tr><th><span class=\"sign__word\" style=\"font-size: 150%;\"> Название вида спорта </span></th><th><span class=\"sign__word\" style=\"font-size: 150%;\"> Описание </span></th></tr> ";
                // Соединение с базой
                logger.Trace("Соединение с базой");
                conn.Open();
                // Инициализация строки для комманды.
                string sqlShowOlympiads = $"USE [Olympiads] SELECT name, discription FROM[dbo].[Sport]";
                // Инициализация команды
                logger.Trace("Инициализация команды");
                SqlCommand cmd = new SqlCommand(sqlShowOlympiads, conn);
                // Инициализируется класс чтения команды
                logger.Trace("Инициализируется класс чтения команды");
                SqlDataReader reader = cmd.ExecuteReader();
                // Считывание данных
                logger.Trace("Считывание данных");
                while (reader.Read())
                {
                    result += $"<tr><td><span class=\"sign__word\" style=\"font-size: 150%;\">{reader[0]}</span></td><td><span class=\"sign__word\" style=\"font-size: 150%;\" >{reader[1]}</span></td></tr>";
                }
                result += "</table>";
                return new HtmlString(result);
            }
        }
    }
}
