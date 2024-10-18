using EAD_Backend.Models;

namespace EAD_Backend.OtherModels
{
    public class ApiResponse<T>
    {
        private object data;
        private string v;
        private Inventory response;


        public string Message { get; set; }
        public T Data { get; set; }
       

        // Constructor with desired parameter order
        public ApiResponse(string message, T data = default )
        {
            Message = message;
            Data = data;
           
        }

        public ApiResponse(string v, Inventory response)
        {
            this.v = v;
            this.response = response;
        }

    }
}
