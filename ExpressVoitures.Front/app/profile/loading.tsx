import React from "react";
import { Skeleton, Card } from "@nextui-org/react";

const Loading = () => {
  return (
    <>
      <Card className="max-w-lg mx-auto py-8" radius="lg">
        <div className="space-y-8">
          <Skeleton className="mx-auto w-4/5 rounded-lg">
            <div className="h-12 w-4/5 rounded-lg bg-default-100"></div>
          </Skeleton>

          <Skeleton className="mx-auto w-4/5 rounded-lg">
            <div className="h-12 w-4/5 rounded-lg bg-default-100"></div>
          </Skeleton>

          <Skeleton className="mx-auto w-4/5 rounded-lg">
            <div className="h-12 w-4/5 rounded-lg bg-default-100"></div>
          </Skeleton>

          <Skeleton className="mx-auto w-4/5 rounded-lg">
            <div className="h-12 w-4/5 rounded-lg bg-default-100"></div>
          </Skeleton>

          <div className="flex justify-center">
            <Skeleton className="mx-auto w-1/5 rounded-lg">
              <div className="h-8 w-1/5 rounded-lg bg-default-100"></div>
            </Skeleton>
          </div>
        </div>
      </Card>
    </>
  );
};

export default Loading;
