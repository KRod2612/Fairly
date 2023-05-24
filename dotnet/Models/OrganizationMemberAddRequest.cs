using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.OrganizationMembersRequest
{
    public class OrganizationMemberAddRequest
    {
        [Required]
        public int OrganizationId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int UserRoleId { get; set; }
        [Required]
        public int PositionTypeId { get; set; }
        [Required]
        public string OrganizationEmail { get; set; }
    }
}
