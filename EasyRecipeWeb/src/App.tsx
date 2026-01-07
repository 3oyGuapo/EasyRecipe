import "./App.css";
import RecipeList from "./components/RecipeList";
import RecipeForm from "./components/RecipeForm";
import RecipeDetail from "./components/RecipeDetail";
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
        </Routes>
      </div>
    </>
  );
}

export default App;
