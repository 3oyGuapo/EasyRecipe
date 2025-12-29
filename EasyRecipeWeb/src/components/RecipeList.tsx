import { useState, useEffect } from "react";
import type { Recipe } from "../types";

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

  return (
    <>
      <h2>Recipe List</h2>

      <div>
        {recipeList.map((recipe) => (
          <div key={recipe.id}>
            <h3>{recipe.recipeName}</h3>
          </div>
        ))}
      </div>
    </>
  );
}

export default RecipeList;
