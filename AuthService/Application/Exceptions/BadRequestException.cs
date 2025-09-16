using Domain.Common;

namespace Application.Exceptions;

public class BadRequestException(string message) : BaseExceptionHandler("Bad Request", 400, message);