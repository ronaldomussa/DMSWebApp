using System;
using System.Linq;
using System.Web;

namespace ENI
{
    public class IsLogged : System.Web.UI.Page
    {
        public static user loggedUser { get; set; }
        public static Version versao { get; set; }

        public void Page_PreInit(object sender, EventArgs e)
        {
            CheckSession();
        }

        private void CheckSession()
        {
            loggedUser = (user)Session[Constants.LoginSession.LOGIN_SESSION];

            if (loggedUser == null)
            {
                CheckCookie();
            }
        }

        private void CheckCookie()
        {
            var userCookie = Request.Cookies[Constants.LoginSession.LOGIN_COOKIE];

            if (userCookie != null) 
            {
                try
                {
                    // cookie = 'id;name;email'
                    // need to split by ';'
                    int userId = int.Parse(Util.stringUntrunc(userCookie.Value).Split(';').FirstOrDefault());

                    // get user by id
                    loggedUser = Controller.Login.GetUserById(userId);

                    if (loggedUser == null)
                        Response.Redirect("~/login?error=noexist");
                    else
                    {
                        SetSessionAndCookie(loggedUser);
                    }
                }
                catch
                {
                    Response.Redirect("~/login?error=cookie");
                }
            }
            else
                Response.Redirect("~/login?error=session");
        }

        public static void SetSessionAndCookie(user userToSession)
        {
            // Add in Session
            HttpContext.Current.Session.Add(Constants.LoginSession.LOGIN_SESSION, userToSession);

            // Add in Cookie
            HttpCookie cookie = new HttpCookie(Constants.LoginSession.LOGIN_COOKIE);

            cookie.Expires = DateTime.Now.AddHours(8);

            string storedCookie = string.Format("{0};{1};{2}", userToSession.id, userToSession.name, userToSession.email);

            cookie.Value = Util.stringTrunc(storedCookie);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void Loggout()
        {
            HttpContext.Current.Session.Remove(Constants.LoginSession.LOGIN_SESSION);
            HttpCookie cookie = new HttpCookie(Constants.LoginSession.LOGIN_COOKIE);
            cookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpContext.Current.Response.Redirect("~/login");
        }

    }
}