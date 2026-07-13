import { useState, useEffect } from "react";
import { useParams, Link } from "react-router-dom";
import type { Recipe } from "../types";
import { toast } from "sonner";

function RecipeDetail() {
  const { id } = useParams();
  const [recipe, setRecipe] = useState<Recipe | null>(null);

  useEffect(() => {
    const fetchDetail = async () => {
      try {
        const response = await fetch(`/api/Recipes/${id}`);
        const detail = await response.json();

        setRecipe(detail);
      } catch (error) {
        toast.error("Failed to fetch details");
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
          {recipe.ingredients.map((ingredient) => (
            <li key={ingredient.name}>{ingredient.name}</li>
          ))}
        </ul>

        <ul>
          {recipe.steps.map((step) => (
            <li key={step.stepOrder}>
              {step.stepOrder} {step.stepContent}
            </li>
          ))}
        </ul>

        <ul>
          {recipe.tags.map((tag) => (
            <span key={tag} style={{ marginRight: "5px", color: "blue" }}>
              #{tag}
            </span>
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
