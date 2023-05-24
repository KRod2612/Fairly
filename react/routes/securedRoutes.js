import { lazy } from "react";

const OrganizationsList = lazy(() =>
  import("../components/organizations/OrganizationsList")
);
const OrganizationsView = lazy(() =>
  import("../components/organizations/OrganizationsView")
);
const OrganizationsForm = lazy(() =>
  import("../components/organizations/OrganizationsForm")
);

const organizationRoutes = [
  {
    path: "/dashboard/admin/organizations/add",
    name: "Add Form",
    element: OrganizationsForm,
    roles: ["SysAdmin", "OrgAdmin", "HiringAdmin", "Employee", "Candidate"],
    exact: true,
    isAnonymous: true,
  }
];
const organizationMembersRoutes = [
  {
    path: "/dashboard/admin/organization/members/list",
    name: "Organization Members List",
    element: OrgMembersList,
    roles: ["SysAdmin","OrgAdmin"],
    exact: true,
    inAnonymous: false,
  }
];

const allRoutes = [
  ...organizationRoutes,
  ...organizationRoutes,
  ...organizationMembersRoutes,
];

export default allRoutes;
