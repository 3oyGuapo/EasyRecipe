import type { Recipe } from "../types";
import styles from "./RecipeList.module.css";
import { Link } from "react-router-dom";

interface RecipeCardProps {
  recipe: Recipe;
  onDelete: () => void; //A method that has no parameter or return value
}

function RecipeCard({ recipe, onDelete }: RecipeCardProps) {
  return (
    <div className={styles.card}>
      <Link to={`/recipe/${recipe.id}`}>
        <h3>{recipe.recipeName}</h3>
      </Link>
      <p>
        Ingredients amount: {recipe.ingredients?.length || 0} {"  "}
        Steps: {recipe.steps?.length || 0}
      </p>

      <div>
        {recipe.tags?.map((tag) => (
          <span key={tag} style={{ marginRight: "5px", color: "blue" }}>
            #{tag}
          </span>
        ))}
      </div>

      {/* Create a button that calls onDelete method for delete recipe */}
      <button onClick={onDelete}>Delete this recipe</button>
    </div>
  );
}

export default RecipeCard;
