"use client";

import { useEffect } from "react";
import { useSession } from "next-auth/react";
import { useRouter } from "next/navigation";

import { Spinner } from "@nextui-org/react";

export default function LoginLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const { data: session, status } = useSession();
  const router = useRouter();

  useEffect(() => {
    if (status === "authenticated") {
      router.push("/dashboard");
    }
  }, [status, router]);

  if (status === "loading") {
    return (
      <div className="fixed inset-0 flex justify-center items-center">
        <Spinner />
      </div>
    );
  }

  if (session) {
    return null;
  }

  return (
    <section className="flex flex-col items-center justify-center">
      {children}
    </section>
  );
}
