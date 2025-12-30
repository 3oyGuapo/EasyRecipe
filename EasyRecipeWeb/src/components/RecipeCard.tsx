import type { Recipe } from "../types";
import styles from "./RecipeList.module.css";

interface RecipeCardProps {
  recipe: Recipe;
}

function RecipeCard({ recipe }: RecipeCardProps) {
  return (
    <div className={styles.card}>
      <h3>{recipe.recipeName}</h3>
      <p>
        Ingredients amount: {recipe.ingredientsList?.length || 0}
        Steps: {recipe.stepsLists?.length || 0}
      </p>

      <div>
        {recipe.tagsList?.map((tag) => (
          <span key={tag.id} style={{ marginRight: "5px", color: "blue" }}>
            #{tag.name}
          </span>
        ))}
      </div>
    </div>
  );
}

export default RecipeCard;
