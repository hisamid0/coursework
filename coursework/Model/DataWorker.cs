using CourseWork.Model.Data;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CourseWork.Model
{
    public static class DataWorker
    {
        public static List<User> GetAllUsers()
        {
            using ApplicationContext db = new ApplicationContext();
            List<User> result = db.Users.ToList();
            return result;
        }
        public static List<Package> GetAllPackages()
        {
            using ApplicationContext db = new ApplicationContext();
            List<Package> result = db.Packages.ToList();
            return result;
        }

        //Выгрузить полученный пакет в базу
        public static string UploadPackage(string something,string ipaddress, DateTime date)
        {

            using ApplicationContext db = new ApplicationContext();
            Package newPackage = new Package { Something = something, IpAddress = ipaddress, Date = date };
            db.Packages.Add(newPackage);
            db.SaveChanges();
            string result = "Пакет добавлен";
            return result;

        }
        
        // Внести данные администратора
        public static string CreatUser(string name, string password)
        {
            string hashPass = GetStringSha256Hash(password);
            using ApplicationContext db = new ApplicationContext();
            User newUser = new User { Name = name, Password = hashPass };
            db.Users.Add(newUser);
            db.SaveChanges();
            string result = "Пользователь добавлен";
            return result;
        }
        //Метод отдающи хэш введённого пароля
        internal static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using System.Security.Cryptography.SHA256Managed sha = new System.Security.Cryptography.SHA256Managed();
            byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
            byte[] hash = sha.ComputeHash(textData);
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }
        // Получить данные администратора
        

    }
}
