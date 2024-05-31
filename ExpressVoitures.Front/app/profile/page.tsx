"use client";

import React, { useEffect, useState, useMemo } from "react";

import { Input, Button, Spinner } from "@nextui-org/react";
import { title } from "@/components/primitives";

interface User {
  id: number;
  firstname: string;
  lastname: string;
  email: string;
  password: string;
}

export default function ProfilePage() {
  const [loading, setLoading] = useState(true);
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    // Get user info
    async function fetchUser() {
      const response = await fetch("/api/user/id");
      const user = await response.json();
      setUser(user);
      setLoading(false);
    }
    fetchUser();
  }, []);

  if (loading || !user) {
    return (
      <div className="fixed inset-0 flex justify-center items-center">
        <Spinner />
      </div>
    );
  }

  return (
    <div>
      <h1 className={title()}>Profile</h1>

      <div className="w-full flex flex-col flex-wrap gap-4 pt-8">
        <Input
          type="fristname"
          label="Firstname"
          defaultValue={user.firstname}
        />
        <Input type="lastname" label="Lastname" defaultValue={user.lastname} />
        <Input type="email" label="Email" defaultValue={user.email} />
        <Input type="password" label="Password" defaultValue={user.password} />

        <div className="flex justify-center">
          <Button className="w-[120px]" color="primary">
            Confirm
          </Button>
        </div>
      </div>
    </div>
  );
}
