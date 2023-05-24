using Sabio.Models.Domain.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Sabio.Models.Domain.Organizations
{
    public class Organization
    {
        public int Id { get; set; }
        public LookUp OrganizationType { get; set; }
        public string Name { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public LocationAddress Location { get; set; }
        public string Phone { get; set; }
        public string SiteUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public bool IsValidated { get; set; }
    }
}
