using System;

namespace Sabio.Models.Domain.OrganizationMembers
{
    public class OrganizationMemberV2
    {
        public int Id { get; set; }
        public MembersOrganizationBase Organization { get; set; }
        public LookUp OrganizationType { get; set; }
        public Member User { get; set; }
        public DateTime DateCreaated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
