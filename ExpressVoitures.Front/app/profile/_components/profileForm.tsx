import React from "react";
import { Card, CardBody, Input, Button, Spacer } from "@nextui-org/react";

interface UserFormProps {
  user: {
    firstname: string;
    lastname: string;
    email: string;
    password: string;
  };
  onSubmit: () => void;
}

const UserForm: React.FC<UserFormProps> = ({ user, onSubmit }) => {
  return (
    <Card className="max-w-lg mx-auto py-8">
      <CardBody className="max-w-md mx-auto flex flex-col justify-center items-center gap-4">
        <Input type="text" label="Firstname" defaultValue={user.firstname} />
        <Input type="text" label="Lastname" defaultValue={user.lastname} />
        <Input type="email" label="Email" defaultValue={user.email} />
        <Input type="password" label="Password" defaultValue={user.password} />

        <Spacer y={4} />

        <div className="flex justify-center">
          <Button className="w-[120px]" color="primary" onClick={onSubmit}>
            Confirm
          </Button>
        </div>
      </CardBody>
    </Card>
  );
};

export default UserForm;
