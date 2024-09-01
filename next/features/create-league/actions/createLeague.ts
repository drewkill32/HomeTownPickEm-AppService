"use server";

import { NewLeagueNameKeys, newLeagueSchema } from "../validation";
import { createClient } from "@/utils/supabase/server";
import slugify from "slugify";
import { Tables } from "@/database.types";

export type JoinLeagueReturnType =
  | { success: true; message?: string }
  | {
      success: false;
      type: "validation";
      error: { message: string; name: NewLeagueNameKeys }[];
    }
  | { success: false; type: "server"; error: { message: string } }
  | null;

function createPostgresError(error: unknown): JoinLeagueReturnType {
  console.error("An error occurred while creating the league", error);
  return {
    success: false,
    type: "server",
    error: {
      message: "An error occurred while creating the league.",
    },
  };
}

export const createLeague = async (
  _: JoinLeagueReturnType,
  formData: FormData,
): Promise<JoinLeagueReturnType> => {
  const result = newLeagueSchema.safeParse(Object.fromEntries(formData));

  if (!result.success) {
    return {
      success: false,
      type: "validation",
      error: result.error.errors.map((error) => ({
        type: "validation",
        name: error.path.join(".") as NewLeagueNameKeys,
        message: error.message,
      })),
    } as JoinLeagueReturnType;
  }
  const supabase = createClient();

  const {
    data: { user },
    error: userError,
  } = await supabase.auth.getUser();

  if (userError) {
    return createPostgresError(userError);
  }

  const newLeague: Omit<Tables<"league">, "id"> = {
    name: result.data.name,
    slug: slugify(result.data.name, { lower: true, remove: /[^a-zA-Z0-9 -]/g }),
    password: result.data.password || null,
    description: result.data.description || null,
    is_public: result.data.public,
    owner_id: user!.id,
    image_url: null,
  };

  const { count, error: lookupError } = await supabase
    .from("league")
    .select("*", { count: "exact", head: true })
    .eq("slug", newLeague.slug);

  if (lookupError) {
    return createPostgresError(lookupError);
  }

  if (count ?? 0 > 0) {
    return {
      success: false,
      type: "validation",
      error: [
        {
          name: "name",
          message:
            "The league name already exists. Please pick a different name.",
        },
      ],
    };
  }

  const { error: insertError } = await supabase
    .from("league")
    .insert(newLeague);

  if (insertError) {
    return createPostgresError(insertError);
  }

  return {
    success: true,
    message: "League created successfully.",
  };
};
