using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Model
{
    public class Product
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Product Name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Product Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Product Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets Product Price
        /// </summary>
        public decimal Price { get; set; }
    }
}
