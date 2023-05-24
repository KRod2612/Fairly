using Sabio.Models;
using Sabio.Models.Domain.OrganizationMembers;
using Sabio.Models.Requests.OrganizationMembersRequest;

namespace Sabio.Services.Interfaces
{
    public interface IOrganizationMemberService
    {
        int AddOrgMember(OrganizationMemberAddRequest model);
        Paged<OrganizationMemberV2> ByOrgByNameByEmail(int pageIndex, int pageSize, string query);
        Paged<OrganizationMemberV2> ByOrgId(int pageIndex, int pageSize, int query);
        void Delete(int id);
        OrganizationMember GetOrgMember(int id);
        void UpdateOrgMember(OrganizationMemberUpdateRequest model);
    }
}