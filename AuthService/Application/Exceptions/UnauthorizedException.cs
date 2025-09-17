using Domain.Common;

namespace Application.Exceptions;

public class UnauthorizedException(string message) : BaseExceptionHandler("Unauthorized", 401, message);
