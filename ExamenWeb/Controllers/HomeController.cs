using Microsoft.AspNetCore.Mvc;
using ExamenWeb.Models;
using NLog;
using System.Data.SqlClient;
using System;
using System.IO;

namespace ExamenWeb.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        // Получение информации о пользователях (логин, пароль)
        public static string sqlUsers = "USE Olympiads SELECT * FROM UsersInfo";
        // Логгер
        public static Logger logger = LogManager.GetLogger("AppLog");
        // Строка для подключения к базе
        public static string connStr = @"Server=DESKTOP-Q9E99O8\DIMAS_WORK;Database=master;Integrated Security=True;TimeOut=3";
        public static User UserProp { get; set; }
        /// <summary>
        /// Создание и согранения нового зарегистрированого пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterNew(User user)
        {
            logger.Info("Регистрация нового пользователя");
            try
            {
                // Происходит проверка введеных пользователем данных для регистрации 
                logger.Info("Происходит проверка введеных пользователем данных для регистрации ");
                logger.Trace("Происходит проверка имени");
                if (!string.IsNullOrEmpty(user.Name))
                {
                    logger.Trace("Происходит проверка фамилии");
                    if (!string.IsNullOrEmpty(user.SurName))
                    {
                        logger.Trace("Происходит проверка номера");
                        if (!string.IsNullOrEmpty(user.PhoneNumber))
                        {
                            logger.Trace("Происходит проверка почты");
                            if (!string.IsNullOrEmpty(user.Gmail))
                            {
                                logger.Trace("Происходит проверка пароля");
                                if (!string.IsNullOrEmpty(user.Password))
                                {
                                    logger.Trace("Инициализируется подключение");
                                    using (var conn = new SqlConnection(connStr))
                                    {
                                        logger.Trace("Подключение к базе");
                                        conn.Open();
                                        logger.Trace($"Состояние подключения: {conn.State}");
                                        // Строка для команды. Строка создания ползователя.
                                        string newUser = $"USE Olympiads INSERT INTO users(name, ser_name, password, vip_status, mail, phone_number) VALUES ('{user.Name}', '{user.SurName}', '{user.Password}', 0, '{user.Gmail}', '{user.Gmail}')";
                                        // Инициализация команды
                                        logger.Trace($"Инициализация команды");
                                        var sql = new SqlCommand(newUser, conn);
                                        // Выполнение команды
                                        logger.Trace($"Выполнение команды");
                                        sql.ExecuteNonQuery();
                                        // Логирование пользователя после регистрации
                                        logger.Trace($"Создан новый пользователь Логин: {user.PhoneNumber} Пароль: {user.Password} Почта: {user.Gmail}");
                                    }
                                    CheckLogin(user);
                                    Authorization(user);
                                }
                                else
                                {
                                    throw new Exception("Введите пароль");
                                }
                            }
                            else
                            {
                                throw new Exception("Введите почты");
                            }
                        }
                        else
                        {
                            throw new Exception("Введите логин");
                        }
                    }
                    else
                    {
                        throw new Exception("Введите фамилию");
                    }
                }
                else
                {
                    throw new Exception("Введите имя");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Введите логин" || ex.Message == "Введите пароль")
                {
                    logger.Error("Сообщение ошибки: " + ex.Message + " Экзэмпляр класса исключения: " + ex.InnerException + " Имя приложения или объекта который создал исключение: " + ex.Source + " Ссылка на файл справки, связанный с исключением: " + ex.HelpLink + " Метод создавший исключение: " + ex.TargetSite);
                }
                else
                {
                    logger.Error("Логин уже существует");
                }
            }
        }

        private int SearIdCountry(Participant participant)
        {
            int country_id = 0;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"USE Olympiads SELECT id FROM Country WHERE name LIKE '{participant.Country}'", conn);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    country_id = Convert.ToInt32(rd.GetValue(0));
                }
            }
            return country_id;
        }
        private int SearIdSpotr(Participant participant)
        {
            int sport_id = 0;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"USE Olympiads SELECT id FROM Sport WHERE name LIKE '{participant.TypeSport}'", conn);
                SqlDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    sport_id = Convert.ToInt32(rd.GetValue(0));
                }
            }
            return sport_id;
        }
        private void NewParticipant(Participant participant)
        {
            int idCountry = SearIdCountry(participant);
            int idSport = SearIdSpotr(participant);
            string sql = $"USE Olympiads INSERT INTO Participant(name, ser_name, patronymic, country_id, type_sport_id, birth, weight, yin, city, years_of_training, count_of_fights, wins, bestResult, Full_name_trainer) VALUES ('{participant.Name}', '{participant.SerName}', '{participant.Patronymic}', {idCountry}, {idSport}, '{participant.Birth}', {participant.Weight}, '{participant.YIN}', '{participant.City}', {participant.YearsOfTraining}, {participant.CountOfFights}, {participant.Wins}, '{participant.BestResult}', '{participant.FullNameTrainer}')";
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                // Инициализация команды
                logger.Trace("Инициализация команды");
                SqlCommand cmd = new SqlCommand(sql, conn);
                // Инициализация транзакции
                logger.Trace("Инициализация транзакции");
                SqlTransaction transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;
                // Выполнение команды и транзакции
                logger.Trace("Выполнение команды и транзакции");
                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
        }

        /// <summary>
        /// Метод для проверки пароля
        /// </summary>
        private void CheckPass(User user)
        {
            // Проверка пароля
            logger.Info("Проверка пароля");
            bool proces = false;
            // Инициализация подключения
            logger.Trace("Инициализация подключения");
            using (var conn = new SqlConnection(connStr))
            {
                // Соединение с базой
                logger.Trace("Соединение с базой");
                conn.Open();
                // Инициализация команды и чтения
                logger.Trace("Инициализация команды и чтения");
                var sql = new SqlCommand($"USE Olympiads SELECT * FROM users u WHERE u.phone_number LIKE '{user.PhoneNumber}'", conn);
                SqlDataReader rdr = sql.ExecuteReader();
                // Считавание и проверка данных
                logger.Trace("Считавание и проверка данных");
                while (rdr.Read())
                {
                    if (user.Password == rdr[3].ToString() && user.PhoneNumber == rdr[6].ToString())
                    {
                        proces = true;
                        user.Id = Convert.ToInt32(rdr[0].ToString());
                        user.Name = rdr[1].ToString();
                        user.SurName = rdr[2].ToString();
                        user.VipStatus = Convert.ToBoolean(rdr[4]);
                        user.Gmail = rdr[5].ToString();
                        UserProp = user;
                        //SaveReestr(logLog.Text, logPass.Text);
                        break;
                    }
                }
                if (proces == false)
                {
                    throw new Exception("Не правильный пароль");
                }
            }
        }
        /// <summary>
        /// Метод для проверки логина
        /// </summary>
        /// <param name="user"></param>
        private void CheckLogin(User user)
        {
            // Проверка логина
            logger.Info("Считавание и проверка данных");
            bool proces = false;
            try
            {
                // Проверка введенного номера/логина
                logger.Trace("Проверка введенного номера/логина");
                if (!string.IsNullOrEmpty(user.PhoneNumber))
                {
                    // Проверка введенного пароля
                    logger.Trace("Проверка введенного пароля");
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        using (var conn = new SqlConnection(connStr))
                        {
                            conn.Open();
                            string sqlLogins = $"USE Olympiads SELECT u.phone_number as 'Номер телефона', u.password as 'Пароль', u.vip_status as 'Вип' FROM users u WHERE u.phone_number LIKE '{user.PhoneNumber}'";
                            var sql = new SqlCommand(sqlLogins, conn);
                            SqlDataReader rdr = sql.ExecuteReader();
                            while (rdr.Read())
                            {
                                if (Convert.ToBoolean(rdr[2]) == true)
                                {

                                    if (user.PhoneNumber == rdr[0].ToString())
                                    {
                                        proces = true;
                                        CheckPass(user);
                                        break;
                                    }
                                }
                            }
                            if (proces == false)
                            {
                                throw new Exception("Не правильный номер или номер не существует\nВозможно не купленно приложение\nКупите его на сайте");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Пустой пароль");
                    }
                }
                else
                {
                    throw new Exception("Пустой логин");
                }
            }
            catch (Exception ex)
            {
                logger.Error("Сообщение ошибки: " + ex.Message + " Экзэмпляр класса исключения: " + ex.InnerException + " Имя приложения или объекта который создал исключение: " + ex.Source + " Ссылка на файл справки, связанный с исключением: " + ex.HelpLink + " Метод создавший исключение: " + ex.TargetSite);
            }
        }
        [Route("ExitUser")]
        public IActionResult ExitUser()
        {
            if (UserProp != null)
                UserProp = null;
            return View("Index");
        }

        [Route("MiniSecretGame")]
        public IActionResult MiniSecretGame()
        {
            return View();
        }

        [Route("InfoOfUs")]
        [HttpGet]
        public IActionResult InfoOfUs()
        {
            return View();
        }
        [Route("Purchase")]
        public IActionResult Purchase()
        {
            if (UserProp == null)
                return View();
            else if (UserProp.VipStatus == true)
                return View("PurchaseDownload");
            else
                return View();
        }
        [Route("PurchaseDownload")]
        public IActionResult PurchaseDownload()
        {
            if (User == null)
                return View();
            else
                return View("PurchaseDownload");
        }
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Header()
        {
            return PartialView("_Header");
        }
        [Route("Authorization")]
        public IActionResult Authorization()
        {
            return View();
        }
        [Route("GetFile")]
        public VirtualFileResult GetFile()
        {
            var filepath = Path.Combine(@"~\", $"Competitions.exe");
            return File(filepath, "text/plain", $"Competitions.exe");
        }
        [Route("Authorization")]
        [HttpPost]
        public IActionResult Authorization(User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "Некорректный номер");
                }
                if (string.IsNullOrEmpty(user.Password))
                {
                    ModelState.AddModelError("Password", "Некорректный пароль");
                }
                if (!string.IsNullOrEmpty(user.PhoneNumber) && !string.IsNullOrEmpty(user.Password))
                    CheckLogin(user);
                if (UserProp.VipStatus == true)
                {
                    return GetFile();
                }
                else
                    return View("Index");
            }
            catch (Exception)
            {
                return View("Authorization");
            }

        }

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult Register(User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Gmail))
                {
                    ModelState.AddModelError("Gmail", "Некорректный адрес");
                }
                if (string.IsNullOrEmpty(user.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "Некорректный номер");
                }
                if (string.IsNullOrEmpty(user.Password))
                {
                    ModelState.AddModelError("Password", "Некорректный пароль");
                }
                if (!string.IsNullOrEmpty(user.PhoneNumber) && !string.IsNullOrEmpty(user.Password))
                {
                    RegisterNew(user);
                    return View("Index");
                }
                return View("Register");
            }
            catch (Exception)
            {
                return View("Register");
            }
            
        }
        [Route("ParticipantForm")]
        public IActionResult ParticipantForm()
        {
            return View();
        }
        [Route("ParticipantForm")]
        [HttpPost]
        public IActionResult ParticipantForm(Participant participant)
        {
            if (string.IsNullOrEmpty(participant.YIN))
            {
                ModelState.AddModelError("YIN", "Некорректный иин");
            }
            if (string.IsNullOrEmpty(participant.Name))
            {
                ModelState.AddModelError("Name", "Некорректное имя");
            }
            if (string.IsNullOrEmpty(participant.SerName))
            {
                ModelState.AddModelError("SerName", "Некорректная фамилия");
            }
            if (string.IsNullOrEmpty(participant.Patronymic))
            {
                ModelState.AddModelError("Patronymic", "Некорректное отчество");
            }
            if (string.IsNullOrEmpty(participant.TypeSport))
            {
                ModelState.AddModelError("TypeSport", "Некорректный вид спорта");
            }
            if (string.IsNullOrEmpty(participant.Weight.ToString()))
            {
                ModelState.AddModelError("Weight", "Некорректный вес");
            }
            if (participant.Weight == 0)
            {
                ModelState.AddModelError("Weight", "Некорректный вес");
            }
            if (string.IsNullOrEmpty(participant.YearsOfTraining.ToString()))
            {
                ModelState.AddModelError("YearsOfTraining", "Некорректно указано количество лет тренировок");
            }
            if (participant.YearsOfTraining == 0)
            {
                ModelState.AddModelError("YearsOfTraining", "Некорректно указано количество лет тренировок");
            }
            if (string.IsNullOrEmpty(participant.City))
            {
                ModelState.AddModelError("City", "Некорректно указан город");
            }
            if (string.IsNullOrEmpty(participant.Country))
            {
                ModelState.AddModelError("Country", "Некорректно указана страна");
            }
            if (participant.Birth == null)
            {
                ModelState.AddModelError("Birth", "Некорректна дата рождения");
            }
            if(participant.Birth != null &&  !String.IsNullOrWhiteSpace(participant.City) && !String.IsNullOrWhiteSpace(participant.Country) && !String.IsNullOrWhiteSpace(participant.Name) && !String.IsNullOrWhiteSpace(participant.SerName) && !String.IsNullOrWhiteSpace(participant.Patronymic) && !String.IsNullOrWhiteSpace(participant.TypeSport) && participant.Weight > 0 && !string.IsNullOrWhiteSpace(participant.YIN) )
            {
                NewParticipant(participant);
                return View("Index");
            }
            return View();
        }
    }
}
