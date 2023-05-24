using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Domain;
using Sabio.Models.Domain.OrganizationMembers;
using Sabio.Models.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.Skills;
using Sabio.Models.Domain.Users;
using Sabio.Models;
using Sabio.Models.Requests.OrganizationMembersRequest;
using Sabio.Models.Domain.Schools;
using Google.Apis.AnalyticsReporting.v4.Data;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class OrganizationMemberService : IOrganizationMemberService
    {
        IDataProvider _data = null;
        public OrganizationMemberService(IDataProvider data)
        {
            _data = data;
        }
        public void Delete(int id)
        {
            string procName = "[dbo].[OrganizationMembers_Delete_ById]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }
            , returnParameters: null);
        }
        public void UpdateOrgMember(OrganizationMemberUpdateRequest model)
        {
            string procName = "[dbo].[OrganizationMembers_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@Id", model.Id);
            }
            , returnParameters: null);
        }
        public int AddOrgMember(OrganizationMemberAddRequest model)
        {
            int id = 0;
            string procName = "[dbo].[OrganizationMembers_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;
                    col.Add(idOut);
                }, returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;
                    int.TryParse(oId.ToString(), out id);
                });
            return id;  
        }

        public Paged<OrganizationMemberV2> ByOrgByNameByEmail (int pageIndex, int pageSize, string query = "")
        {
            Paged<OrganizationMemberV2> pagedList = null;
            List<OrganizationMemberV2> list = null;
            int totalCount = 0;
            string procName = "[dbo].[OrganizationMembers_Select_ByOrgByEmailByName]";
        

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@Query", query);
            },
            (reader, recordSetIndex) => 
            {
                int counter = 0;
                OrganizationMemberV2 orgMembers = SingleOrgMemberMapperV2(reader, ref counter);
                totalCount = reader.GetSafeInt32(counter++);
                if (list == null)
                {
                    list = new List<OrganizationMemberV2>();
                }
                list.Add(orgMembers);
            }
            );
            if (list != null)
            {
                pagedList = new Paged<OrganizationMemberV2>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<OrganizationMemberV2> ByOrgId(int pageIndex, int pageSize, int query)
        {
            Paged<OrganizationMemberV2> pagedList = null;
            List<OrganizationMemberV2> list = null;
            int totalCount = 0;
            string procName = "[dbo].[OrganizationMembers_Select_ByOrgId]";
            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@Query", query);
            },
            (reader, recordSetIndex) =>
            {
                int counter = 0;
                OrganizationMemberV2 orgMembers = SingleOrgMemberMapperV2(reader, ref counter);
                totalCount = reader.GetSafeInt32(counter++);
                if (list == null)
                {
                    list = new List<OrganizationMemberV2>();
                }
                list.Add(orgMembers);
            }
            );
            if (list  != null)
            {
                pagedList = new Paged<OrganizationMemberV2>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        } 
        public OrganizationMember GetOrgMember(int id)
        {
            string procName = "[dbo].[OrganizationMembers_SelectById]";
            OrganizationMember orgMember = null;
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)
            {
                int startingIndx = 0;
                orgMember = SingleOrgMemberMapper(reader, ref startingIndx);
            });
            return orgMember;
        }
        private static OrganizationMember SingleOrgMemberMapper(IDataReader reader, ref int startingIndx)
        {
            OrganizationMember orgMember = new OrganizationMember();
            LookUp lookUp = new LookUp();
            BaseLocation loc = new BaseLocation();
            MembersOrganizationBase memOrg = new MembersOrganizationBase();
            orgMember.Id = reader.GetSafeInt32(startingIndx++);
            memOrg.Id = reader.GetSafeInt32(startingIndx++);
            memOrg.Name = reader.GetSafeString(startingIndx++);
            lookUp.Id = reader.GetSafeInt32( startingIndx++);
            lookUp.Name = reader.GetSafeString( startingIndx++);
            loc.LineOne = reader.GetSafeString(startingIndx++);
            loc.LineTwo = reader.GetSafeString(startingIndx++);
            loc.City = reader.GetSafeString(startingIndx++);
            loc.State = reader.GetSafeString(startingIndx++);
            loc.Zip = reader.GetSafeString(startingIndx++);
            memOrg.Phone = reader.GetSafeString(startingIndx++);
            memOrg.SiteUrl = reader.GetSafeString(startingIndx++);
            memOrg.Logo = reader.GetSafeString(startingIndx++);
            memOrg.Description = reader.GetSafeString(startingIndx++);
            orgMember.Members = reader.DeserializeObject<List<Member>>(startingIndx++);
            orgMember.DateCreaated = reader.GetSafeDateTime(startingIndx++);
            orgMember.DateModified = reader.GetSafeDateTime(startingIndx++);
            orgMember.Location = loc;
            orgMember.OrganizationTypeId = lookUp;
            orgMember.Organization = memOrg;
            return orgMember;
        }
        private static OrganizationMemberV2 SingleOrgMemberMapperV2(IDataReader reader, ref int startingIndx)
        {
            OrganizationMemberV2 orgMember = new OrganizationMemberV2();
            LookUp lookUp = new LookUp();
            MembersOrganizationBase memOrg = new MembersOrganizationBase();
            Member member = new Member();
            orgMember.Id = reader.GetInt32(startingIndx++);
            memOrg.Id = reader.GetSafeInt32(startingIndx++);
            memOrg.Name = reader.GetSafeString(startingIndx++);
            memOrg.Description = reader.GetSafeString(startingIndx++);
            memOrg.Phone = reader.GetSafeString(startingIndx++);
            memOrg.Logo = reader.GetSafeString(startingIndx++);
            memOrg.SiteUrl = reader.GetSafeString(startingIndx++);
            lookUp.Id = reader.GetSafeInt32(startingIndx++);
            lookUp.Name = reader.GetSafeString(startingIndx++);
            member.Id = reader.GetSafeInt32(startingIndx++);
            member.FirstName = reader.GetSafeString(startingIndx++);
            member.LastName = reader.GetSafeString(startingIndx++);
            member.Mi = reader.GetSafeString(startingIndx++);
            member.Email = reader.GetSafeString(startingIndx++);
            member.AvatarUrl = reader.GetSafeString(startingIndx++);
            lookUp.Id = reader.GetSafeInt32(startingIndx++);
            lookUp.Name = reader.GetSafeString(startingIndx++);
            lookUp.Id = reader.GetSafeInt32(startingIndx++);
            lookUp.Name = reader.GetString(startingIndx++);
            member.OrganizationEmail = reader.GetSafeString(startingIndx++);
            orgMember.DateCreaated = reader.GetSafeDateTime(startingIndx++);
            orgMember.DateModified = reader.GetSafeDateTime(startingIndx++);
            orgMember.OrganizationType = lookUp;
            member.Role = lookUp;
            member.Position = lookUp;
            orgMember.Organization = memOrg;
            orgMember.User = member;
            return orgMember;
        }
        private static void AddCommonParams(OrganizationMemberAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@OrganizationId", model.OrganizationId);
            col.AddWithValue("@UserId", model.UserId);
            col.AddWithValue("@UserRoleId", model.UserRoleId);
            col.AddWithValue("@PositionType", model.PositionTypeId);
            col.AddWithValue("@OrganizationEmail", model.OrganizationEmail);
        }

    }
}
