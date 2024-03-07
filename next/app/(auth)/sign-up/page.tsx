import SimpleLayout from "@/components/SimpleLayout";
import { SignUpForm } from "./SignUpForm";
import { Metadata } from "next";
import { metadataTitle } from "@/utils";

export const metadata: Metadata = metadataTitle("Sign Up");

export default function Signup({
  searchParams,
}: {
  searchParams: {
    message: string | undefined;
    redirectUrl: string | undefined;
  };
}) {
  return (
    <SimpleLayout heading="Sign in to St. Pete Pick'em">
      <SignUpForm message={searchParams.message} />
    </SimpleLayout>
  );
}
