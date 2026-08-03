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

test("handleIngredientChange should change specified ingredient field", () => {
  const { result } = renderRecipeForm();

  act(() => {
    result.current.addIngredient();
  });

  act(() => {
    result.current.handleIngredientChange(0, "name", "flour");
  });

  expect(result.current.ingredients[0].name).toBe("flour");
  expect(result.current.ingredients[0].unitAmount).toBe("");
});

test("getTagsArray should split comma and convert to array", () => {
  const { result } = renderRecipeForm();

  act(() => {
    result.current.setTagInput("lunch, dinner, ");
  });

  expect(result.current.getTagsArray()).toEqual(["lunch", "dinner"]);
});

test("getPayload should return correct structure", () => {
  const { result } = renderRecipeForm();

  act(() => {
    result.current.setTagInput("dinner");
    result.current.addIngredient();
    result.current.addStep();
  });

  const payload = result.current.getPayload();

  expect(payload).toHaveProperty("recipeName");
  expect(payload).toHaveProperty("ingredients");
  expect(payload).toHaveProperty("steps");
  expect(payload.tags).toEqual(["dinner"]);
});
