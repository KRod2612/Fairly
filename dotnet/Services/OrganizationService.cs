using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Domain.Organizations;
using Sabio.Models.Requests.OrganizationRequests;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class OrganizationService : IOrganizationService
    {
        IDataProvider _data = null;
        public OrganizationService(IDataProvider data)
        {
            _data = data;
        }
        public void Delete(int id)
        {
            string procName = "[dbo].[Organizations_DeleteById]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }
            , returnParameters: null);
        }
        public void UpdateValidation(OrganizationUpdateValidation model)
        {
            string procName = "[dbo].[Organization_Update_IsValidated]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@IsValidated", model.IsValidated);
                col.AddWithValue("@Id", model.Id);
            }, returnParameters: null);
        }
        public void Update(OrganizationsUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Organizations_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col, userId);
                col.AddWithValue("@Id", model.Id);
            }
            , returnParameters: null);
        }
        public int Add(OrganizationAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[Organizations_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col, userId);

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

        private static void AddCommonParams(OrganizationAddRequest model, SqlParameterCollection col, int userId)
        {
            col.AddWithValue("@OrganizationTypeId", model.OrganizationTypeId);
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Headline", model.Headline);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@Logo", model.Logo);
            col.AddWithValue("@LocationId", model.LocationId);
            col.AddWithValue("@Phone", model.Phone);
            col.AddWithValue("@SiteUrl", model.SiteUrl);
            col.AddWithValue("@CreatedBy", userId);
        }

        public Paged<Organization> AllOrganizations(int pageIndex, int pageSize)
        {
            Paged<Organization> pagedList = null;
            List<Organization> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Organizations_SelectAll]";
            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            },
            (reader, recordSetIndex) =>
            {
                int counter = 0;
                Organization org = SingleOrganizationMapper(reader, ref counter);
                totalCount = reader.GetSafeInt32(counter++);

                if (list == null)
                {
                    list = new List<Organization>();
                }
                list.Add(org);
            }
            );
            if (list != null)
            {
                pagedList = new Paged<Organization>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<Organization> SearchCurrent(int pageIndex, int pageSize, int query)
        {
            Paged<Organization> pagedList = null;
            List<Organization> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Organizations_Select_ByCreatedBy]";
            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@Query", query);
            },
            (reader, recordSetIndex) =>
            {
                int counter = 0;
                Organization org = SingleOrganizationMapper(reader, ref counter);
                totalCount = reader.GetSafeInt32(counter++);

                if (list == null)
                {
                    list = new List<Organization>();
                }
                list.Add(org);
            }
            );
            if (list != null)
            {
                pagedList = new Paged<Organization>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Organization GetOrg(int id)
        {
            string procName = "[dbo].[Organizations_Select_ById]";
            Organization org = null;
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndx = 0;
                org = SingleOrganizationMapper(reader, ref startingIndx);
            }
            );
            return org;
        }

        public Organization GetOrganization(int userId)
        {
            string procName = "[dbo].[Organizations_SelectCurrentUserOrg]";
            Organization org = null;
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@UserId", userId);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                org = SingleOrganizationMapper(reader, ref startingIndex);
            }
            ); 
            return org;
        }

        public List<Organization> GetAll()
        {
            List<Organization> myOrgs = null;

            _data.ExecuteCmd("[dbo].[Organizations_SelectAllOrgsNonPaged]",
                inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndx = 0;
                    Organization org = SingleOrganizationMapper(reader, ref startingIndx);

                    if (myOrgs == null)
                    {
                        myOrgs = new List<Organization>();
                    }
                    myOrgs.Add(org);
                });

            return myOrgs;
        }

        private static Organization SingleOrganizationMapper(IDataReader reader, ref int startingIndx)
        {
            Organization org = new Organization();
            LocationAddress loc = new LocationAddress();
            LookUp lookUp = new LookUp();
            org.Id = reader.GetSafeInt32(startingIndx++);
            lookUp.Id = reader.GetSafeInt32(startingIndx++);
            lookUp.Name = reader.GetString(startingIndx++);
            org.OrganizationType = lookUp;
            org.Name = reader.GetSafeString(startingIndx++);
            org.Headline = reader.GetSafeString(startingIndx++);
            org.Description = reader.GetSafeString(startingIndx++);
            org.Logo = reader.GetSafeString(startingIndx++);
            loc.LineOne = reader.GetSafeString(startingIndx++);
            loc.LineTwo = reader.GetSafeString(startingIndx++);
            loc.City = reader.GetSafeString(startingIndx++);
            loc.Zip = reader.GetSafeString(startingIndx++);
            org.Location = loc;
            org.Phone = reader.GetSafeString(startingIndx++);
            org.SiteUrl = reader.GetSafeString(startingIndx++);
            org.DateCreated = reader.GetSafeDateTime(startingIndx++);
            org.DateModified = reader.GetSafeDateTime(startingIndx++);
            org.CreatedBy = reader.GetSafeInt32(startingIndx++);
            org.IsValidated = reader.GetSafeBool(startingIndx++);
            return org;
        }

        public void AddOrgLoc(OrgLocAddRequest model, int userId)
        {
            string procName = "[dbo].[OrganizationLocations_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@OrganizationId", model.OrganizationId);
                col.AddWithValue("@LocationId", model.LocationId);
                col.AddWithValue("@CreatedBy", userId);
            },

            returnParameters: null);
        }

        public OrganizationLocations GetOrgLoc(int organizationId)
        {
            OrganizationLocations orgLoc = null;

            string procName = "[dbo].[OrganizationLocations_Select_ByOrgId]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@OrganizationId", organizationId);

            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                orgLoc = MapSingleOrgLoc(reader, ref startingIndex);

            });

            return orgLoc;
        }

        public void DeleteOrgLoc(int organizationId, int locationId)
        {
            string procName = "[dbo].[OrganizationLocations_Delete]";
            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection paramCollection)
            {

                paramCollection.AddWithValue("@LocationId", locationId);
                paramCollection.AddWithValue("@OrganizationId", organizationId);
            },

            returnParameters: null);
        }

        public Paged<OrganizationLocations> GetOrgLocPage(int pageIndex, int pageSize, string nameQuery, int orgTypeId)
        {
            Paged<OrganizationLocations> pagedList = null;
            List<OrganizationLocations> list = null;
            int totalCount = 0;

            if (nameQuery == null)
            {
                nameQuery = "";
            }

            _data.ExecuteCmd("dbo.OrganizationLocations_Select_ByName_ByOrgType_Pagination",
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                    param.AddWithValue("@NameQuery", nameQuery);
                    param.AddWithValue("@OrganizationTypeId", orgTypeId);
                },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    OrganizationLocations orgLoc = MapSingleOrgLoc(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (list == null)
                    {
                        list = new List<OrganizationLocations>();
                    }

                    list.Add(orgLoc);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<OrganizationLocations>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        private static OrganizationLocations MapSingleOrgLoc(IDataReader reader, ref int startingIndex)
        {
            OrganizationLocations orgLoc = new OrganizationLocations();

            orgLoc.OrganizationId = reader.GetSafeInt32(startingIndex++);
            orgLoc.OrganizationType = reader.DeserializeObject<List<LookUp>>(startingIndex++);
            orgLoc.Name = reader.GetSafeString(startingIndex++);
            orgLoc.Headline = reader.GetSafeString(startingIndex++);
            orgLoc.Description = reader.GetSafeString(startingIndex++);
            orgLoc.Logo = reader.GetSafeString(startingIndex++);
            orgLoc.Phone = reader.GetSafeString(startingIndex++);
            orgLoc.SiteUrl = reader.GetSafeUri(startingIndex++);
            orgLoc.IsValidated = reader.GetSafeBool(startingIndex++);
            orgLoc.DateCreated = reader.GetSafeDateTime(startingIndex++);
            orgLoc.DateModified = reader.GetSafeDateTime(startingIndex++);
            orgLoc.JobCount = reader.GetSafeInt32(startingIndex++);
            orgLoc.Location = reader.DeserializeObject<List<Location>>(startingIndex++);


            return orgLoc;
        }
    }
}
