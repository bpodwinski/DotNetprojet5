import React from "react";
import { Skeleton, Card } from "@nextui-org/react";

const Loading = () => {
  return (
    <>
      <Card className="w-full space-y-5 p-4" radius="lg">
        <Skeleton className="rounded-lg">
          <div className="h-8 rounded-lg bg-default-100"></div>
        </Skeleton>
        <div className="space-y-8">
          <Skeleton className="w-4/5 rounded-lg">
            <div className="h-5 w-4/5 rounded-lg bg-default-100"></div>
          </Skeleton>
          <Skeleton className="w-4/5 rounded-lg">
            <div className="h-5 w-4/5 rounded-lg bg-default-100"></div>
          </Skeleton>
          <Skeleton className="w-4/5 rounded-lg">
            <div className="h-5 w-4/5 rounded-lg bg-default-100"></div>
          </Skeleton>
          <Skeleton className="w-4/5 rounded-lg">
            <div className="h-5 w-4/5 rounded-lg bg-default-100"></div>
          </Skeleton>
          <Skeleton className="w-5/5 rounded-lg">
            <div className="h-5 w-5/5 rounded-lg bg-default-100"></div>
          </Skeleton>
          <Skeleton className="w-5/5 rounded-lg">
            <div className="h-5 w-5/5 rounded-lg bg-default-100"></div>
          </Skeleton>
          <div className="flex justify-center">
            <Skeleton className="w-1/5 rounded-lg">
              <div className="h-8 w-1/5 rounded-lg bg-default-100"></div>
            </Skeleton>
          </div>
        </div>
      </Card>
    </>
  );
};

export default Loading;
