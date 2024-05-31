"use client";

import { useSession } from "next-auth/react";
import { useRouter } from "next/navigation";

import { Spinner } from "@nextui-org/react";

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const { data: session, status } = useSession();
  const router = useRouter();

  if (status === "loading") {
    return (
      <div className="fixed inset-0 flex justify-center items-center">
        <Spinner />
      </div>
    );
  }

  if (!session) {
    router.push("/login");
    return null;
  }

  return (
    <section className="flex flex-row items-center justify-center gap-4">
      <div className="display-block w-full">{children}</div>
    </section>
  );
}
