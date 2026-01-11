import "./App.css";
import RecipeList from "./components/RecipeList";
import RecipeForm from "./components/RecipeForm";
import RecipeDetail from "./components/RecipeDetail";
import RecipeEdit from "./components/RecipeEdit";
import { Routes, Route } from "react-router-dom";

function App() {
  return (
    <>
      <div>
        <h1>Recipe List</h1>
      </div>

      <div>
        <Routes>
          <Route path="/" element={<RecipeList />} />

          <Route path="/add" element={<RecipeForm />} />

          <Route path="/recipe/:id" element={<RecipeDetail />} />

          <Route path="recipe/edit/:id" element={<RecipeEdit />} />
        </Routes>
      </div>
    </>
  );
}

export default App;
