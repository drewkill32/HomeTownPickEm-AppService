import { MergeDeep } from "type-fest";
import {
  Database as DatabaseGenerated,
  Tables as TablesGenerated,
} from "./database-generated.types";

// Override the type for a specific column in a view:
export type Database = MergeDeep<
  DatabaseGenerated,
  {
    public: {
      Functions: {
        create_league: {
          Args: {
            p_name: string;
            p_slug: string;
            p_password: string | null;
            p_description: string | null;
            p_is_public: boolean;
            p_owner_id: string;
            p_image_url: string | null;
            p_year: number;
          };
          Returns: string;
        };
      };
    };
  }
>;

type PublicSchema = Database[Extract<keyof Database, "public">];

export type Tables<
  PublicTableNameOrOptions extends
    | keyof (PublicSchema["Tables"] & PublicSchema["Views"])
    | { schema: keyof Database },
  TableName extends PublicTableNameOrOptions extends { schema: keyof Database }
    ? keyof (Database[PublicTableNameOrOptions["schema"]]["Tables"] &
        Database[PublicTableNameOrOptions["schema"]]["Views"])
    : never = never,
> = TablesGenerated<PublicTableNameOrOptions, TableName>;
