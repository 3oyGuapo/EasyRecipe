import { test, expect } from "vitest";
import { renderHook, act } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import { useRecipeForm } from "./useRecipeForm";

test("Initial value should be empty string and array", () => {
  const { result } = renderHook(() => useRecipeForm(), {
    wrapper: MemoryRouter,
  });

  expect(result.current.recipeName).toBe("");
  expect(result.current.ingredients).toEqual([]);
  expect(result.current.steps).toEqual([]);
  expect(result.current.tagInput).toBe("");
});
