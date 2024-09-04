using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyerSeller.Data__Data_Access_Layer_.DTOs
{
    public class UpdateProductCategoryDTO
    {
        /// <summary>
        /// Category Name
        /// </summary>
        public string? CategoryDisplayName { get; set; }

        /// <summary>
        /// Active or not
        /// </summary>
        public bool? IsActive { get; set; }
    }
}
