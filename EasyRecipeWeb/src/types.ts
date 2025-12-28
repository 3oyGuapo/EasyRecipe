export interface Recipe {
  id: number;
  recipeName: string;
  ingredientsList: Ingredient[];
  stepsLists: Step[];
  tagsList: Tag[];
}

export interface Ingredient {
  id: number;
  name: string;
  unitAmount: string;
  recipeId: number;
}

export interface Step {
  stepContent: string;
  id: number;
  stepOrder: number;
  recipeId: number;
}

export interface Tag {
  id: number;
  name: string;
  recipeList?: Recipe[];
}
