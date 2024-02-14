namespace ExpenseTrackingSystem.Services.Model
{


    public class ServiceResult
    {
        public bool SuccessSituation { get; }
        public string Message { get; }

        private ServiceResult(bool success, string message)
        {
            SuccessSituation = success;
            Message = message;
        }

        public static ServiceResult Success(string message)
        {
            return new ServiceResult(true, message);
        }

        public static ServiceResult Error(string message)
        {
            return new ServiceResult(false, message);
        }
    }
}
