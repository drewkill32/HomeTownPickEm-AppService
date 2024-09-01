export type Json =
  | string
  | number
  | boolean
  | null
  | { [key: string]: Json | undefined }
  | Json[]

export type Database = {
  public: {
    Tables: {
      __EFMigrationsHistory: {
        Row: {
          MigrationId: string
          ProductVersion: string
        }
        Insert: {
          MigrationId: string
          ProductVersion: string
        }
        Update: {
          MigrationId?: string
          ProductVersion?: string
        }
        Relationships: []
      }
      ApplicationUserSeason: {
        Row: {
          MembersId: string
          SeasonsId: number
        }
        Insert: {
          MembersId: string
          SeasonsId: number
        }
        Update: {
          MembersId?: string
          SeasonsId?: number
        }
        Relationships: [
          {
            foreignKeyName: "FK_ApplicationUserSeason_AspNetUsers_MembersId"
            columns: ["MembersId"]
            isOneToOne: false
            referencedRelation: "AspNetUsers"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_ApplicationUserSeason_AspNetUsers_MembersId"
            columns: ["MembersId"]
            isOneToOne: false
            referencedRelation: "Leaderboard"
            referencedColumns: ["UserId"]
          },
          {
            foreignKeyName: "FK_ApplicationUserSeason_AspNetUsers_MembersId"
            columns: ["MembersId"]
            isOneToOne: false
            referencedRelation: "WeeklyLeaderboard"
            referencedColumns: ["UserId"]
          },
          {
            foreignKeyName: "FK_ApplicationUserSeason_Season_SeasonsId"
            columns: ["SeasonsId"]
            isOneToOne: false
            referencedRelation: "Season"
            referencedColumns: ["Id"]
          },
        ]
      }
      AspNetRoleClaims: {
        Row: {
          ClaimType: string | null
          ClaimValue: string | null
          Id: number
          RoleId: string
        }
        Insert: {
          ClaimType?: string | null
          ClaimValue?: string | null
          Id?: number
          RoleId: string
        }
        Update: {
          ClaimType?: string | null
          ClaimValue?: string | null
          Id?: number
          RoleId?: string
        }
        Relationships: [
          {
            foreignKeyName: "FK_AspNetRoleClaims_AspNetRoles_RoleId"
            columns: ["RoleId"]
            isOneToOne: false
            referencedRelation: "AspNetRoles"
            referencedColumns: ["Id"]
          },
        ]
      }
      AspNetRoles: {
        Row: {
          ConcurrencyStamp: string | null
          Id: string
          Name: string | null
          NormalizedName: string | null
        }
        Insert: {
          ConcurrencyStamp?: string | null
          Id: string
          Name?: string | null
          NormalizedName?: string | null
        }
        Update: {
          ConcurrencyStamp?: string | null
          Id?: string
          Name?: string | null
          NormalizedName?: string | null
        }
        Relationships: []
      }
      AspNetUserClaims: {
        Row: {
          ClaimType: string | null
          ClaimValue: string | null
          Id: number
          UserId: string
        }
        Insert: {
          ClaimType?: string | null
          ClaimValue?: string | null
          Id?: number
          UserId: string
        }
        Update: {
          ClaimType?: string | null
          ClaimValue?: string | null
          Id?: number
          UserId?: string
        }
        Relationships: [
          {
            foreignKeyName: "FK_AspNetUserClaims_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "AspNetUsers"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_AspNetUserClaims_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "Leaderboard"
            referencedColumns: ["UserId"]
          },
          {
            foreignKeyName: "FK_AspNetUserClaims_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "WeeklyLeaderboard"
            referencedColumns: ["UserId"]
          },
        ]
      }
      AspNetUserLogins: {
        Row: {
          LoginProvider: string
          ProviderDisplayName: string | null
          ProviderKey: string
          UserId: string
        }
        Insert: {
          LoginProvider: string
          ProviderDisplayName?: string | null
          ProviderKey: string
          UserId: string
        }
        Update: {
          LoginProvider?: string
          ProviderDisplayName?: string | null
          ProviderKey?: string
          UserId?: string
        }
        Relationships: [
          {
            foreignKeyName: "FK_AspNetUserLogins_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "AspNetUsers"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_AspNetUserLogins_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "Leaderboard"
            referencedColumns: ["UserId"]
          },
          {
            foreignKeyName: "FK_AspNetUserLogins_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "WeeklyLeaderboard"
            referencedColumns: ["UserId"]
          },
        ]
      }
      AspNetUserRoles: {
        Row: {
          RoleId: string
          UserId: string
        }
        Insert: {
          RoleId: string
          UserId: string
        }
        Update: {
          RoleId?: string
          UserId?: string
        }
        Relationships: [
          {
            foreignKeyName: "FK_AspNetUserRoles_AspNetRoles_RoleId"
            columns: ["RoleId"]
            isOneToOne: false
            referencedRelation: "AspNetRoles"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_AspNetUserRoles_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "AspNetUsers"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_AspNetUserRoles_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "Leaderboard"
            referencedColumns: ["UserId"]
          },
          {
            foreignKeyName: "FK_AspNetUserRoles_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "WeeklyLeaderboard"
            referencedColumns: ["UserId"]
          },
        ]
      }
      AspNetUsers: {
        Row: {
          AccessFailedCount: number
          ConcurrencyStamp: string | null
          Email: string | null
          EmailConfirmed: boolean
          FirstName: string | null
          Id: string
          IsMigrated: boolean
          LastName: string | null
          LockoutEnabled: boolean
          LockoutEnd: string | null
          NormalizedEmail: string | null
          NormalizedUserName: string | null
          PasswordHash: string | null
          PhoneNumber: string | null
          PhoneNumberConfirmed: boolean
          ProfileImg: string | null
          SecurityStamp: string | null
          TeamId: number | null
          TwoFactorEnabled: boolean
          UserName: string | null
        }
        Insert: {
          AccessFailedCount: number
          ConcurrencyStamp?: string | null
          Email?: string | null
          EmailConfirmed: boolean
          FirstName?: string | null
          Id: string
          IsMigrated?: boolean
          LastName?: string | null
          LockoutEnabled: boolean
          LockoutEnd?: string | null
          NormalizedEmail?: string | null
          NormalizedUserName?: string | null
          PasswordHash?: string | null
          PhoneNumber?: string | null
          PhoneNumberConfirmed: boolean
          ProfileImg?: string | null
          SecurityStamp?: string | null
          TeamId?: number | null
          TwoFactorEnabled: boolean
          UserName?: string | null
        }
        Update: {
          AccessFailedCount?: number
          ConcurrencyStamp?: string | null
          Email?: string | null
          EmailConfirmed?: boolean
          FirstName?: string | null
          Id?: string
          IsMigrated?: boolean
          LastName?: string | null
          LockoutEnabled?: boolean
          LockoutEnd?: string | null
          NormalizedEmail?: string | null
          NormalizedUserName?: string | null
          PasswordHash?: string | null
          PhoneNumber?: string | null
          PhoneNumberConfirmed?: boolean
          ProfileImg?: string | null
          SecurityStamp?: string | null
          TeamId?: number | null
          TwoFactorEnabled?: boolean
          UserName?: string | null
        }
        Relationships: [
          {
            foreignKeyName: "FK_AspNetUsers_Teams_TeamId"
            columns: ["TeamId"]
            isOneToOne: false
            referencedRelation: "Leaderboard"
            referencedColumns: ["TeamId"]
          },
          {
            foreignKeyName: "FK_AspNetUsers_Teams_TeamId"
            columns: ["TeamId"]
            isOneToOne: false
            referencedRelation: "Teams"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_AspNetUsers_Teams_TeamId"
            columns: ["TeamId"]
            isOneToOne: false
            referencedRelation: "WeeklyLeaderboard"
            referencedColumns: ["TeamId"]
          },
        ]
      }
      AspNetUserTokens: {
        Row: {
          LoginProvider: string
          Name: string
          UserId: string
          Value: string | null
        }
        Insert: {
          LoginProvider: string
          Name: string
          UserId: string
          Value?: string | null
        }
        Update: {
          LoginProvider?: string
          Name?: string
          UserId?: string
          Value?: string | null
        }
        Relationships: [
          {
            foreignKeyName: "FK_AspNetUserTokens_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "AspNetUsers"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_AspNetUserTokens_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "Leaderboard"
            referencedColumns: ["UserId"]
          },
          {
            foreignKeyName: "FK_AspNetUserTokens_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "WeeklyLeaderboard"
            referencedColumns: ["UserId"]
          },
        ]
      }
      Calendar: {
        Row: {
          FirstGameStart: string
          LastGameStart: string
          Season: string
          SeasonType: string
          Week: number
        }
        Insert: {
          FirstGameStart: string
          LastGameStart: string
          Season: string
          SeasonType: string
          Week: number
        }
        Update: {
          FirstGameStart?: string
          LastGameStart?: string
          Season?: string
          SeasonType?: string
          Week?: number
        }
        Relationships: []
      }
      Games: {
        Row: {
          AwayId: number
          AwayPoints: number | null
          HomeId: number
          HomePoints: number | null
          Id: number
          Season: string | null
          SeasonType: string | null
          StartDate: string
          StartTimeTbd: boolean
          Week: number
        }
        Insert: {
          AwayId: number
          AwayPoints?: number | null
          HomeId: number
          HomePoints?: number | null
          Id: number
          Season?: string | null
          SeasonType?: string | null
          StartDate: string
          StartTimeTbd: boolean
          Week: number
        }
        Update: {
          AwayId?: number
          AwayPoints?: number | null
          HomeId?: number
          HomePoints?: number | null
          Id?: number
          Season?: string | null
          SeasonType?: string | null
          StartDate?: string
          StartTimeTbd?: boolean
          Week?: number
        }
        Relationships: [
          {
            foreignKeyName: "FK_Games_Teams_AwayId"
            columns: ["AwayId"]
            isOneToOne: false
            referencedRelation: "Leaderboard"
            referencedColumns: ["TeamId"]
          },
          {
            foreignKeyName: "FK_Games_Teams_AwayId"
            columns: ["AwayId"]
            isOneToOne: false
            referencedRelation: "Teams"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_Games_Teams_AwayId"
            columns: ["AwayId"]
            isOneToOne: false
            referencedRelation: "WeeklyLeaderboard"
            referencedColumns: ["TeamId"]
          },
          {
            foreignKeyName: "FK_Games_Teams_HomeId"
            columns: ["HomeId"]
            isOneToOne: false
            referencedRelation: "Leaderboard"
            referencedColumns: ["TeamId"]
          },
          {
            foreignKeyName: "FK_Games_Teams_HomeId"
            columns: ["HomeId"]
            isOneToOne: false
            referencedRelation: "Teams"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_Games_Teams_HomeId"
            columns: ["HomeId"]
            isOneToOne: false
            referencedRelation: "WeeklyLeaderboard"
            referencedColumns: ["TeamId"]
          },
        ]
      }
      league: {
        Row: {
          description: string | null
          id: string
          image_url: string | null
          is_public: boolean
          name: string
          owner_id: string | null
          password: string | null
          slug: string
        }
        Insert: {
          description?: string | null
          id?: string
          image_url?: string | null
          is_public?: boolean
          name: string
          owner_id?: string | null
          password?: string | null
          slug: string
        }
        Update: {
          description?: string | null
          id?: string
          image_url?: string | null
          is_public?: boolean
          name?: string
          owner_id?: string | null
          password?: string | null
          slug?: string
        }
        Relationships: [
          {
            foreignKeyName: "league_owner_id_fkey"
            columns: ["owner_id"]
            isOneToOne: false
            referencedRelation: "users"
            referencedColumns: ["id"]
          },
        ]
      }
      League: {
        Row: {
          Id: number
          ImageUrl: string | null
          Name: string | null
          password: string | null
          Slug: string | null
        }
        Insert: {
          Id?: number
          ImageUrl?: string | null
          Name?: string | null
          password?: string | null
          Slug?: string | null
        }
        Update: {
          Id?: number
          ImageUrl?: string | null
          Name?: string | null
          password?: string | null
          Slug?: string | null
        }
        Relationships: []
      }
      PendingInvites: {
        Row: {
          LeagueId: number
          Season: string
          TeamId: number | null
          UserId: string
        }
        Insert: {
          LeagueId: number
          Season: string
          TeamId?: number | null
          UserId: string
        }
        Update: {
          LeagueId?: number
          Season?: string
          TeamId?: number | null
          UserId?: string
        }
        Relationships: []
      }
      Pick: {
        Row: {
          GameId: number
          Id: number
          Points: number
          SeasonId: number
          SelectedTeamId: number | null
          UserId: string | null
        }
        Insert: {
          GameId: number
          Id?: number
          Points: number
          SeasonId: number
          SelectedTeamId?: number | null
          UserId?: string | null
        }
        Update: {
          GameId?: number
          Id?: number
          Points?: number
          SeasonId?: number
          SelectedTeamId?: number | null
          UserId?: string | null
        }
        Relationships: [
          {
            foreignKeyName: "FK_Pick_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "AspNetUsers"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_Pick_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "Leaderboard"
            referencedColumns: ["UserId"]
          },
          {
            foreignKeyName: "FK_Pick_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "WeeklyLeaderboard"
            referencedColumns: ["UserId"]
          },
          {
            foreignKeyName: "FK_Pick_Games_GameId"
            columns: ["GameId"]
            isOneToOne: false
            referencedRelation: "Games"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_Pick_Season_SeasonId"
            columns: ["SeasonId"]
            isOneToOne: false
            referencedRelation: "Season"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_Pick_Teams_SelectedTeamId"
            columns: ["SelectedTeamId"]
            isOneToOne: false
            referencedRelation: "Leaderboard"
            referencedColumns: ["TeamId"]
          },
          {
            foreignKeyName: "FK_Pick_Teams_SelectedTeamId"
            columns: ["SelectedTeamId"]
            isOneToOne: false
            referencedRelation: "Teams"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_Pick_Teams_SelectedTeamId"
            columns: ["SelectedTeamId"]
            isOneToOne: false
            referencedRelation: "WeeklyLeaderboard"
            referencedColumns: ["TeamId"]
          },
        ]
      }
      RefreshTokens: {
        Row: {
          AddedDate: string
          ExpiryDate: string
          Id: number
          IpAddress: string | null
          JwtId: string | null
          Token: string | null
          UserAgent: string | null
          UserId: string
        }
        Insert: {
          AddedDate: string
          ExpiryDate: string
          Id?: number
          IpAddress?: string | null
          JwtId?: string | null
          Token?: string | null
          UserAgent?: string | null
          UserId: string
        }
        Update: {
          AddedDate?: string
          ExpiryDate?: string
          Id?: number
          IpAddress?: string | null
          JwtId?: string | null
          Token?: string | null
          UserAgent?: string | null
          UserId?: string
        }
        Relationships: [
          {
            foreignKeyName: "FK_RefreshTokens_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "AspNetUsers"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_RefreshTokens_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "Leaderboard"
            referencedColumns: ["UserId"]
          },
          {
            foreignKeyName: "FK_RefreshTokens_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "WeeklyLeaderboard"
            referencedColumns: ["UserId"]
          },
        ]
      }
      Season: {
        Row: {
          Active: boolean
          Id: number
          LeagueId: number
          Year: string | null
        }
        Insert: {
          Active: boolean
          Id?: number
          LeagueId: number
          Year?: string | null
        }
        Update: {
          Active?: boolean
          Id?: number
          LeagueId?: number
          Year?: string | null
        }
        Relationships: [
          {
            foreignKeyName: "FK_Season_League_LeagueId"
            columns: ["LeagueId"]
            isOneToOne: false
            referencedRelation: "Leaderboard"
            referencedColumns: ["LeagueId"]
          },
          {
            foreignKeyName: "FK_Season_League_LeagueId"
            columns: ["LeagueId"]
            isOneToOne: false
            referencedRelation: "League"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_Season_League_LeagueId"
            columns: ["LeagueId"]
            isOneToOne: false
            referencedRelation: "WeeklyLeaderboard"
            referencedColumns: ["LeagueId"]
          },
        ]
      }
      SeasonCaches: {
        Row: {
          LastRefresh: string
          Season: string
          Type: string
          Week: number
        }
        Insert: {
          LastRefresh: string
          Season: string
          Type: string
          Week: number
        }
        Update: {
          LastRefresh?: string
          Season?: string
          Type?: string
          Week?: number
        }
        Relationships: []
      }
      SeasonTeam: {
        Row: {
          SeasonsId: number
          TeamsId: number
        }
        Insert: {
          SeasonsId: number
          TeamsId: number
        }
        Update: {
          SeasonsId?: number
          TeamsId?: number
        }
        Relationships: [
          {
            foreignKeyName: "FK_SeasonTeam_Season_SeasonsId"
            columns: ["SeasonsId"]
            isOneToOne: false
            referencedRelation: "Season"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_SeasonTeam_Teams_TeamsId"
            columns: ["TeamsId"]
            isOneToOne: false
            referencedRelation: "Leaderboard"
            referencedColumns: ["TeamId"]
          },
          {
            foreignKeyName: "FK_SeasonTeam_Teams_TeamsId"
            columns: ["TeamsId"]
            isOneToOne: false
            referencedRelation: "Teams"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_SeasonTeam_Teams_TeamsId"
            columns: ["TeamsId"]
            isOneToOne: false
            referencedRelation: "WeeklyLeaderboard"
            referencedColumns: ["TeamId"]
          },
        ]
      }
      Teams: {
        Row: {
          Abbreviation: string | null
          AltColor: string | null
          Color: string | null
          Conference: string | null
          Division: string | null
          Id: number
          Logos: string | null
          Mascot: string | null
          School: string | null
        }
        Insert: {
          Abbreviation?: string | null
          AltColor?: string | null
          Color?: string | null
          Conference?: string | null
          Division?: string | null
          Id: number
          Logos?: string | null
          Mascot?: string | null
          School?: string | null
        }
        Update: {
          Abbreviation?: string | null
          AltColor?: string | null
          Color?: string | null
          Conference?: string | null
          Division?: string | null
          Id?: number
          Logos?: string | null
          Mascot?: string | null
          School?: string | null
        }
        Relationships: []
      }
      WeeklyGamePicks: {
        Row: {
          GameId: number
          Id: string
          TotalPoints: number
          UserId: string | null
          WeeklyGameId: string
        }
        Insert: {
          GameId: number
          Id?: string
          TotalPoints: number
          UserId?: string | null
          WeeklyGameId: string
        }
        Update: {
          GameId?: number
          Id?: string
          TotalPoints?: number
          UserId?: string | null
          WeeklyGameId?: string
        }
        Relationships: [
          {
            foreignKeyName: "FK_WeeklyGamePicks_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "AspNetUsers"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_WeeklyGamePicks_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "Leaderboard"
            referencedColumns: ["UserId"]
          },
          {
            foreignKeyName: "FK_WeeklyGamePicks_AspNetUsers_UserId"
            columns: ["UserId"]
            isOneToOne: false
            referencedRelation: "WeeklyLeaderboard"
            referencedColumns: ["UserId"]
          },
          {
            foreignKeyName: "FK_WeeklyGamePicks_WeeklyGames_WeeklyGameId"
            columns: ["WeeklyGameId"]
            isOneToOne: false
            referencedRelation: "WeeklyGames"
            referencedColumns: ["Id"]
          },
        ]
      }
      WeeklyGames: {
        Row: {
          GameId: number
          Id: string
          SeasonId: number
          Week: number
        }
        Insert: {
          GameId: number
          Id?: string
          SeasonId: number
          Week: number
        }
        Update: {
          GameId?: number
          Id?: string
          SeasonId?: number
          Week?: number
        }
        Relationships: [
          {
            foreignKeyName: "FK_WeeklyGames_Games_GameId"
            columns: ["GameId"]
            isOneToOne: false
            referencedRelation: "Games"
            referencedColumns: ["Id"]
          },
          {
            foreignKeyName: "FK_WeeklyGames_Season_SeasonId"
            columns: ["SeasonId"]
            isOneToOne: false
            referencedRelation: "Season"
            referencedColumns: ["Id"]
          },
        ]
      }
    }
    Views: {
      Leaderboard: {
        Row: {
          LeagueId: number | null
          LeagueName: string | null
          LeagueSlug: string | null
          TeamId: number | null
          TeamLogos: string | null
          TeamMascot: string | null
          TeamSchool: string | null
          TotalPoints: number | null
          UserFirstName: string | null
          UserId: string | null
          UserLastName: string | null
          Year: string | null
        }
        Relationships: []
      }
      WeeklyLeaderboard: {
        Row: {
          LeagueId: number | null
          LeagueName: string | null
          LeagueSlug: string | null
          TeamId: number | null
          TeamLogos: string | null
          TeamMascot: string | null
          TeamSchool: string | null
          TotalPoints: number | null
          UserFirstName: string | null
          UserId: string | null
          UserLastName: string | null
          Week: number | null
          Year: string | null
        }
        Relationships: []
      }
    }
    Functions: {
      [_ in never]: never
    }
    Enums: {
      [_ in never]: never
    }
    CompositeTypes: {
      [_ in never]: never
    }
  }
}

type PublicSchema = Database[Extract<keyof Database, "public">]

export type Tables<
  PublicTableNameOrOptions extends
    | keyof (PublicSchema["Tables"] & PublicSchema["Views"])
    | { schema: keyof Database },
  TableName extends PublicTableNameOrOptions extends { schema: keyof Database }
    ? keyof (Database[PublicTableNameOrOptions["schema"]]["Tables"] &
        Database[PublicTableNameOrOptions["schema"]]["Views"])
    : never = never,
> = PublicTableNameOrOptions extends { schema: keyof Database }
  ? (Database[PublicTableNameOrOptions["schema"]]["Tables"] &
      Database[PublicTableNameOrOptions["schema"]]["Views"])[TableName] extends {
      Row: infer R
    }
    ? R
    : never
  : PublicTableNameOrOptions extends keyof (PublicSchema["Tables"] &
        PublicSchema["Views"])
    ? (PublicSchema["Tables"] &
        PublicSchema["Views"])[PublicTableNameOrOptions] extends {
        Row: infer R
      }
      ? R
      : never
    : never

export type TablesInsert<
  PublicTableNameOrOptions extends
    | keyof PublicSchema["Tables"]
    | { schema: keyof Database },
  TableName extends PublicTableNameOrOptions extends { schema: keyof Database }
    ? keyof Database[PublicTableNameOrOptions["schema"]]["Tables"]
    : never = never,
> = PublicTableNameOrOptions extends { schema: keyof Database }
  ? Database[PublicTableNameOrOptions["schema"]]["Tables"][TableName] extends {
      Insert: infer I
    }
    ? I
    : never
  : PublicTableNameOrOptions extends keyof PublicSchema["Tables"]
    ? PublicSchema["Tables"][PublicTableNameOrOptions] extends {
        Insert: infer I
      }
      ? I
      : never
    : never

export type TablesUpdate<
  PublicTableNameOrOptions extends
    | keyof PublicSchema["Tables"]
    | { schema: keyof Database },
  TableName extends PublicTableNameOrOptions extends { schema: keyof Database }
    ? keyof Database[PublicTableNameOrOptions["schema"]]["Tables"]
    : never = never,
> = PublicTableNameOrOptions extends { schema: keyof Database }
  ? Database[PublicTableNameOrOptions["schema"]]["Tables"][TableName] extends {
      Update: infer U
    }
    ? U
    : never
  : PublicTableNameOrOptions extends keyof PublicSchema["Tables"]
    ? PublicSchema["Tables"][PublicTableNameOrOptions] extends {
        Update: infer U
      }
      ? U
      : never
    : never

export type Enums<
  PublicEnumNameOrOptions extends
    | keyof PublicSchema["Enums"]
    | { schema: keyof Database },
  EnumName extends PublicEnumNameOrOptions extends { schema: keyof Database }
    ? keyof Database[PublicEnumNameOrOptions["schema"]]["Enums"]
    : never = never,
> = PublicEnumNameOrOptions extends { schema: keyof Database }
  ? Database[PublicEnumNameOrOptions["schema"]]["Enums"][EnumName]
  : PublicEnumNameOrOptions extends keyof PublicSchema["Enums"]
    ? PublicSchema["Enums"][PublicEnumNameOrOptions]
    : never
