import React, { ComponentProps } from "react";
import Logo from "../icons/Logo";
import { buttonVariants } from "../ui/button";
import { MobileNavMenuButton } from "./MobileNavMenuButton";
import NavMenu from "./NavMenu";
import { getUser } from "@/server/user";
import Account from "../icons/Account";
import { cn } from "@/utils/tailwind";
import { getProtectedRoutes, navItems } from "./navItems";
import Link from "next/link";

interface HeaderProps extends ComponentProps<"header"> {}

export default async function Header({ className, ...rest }: HeaderProps) {
  const { isAuthenticated, user } = await getUser();
  const protectedRoutes = getProtectedRoutes(user);
  return (
    <header
      className={cn("bg-white px-4 sm:px-6 lg:px-8", className)}
      {...rest}
    >
      <div className="flex h-16 items-center justify-evenly">
        <div className="flex grow items-center gap-5 pl-2">
          <Logo className="text-4xl text-emerald-700" width={55} height={65} />
          <h1 className="font-bold text-[#002244] sm:text-2xl">
            St. Pete Pick'em
          </h1>
        </div>
        <MobileNavMenuButton
          navigationItems={[...navItems, ...protectedRoutes]}
          className="mr-2 md:hidden order-2 ml-2"
        />

        <nav className="hidden justify-self-end md:block md:shrink md:pr-3">
          <ul className="hidden items-center space-x-4 md:flex">
            {navItems.map((item) => (
              <li key={item.label}>
                <a className="transition hover:text-[#d50a0a]" href={item.href}>
                  {item.label}
                </a>
              </li>
            ))}
          </ul>
        </nav>
        {isAuthenticated ? (
          <NavMenu menuItems={protectedRoutes} className="hidden md:block">
            <Account className="text-2xl" />
          </NavMenu>
        ) : (
          <Link
            className={cn(buttonVariants(), "justify-self-start")}
            href="/login"
          >
            Log In
          </Link>
        )}
      </div>
    </header>
  );
}
