using ProductManagement.Infrastructure.Model;

namespace ProductManagment.Helpers
{
    public static class ValidationHelper
    {
        public static ValidationResult ValidateProduct(Product product)
        {
            if (product == null) { return new ValidationResult(false, Constants.ProductEmpty); }
            if (string.IsNullOrEmpty(product.Name)) { return new ValidationResult(false, Constants.ProductNameEmpty); }
            if (string.IsNullOrEmpty(product.Category)) { return new ValidationResult(false, Constants.ProductCategoryEmpty); }
            if (product.Price <= 0) { return new ValidationResult(false, Constants.ProductPriceLessThanZero); }

            return new ValidationResult(true);
        }

        public static ValidationResult ValidateProductUpdate(Product product)
        {
            if (product == null) { return new ValidationResult(false, Constants.ProductEmpty); }
            if (product.Id < 0) { return new ValidationResult(false, Constants.ProductIdLessThanZero); }
            return ValidateProduct(product);
        }
    }
}
