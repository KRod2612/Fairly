import axios from "axios";
import * as helper from "../services/serviceHelpers";

const endpoint = `${helper.API_HOST_PREFIX}/api/orgMembers`;

const getByOrgId = (id, index, size) => {
  const config = {
    method: "GET",
    url: `${endpoint}/organization/search?pageIndex=${index}&pageSize=${size}&query=${id}`,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};
const getByOrgNameEmail = (query, index, size) => {
  const config = {
    method: "GET",
    url: `${endpoint}/search?pageIndex=${index}&pageSize=${size}&query=${query}`, 
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};
const deleteMember = (id) => {
  const config = {
    method: "DELETE",
    url: `${endpoint}/${id}`, 
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};
export default { getByOrgId, getByOrgNameEmail, deleteMember };