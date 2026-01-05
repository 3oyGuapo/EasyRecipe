import { useState, useEffect } from "react";
import type { Recipe } from "../types.ts";

interface RecipeFormProps {
  onCreated: (recipe: Recipe) => void;
}

function RecipeForm({ onCreated }: RecipeFormProps) {
  const [recipeName, setRecipeName] = useState("");
  const [ingredients, setIngredients] = useState<
    { ingredientName: string; unitAmount: string }[]
  >([]);

  const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setRecipeName(e.target.value);
  };

  const handleCreate = async () => {
    const payload = {
      name: recipeName,
      ingredientsList: [],
      stepsList: [],
    };

    try {
      const response = await fetch("https://localhost:7287/api/Recipes", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(payload),
      });

      const createdRecipe = await response.json();

      onCreated(createdRecipe);

      setRecipeName("");
    } catch (error) {
      console.error("Fail to create new recipe", error);
    }
  };

  const addIngredient = () => {
    setIngredients([...ingredients, { ingredientName: "", unitAmount: "" }]);
  };

  const handleIngredientChange = (
    index: number,
    field: "name" | "amount",
    value: string
  ) => {
    const newIngredients = [...ingredients];

    if (field === "name") {
      newIngredients[index].ingredientName = value;
    } else {
      newIngredients[index].unitAmount = value;
    }

    setIngredients(newIngredients);
  };

  return (
    <div>
      <input
        type="text"
        placeholder="Enter new recipe name:"
        value={recipeName}
        onChange={onChange}
      />
      <button onClick={handleCreate}>Create recipe</button>

      <div className="ingredients-List">
        <h3>Ingredients</h3>

        {ingredients.map((ingredient, index) => (
          <div
            key={index}
            style={{ display: "flex", gap: "10px", marginBottom: "10px" }}
          >
            <input
              placeholder="Ingredient Name"
              value={ingredient.ingredientName}
              onChange={(e) =>
                handleIngredientChange(index, "name", e.target.value)
              }
            />

            <input
              placeholder="Amount"
              value={ingredient.unitAmount}
              onChange={(e) =>
                handleIngredientChange(index, "amount", e.target.value)
              }
            />
          </div>
        ))}
      </div>

      <button onClick={addIngredient}>Add ingredient</button>
    </div>
  );
}

export default RecipeForm;
