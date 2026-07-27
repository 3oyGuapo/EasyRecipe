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

test("onChange should update recipeName", () => {
  const { result } = renderRecipeForm();

  act(() => {
    result.current.onChange({
      target: { value: "Pasta" },
    } as React.ChangeEvent<HTMLInputElement>);
  });

  expect(result.current.recipeName).toBe("Pasta");
});

test("addIngredient should add an empty ingredient", () => {
  const { result } = renderRecipeForm();

  act(() => {
    result.current.addIngredient();
  });

  expect(result.current.ingredients).toHaveLength(1);
  expect(result.current.ingredients[0]).toEqual({ name: "", unitAmount: "" });
});

test("Calling addIngredient multiple times should increase array length", () => {
  const { result } = renderRecipeForm();

  act(() => {
    result.current.addIngredient();
  });
  act(() => {
    result.current.addIngredient();
  });

  expect(result.current.ingredients).toHaveLength(2);
  expect(result.current.ingredients[0]).toEqual({ name: "", unitAmount: "" });
  expect(result.current.ingredients[1]).toEqual({ name: "", unitAmount: "" });
});

test("addStep should calculate stepOrder based on the order", () => {
  const { result } = renderRecipeForm();

  act(() => {
    result.current.addStep();
  });
  act(() => {
    result.current.addStep();
  });
  act(() => {
    result.current.addStep();
  });

  expect(result.current.steps).toHaveLength(3);
  expect(result.current.steps[0].stepOrder).toBe(1);
  expect(result.current.steps[2].stepOrder).toBe(3);
});
