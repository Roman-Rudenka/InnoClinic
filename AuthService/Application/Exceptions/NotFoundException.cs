using Domain.Common;

namespace Application.Exceptions;

public class NotFoundException(string message) : BaseExceptionHandler("Not Found", 404, message);
