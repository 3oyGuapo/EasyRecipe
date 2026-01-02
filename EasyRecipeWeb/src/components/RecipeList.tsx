import { useState, useEffect } from "react";
import type { Recipe } from "../types";
import RecipeCard from "./RecipeCard";
import RecipeForm from "./RecipeForm";

function RecipeList() {
  const [recipeList, setRecipeList] = useState<Recipe[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        //Fetch response from the address
        const response = await fetch("https://localhost:7287/api/Recipes");
        const data = await response.json();
        setRecipeList(data);
      } catch (error) {
        console.error("Error occur", error);
      }
    };

    fetchData();
  }, []);

  // Method that has the logic for deleting recipe
  const handleDelete = async (id: number) => {
    try {
      const response = await fetch(`https://localhost:7287/api/Recipes/${id}`, {
        method: "DELETE",
      });

      if (response.ok) {
        setRecipeList(
          (originalList) => originalList.filter((recipe) => recipe.id !== id) // Keep recipes that has different id than the target recipe using filter
        );
      } else {
        alert("Delete fail.");
      }
    } catch (error) {
      console.error("Deletion error", error);
    }
  };

  return (
    <>
      <h2>Recipe List</h2>

      <div>
        <RecipeForm
          onCreated={(newRecipe) => setRecipeList([...recipeList, newRecipe])}
        />
      </div>
      <div>
        {recipeList.map((recipe) => (
          <RecipeCard
            key={recipe.id}
            recipe={recipe}
            onDelete={() => handleDelete(recipe.id)} // Pass in the logic method to child component
          />
        ))}
      </div>
    </>
  );
}

export default RecipeList;
