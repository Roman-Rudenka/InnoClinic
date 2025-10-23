namespace Application.Exceptions;

public abstract class BaseException(int statusCode, string message) : Exception(message)
{ 
    public int StatusCode { get; set; } = statusCode;
}

public class BadRequestException(string message) : BaseException(400, message);

public class NotFoundException(string message) : BaseException(404, message);

public class UnauthorizedException(string message) : BaseException(401, message);

