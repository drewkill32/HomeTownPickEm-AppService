"use server";

import { NewLeagueNameKeys, newLeagueSchema } from "../validation";
import { createClient } from "@/utils/supabase/server";
import slugify from "slugify";
import { revalidatePath } from "next/cache";

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
  const slug = slugify(result.data.name, {
    lower: true,
    remove: /[^a-zA-Z0-9 -]/g,
  });
  const year = new Date().getFullYear();

  console.log({
    p_name: result.data.name,
    p_slug: slug,
    p_password: result.data.password || null,
    p_description: result.data.description || null,
    p_is_public: result.data.public,
    p_owner_id: user!.id,
    p_image_url: null,
    p_year: year,
  });
  const { error } = await supabase.rpc("create_league", {
    p_name: result.data.name,
    p_slug: slug,
    p_password: result.data.password || null,
    p_description: result.data.description || null,
    p_is_public: result.data.public,
    p_owner_id: user!.id,
    p_image_url: null,
    p_year: year,
  });

  console.log(error);
  if (error) {
    if (error.message.includes("already exists")) {
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
    return {
      success: false,
      type: "server",
      error: {
        message: "An error occurred while creating the league.",
      },
    };
  }

  revalidatePath("/create-league");
  return {
    success: true,
    message: "League created",
  };
};
