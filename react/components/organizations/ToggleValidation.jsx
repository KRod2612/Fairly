import { useState } from "react";
import React from "react";
import { Form } from "react-bootstrap";
import propTypes from "prop-types";

function ToggleValidation({ isValid, isCardVal }) {
  const [switchState, setSwitchState] = useState(isCardVal);
  const handleChange = () => {
    setSwitchState(!switchState);
    isValid(!switchState);
  };

  return (
    <Form.Check
      type="switch"
      id="exampleCustomSwitch"
      name="customSwitch"
      label="Valid Organization"
      defaultChecked={switchState}
      onChange={handleChange}
    />
  );
}
ToggleValidation.propTypes = {
  isCardVal: propTypes.bool.isRequired,
  isValid: propTypes.func.isRequired,
};
export default ToggleValidation;
