import { useState } from "react";
import type { Recipe } from "../types.ts";
import { useNavigate, Link } from "react-router-dom";

function RecipeForm() {
  const [recipeName, setRecipeName] = useState("");
  const [ingredients, setIngredients] = useState<
    { name: string; unitAmount: string }[]
  >([]);
  const [steps, setSteps] = useState<
    { stepContent: string; stepOrder: number }[]
  >([]);
  const [tagInput, setTagInput] = useState("");
  const navigate = useNavigate();

  const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setRecipeName(e.target.value);
  };

  const handleCreate = async () => {
    const tagsArray = tagInput
      .split(",")
      .map((tag) => tag.trim())
      .filter((tag) => tag.length > 0);

    const payload = {
      name: recipeName,
      ingredientsList: ingredients,
      stepsList: steps,
      tagsList: tagsArray,
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

      setRecipeName("");

      navigate("/");
    } catch (error) {
      console.error("Fail to create new recipe", error);
    }
  };

  const addIngredient = () => {
    setIngredients([...ingredients, { name: "", unitAmount: "" }]);
  };

  const addStep = () => {
    setSteps([...steps, { stepContent: "", stepOrder: steps.length + 1 }]);
  };

  const handleIngredientChange = (
    index: number,
    field: "name" | "amount",
    value: string
  ) => {
    const newIngredients = [...ingredients];

    if (field === "name") {
      newIngredients[index].name = value;
    } else {
      newIngredients[index].unitAmount = value;
    }

    setIngredients(newIngredients);
  };

  const handleStepChange = (index: number, value: string) => {
    const newSteps = [...steps];

    newSteps[index].stepContent = value;
    setSteps(newSteps);
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
                required
                placeholder="Ingredient Name"
                value={ingredient.name}
                onChange={(e) =>
                  handleIngredientChange(index, "name", e.target.value)
                }
              />

              <input
                required
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

        <div className="steps-List">
          <h3>Steps</h3>

          {steps.map((step, index) => (
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
                onChange={(e) => handleStepChange(index, e.target.value)}
                style={{ flex: 1 }}
              />
            </div>
          ))}
        </div>

        <button onClick={addStep}>Add steps</button>

        <div style={{ marginTop: "20px" }}>
          <h3>Tags (use comma to separate)</h3>

          <input
            placeholder="e.g. breakfast, lunch, dissert"
            value={tagInput}
            onChange={(e) => setTagInput(e.target.value)}
            style={{ width: "100%" }}
          />
        </div>
      </div>
    </>
  );
}

export default RecipeForm;
