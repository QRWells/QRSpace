namespace QRSpace.Shared.Enums
{
    public static class UserStates
    {
        public const ulong StateUserLoggedIn = 0b0001;
        public const ulong StateUserAway = 0b0010;
        public const ulong StateUserWaitingForShogi = 0b0100;
    }
}