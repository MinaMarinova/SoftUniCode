namespace SharedTrip.Controllers
{
    using SharedTrip.Services;
    using SharedTrip.ViewModels.Users;
    using SIS.HTTP;
    using SIS.MvcFramework;
    
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public HttpResponse Register()
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterInputModel inputModel)
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }

            if(string.IsNullOrWhiteSpace(inputModel.Username) || string.IsNullOrWhiteSpace(inputModel.Email) || string.IsNullOrWhiteSpace(inputModel.Password))
            {
                return this.View();
            }

            if (inputModel.Username.Length < 5 || inputModel.Username.Length > 20)
            {
                return this.View();
            }

            if (inputModel.Password.Length < 6 || inputModel.Password.Length > 20)
            {
                return this.View();
            }

            if (inputModel.Password != inputModel.ConfirmPassword)
            {
                return this.View();
            }

            if (this.usersService.IsEmailUsed(inputModel.Email))
            {
                return this.View();
            }

            if (this.usersService.IsUsernameUsed(inputModel.Username))
            {
                return this.View();
            }

            this.usersService.CreateUser(inputModel.Username, inputModel.Email, inputModel.Password);

            return Redirect("/Users/Login");
        }

        public HttpResponse Login()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.View();
            }

            return Redirect("/");
        }

        [HttpPost]
        public HttpResponse Login(LoginInputModel inputModel)
        {
            var userId = this.usersService.GetUserId(inputModel.Username, inputModel.Password);

            if (userId == null)
            {
                return this.View();
            }

            this.SignIn(userId);
            return Redirect("/");
        }

        public HttpResponse Logout()
        {
            if (this.IsUserLoggedIn())
            {
                this.SignOut();
            }

            return this.Redirect("/");
        }
    }
}
