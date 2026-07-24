import { test, expect } from "vitest";
import { renderHook, act } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import { useRecipeForm } from "./useRecipeForm";

function renderRecipeForm(id?: string) {
  return renderHook(() => useRecipeForm(id), {
    wrapper: MemoryRouter,
  });
}

test("Initial value should be empty string and array", () => {
  const { result } = renderRecipeForm();

  expect(result.current.recipeName).toBe("");
  expect(result.current.ingredients).toEqual([]);
  expect(result.current.steps).toEqual([]);
  expect(result.current.tagInput).toBe("");
});
