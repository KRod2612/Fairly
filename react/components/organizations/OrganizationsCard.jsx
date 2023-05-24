import React from "react";
import { Card, Button } from "react-bootstrap";
import orgDefaultImage from "../../assets/img/photos/unsplash-1.jpg";
import propTypes from "prop-types";

export default function OrganizationsCard(props) {
  return (
    <Card>
      <Card.Img
        width="100%"
        src={props.input.logo ? props.input.logo : orgDefaultImage}
        alt="Card image cap"
      />
      <Card.Body>
        <Card.Title className="mb-0 mt-2">
          {props.input.name ? props.input.name : "Organization Name"}{" "}
        </Card.Title>
        <Card.Subtitle className="mt-2">
          {props.input.siteUrl ? props.input.siteUrl : "Organization Site Url"}
        </Card.Subtitle>
        <Card.Text className="mt-2">
          {props.input.description
            ? props.input.description
            : "Organization Description"}
        </Card.Text>
        <Button disabled variant="primary">
          See More
        </Button>
      </Card.Body>
    </Card>
  );
}
OrganizationsCard.propTypes = {
  input: propTypes.shape({
    logo: propTypes.string.isRequired,
    name: propTypes.string.isRequired,
    siteUrl: propTypes.string.isRequired,
    description: propTypes.string.isRequired,
  }).isRequired,
};
