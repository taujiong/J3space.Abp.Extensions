namespace J3space.Abp.Account
{
    public enum LoginResultType : byte
    {
        Success = 0,

        InvalidUserNameOrPassword = 1,

        NotAllowed = 2,

        LockedOut = 3,

        RequiresTwoFactor = 4
    }
}
