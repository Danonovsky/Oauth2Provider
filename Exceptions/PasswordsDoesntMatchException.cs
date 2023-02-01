namespace Oauth2Provider.Exceptions;

public class PasswordsDoesntMatchException : BaseException {
    public PasswordsDoesntMatchException(string Message) : base(Message){}
}