"use client";

import { useState } from "react";
import { cn } from "@/utils/tailwind";
import { useEscape } from "@/hooks/useEscape";
import { NavItem } from "./navItems";

export interface NavMenuProps {
  children: React.ReactNode;
  className?: string;
  menuItems: NavItem[];
}

export default function NavMenu({
  children,
  menuItems,
  className,
  ...props
}: NavMenuProps) {
  const [open, setOpen] = useState(false);
  useEscape(() => setOpen(false));

  return (
    <>
      <div
        className={cn("relative z-20", className)}
        onMouseEnter={() => setOpen(true)}
        onMouseLeave={() => setOpen(false)}
        {...props}
      >
        <span
          aria-haspopup="true"
          aria-expanded={open}
          onFocus={() => setOpen(true)}
          className=" relative flex gap-1"
        >
          {children}
        </span>

        {open && (
          <>
            <div className="absolute h-4 w-full" />
            <div
              role="menu"
              onMouseLeave={() => setOpen(false)}
              onBlur={(e) => {
                if (!e.currentTarget.contains(e.relatedTarget)) {
                  setOpen(false);
                }
              }}
              className={`absolute right-0 mt-2 flex w-48 flex-col rounded-lg border border-gray-300 bg-white py-2 text-primary shadow-md `}
            >
              {menuItems.map((item) => (
                <a
                  key={item.label}
                  role="menuitem"
                  href={item.href}
                  className="px-4 py-3 hover:bg-primary hover:text-white focus:bg-primary focus:text-white"
                >
                  {item.label}
                </a>
              ))}
            </div>
          </>
        )}
      </div>
      {open && (
        <button
          tabIndex={-1}
          onClick={() => setOpen(false)}
          className="fixed inset-0 h-full w-full cursor-default"
        ></button>
      )}
    </>
  );
}
