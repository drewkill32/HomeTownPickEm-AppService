"use client";

import { createClient } from "@/utils/supabase/client";
import { User } from "@supabase/supabase-js";
import { createContext, useContext, useEffect, useState } from "react";

export type SupabaseContextType = {
  user: User | null;
  supabase: ReturnType<typeof createClient>;
};

const SupabaseContext = createContext<SupabaseContextType>(
  {} as SupabaseContextType,
);

type SupabaseContextProviderProps = {
  children: React.ReactNode;
  user: User | null;
};

export default function SupabaseContextProvider({
  user: serverUser,
  children,
}: SupabaseContextProviderProps) {
  const [user, setUser] = useState(serverUser);
  const supabase = createClient();

  useEffect(() => {
    (async () => {
      const {
        data: { user },
      } = await supabase.auth.getUser();
      setUser(user);
    })();
  }, [supabase]);

  useEffect(() => {
    const {
      data: { subscription },
    } = supabase.auth.onAuthStateChange(async (event, session) => {
      setUser(session?.user ?? null);
    });

    return () => {
      subscription.unsubscribe();
    };
  }, [supabase]);

  return (
    <SupabaseContext.Provider value={{ user, supabase }}>
      {children}
    </SupabaseContext.Provider>
  );
}

export const useSupabase = () => {
  return useContext(SupabaseContext);
};
