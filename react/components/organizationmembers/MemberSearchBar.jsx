import React, { useEffect, useState } from "react";
import { Form, InputGroup, Button } from "react-bootstrap";
import propTypes from "prop-types";
import { Search, X } from "react-feather";

function MemberSearchBar({ mSearch }) {
  const [searchTerm, setSearchTerm] = useState("");
  useEffect(() => {
    mSearch(searchTerm);
  }, [searchTerm]);
  const handleChange = (e) => {
    setSearchTerm(e.target.value);
  };
  const handleClose = () => {
    setSearchTerm("");
  };

  return (
    <Form>
      <InputGroup className="input-group-navbar border rounded">
        <Form.Control
          placeholder="Search files"
          aria-label="Search"
          onChange={handleChange}
          value={searchTerm}
        />
        <Button onClick={handleClose}>{searchTerm.length > 0 && <X />}</Button>
        <Button disabled>
          <Search className="feather" />
        </Button>
      </InputGroup>
    </Form>
  );
}
MemberSearchBar.propTypes = {
  mSearch: propTypes.func.isRequired,
};
export default MemberSearchBar;
