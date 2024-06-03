"use client";

import React, { useEffect, useState } from "react";
import dynamic from "next/dynamic";

import { Spacer } from "@nextui-org/react";
import { title } from "@/components/primitives";
import Loading from "./loading";

// Import component
const UserForm = dynamic(() => import("./_components/profileForm"), {
  ssr: false,
  loading: () => <Loading />,
});

interface User {
  id: number;
  firstname: string;
  lastname: string;
  email: string;
  password: string;
}

export default function ProfilePage() {
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    async function fetchUser() {
      const response = await fetch("/api/user/id");
      const user = await response.json();
      setUser(user);
    }
    fetchUser();
  }, []);

  const handleSubmit = () => {
    console.log("Form submitted");
  };

  if (!user) {
    return null;
  }

  return (
    <div>
      <div className="text-center">
        <h1 className={title()}>Profile</h1>
      </div>

      <Spacer y={8} />

      <UserForm user={user} onSubmit={handleSubmit} />
    </div>
  );
}
