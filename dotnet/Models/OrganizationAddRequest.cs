using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.OrganizationRequests
{
    public class OrganizationAddRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int OrganizationTypeId { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Name { get; set; }
        [StringLength(200, MinimumLength = 2)]
        public string Headline { get; set; }
        [StringLength(200, MinimumLength = 2)]
        public string Description { get; set; }
        [StringLength(225, MinimumLength = 2)]
        public string Logo { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int LocationId { get; set; }
        [StringLength(50, MinimumLength = 2)]
        public string Phone { get; set; }
        [StringLength(225, MinimumLength = 2)]
        public string SiteUrl { get; set; }

    }
}
