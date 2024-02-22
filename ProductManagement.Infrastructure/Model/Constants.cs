using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Model
{
    public static class Constants
    {
        public const string ProductNameEmpty = "Product name cannot be empty";
        public const string ProductNameDuplicate = "Product name cannot be duplicate";
        public const string ProductNotFound = "Product with given details was not found";
        public const string ProductEmpty = "Product cannot have empty data";
        public const string ProductCategoryEmpty = "Product category cannot be empty";
        public const string ProductPriceLessThanZero = "Product price cannot be less than 0";
        public const string ProductIdLessThanZero = "Product Id cannot be less than 0";
        public const string InValidSortOrder = "InValid Sort Order";
    }

    public class ValidationResult
    {
        public ValidationResult()
        {

        }

        public ValidationResult(bool success)
        {
            this.Success = success;
        }

        public ValidationResult(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public enum SortOrder
    {
        Asc, Desc, Ascending, Descending, asc, desc, descending, ascending
    }
}
