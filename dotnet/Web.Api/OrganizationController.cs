using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Organizations;
using Sabio.Models.Requests.OrganizationRequests;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/organizations")]
    [ApiController]
    public class OrganizationController : BaseApiController
    {
        private IOrganizationService _service = null;
        private IAuthenticationService<int> _authService;
        public OrganizationController(IOrganizationService service, IAuthenticationService<int> auth, ILogger<OrganizationController> logger) : base(logger)
        {
            _service = service;
            _authService = auth;
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
        [HttpPut("isValid")]
        public ActionResult<SuccessResponse> Update(OrganizationUpdateValidation model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _service.UpdateValidation(model);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }
        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(OrganizationsUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.Update(model, userId);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }
        [HttpPost]
        public ActionResult<ItemResponse<int>> Add(OrganizationAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }
        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Organization>>> AllOrganizations(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse result = null;
            try
            {
                Paged<Organization> page = _service.AllOrganizations(pageIndex, pageSize);
                if (page == null)
                {
                    code = 404;
                    result = new ErrorResponse("Records Not Found");
                }
                else
                {
                    result = new ItemResponse<Paged<Organization>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                result = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, result);
        }
        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Organization>>> SearchCurrent(int pageIndex, int pageSize, int query)
        {
            int code = 200;
            BaseResponse result = null;
            try
            {
                Paged<Organization> paged = _service.SearchCurrent(pageIndex, pageSize, query);
                if (paged == null)
                {
                    code = 404;
                    result = new ErrorResponse("Records Not Found");
                }
                else
                {
                    result = new ItemResponse<Paged<Organization>> { Item = paged };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                result = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, result);
        }
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Organization>> GetOrg(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                Organization organization = _service.GetOrg(id);
                if (organization == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found."); ;
                }
                else
                {
                    response = new ItemResponse<Organization>()
                    {
                        Item = organization
                    };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"GenericError: {ex.Message}");
            }
            return StatusCode(iCode, response);
        }

        [HttpPost("locations")]
        public ActionResult<SuccessResponse> Create(OrgLocAddRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                _service.AddOrgLoc(model, userId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("{organizationId:int}/locations")]
        public ActionResult GetById(int organizationId)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                OrganizationLocations OrgLoc = _service.GetOrgLoc(organizationId);


                if (OrgLoc == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<OrganizationLocations> { Item = OrgLoc };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);
        }

        [HttpGet("user/{userId:int}")]
        public ActionResult GetOrganizationByUserId(int userId)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                Organization org = _service.GetOrganization(userId);
                if (org == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Organization> { Item = org };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);
        }

        [HttpGet("")]
        public ActionResult<ItemsResponse<Organization>> GetAll()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Organization> list = _service.GetAll();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found");
                }
                else
                {
                    response = new ItemsResponse<Organization> { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }

        [HttpDelete("{organizationId:int}/location/{locationId:int}")]
        public ActionResult<SuccessResponse> DeleteById(int organizationId, int locationId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.DeleteOrgLoc(organizationId, locationId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("locations/paginate")]
        public ActionResult<ItemResponse<Paged<OrganizationLocations>>> GetPage(int pageIndex, int pageSize, string nameQuery, int orgTypeId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<OrganizationLocations> page = _service.GetOrgLocPage(pageIndex, pageSize, nameQuery, orgTypeId);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<OrganizationLocations>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }

    }
}