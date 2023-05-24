import React from "react";
import PropTypes from "prop-types";
import { Card } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import debug from "sabio-debug";
import toastr from "toastr";
import "./OrganizationStyles.css";
import ToggleValidation from "./ToggleValidation";
import organizationsServices from "../../services/organizationsServices";
import { Briefcase } from "react-feather";

const _loggerPage = debug.extend("ORG");

function OrganizationAdminCard(props) {
  const orgObj = props.organization;
  _loggerPage(orgObj, "here");

  const navigate = useNavigate();

  const onLocalViewMoreClicked = (e) => {
    e.preventDefault();
    let viewMorePageUrl = "/dashboard/admin/organizations/view";
    navigate(viewMorePageUrl, { state: orgObj });
  };

  const isValid = (orgVal) => {
    const payload = {
      isValidated: orgVal,
      id: props.organization.organizationId,
    };
    organizationsServices
      .validationUpdate(payload)
      .then(onValidSuccess)
      .catch(onValidError);
    function onValidSuccess(response) {
      _loggerPage(response);
      toastr.success("Validation updated");
    }
    function onValidError(err) {
      _loggerPage(err);
      toastr.error("Error updating validation", err);
    }
  };

  return (
    <tr>
      <td>
        <img
          src={orgObj.logo}
          className="organization-admin-card-image-size"
          alt={orgObj.name}
        />
      </td>
      <td>
        <Card.Title className="organization-admin-card-title-underline">
          {" "}
          <a onClick={onLocalViewMoreClicked}>{orgObj.name}</a>
        </Card.Title>
        <strong>
          {orgObj.location[0].city}, {orgObj.location[0].state.name}
        </strong>{" "}
        <p>{orgObj.description}</p>
        <p>
          <Briefcase className="mb-1" size={17} /> {orgObj.jobCount}
          {orgObj.jobCount === 1 ? " Job" : " Jobs"}
        </p>
      </td>
      <td>
        <ToggleValidation
          isCardVal={orgObj.isValidated}
          isValid={isValid}
        ></ToggleValidation>
      </td>
    </tr>
  );
}
OrganizationAdminCard.propTypes = {
  organization: PropTypes.shape({
    logo: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    organizationId: PropTypes.number.isRequired,
    isValidated: PropTypes.bool.isRequired,
    jobCount: PropTypes.number.isRequired,
    location: PropTypes.arrayOf(
      PropTypes.shape({
        city: PropTypes.string.isRequired,
        state: PropTypes.shape({
          name: PropTypes.string.isRequired,
        }).isRequired,
      }).isRequired
    ).isRequired,
  }).isRequired,
};
export default OrganizationAdminCard;
