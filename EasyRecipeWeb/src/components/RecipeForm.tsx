import { Link } from "react-router-dom";
import { useRecipeForm } from "../hooks/useRecipeForm";
import { toast } from "sonner";

function RecipeForm() {
  const form = useRecipeForm();

  const handleCreate = async () => {
    try {
      const response = await fetch("/api/Recipes", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(form.getPayload()),
      });

      if (response.ok) {
        toast.success("Recipe created successfully");
        form.navigate("/");
      } else {
        toast.error("Failed to create recipe");
      }
    } catch (error) {
      toast.error("An unexpected error occur, failed to create recipe.");
    }
  };

  return (
    <>
      <div>
        <Link to="/">
          <button>Home page</button>
        </Link>
      </div>

      <div>
        <input
          required
          type="text"
          placeholder="Enter new recipe name:"
          value={form.recipeName}
          onChange={form.onChange}
        />
        <button onClick={handleCreate}>Create recipe</button>

        <div className="ingredients-List">
          <h3>Ingredients</h3>

          {form.ingredients.map((ingredient, index) => (
            <div
              key={index}
              style={{ display: "flex", gap: "10px", marginBottom: "10px" }}
            >
              <input
                required
                placeholder="Ingredient Name"
                value={ingredient.name}
                onChange={(e) =>
                  form.handleIngredientChange(index, "name", e.target.value)
                }
              />

              <input
                required
                placeholder="Amount"
                value={ingredient.unitAmount}
                onChange={(e) =>
                  form.handleIngredientChange(index, "amount", e.target.value)
                }
              />
            </div>
          ))}
        </div>

        <button onClick={form.addIngredient}>Add ingredient</button>

        <div className="steps-List">
          <h3>Steps</h3>

          {form.steps.map((step, index) => (
            <div
              key={index}
              style={{ display: "flex", gap: "10px", marginBottom: "10px" }}
            >
              <span style={{ padding: "5px", fontWeight: "bold" }}>
                Step {step.stepOrder}
              </span>
              <input
                required
                placeholder="Step details:"
                value={step.stepContent}
                onChange={(e) => form.handleStepChange(index, e.target.value)}
                style={{ flex: 1 }}
              />
            </div>
          ))}
        </div>

        <button onClick={form.addStep}>Add steps</button>

        <div style={{ marginTop: "20px" }}>
          <h3>Tags (use comma to separate)</h3>

          <input
            placeholder="e.g. breakfast, lunch, dissert"
            value={form.tagInput}
            onChange={(e) => form.setTagInput(e.target.value)}
            style={{ width: "100%" }}
          />
        </div>
      </div>
    </>
  );
}

export default RecipeForm;
