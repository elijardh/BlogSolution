using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogWeb.Model.Response;

namespace BlogWeb.Services
{
    public class UserService
    {
        public UserModel? userModel;

        public void setUserModel(UserModel? userModel)
        {
            this.userModel = userModel;
        }
    }
}