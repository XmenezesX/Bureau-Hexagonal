using BureauHexagonal.Core.Common.NotificationError;
using System.Text.Json.Serialization;

namespace BureauHexagonal.Core.Common.Operation
{
    public enum ErrorType
    {
        ValidationError = 1,
        DataBaseError = 2,
        CacheError = 3,
        BusinessError = 4,
        InternalError = 5,
        UndefinedError = 6,
        ExceptionError = 7,
        ProviderError = 8,
        EntityNotFound = 9,
    }

    public interface IOperation
    {
    }

    public interface IOperation<T> : IOperation
    {
    }

    public interface IOperationSuccess : IOperation
    {
    }

    public interface IOperationSuccess<T> : IOperationSuccess, IOperation<T>
    {
        T Data { get; init; }
    }

    public interface IOperationFail : IOperation
    {
        NotificationErrors NotificationErrors { get; init; }

        ErrorType ErrorType { get; init; }
    }

    public interface IOperationFail<T> : IOperationFail, IOperation<T>
    {
    }

    internal class OperationSuccess<T>(T result) : IOperationSuccess<T>
    {
        public T Data { get; init; } = result;
    }

    internal class OperationFail<T> : IOperationFail<T>
    {
        public OperationFail(NotificationErrors notificationErrors, ErrorType errorType)
        {
            NotificationErrors = notificationErrors;
            ErrorType = errorType;
        }

        [JsonPropertyName("notificationErrors")]
        public NotificationErrors NotificationErrors { get; init; }
        
        [JsonPropertyName("errorType")]
        public ErrorType ErrorType { get; init; }

        [JsonPropertyName("errorTypeDescription")]
        public string ErrorTypeDescription => ErrorType.ToString();
    }

    public static class OperationFactory
    {
        public static IOperationSuccess CreateSuccess()
        {
            return new OperationSuccess<object>(null!);
        }

        public static IOperationFail CreateFail(NotificationErrors notificationErrors)
        {
            return new OperationFail<object>(notificationErrors, ErrorType.UndefinedError);
        }

        public static IOperationFail<T> CreateFail<T>()
        {
            return new OperationFail<T>(NotificationErrors.Empty, ErrorType.UndefinedError);
        }

        public static IOperationFail CreateFail<T>(NotificationErrors notificationErrors)
        {
            return new OperationFail<T>(notificationErrors, ErrorType.UndefinedError);
        }

        public static IOperationFail CreateFail(NotificationErrors notificationErrors, ErrorType errorType)
        {
            return new OperationFail<object>(notificationErrors, errorType);
        }

        public static IOperationFail<T> CreateFail<T>(NotificationErrors notificationErrors, ErrorType errorType)
        {
            return new OperationFail<T>(notificationErrors, errorType);
        }

        public static IOperationFail CreateFail(Exception exception)
        {
            return new OperationFail<object>(NotificationErrors.Create(exception), ErrorType.ExceptionError);
        }

        public static IOperationFail<T> CreateFail<T>(Exception exception)
        {
            return new OperationFail<T>(NotificationErrors.Create(exception), ErrorType.ExceptionError);
        }

        public static IOperationFail CreateFail()
        {
            return new OperationFail<object>(NotificationErrors.Empty, ErrorType.UndefinedError);
        }

        public static IOperationSuccess<T> CreateSuccess<T>(T data)
        {
            return new OperationSuccess<T>(data);
        }

        public static IOperationSuccess<T> CreateSuccess<T>()
        {
            return new OperationSuccess<T>(default);
        }
    }

    public static class OperationExtends
    {
        public static bool IsFail(this IOperation operation)
        {
            return operation is IOperationFail;
        }

        public static bool IsFail<T>(this IOperation operation)
        {
            return operation is IOperationFail<T>;
        }

        public static bool IsSuccess(this IOperation operation)
        {
            return operation is IOperationSuccess;
        }

        public static bool IsSuccess<T>(this IOperation operation)
        {
            return operation is IOperationSuccess<T>;
        }

        public static NotificationErrors GetErrors(this IOperation operation)
        {
            if (operation is IOperationFail fail)
                return fail.NotificationErrors;

            return NotificationErrors.Empty;
        }

        public static IOperationFail<T> ToFail<T>()
        {
            return OperationFactory.CreateFail<T>();
        }

        public static IOperation<T> ToFail<T>(this NotificationErrors notification, ErrorType errorType)
        {
            return OperationFactory.CreateFail<T>(notification, errorType);
        }

        public static IOperation ToFail(this NotificationErrors notification, ErrorType errorType)
        {
            return OperationFactory.CreateFail(notification, errorType);
        }

        public static IOperationSuccess<T> ToSuccess<T>(this T data)
        {
            return OperationFactory.CreateSuccess(data);
        }



        public static T SuccessAs<T>(this IOperation operation)
        {
            if (operation is IOperationSuccess<T> success)
                return success.Data;
            
            return default!;
        }
    }
}
