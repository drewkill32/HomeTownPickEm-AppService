import SimpleLayout from "@/components/SimpleLayout";
import { SignUpForm } from "./SignUpForm";

export default function Login({
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
