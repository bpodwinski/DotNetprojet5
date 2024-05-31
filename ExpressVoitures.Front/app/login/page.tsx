"use client";

import { useState, useMemo } from "react";
import { signIn } from "next-auth/react";

import { title } from "@/components/primitives";
import {
  Card,
  CardHeader,
  CardBody,
  Input,
  Button,
  Popover,
  PopoverTrigger,
  PopoverContent,
} from "@nextui-org/react";

export default function LoginPage() {
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [error, setError] = useState<string | null>(null);
  const [isPopoverVisible, setPopoverVisible] = useState<boolean>(false);

  // Validations
  const validateEmail = (email: string) =>
    email.match(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/i);
  const isInvalid = useMemo(() => {
    if (email === "") return false;

    return validateEmail(email) ? false : true;
  }, [email]);

  const handleSignIn = async () => {
    setError(null);
    setPopoverVisible(false);
    const result = await signIn("credentials", {
      redirect: false,
      email,
      password,
    });

    if (result?.error) {
      setError(result.error);
      setPopoverVisible(true);
      console.error(result);
    }
  };

  return (
    <Card className="w-[600px] p-6">
      <CardHeader>
        <h1 className={title()}>Sign in</h1>
      </CardHeader>
      <CardBody className="gap-4">
        <Input
          type="email"
          label="Email"
          value={email}
          isInvalid={isInvalid}
          color={isInvalid ? "danger" : "default"}
          errorMessage="Please enter a valid email"
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <Input
          type="password"
          label="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <div className="flex justify-center">
          <Popover
            placement="bottom"
            showArrow={true}
            color="danger"
            isOpen={isPopoverVisible}
            onClose={() => setPopoverVisible(false)}
          >
            <PopoverTrigger>
              <Button
                onClick={handleSignIn}
                className="w-[120px]"
                color="primary"
              >
                Sign in
              </Button>
            </PopoverTrigger>
            <PopoverContent>
              <div className="px-1 py-2">
                <div className="text-small font-bold">Error</div>
                <div className="text-tiny">{error}</div>
              </div>
            </PopoverContent>
          </Popover>
        </div>
      </CardBody>
    </Card>
  );
}
