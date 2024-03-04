import SimpleLayout from "@/components/SimpleLayout";
import { SignInForm } from "./LoginForm";

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
      <SignInForm
        message={searchParams.message}
        redirectUrl={searchParams.redirectUrl}
        newUser={searchParams.newUser}
      />
    </SimpleLayout>
  );
}
