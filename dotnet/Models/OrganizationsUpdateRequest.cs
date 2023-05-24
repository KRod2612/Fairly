using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.OrganizationRequests
{
    public class OrganizationsUpdateRequest : OrganizationAddRequest , IModelIdentifier
    {
        public int Id { get; set; }
    }
}
