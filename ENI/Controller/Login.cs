using System.Linq;
using System;

namespace ENI.Controller
{
    public class Login
    {
        public static user SingIn(string email, string password)
        {
            eniEntities db = new eniEntities();
            string md5Pass = Util.GenerateHashMd5(password);

            try
            {
                var userVerify = (from a in db.user
                                  where a.email == email 
                                  && a.password == md5Pass 
                                  && a.is_active == true
                                  select a);

                if (!userVerify.Any())
                    return null;

                return userVerify.FirstOrDefault(); ;
            }
            catch (Exception er)
            {
                return null;
            }
        }

        public static user GetUserById(int user_id)
        {
            eniEntities db = new eniEntities();

            try
            {
                user loggedUser = (from a in db.user
                                      where a.id == user_id
                                      select a).FirstOrDefault();

                return loggedUser;
            }
            catch (Exception er)
            {
                return null;
            }
        }
       
    }
}