namespace User.Service.Exceptions;

public class DuplicateUserException : Exception
{
    public DuplicateUserException(string email) 
        : base($"User with email '{email}' already exists.")
    {
    }
}
