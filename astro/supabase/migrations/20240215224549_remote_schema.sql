
SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

CREATE SCHEMA IF NOT EXISTS "public";

ALTER SCHEMA "public" OWNER TO "pg_database_owner";

SET default_tablespace = '';

SET default_table_access_method = "heap";

CREATE TABLE IF NOT EXISTS "public"."ApplicationUserSeason" (
    "MembersId" text NOT NULL,
    "SeasonsId" integer NOT NULL
);

ALTER TABLE "public"."ApplicationUserSeason" OWNER TO "postgres";

CREATE TABLE IF NOT EXISTS "public"."AspNetRoleClaims" (
    "Id" integer NOT NULL,
    "RoleId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text
);

ALTER TABLE "public"."AspNetRoleClaims" OWNER TO "postgres";

ALTER TABLE "public"."AspNetRoleClaims" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME "public"."AspNetRoleClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE IF NOT EXISTS "public"."AspNetRoles" (
    "Id" text NOT NULL,
    "Name" character varying(256),
    "NormalizedName" character varying(256),
    "ConcurrencyStamp" text
);

ALTER TABLE "public"."AspNetRoles" OWNER TO "postgres";

CREATE TABLE IF NOT EXISTS "public"."AspNetUserClaims" (
    "Id" integer NOT NULL,
    "UserId" text NOT NULL,
    "ClaimType" text,
    "ClaimValue" text
);

ALTER TABLE "public"."AspNetUserClaims" OWNER TO "postgres";

ALTER TABLE "public"."AspNetUserClaims" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME "public"."AspNetUserClaims_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE IF NOT EXISTS "public"."AspNetUserLogins" (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text,
    "UserId" text NOT NULL
);

ALTER TABLE "public"."AspNetUserLogins" OWNER TO "postgres";

CREATE TABLE IF NOT EXISTS "public"."AspNetUserRoles" (
    "UserId" text NOT NULL,
    "RoleId" text NOT NULL
);

ALTER TABLE "public"."AspNetUserRoles" OWNER TO "postgres";

CREATE TABLE IF NOT EXISTS "public"."AspNetUserTokens" (
    "UserId" text NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text
);

ALTER TABLE "public"."AspNetUserTokens" OWNER TO "postgres";

CREATE TABLE IF NOT EXISTS "public"."AspNetUsers" (
    "Id" text NOT NULL,
    "FirstName" text,
    "LastName" text,
    "TeamId" integer,
    "ProfileImg" text,
    "UserName" character varying(256),
    "NormalizedUserName" character varying(256),
    "Email" character varying(256),
    "NormalizedEmail" character varying(256),
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text,
    "SecurityStamp" text,
    "ConcurrencyStamp" text,
    "PhoneNumber" text,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    "IsMigrated" boolean DEFAULT false NOT NULL
);

ALTER TABLE "public"."AspNetUsers" OWNER TO "postgres";

CREATE TABLE IF NOT EXISTS "public"."Calendar" (
    "Season" text NOT NULL,
    "Week" integer NOT NULL,
    "SeasonType" text NOT NULL,
    "FirstGameStart" timestamp with time zone NOT NULL,
    "LastGameStart" timestamp with time zone NOT NULL
);

ALTER TABLE "public"."Calendar" OWNER TO "postgres";

CREATE TABLE IF NOT EXISTS "public"."Games" (
    "Id" integer NOT NULL,
    "Season" text,
    "Week" integer NOT NULL,
    "SeasonType" text,
    "StartDate" timestamp with time zone NOT NULL,
    "StartTimeTbd" boolean NOT NULL,
    "HomeId" integer NOT NULL,
    "HomePoints" integer,
    "AwayId" integer NOT NULL,
    "AwayPoints" integer
);

ALTER TABLE "public"."Games" OWNER TO "postgres";

CREATE TABLE IF NOT EXISTS "public"."League" (
    "Id" integer NOT NULL,
    "Name" text,
    "Slug" text,
    "ImageUrl" text
);

ALTER TABLE "public"."League" OWNER TO "postgres";

CREATE TABLE IF NOT EXISTS "public"."Pick" (
    "Id" integer NOT NULL,
    "SeasonId" integer NOT NULL,
    "GameId" integer NOT NULL,
    "UserId" text,
    "Points" integer NOT NULL,
    "SelectedTeamId" integer
);

ALTER TABLE "public"."Pick" OWNER TO "postgres";

CREATE TABLE IF NOT EXISTS "public"."Season" (
    "Id" integer NOT NULL,
    "LeagueId" integer NOT NULL,
    "Year" text,
    "Active" boolean NOT NULL
);

ALTER TABLE "public"."Season" OWNER TO "postgres";

CREATE TABLE IF NOT EXISTS "public"."Teams" (
    "Id" integer NOT NULL,
    "School" text,
    "Mascot" text,
    "Abbreviation" text,
    "Conference" text,
    "Division" text,
    "Color" text,
    "AltColor" text,
    "Logos" text
);

ALTER TABLE "public"."Teams" OWNER TO "postgres";

CREATE OR REPLACE VIEW "public"."Leaderboard" AS
 SELECT u."Id" AS "UserId",
    u."FirstName" AS "UserFirstName",
    u."LastName" AS "UserLastName",
    t."Id" AS "TeamId",
    t."School" AS "TeamSchool",
    t."Mascot" AS "TeamMascot",
    t."Logos" AS "TeamLogos",
    l."Id" AS "LeagueId",
    l."Name" AS "LeagueName",
    l."Slug" AS "LeagueSlug",
    s."Year",
    sum(p."Points") AS "TotalPoints"
   FROM (((((public."Season" s
     JOIN public."League" l ON ((s."LeagueId" = l."Id")))
     JOIN public."ApplicationUserSeason" aus ON ((aus."SeasonsId" = s."Id")))
     JOIN public."AspNetUsers" u ON ((u."Id" = aus."MembersId")))
     JOIN public."Teams" t ON ((t."Id" = u."TeamId")))
     JOIN public."Pick" p ON (((p."UserId" = u."Id") AND (p."SeasonId" = s."Id"))))
  GROUP BY u."Id", u."FirstName", u."LastName", t."Id", t."School", t."Mascot", t."Logos", l."Id", l."Name", l."Slug", s."Year";

ALTER TABLE "public"."Leaderboard" OWNER TO "postgres";

ALTER TABLE "public"."League" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME "public"."League_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE IF NOT EXISTS "public"."PendingInvites" (
    "LeagueId" integer NOT NULL,
    "Season" text NOT NULL,
    "UserId" text NOT NULL,
    "TeamId" integer
);

ALTER TABLE "public"."PendingInvites" OWNER TO "postgres";

ALTER TABLE "public"."Pick" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME "public"."Pick_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE IF NOT EXISTS "public"."RefreshTokens" (
    "Id" integer NOT NULL,
    "Token" text,
    "JwtId" text,
    "AddedDate" timestamp with time zone NOT NULL,
    "ExpiryDate" timestamp with time zone NOT NULL,
    "UserId" text NOT NULL,
    "IpAddress" text,
    "UserAgent" text
);

ALTER TABLE "public"."RefreshTokens" OWNER TO "postgres";

ALTER TABLE "public"."RefreshTokens" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME "public"."RefreshTokens_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE IF NOT EXISTS "public"."SeasonCaches" (
    "Season" text NOT NULL,
    "Week" integer NOT NULL,
    "Type" text NOT NULL,
    "LastRefresh" timestamp with time zone NOT NULL
);

ALTER TABLE "public"."SeasonCaches" OWNER TO "postgres";

CREATE TABLE IF NOT EXISTS "public"."SeasonTeam" (
    "SeasonsId" integer NOT NULL,
    "TeamsId" integer NOT NULL
);

ALTER TABLE "public"."SeasonTeam" OWNER TO "postgres";

ALTER TABLE "public"."Season" ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME "public"."Season_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);

CREATE TABLE IF NOT EXISTS "public"."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);

ALTER TABLE "public"."__EFMigrationsHistory" OWNER TO "postgres";

ALTER TABLE ONLY "public"."ApplicationUserSeason"
    ADD CONSTRAINT "PK_ApplicationUserSeason" PRIMARY KEY ("MembersId", "SeasonsId");

ALTER TABLE ONLY "public"."AspNetRoleClaims"
    ADD CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id");

ALTER TABLE ONLY "public"."AspNetRoles"
    ADD CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id");

ALTER TABLE ONLY "public"."AspNetUserClaims"
    ADD CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id");

ALTER TABLE ONLY "public"."AspNetUserLogins"
    ADD CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey");

ALTER TABLE ONLY "public"."AspNetUserRoles"
    ADD CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId");

ALTER TABLE ONLY "public"."AspNetUserTokens"
    ADD CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name");

ALTER TABLE ONLY "public"."AspNetUsers"
    ADD CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id");

ALTER TABLE ONLY "public"."Calendar"
    ADD CONSTRAINT "PK_Calendar" PRIMARY KEY ("Week", "Season", "SeasonType");

ALTER TABLE ONLY "public"."Games"
    ADD CONSTRAINT "PK_Games" PRIMARY KEY ("Id");

ALTER TABLE ONLY "public"."League"
    ADD CONSTRAINT "PK_League" PRIMARY KEY ("Id");

ALTER TABLE ONLY "public"."PendingInvites"
    ADD CONSTRAINT "PK_PendingInvites" PRIMARY KEY ("UserId", "Season", "LeagueId");

ALTER TABLE ONLY "public"."Pick"
    ADD CONSTRAINT "PK_Pick" PRIMARY KEY ("Id");

ALTER TABLE ONLY "public"."RefreshTokens"
    ADD CONSTRAINT "PK_RefreshTokens" PRIMARY KEY ("Id");

ALTER TABLE ONLY "public"."Season"
    ADD CONSTRAINT "PK_Season" PRIMARY KEY ("Id");

ALTER TABLE ONLY "public"."SeasonCaches"
    ADD CONSTRAINT "PK_SeasonCaches" PRIMARY KEY ("Season", "Type", "Week");

ALTER TABLE ONLY "public"."SeasonTeam"
    ADD CONSTRAINT "PK_SeasonTeam" PRIMARY KEY ("SeasonsId", "TeamsId");

ALTER TABLE ONLY "public"."Teams"
    ADD CONSTRAINT "PK_Teams" PRIMARY KEY ("Id");

ALTER TABLE ONLY "public"."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");

CREATE INDEX "EmailIndex" ON public."AspNetUsers" USING btree ("NormalizedEmail");

CREATE INDEX "IX_ApplicationUserSeason_SeasonsId" ON public."ApplicationUserSeason" USING btree ("SeasonsId");

CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON public."AspNetRoleClaims" USING btree ("RoleId");

CREATE INDEX "IX_AspNetUserClaims_UserId" ON public."AspNetUserClaims" USING btree ("UserId");

CREATE INDEX "IX_AspNetUserLogins_UserId" ON public."AspNetUserLogins" USING btree ("UserId");

CREATE INDEX "IX_AspNetUserRoles_RoleId" ON public."AspNetUserRoles" USING btree ("RoleId");

CREATE INDEX "IX_AspNetUsers_TeamId" ON public."AspNetUsers" USING btree ("TeamId");

CREATE INDEX "IX_Games_AwayId" ON public."Games" USING btree ("AwayId");

CREATE INDEX "IX_Games_HomeId" ON public."Games" USING btree ("HomeId");

CREATE INDEX "IX_Pick_GameId" ON public."Pick" USING btree ("GameId");

CREATE INDEX "IX_Pick_SeasonId" ON public."Pick" USING btree ("SeasonId");

CREATE INDEX "IX_Pick_SelectedTeamId" ON public."Pick" USING btree ("SelectedTeamId");

CREATE INDEX "IX_Pick_UserId" ON public."Pick" USING btree ("UserId");

CREATE INDEX "IX_RefreshTokens_UserId" ON public."RefreshTokens" USING btree ("UserId");

CREATE INDEX "IX_SeasonTeam_TeamsId" ON public."SeasonTeam" USING btree ("TeamsId");

CREATE INDEX "IX_Season_LeagueId" ON public."Season" USING btree ("LeagueId");

CREATE INDEX "IX_Season_Year" ON public."Season" USING btree ("Year");

CREATE UNIQUE INDEX "RoleNameIndex" ON public."AspNetRoles" USING btree ("NormalizedName");

CREATE UNIQUE INDEX "UserNameIndex" ON public."AspNetUsers" USING btree ("NormalizedUserName");

ALTER TABLE ONLY "public"."ApplicationUserSeason"
    ADD CONSTRAINT "FK_ApplicationUserSeason_AspNetUsers_MembersId" FOREIGN KEY ("MembersId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."ApplicationUserSeason"
    ADD CONSTRAINT "FK_ApplicationUserSeason_Season_SeasonsId" FOREIGN KEY ("SeasonsId") REFERENCES public."Season"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."AspNetRoleClaims"
    ADD CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."AspNetUserClaims"
    ADD CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."AspNetUserLogins"
    ADD CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."AspNetRoles"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."AspNetUserRoles"
    ADD CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."AspNetUserTokens"
    ADD CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."AspNetUsers"
    ADD CONSTRAINT "FK_AspNetUsers_Teams_TeamId" FOREIGN KEY ("TeamId") REFERENCES public."Teams"("Id");

ALTER TABLE ONLY "public"."Games"
    ADD CONSTRAINT "FK_Games_Teams_AwayId" FOREIGN KEY ("AwayId") REFERENCES public."Teams"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."Games"
    ADD CONSTRAINT "FK_Games_Teams_HomeId" FOREIGN KEY ("HomeId") REFERENCES public."Teams"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."Pick"
    ADD CONSTRAINT "FK_Pick_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id");

ALTER TABLE ONLY "public"."Pick"
    ADD CONSTRAINT "FK_Pick_Games_GameId" FOREIGN KEY ("GameId") REFERENCES public."Games"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."Pick"
    ADD CONSTRAINT "FK_Pick_Season_SeasonId" FOREIGN KEY ("SeasonId") REFERENCES public."Season"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."Pick"
    ADD CONSTRAINT "FK_Pick_Teams_SelectedTeamId" FOREIGN KEY ("SelectedTeamId") REFERENCES public."Teams"("Id");

ALTER TABLE ONLY "public"."RefreshTokens"
    ADD CONSTRAINT "FK_RefreshTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES public."AspNetUsers"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."SeasonTeam"
    ADD CONSTRAINT "FK_SeasonTeam_Season_SeasonsId" FOREIGN KEY ("SeasonsId") REFERENCES public."Season"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."SeasonTeam"
    ADD CONSTRAINT "FK_SeasonTeam_Teams_TeamsId" FOREIGN KEY ("TeamsId") REFERENCES public."Teams"("Id") ON DELETE CASCADE;

ALTER TABLE ONLY "public"."Season"
    ADD CONSTRAINT "FK_Season_League_LeagueId" FOREIGN KEY ("LeagueId") REFERENCES public."League"("Id") ON DELETE CASCADE;

REVOKE USAGE ON SCHEMA "public" FROM PUBLIC;
GRANT USAGE ON SCHEMA "public" TO "postgres";
GRANT USAGE ON SCHEMA "public" TO "anon";
GRANT USAGE ON SCHEMA "public" TO "authenticated";
GRANT USAGE ON SCHEMA "public" TO "service_role";

GRANT ALL ON TABLE "public"."ApplicationUserSeason" TO "anon";
GRANT ALL ON TABLE "public"."ApplicationUserSeason" TO "authenticated";
GRANT ALL ON TABLE "public"."ApplicationUserSeason" TO "service_role";

GRANT ALL ON TABLE "public"."AspNetRoleClaims" TO "anon";
GRANT ALL ON TABLE "public"."AspNetRoleClaims" TO "authenticated";
GRANT ALL ON TABLE "public"."AspNetRoleClaims" TO "service_role";

GRANT ALL ON SEQUENCE "public"."AspNetRoleClaims_Id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."AspNetRoleClaims_Id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."AspNetRoleClaims_Id_seq" TO "service_role";

GRANT ALL ON TABLE "public"."AspNetRoles" TO "anon";
GRANT ALL ON TABLE "public"."AspNetRoles" TO "authenticated";
GRANT ALL ON TABLE "public"."AspNetRoles" TO "service_role";

GRANT ALL ON TABLE "public"."AspNetUserClaims" TO "anon";
GRANT ALL ON TABLE "public"."AspNetUserClaims" TO "authenticated";
GRANT ALL ON TABLE "public"."AspNetUserClaims" TO "service_role";

GRANT ALL ON SEQUENCE "public"."AspNetUserClaims_Id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."AspNetUserClaims_Id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."AspNetUserClaims_Id_seq" TO "service_role";

GRANT ALL ON TABLE "public"."AspNetUserLogins" TO "anon";
GRANT ALL ON TABLE "public"."AspNetUserLogins" TO "authenticated";
GRANT ALL ON TABLE "public"."AspNetUserLogins" TO "service_role";

GRANT ALL ON TABLE "public"."AspNetUserRoles" TO "anon";
GRANT ALL ON TABLE "public"."AspNetUserRoles" TO "authenticated";
GRANT ALL ON TABLE "public"."AspNetUserRoles" TO "service_role";

GRANT ALL ON TABLE "public"."AspNetUserTokens" TO "anon";
GRANT ALL ON TABLE "public"."AspNetUserTokens" TO "authenticated";
GRANT ALL ON TABLE "public"."AspNetUserTokens" TO "service_role";

GRANT ALL ON TABLE "public"."AspNetUsers" TO "anon";
GRANT ALL ON TABLE "public"."AspNetUsers" TO "authenticated";
GRANT ALL ON TABLE "public"."AspNetUsers" TO "service_role";

GRANT ALL ON TABLE "public"."Calendar" TO "anon";
GRANT ALL ON TABLE "public"."Calendar" TO "authenticated";
GRANT ALL ON TABLE "public"."Calendar" TO "service_role";

GRANT ALL ON TABLE "public"."Games" TO "anon";
GRANT ALL ON TABLE "public"."Games" TO "authenticated";
GRANT ALL ON TABLE "public"."Games" TO "service_role";

GRANT ALL ON TABLE "public"."League" TO "anon";
GRANT ALL ON TABLE "public"."League" TO "authenticated";
GRANT ALL ON TABLE "public"."League" TO "service_role";

GRANT ALL ON TABLE "public"."Pick" TO "anon";
GRANT ALL ON TABLE "public"."Pick" TO "authenticated";
GRANT ALL ON TABLE "public"."Pick" TO "service_role";

GRANT ALL ON TABLE "public"."Season" TO "anon";
GRANT ALL ON TABLE "public"."Season" TO "authenticated";
GRANT ALL ON TABLE "public"."Season" TO "service_role";

GRANT ALL ON TABLE "public"."Teams" TO "anon";
GRANT ALL ON TABLE "public"."Teams" TO "authenticated";
GRANT ALL ON TABLE "public"."Teams" TO "service_role";

GRANT ALL ON TABLE "public"."Leaderboard" TO "anon";
GRANT ALL ON TABLE "public"."Leaderboard" TO "authenticated";
GRANT ALL ON TABLE "public"."Leaderboard" TO "service_role";

GRANT ALL ON SEQUENCE "public"."League_Id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."League_Id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."League_Id_seq" TO "service_role";

GRANT ALL ON TABLE "public"."PendingInvites" TO "anon";
GRANT ALL ON TABLE "public"."PendingInvites" TO "authenticated";
GRANT ALL ON TABLE "public"."PendingInvites" TO "service_role";

GRANT ALL ON SEQUENCE "public"."Pick_Id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."Pick_Id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."Pick_Id_seq" TO "service_role";

GRANT ALL ON TABLE "public"."RefreshTokens" TO "anon";
GRANT ALL ON TABLE "public"."RefreshTokens" TO "authenticated";
GRANT ALL ON TABLE "public"."RefreshTokens" TO "service_role";

GRANT ALL ON SEQUENCE "public"."RefreshTokens_Id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."RefreshTokens_Id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."RefreshTokens_Id_seq" TO "service_role";

GRANT ALL ON TABLE "public"."SeasonCaches" TO "anon";
GRANT ALL ON TABLE "public"."SeasonCaches" TO "authenticated";
GRANT ALL ON TABLE "public"."SeasonCaches" TO "service_role";

GRANT ALL ON TABLE "public"."SeasonTeam" TO "anon";
GRANT ALL ON TABLE "public"."SeasonTeam" TO "authenticated";
GRANT ALL ON TABLE "public"."SeasonTeam" TO "service_role";

GRANT ALL ON SEQUENCE "public"."Season_Id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."Season_Id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."Season_Id_seq" TO "service_role";

GRANT ALL ON TABLE "public"."__EFMigrationsHistory" TO "anon";
GRANT ALL ON TABLE "public"."__EFMigrationsHistory" TO "authenticated";
GRANT ALL ON TABLE "public"."__EFMigrationsHistory" TO "service_role";

ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES  TO "postgres";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES  TO "anon";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES  TO "authenticated";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES  TO "service_role";

ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS  TO "postgres";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS  TO "anon";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS  TO "authenticated";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS  TO "service_role";

ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES  TO "postgres";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES  TO "anon";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES  TO "authenticated";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES  TO "service_role";

RESET ALL;