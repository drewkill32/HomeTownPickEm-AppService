import React from "react";
import Logo from "@/components/icons/Logo";

type Props = {
  children?: React.ReactNode;
  heading: string;
};

export default function SimpleLayout({ children, heading }: Props) {
  return (
    <>
      <div className="mt-12 flex flex-col space-y-1.5 p-6">
        <Logo className="m-auto h-16 w-16" />
        <h3 className="whitespace-nowrap text-center text-2xl font-semibold tracking-tight">
          {heading}
        </h3>
      </div>
      <div className="m-auto mx-auto my-4 max-w-sm px-3 text-card-foreground">
        {children}
      </div>
    </>
  );
}
