namespace J3space.AuthServer.IdentityServer
{
    public class UserLoginDto
    {
        public string UserNameOrEmailAddress { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}