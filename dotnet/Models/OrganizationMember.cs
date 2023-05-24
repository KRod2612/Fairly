using Sabio.Models.Domain.Organizations;
using Sabio.Models.Domain.Schools;
using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.OrganizationMembers
{
    public class OrganizationMember
    {
        public int Id { get; set; }
        public MembersOrganizationBase Organization { get; set; }
        public LookUp OrganizationTypeId { get; set; }
        public  BaseLocation Location { get; set; }
        public List<Member> Members { get; set; }
        public DateTime DateCreaated { get; set; }
        public DateTime DateModified { get; set; }
    }
    
}
