namespace My_Core_Project.Models
{
    public class ResponseModel
    {
        /// <summary>
        /// 0 = Success, -1 = Failure, -2 = Required Field Missing, etc.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Human-readable message for UI
        /// </summary>
        public string? Message { get; set; }

        public ResponseModel() { }

        public ResponseModel(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }

}
