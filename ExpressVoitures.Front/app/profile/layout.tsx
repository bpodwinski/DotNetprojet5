"use client";

import { useSession } from "next-auth/react";
import { Suspense } from "react";
import Loading from "./loading";

export default function ProfileLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const { data: session, status } = useSession();

  if (status === "loading") {
    return null;
  }
  return (
    <section className="flex flex-row items-center justify-center">
      <div className="display-block w-full">
        <Suspense fallback={<Loading />}>{children}</Suspense>
      </div>
    </section>
  );
}
