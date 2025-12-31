import type { Recipe } from "../types";
import styles from "./RecipeList.module.css";

interface RecipeCardProps {
  recipe: Recipe;
  onDelete: () => void; //A method that has no parameter or return value
}

function RecipeCard({ recipe, onDelete }: RecipeCardProps) {
  return (
    <div className={styles.card}>
      <h3>{recipe.recipeName}</h3>
      <p>
        Ingredients amount: {recipe.ingredientsList?.length || 0} {"  "}
        Steps: {recipe.stepsList?.length || 0}
      </p>

      <div>
        {recipe.tagsList?.map((tag) => (
          <span key={tag.id} style={{ marginRight: "5px", color: "blue" }}>
            #{tag.name}
          </span>
        ))}
      </div>

      {/* Create a button that calls onDelete method for delete recipe */}
      <button onClick={onDelete}>Delete this recipe</button>
    </div>
  );
}

export default RecipeCard;
