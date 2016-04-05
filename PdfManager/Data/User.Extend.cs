using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfManager.Data
{
    public partial class User
    {
    }
    public static class UserExtend
    {
        public static bool Login(this PdfManageModelContainer container, string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(password));

            if (!container.UserSet.Any())
                CreateDefaultUser(container);

            if (!container.ExistUser(name))
                return false;

            var user = container.UserSet.First(n => n.Username == name);
            return user.Password == password;
        }

        private static void CreateDefaultUser(PdfManageModelContainer container)
        {
            container.Register("admin", "admin123");
        }

        public static bool Register(this PdfManageModelContainer container, string name, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(password));

            if (container.ExistUser(name))
                return false;

            var user = container.UserSet.Create();
            user.LastLoginTime = DateTime.Now;
            user.Username = name;
            user.Password = password;
            container.UserSet.Add(user);
            container.SaveChanges();
            return true;
        }
        public static bool ExistUser(this PdfManageModelContainer container, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(nameof(name));

            return container.UserSet.Where(n => n.Username == name).Any();
        }
    }
}
