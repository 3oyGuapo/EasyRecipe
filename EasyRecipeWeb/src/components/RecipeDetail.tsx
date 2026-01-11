import { useState, useEffect } from "react";
import { useParams, Link } from "react-router-dom";
import type { Recipe } from "../types";

function RecipeDetail() {
  const { id } = useParams();
  const [recipe, setRecipe] = useState<Recipe | null>(null);

  useEffect(() => {
    const fetchDetail = async () => {
      try {
        const response = await fetch(
          `https://localhost:7287/api/Recipes/${id}`
        );
        const detail = await response.json();

        setRecipe(detail);
      } catch (error) {
        console.error("Error fetching details", error);
      }
    };
    fetchDetail();
  }, [id]);

  if (!recipe) {
    return <div>Loading...</div>;
  }

  return (
    <>
      <div>
        <h2>Recipe Id: {id}</h2>

        <ul>
          {recipe.ingredientsList.map((ingredient) => (
            <li>{ingredient.name}</li>
          ))}
        </ul>

        <ul>
          {recipe.stepsList.map((step) => (
            <li>
              {step.stepOrder} {step.stepContent}
            </li>
          ))}
        </ul>
      </div>
      <div>
        <Link to={`/recipe/edit/${id}`}>
          <button>Edit</button>
        </Link>
      </div>
    </>
  );
}

export default RecipeDetail;
