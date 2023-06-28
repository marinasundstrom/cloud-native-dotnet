namespace BlazorApp1.Domain;

public static class Errors
{
    public static class Users
    {
        public static readonly Error UserNotFound = new Error(nameof(UserNotFound), "User not found", string.Empty);
    }
}
