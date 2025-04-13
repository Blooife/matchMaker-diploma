namespace MessageQueue.Constants;

public static class MessageTypeConstants
{
    public static class UserMessages
    {
        public const string UserCreated = "UserCreated";
        public const string UserDeleted = "UserDeleted";
    }
    
    public static class ProfileMessages
    {
        public const string ProfileCreated = "ProfileCreated";
        public const string ProfileUpdated = "ProfileUpdated";
        public const string ProfileDeleted = "ProfileDeleted";
    }
}