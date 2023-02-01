namespace Oauth2Provider.Exceptions;

public class UserNotFoundException : BaseException
{
    public UserNotFoundException(string Message) : base(Message){}
}