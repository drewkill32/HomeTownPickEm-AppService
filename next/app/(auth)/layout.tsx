import { buttonVariants } from "@/components/ui/button";
import { ArrowLeft } from "lucide-react";
import Link from "next/link";

export default function AuthLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <>
      <div className="ml-4 mt-6">
        <Link
          href="/"
          className={buttonVariants({
            variant: "ghost",
          })}
        >
          <ArrowLeft className="mr-1" /> Back
        </Link>
      </div>
      {children}
    </>
  );
}
