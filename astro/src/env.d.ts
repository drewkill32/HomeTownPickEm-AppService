/// <reference path="../.astro/types.d.ts" />
/// <reference types="astro/client" />

interface ImportMetaEnv {
  readonly SUPABASE_URL: string;
  readonly SUPABASE_ANON_KEY: string;
  readonly PUBLIC_REGISTER_EMAIL: string;
  readonly PUBLIC_REGISTER_PASSWORD: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
