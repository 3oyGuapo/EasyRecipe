import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

export function useRecipeForm(id?: string) {
  const [recipeName, setRecipeName] = useState("");
  const [ingredients, setIngredients] = useState<
    { name: string; unitAmount: string }[]
  >([]);
  const [steps, setSteps] = useState<
    { stepContent: string; stepOrder: number }[]
  >([]);
  const [tagInput, setTagInput] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    if (!id) return;

    const fetchDetail = async () => {
      try {
        const response = await fetch(`/api/Recipes/${id}`);
        const detail = await response.json();

        setRecipeName(detail.recipeName);
        setIngredients(detail.ingredients);
        setSteps(detail.steps);
        setTagInput(detail.tags.join(","));
      } catch (error) {
        console.error("Error occur", error);
      }
    };
    fetchDetail();
  }, [id]);

  const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setRecipeName(e.target.value);
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

  //return array of tags, without {} so it will auto return without return keyword
  const getTagsArray = () =>
    tagInput
      .split(",")
      .map((tag) => tag.trim())
      .filter((tag) => tag.length > 0);

  const getPayload = () => ({
    recipeName,
    ingredients,
    steps,
    tags: getTagsArray(),
  });

  return {
    recipeName,
    ingredients,
    steps,
    tagInput,
    setTagInput,

    onChange,
    addIngredient,
    addStep,

    handleIngredientChange,
    handleStepChange,

    getPayload,
    getTagsArray,
    navigate,
  };
}
