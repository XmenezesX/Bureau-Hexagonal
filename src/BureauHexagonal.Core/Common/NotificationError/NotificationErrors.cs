using System.Text.Json.Serialization;

namespace BureauHexagonal.Core.Common.NotificationError
{
    public sealed class NotificationErrors
    {
        [JsonPropertyName("errors")]
        public List<Errors> _errors { get; private set; } = new List<Errors>();

        [JsonPropertyName("friendlyMessages")]
        public List<string> _friendlyMessages { get; private set; } = new List<string>();

        public static NotificationErrors Empty => new NotificationErrors();
        private NotificationErrors() { }

        public static NotificationErrors Create(string propertyName, string message, string friendlyMessage = "")
        {
            var notification = Empty;
            notification.AddError(propertyName, message, friendlyMessage);
            return notification;
        }

        public void AddError(string propertyName, string message, string friendlyMessage = "")
        {
            _errors.Add(new Errors
            {
                Message = message,
                PropertyName = propertyName,
            });

            if (string.IsNullOrWhiteSpace(friendlyMessage))
            {
                _friendlyMessages.Add(message);
                return;
            }

            _friendlyMessages.Add(friendlyMessage);
        }

        public static NotificationErrors Create(Exception exception)
        {
            int exceptionCounter = 1;
            var notificationErrors = Create(nameof(exception), exception.Message);

            while (exception.InnerException != null)
            {
                notificationErrors.AddError(nameof(exception) + "_" + (++exceptionCounter).ToString(), exception.InnerException.Message);
                exception = exception.InnerException;
            }

            return notificationErrors;
        }

        public bool HaveError()
        {
            return _errors.Count > 0;
        }
    }
}
