"use client";

import SimpleLayout from "@/components/SimpleLayout";
import { Loader2 } from "lucide-react";
import { createClient } from "@/utils/supabase/client";
import { useRouter } from "next/navigation";
import { useEffect } from "react";

export default function Signout() {
  const router = useRouter();
  const supabase = createClient();
  useEffect(() => {
    (async () => {
      await supabase.auth.signOut();
    })();
  }, []);

  useEffect(() => {
    const {
      data: { subscription },
    } = supabase.auth.onAuthStateChange((event, session) => {
      router.refresh();
      if (event === "SIGNED_OUT") {
        router.push("/");
      }
    });
    return () => {
      subscription?.unsubscribe();
    };
  }, [supabase, router]);

  return (
    <SimpleLayout heading="Signing out">
      <div className="flex flex-col items-center justify-center gap-3">
        <Loader2 className="mr-2 h-12 w-12 animate-spin text-primary" />
        <p className="text-lg">Please wait...</p>
      </div>
    </SimpleLayout>
  );
}
