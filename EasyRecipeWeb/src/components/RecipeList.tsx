import { useState, useEffect } from "react";
import type { Recipe } from "../types";
import RecipeCard from "./RecipeCard";
import { Link } from "react-router-dom";
import { toast } from "sonner";

function RecipeList() {
  const [recipeList, setRecipeList] = useState<Recipe[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  useEffect(() => {
    const fetchData = async () => {
      try {
        //Fetch response from the address
        const response = await fetch(
          `/api/Recipes?pageNumber=${currentPage}&pageSize=10`
        );
        const data = await response.json();
        setRecipeList(data.items);
        setTotalPages(data.totalPages);
      } catch (error) {
        console.error("Error occur", error);
      }
    };

    fetchData();
  }, [currentPage]);

  // Method that has the logic for deleting recipe
  const handleDelete = async (id: number) => {
    try {
      const response = await fetch(`/api/Recipes/${id}`, {
        method: "DELETE",
      });

      if (response.ok) {
        //setRecipeList(
        //(originalList) => originalList.filter((recipe) => recipe.id !== id) // Keep recipes that has different id than the target recipe using filter
        //);
        setCurrentPage(currentPage);
        toast.success("Recipe deleted successfully");
      } else {
        toast.error("Failed to delete recipe");
      }
    } catch (error) {
      toast.error("An unexpected error occur, failed to delete recipe.");
    }
  };

  return (
    <>
      <h2>Recipe List</h2>
      <Link to="/add">
        <button>Add recipe</button>
      </Link>
      <div>
        {recipeList.map((recipe) => (
          <RecipeCard
            key={recipe.id}
            recipe={recipe}
            onDelete={() => handleDelete(recipe.id)} // Pass in the logic method to child component
          />
        ))}
      </div>

      <div
        style={{
          margin: "20px",
          display: "flex",
          gap: "10px",
          alignItems: "center",
        }}
      >
        <button
          onClick={() => setCurrentPage((page) => Math.max(1, page - 1))}
          disabled={currentPage === 1}
        >
          Previous Page
        </button>

        <span>
          Page {currentPage}/{totalPages}
        </span>

        <button
          onClick={() =>
            setCurrentPage((page) => Math.min(totalPages, page + 1))
          }
          disabled={currentPage === totalPages}
        >
          Next Page
        </button>
      </div>
    </>
  );
}

export default RecipeList;
