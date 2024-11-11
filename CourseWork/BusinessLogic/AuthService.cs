using DataAccess;
using Model;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLogic
{
    public class AuthService
    {
        private readonly IRepository _repository;

        public AuthService(IRepository repository)
        {
            _repository = repository;
        }
        
        public bool CheckHaveUser(string login, string password, User user)
        {
            return _repository.SignIn(login, GetHash(password), user);
        }

        public void RegisterUserAdd(string name, string email, string numberOfPhone, string login, string password, User user)
        {
            user.Name = name;
            user.Password = GetHash(password);
            user.Login = login; user.Email = email;
            user.NumberOfPhone = numberOfPhone;
            _repository.AddClient(user);
        }

        public string GetHash(string password)
        {
            var md5 = MD5.Create(); var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        public bool CheckRegister(string name, string mail, string phone, string login, string password, User user)
        {
            try
            {
                if (!string.IsNullOrEmpty(name) && CheckEmailAddress(mail) && CheckNumberOfPhone(phone) && CheckLogin(login) && CheckPassword(password))
                {
                    RegisterUserAdd(name, mail, phone, login, password, user);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool CheckName(string name) 
        {
            if (!string.IsNullOrEmpty(name))
                return true;
            throw new Exception("Поле ФИО не может быть пустым");
        }

        public bool CheckNumberOfPhone(string phone)
        {
            string regex = @"[1-9]{1}[0-9]{10}";
            if (Regex.IsMatch(phone, regex, RegexOptions.IgnoreCase))
                return true;
            throw new Exception("Неправильно введен номер телефона");
        }

        public bool CheckEmailAddress(string emailaddress)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|ru)$";
            if (Regex.IsMatch(emailaddress, regex, RegexOptions.IgnoreCase))
                return true;
            throw new Exception("Неправильно введена почта");
        }

        public bool CheckPassword(string password)
        {
            Regex hasNumber = new Regex(@"[0-9]+"); Regex hasMiniMaxLength = new Regex(@".{8,12}");
            Regex hasSpecialSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?-]"); Regex hasUpperLetters = new Regex(@"[A-Z]");
            Regex hasLowerLetters = new Regex(@"[a-z]");
            if (hasNumber.IsMatch(password) && hasMiniMaxLength.IsMatch(password) && hasSpecialSymbols.IsMatch(password) && hasUpperLetters.IsMatch(password) && hasLowerLetters.IsMatch(password))
                return true; 
            throw new Exception("Пароль должен содержать строчные и пропусные буквы, специальные символы и цифры");
        }

        public bool CheckLogin(string login)
        {
            if (!_repository.LoginSearch(login))
                return true;
            throw new Exception("Такой логин уже существует");
        }
    }
}
