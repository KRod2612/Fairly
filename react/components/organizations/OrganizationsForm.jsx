import React, { useEffect, useState } from "react";
import * as lookUpService from "../../services/lookUpService";
import organizationsServices from "../../services/organizationsServices";
import { Helmet } from "react-helmet-async";
import debug from "sabio-debug";
import { Formik, Field } from "formik";
import OrgSchema from "../../schemas/orgSchema";
import { Button, Card, Container, Col, Row, Form } from "react-bootstrap";
import Swal from "sweetalert2";
import OrganizationsCard from "./OrganizationsCard";

function OrganizationsForm() {
  const formData = {
    organizationTypeId: "",
    name: "",
    headline: "",
    description: "",
    logo: "",
    locationId: 5,
    phone: "",
    siteUrl: "",
  };

  const [ratingInfo, setRatingInfo] = useState({
    commentId: 178,
    entityTypeId: 0,
    entityId: 0,
  });

  const [orgType, setOrgType] = useState([]);

  const _logger = debug.extend("OrganizationsForm");

  useEffect(() => {
    const payload = ["OrganizationTypes"];

    lookUpService.getTypes(payload).then(getTypeSuccess).catch(getTypeError);
  }, []);

  const mapType = (type) => {
    return (
      <option value={type.id} key={`OrgType_${type.id}`}>
        {type.name}
      </option>
    );
  };

  function getTypeSuccess(response) {
    const orgTypesArr = response.item.organizationTypes;
    setOrgType(orgTypesArr);
  }
  function getTypeError(err) {
    _logger(err);
  }

  const handleSubmit = (values) => {
    values.organizationTypeId = parseInt(values.organizationTypeId);

    organizationsServices
      .orgInsert(values)
      .then(orgInsertSuccess)
      .catch(orgInsertError);
  };

  function orgInsertSuccess(response) {
    Swal.fire({
      icon: "success",
      title: "Organization is added",
      confirmButtonText: "Ok",
    });
    _logger(response);

    setRatingInfo((prevState) => {
      let newState = { ...prevState };

      newState.entityId = response.item;

      return newState;
    });
  }

  function orgInsertError(err) {
    Swal.fire({
      icon: "error",
      title: "Could not add organization, please try agian",
      confirmButtonText: "Try Again",
    });
    _logger(err);
  }

  return (
    <React.Fragment>
      <Helmet title="Formik" />
      <Container className="p-0 justify-content-center ">
        <h1>Organization Form</h1>

        <Card>
          <Card.Header className="text-center">
            <Card.Title>Add Organization</Card.Title>
          </Card.Header>
          <Card.Body>
            <Formik
              enableReinitialize={true}
              validationSchema={OrgSchema.orgSchema}
              initialValues={formData}
              onSubmit={handleSubmit}
            >
              {({ handleSubmit, handleChange, values, touched, errors }) => (
                <Row>
                  <Col>
                    <Form onSubmit={handleSubmit}>
                      <Row>
                        <Col md={6}>
                          <Form.Group
                            className="mb-3"
                            controlId="validationFormik01"
                          >
                            <Form.Label>Name</Form.Label>
                            <Field
                              type="text"
                              name="name"
                              value={values.name}
                              onChange={handleChange}
                              className={
                                errors.name && touched.name
                                  ? "form-control error"
                                  : "form-control"
                              }
                            />
                            {errors.name && touched.name && (
                              <span className="input-feedback text-danger">
                                {errors.name}
                              </span>
                            )}
                          </Form.Group>
                        </Col>
                        <Col md={6}>
                          <Form.Group
                            className="mb-3"
                            controlId="validationFormik01"
                          >
                            <Form.Label>Headline</Form.Label>
                            <Field
                              type="text"
                              name="headline"
                              value={values.headline}
                              onChange={handleChange}
                              className={
                                errors.headline && touched.headline
                                  ? "form-control error"
                                  : "form-control"
                              }
                            />
                            {errors.headline && touched.headline && (
                              <span className="input-feedback text-danger">
                                {errors.headline}
                              </span>
                            )}
                          </Form.Group>
                        </Col>
                      </Row>
                      <Row>
                        <Form.Group
                          className="mb-3"
                          controlId="validationFormik01"
                        >
                          <Form.Label>Description</Form.Label>
                          <Field
                            component="textarea"
                            name="description"
                            value={values.description}
                            onChange={handleChange}
                            className={
                              errors.description && touched.description
                                ? "form-control error"
                                : "form-control"
                            }
                          />
                          {errors.description && touched.description && (
                            <span className="input-feedback text-danger">
                              {errors.description}
                            </span>
                          )}
                        </Form.Group>
                      </Row>
                      <Row>
                        <Form.Group
                          className="mb-3"
                          controlId="validationFormik01"
                        >
                          <Form.Label>Location Id</Form.Label>
                          <Field //locationId will be changed once organizationLocations form component is made for
                            type="number"
                            name="locationId"
                            value={values.locationId} //for the time being locationId is set to 5
                            onChange={handleChange}
                            className={
                              errors.locationId && touched.locationId
                                ? "form-control error"
                                : "form-control"
                            }
                          />
                          {errors.locationId && touched.locationId && (
                            <span className="input-feedback text-danger">
                              {errors.locationId}
                            </span>
                          )}
                        </Form.Group>
                      </Row>
                      <Row>
                        <Col md={6}>
                          <Form.Group className="mb-3">
                            <Form.Label>Phone</Form.Label>
                            <Field
                              type="string"
                              name="phone"
                              value={values.phone}
                              className={
                                errors.phone && touched.phone
                                  ? "form-control error"
                                  : "form-control"
                              }
                            />
                            {errors.phone && touched.phone && (
                              <span className="input-feedback text-danger">
                                {errors.phone}
                              </span>
                            )}
                          </Form.Group>
                        </Col>
                        <Col md={6}>
                          <Form.Group
                            className="col mb-3"
                            controlId="validationFormik01"
                          >
                            <Form.Label>Organization Type</Form.Label>
                            <Field
                              component="select"
                              name="organizationTypeId"
                              values={values.organizationTypeId}
                              onChange={handleChange}
                              className={
                                errors.organizationTypeId &&
                                touched.organizationTypeId
                                  ? "form-select error"
                                  : "form-select"
                              }
                            >
                              <option value="">Select Type</option>
                              {orgType.map(mapType)}
                            </Field>
                            {errors.organizationTypeId &&
                              touched.organizationTypeId && (
                                <span className="input-feedback text-danger">
                                  {errors.organizationTypeId}
                                </span>
                              )}
                          </Form.Group>
                        </Col>
                      </Row>
                      <Row>
                        <Col md={6}>
                          <Form.Group
                            className="mb-3"
                            controlId="validationFormik01"
                          >
                            <Form.Label>Site Url</Form.Label>
                            <Field
                              type="text"
                              name="siteUrl"
                              value={values.siteUrl}
                              onChange={handleChange}
                              className={
                                errors.siteUrl && touched.siteUrl
                                  ? "form-control error"
                                  : "form-control"
                              }
                            />
                            {errors.siteUrl && touched.siteUrl && (
                              <span className="input-feedback text-danger">
                                {errors.siteUrl}
                              </span>
                            )}
                          </Form.Group>
                        </Col>
                        <Col md={6}>
                          <Form.Group
                            className="mb-3"
                            controlId="validationFormik01"
                          >
                            <Form.Label>Logo</Form.Label>
                            <Field
                              type="text"
                              name="logo"
                              value={values.logo}
                              onChange={handleChange}
                              className={
                                errors.logo && touched.logo
                                  ? "form-control error"
                                  : "form-control"
                              }
                            />
                            {errors.logo && touched.logo && (
                              <span className="input-feedback text-danger">
                                {errors.logo}
                              </span>
                            )}
                          </Form.Group>
                        </Col>
                      </Row>
                      <Row>
                        <Form.Group
                          className="mb-3"
                          controlId="validationFormik01"
                        >
                          <Form.Label>Overall Rating</Form.Label>
                        </Form.Group>
                        <div className="d-grid gap-2 d-md-flex justify-content-md-end">
                          <Button type="submit" className="me-md-2">
                            Submit form
                          </Button>
                        </div>
                      </Row>
                    </Form>
                  </Col>
                  <Col md={4} className="">
                    <OrganizationsCard input={values} />
                  </Col>
                </Row>
              )}
            </Formik>
          </Card.Body>
        </Card>
      </Container>
    </React.Fragment>
  );
}

export default OrganizationsForm;
