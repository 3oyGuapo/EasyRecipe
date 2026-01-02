import { useState, useEffect } from "react";
import type { Recipe } from "../types.ts";

interface RecipeFormProps {
  onCreated: (recipe: Recipe) => void;
}

function RecipeForm({ onCreated }: RecipeFormProps) {
  const [recipeName, setRecipeName] = useState("");

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

  return (
    <div>
      <input
        type="text"
        placeholder="Enter new recipe name:"
        value={recipeName}
        onChange={onChange}
      />
      <button onClick={handleCreate}>Create recipe</button>
    </div>
  );
}

export default RecipeForm;
