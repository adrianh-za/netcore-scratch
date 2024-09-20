namespace MixedAPIs.Exceptions;

[Serializable]
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException() : base() { }

    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }

    public BadRequestException() : base() { }

    public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class ApiVersionException : Exception
{
    public ApiVersionException(string message) : base(message) { }

    public ApiVersionException() : base() { }

    public ApiVersionException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class AuthenticationException : Exception
{
    public AuthenticationException(string message) : base(message) { }

    public AuthenticationException() : base() { }

    public AuthenticationException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class AuthorizationException : Exception
{
    public AuthorizationException(string message) : base(message) { }

    public AuthorizationException() : base() { }

    public AuthorizationException(string message, Exception innerException) : base(message, innerException) { }
}

[Serializable]
public class ValidationFailException : Exception
{
    public const string DefaultMessage = "Validation error(s) occurred";

    public ValidationFailException() : base() { }

    public ValidationFailException(string message) : base(message) { }

    public ValidationFailException(string message, Exception innerException) : base(message, innerException) { }

    public ValidationFailException(IDictionary<string, List<string>> validationMessages) : this(DefaultMessage, validationMessages) { }

    public ValidationFailException(string message, IDictionary<string, List<string>> validationMessages) : base(message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentException($"'{nameof(message)}' cannot be null or empty.", nameof(message));

        ArgumentNullException.ThrowIfNull(validationMessages);

        Messages = validationMessages;
    }

    public ValidationFailException(string key, string errorMessage) : this(DefaultMessage, key, errorMessage) { }

    public ValidationFailException(string message, string key, string errorMessage) : base(message)
    {
        if (string.IsNullOrEmpty(message))
            throw new ArgumentException($"'{nameof(message)}' cannot be null or empty.", nameof(message));

        if (string.IsNullOrEmpty(key))
            throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));

        if (string.IsNullOrEmpty(errorMessage))
            throw new ArgumentException($"'{nameof(errorMessage)}' cannot be null or empty.", nameof(errorMessage));

        Messages = new Dictionary<string, List<string>>
        {
            { key, [errorMessage] }
        };
    }

    public IDictionary<string, List<string>> Messages { get; init; } = null!;
}

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }

    public ConflictException() : base() { }

    public ConflictException(string message, Exception innerException) : base(message, innerException) { }
}