import { type SVGProps } from "react";
import { useForm } from "react-hook-form";

type Props = {
  errorMsg?: string | null;
};

type Inputs = {
  email: string;
  password: string;
  confirmPassword: string;
};

const passwordLength = 4;

const defaultValues =
  import.meta.env.MODE === "development"
    ? {
        email: import.meta.env.PUBLIC_REGISTER_EMAIL,
        password: import.meta.env.PUBLIC_REGISTER_PASSWORD,
        confirmPassword: import.meta.env.PUBLIC_REGISTER_PASSWORD,
      }
    : ({} as Inputs);

export const RegisterForm = ({ errorMsg }: Props) => {
  const {
    register,
    formState: { errors, isDirty, isValid },
  } = useForm<Inputs>({
    mode: "all",
    criteriaMode: "all",
    defaultValues,
  });

  return (
    <div className="m-auto mx-auto my-4 max-w-sm px-3 text-card-foreground">
      <form
        className="rounded-lg border bg-card"
        action="/api/auth/register"
        method="post"
      >
        <div className="space-y-4 p-6">
          {errorMsg && (
            <div className="mb-2 rounded-sm border border-red-500 p-3">
              <h2 className="text-center text-sm font-bold text-red-500">
                ERROR
              </h2>
              <hr className="mx-auto my-2 h-px w-4/5 border-0 bg-red-500" />
              <p className="text-center text-sm font-semibold text-red-500">
                {errorMsg}
              </p>
            </div>
          )}
          <div className="space-y-2">
            <label
              className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
              htmlFor="email"
            >
              Email
            </label>
            <input
              className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"
              id="email"
              placeholder="m@example.com"
              required
              type="email"
              {...register("email", { required: true })}
            />
          </div>
          <div className="space-y-2">
            <label
              className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
              htmlFor="password"
            >
              Password
            </label>

            <input
              className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"
              id="password"
              required
              type="password"
              {...register("password", {
                required: true,
                validate: {
                  hasLetter: (value) => /[a-zA-Z]/.test(value),
                  hasNumberOrSpecial: (value) =>
                    /[0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]+/.test(value),
                  hasLength: (value) => value.length >= passwordLength,
                },
              })}
            />
            <p className="space-y-2 text-sm font-semibold">
              Your password must contain at least
            </p>
            <ul>
              <li className="flex items-center gap-1 text-sm">
                {!isDirty || errors?.password?.types?.hasLetter ? (
                  <CircleOutline />
                ) : (
                  <CheckmarkCirle className="text-green-500" />
                )}
                <p>1 letter </p>
              </li>
              <li className="flex items-center gap-1 text-sm">
                {!isDirty || errors?.password?.types?.hasLength ? (
                  <CircleOutline />
                ) : (
                  <CheckmarkCirle className="text-green-500" />
                )}
                <p>{passwordLength} characters</p>
              </li>
              <li className="flex items-center gap-1 text-sm">
                {!isDirty || errors.password?.types?.hasNumberOrSpecial ? (
                  <CircleOutline />
                ) : (
                  <CheckmarkCirle className="text-green-500" />
                )}
                <p>1 number or special character</p>
              </li>
            </ul>
          </div>
          <div className="space-y-2">
            <label
              className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
              htmlFor="confirm-password"
            >
              Confirm Password
            </label>

            <input
              className="flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"
              id="confirm-password"
              required
              type="password"
              {...register("confirmPassword", {
                required: true,
                validate: (value, formValues) => value === formValues.password,
              })}
            />
            <div className="flex items-center gap-1 text-sm">
              {!isDirty || errors?.confirmPassword ? (
                <CircleOutline />
              ) : (
                <CheckmarkCirle className="text-green-500" />
              )}
              <p>Matches Password</p>
            </div>
          </div>
        </div>
        <div className="flex flex-col gap-1 p-6">
          <button
            type="submit"
            disabled={!isValid}
            className="mx-auto inline-flex h-10 w-full items-center justify-center whitespace-nowrap rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground ring-offset-background transition-colors hover:bg-primary/90 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50"
          >
            Create an Account
          </button>
        </div>
        <div className="p-2 text-center">
          <p>
            Already have an account? <a href="/signin">Sign In</a>
          </p>
        </div>
      </form>
    </div>
  );
};

export function CheckmarkCirle(props: SVGProps<SVGSVGElement>) {
  return (
    <svg
      xmlns="http://www.w3.org/2000/svg"
      width="1em"
      height="1em"
      viewBox="0 0 24 24"
      {...props}
    >
      <path
        fill="currentColor"
        d="m10.6 16.6l7.05-7.05l-1.4-1.4l-5.65 5.65l-2.85-2.85l-1.4 1.4zM12 22q-2.075 0-3.9-.788t-3.175-2.137q-1.35-1.35-2.137-3.175T2 12q0-2.075.788-3.9t2.137-3.175q1.35-1.35 3.175-2.137T12 2q2.075 0 3.9.788t3.175 2.137q1.35 1.35 2.138 3.175T22 12q0 2.075-.788 3.9t-2.137 3.175q-1.35 1.35-3.175 2.138T12 22"
      ></path>
    </svg>
  );
}

export function CircleOutline(props: SVGProps<SVGSVGElement>) {
  return (
    <svg
      xmlns="http://www.w3.org/2000/svg"
      width="1em"
      height="1em"
      viewBox="0 0 24 24"
      {...props}
    >
      <path
        fill="currentColor"
        d="M12 22q-2.075 0-3.9-.788t-3.175-2.137q-1.35-1.35-2.137-3.175T2 12q0-2.075.788-3.9t2.137-3.175q1.35-1.35 3.175-2.137T12 2q2.075 0 3.9.788t3.175 2.137q1.35 1.35 2.138 3.175T22 12q0 2.075-.788 3.9t-2.137 3.175q-1.35 1.35-3.175 2.138T12 22m0-2q3.35 0 5.675-2.325T20 12q0-3.35-2.325-5.675T12 4Q8.65 4 6.325 6.325T4 12q0 3.35 2.325 5.675T12 20m0-8"
      ></path>
    </svg>
  );
}
