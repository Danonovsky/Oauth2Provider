namespace Oauth2Provider.Exceptions;

public class UserAlreadyExistsException : BaseException
{
    public UserAlreadyExistsException(string Message) : base(Message){}
}