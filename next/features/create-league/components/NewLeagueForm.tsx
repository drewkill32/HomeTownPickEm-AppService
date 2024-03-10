"use client";

import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
  SubmitButton,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { Checkbox } from "@/components/ui/checkbox";
import { useFormState } from "react-dom";
import { createLeague } from "../actions/createLeague";
import { newLeagueSchema } from "../validation";
import { useEffect } from "react";
import { useToast } from "@/components/ui/use-toast";

type Props = {};

export function NewLeagueForm({}: Props) {
  const form = useForm<z.infer<typeof newLeagueSchema>>({
    resolver: zodResolver(newLeagueSchema),
    mode: "onChange",
    defaultValues: {
      leagueName: "",
      description: "",
      password: undefined,
      public: false,
    },
  });

  const [state, formAction] = useFormState(createLeague, {
    error: "",
  });

  const { toast } = useToast();

  useEffect(() => {
    if (state.error) {
      toast({
        variant: "destructive",
        title: "Uh oh! Something went wrong.",
        description: state.error,
      });
    }
  }, [state]);

  return (
    <Form {...form}>
      <form className="space-y-4" action={formAction}>
        <FormField
          control={form.control}
          name="leagueName"
          render={({ field }) => (
            <FormItem>
              <FormLabel>League Name</FormLabel>
              <FormControl>
                <Input {...field} placeholder="Enter the league name" />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="description"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Description</FormLabel>
              <FormControl>
                <Input {...field} placeholder="Enter the league name" />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="password"
          render={({ field }) => (
            <FormItem>
              <FormLabel>
                Password
                <FormDescription className="inline">(optional)</FormDescription>
              </FormLabel>
              <FormControl>
                <Input {...field} placeholder="Enter the league name" />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
        <FormField
          control={form.control}
          name="public"
          render={({ field }) => (
            <FormItem>
              <FormLabel className="flex cursor-pointer flex-row items-start space-x-3 space-y-0 rounded-md border p-4">
                <FormControl>
                  <Checkbox
                    checked={field.value}
                    name="public"
                    value={field.value.toString()}
                    onCheckedChange={field.onChange}
                  />
                </FormControl>
                <div className="space-y-1 leading-none">
                  Public
                  <FormDescription>
                    Allow your league to be discoverable by others
                  </FormDescription>
                </div>
                <FormMessage />
              </FormLabel>
            </FormItem>
          )}
        />
        <SubmitButton className="w-full">Create League</SubmitButton>
      </form>
    </Form>
  );
}
