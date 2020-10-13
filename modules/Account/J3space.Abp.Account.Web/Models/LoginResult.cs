namespace J3space.Abp.Account.Web.Models
{
    public class LoginResult
    {
        public LoginResult(LoginResultType result)
        {
            Result = result;
        }

        public LoginResultType Result { get; }

        public string Description => Result.ToString();
    }
}