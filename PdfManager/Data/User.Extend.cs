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
            throw new NotImplementedException();
        }
        public static bool Register(this PdfManageModelContainer container, string name, string password)
        {
            throw new NotImplementedException();
        }
        public static bool ExistUser(this PdfManageModelContainer container, string name)
        {
            return container.UserSet.Where(n => n.Username == name).Any();
        }
    }
}
