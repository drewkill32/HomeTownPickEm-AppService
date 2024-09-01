import SimpleLayout from "@/components/SimpleLayout";
import { LoginForm } from "./LoginForm";
import { Metadata } from "next";
import { metadataTitle } from "@/utils";

export const metadata: Metadata = metadataTitle("Login");

export default function Login({
  searchParams,
}: {
  searchParams: {
    message: string | undefined;
    redirectUrl: string | undefined;
    newUser: string | undefined;
  };
}) {
  return (
    <SimpleLayout heading="Sign in to St. Pete Pick'em">
      <LoginForm
        message={searchParams.message}
        redirectUrl={searchParams.redirectUrl}
        newUser={searchParams.newUser}
      />
    </SimpleLayout>
  );
}
