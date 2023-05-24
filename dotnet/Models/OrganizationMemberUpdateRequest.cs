using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.OrganizationMembersRequest
{
    public class OrganizationMemberUpdateRequest : OrganizationMemberAddRequest , IModelIdentifier
    {
        public int Id { get; set; }
    }
}
