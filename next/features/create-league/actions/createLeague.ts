"use server";

import { ErrorOption, FieldError, appendErrors } from "react-hook-form";
import { NewLeagueNameKeys, newLeagueSchema } from "../validation";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";

export interface FormErrorOption extends ErrorOption {
  name?: NewLeagueNameKeys;
}

export type JoinLeagueReturnType =
  | { success: true; message?: string }
  | { success: false; error: FormErrorOption | FormErrorOption[] }
  | null;

export const createLeague = async (
  _: JoinLeagueReturnType,
  formData: FormData,
): Promise<JoinLeagueReturnType> => {
  const result = newLeagueSchema.safeParse(Object.fromEntries(formData));

  if (!result.success) {
    const errors = result.error.errors.map((error) => {
      return {
        type: "manual",
        name: error.path.join(".") as NewLeagueNameKeys,
        message: error.message,
      };
    });
    return {
      success: false,
      error: errors,
    };
  }
  const data = result.data;
  if (["stpete"].includes(data.leagueName.toLowerCase())) {
    return {
      success: false,
      error: {
        type: "server",
        name: "leagueName",
        message:
          "The league name already exists. Please pick a different name.",
      },
    };
  }
  return {
    success: false,
    error: {
      type: "root",
      message: "Not implemented yet.",
    },
  };
};
