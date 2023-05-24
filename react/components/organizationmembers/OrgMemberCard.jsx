import React from "react";
import propTypes from "prop-types";
import { UserX, MoreHorizontal } from "react-feather";
import { Button } from "react-bootstrap";
import { useState } from "react";

const OrgMemberCard = ({ membersList, deleteMemberId }) => {
  const [open, setOpen] = useState(false);

  const handleClick = () => {
    deleteMemberId(membersList.id);
  };
  return (
    <tr>
      <td>
        <img
          src={membersList.avatarUrl}
          width="48"
          height="48"
          className="rounded-circle me-2"
          alt="Avatar"
        />
        {membersList.firstName} {membersList.mi.length >= 1 && membersList.mi}{" "}
        {membersList.lastName}
      </td>
      <td>{membersList.email}</td>
      <td>{membersList.position.name}</td>
      <td>{membersList.role.name}</td>
      <td>
        <Button
          className={open ? "btn-danger" : "btn-info"}
          onMouseEnter={() => setOpen(true)}
          onMouseLeave={() => setOpen(false)}
          onClick={handleClick}
        >
          {open ? (
            <UserX className="feather" />
          ) : (
            <MoreHorizontal className="feather" />
          )}
        </Button>
      </td>
    </tr>
  );
};
OrgMemberCard.propTypes = {
  membersList: propTypes.shape({
    avatarUrl: propTypes.string.isRequired,
    firstName: propTypes.string.isRequired,
    lastName: propTypes.string.isRequired,
    mi: propTypes.string.isRequired,
    email: propTypes.string.isRequired,
    role: propTypes.string.isRequired,
    position: propTypes.string.isRequired,
    id: propTypes.number.isRequired,
  }),
  deleteMemberId: propTypes.func.isRequired,
};
export default OrgMemberCard;
