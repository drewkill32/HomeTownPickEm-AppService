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
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Checkbox } from "@/components/ui/checkbox";
import { useFormState } from "react-dom";
import { createLeague } from "../actions/createLeague";
import { NewLeague, newLeagueSchema } from "../validation";
import { useEffect } from "react";
import { useToast } from "@/components/ui/use-toast";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";

export function NewLeagueForm() {
  const form = useForm<NewLeague>({
    resolver: zodResolver(newLeagueSchema),
    mode: "onBlur",
    defaultValues: {
      name: "",
      description: "",
      password: "",
      public: false,
    },
  });

  const [state, formAction] = useFormState(createLeague, null);

  const { toast } = useToast();

  useEffect(() => {
    if (state === null) return;
    if (state.success) {
      form.reset();
      toast({
        title: "Success",
        description: state.message,
      });
    } else {
      if (state.type === "validation") {
        state.error.forEach((error) => {
          form.setError(error.name, {
            message: error.message,
          });
        });
        if (state.error.length === 0) {
          form.setFocus(state.error[0].name);
        }
      } else {
        toast({
          variant: "destructive",
          title: "Uh oh! Something went wrong.",
          description: state.error.message,
        });
      }
    }
  }, [state]);

  return (
    <Form {...form}>
      <form className="space-y-4" action={formAction}>
        <FormField
          control={form.control}
          name="name"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Name</FormLabel>
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
                <Textarea
                  {...field}
                  placeholder="Enter the league description"
                />
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
                <FormDescription className="inline">
                  {" "}
                  (optional)
                </FormDescription>
              </FormLabel>
              <FormControl>
                <Input
                  {...field}
                  placeholder="Enter a password for the league"
                />
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
              <FormLabel className="flex flex-row items-start space-x-3 space-y-0 rounded-md border p-4">
                <FormControl>
                  <Checkbox
                    {...{ ...field, value: field.value.toString() }}
                    name="public"
                    className="mr-2"
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
