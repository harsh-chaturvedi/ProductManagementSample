using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Model
{
    public class ResponseBase
    {
        public ResponseBase()
        {

        }

        public ResponseBase(bool success)
        {
            this.Success = success;
        }

        public ResponseBase(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }

        /// <summary>
        /// Flag to determine if the operation is successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message to be passed to the calling method.
        /// </summary>
        public string Message { get; set; }

    }

    public class AddProductResponse : ResponseBase
    {
        public int Id { get; set; }
        public AddProductResponse()
        {

        }

        public AddProductResponse(bool success) : base(success)
        {
        }

        public AddProductResponse(bool success, string message) : base(success, message)
        {
        }
    }

    public class GetAllProductResponse : ResponseBase
    {
        public IEnumerable<Product> Products { get; set; }
        public GetAllProductResponse()
        {

        }

        public GetAllProductResponse(bool success) : base(success)
        {
        }

        public GetAllProductResponse(bool success, string message) : base(success, message)
        {
        }
    }

    public class TotalCountResponse : ResponseBase
    {
        public int Count { get; set; }
        public TotalCountResponse()
        {

        }

        public TotalCountResponse(bool success) : base(success)
        {
        }

        public TotalCountResponse(bool success, string message) : base(success, message)
        {
        }
    }
}
