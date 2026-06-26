export interface Recipe {
  id: number;
  recipeName: string;
  ingredients: Ingredient[];
  steps: Step[];
  tags: string[];
  createdAt: string;
}

export interface Ingredient {
  name: string;
  unitAmount: string;
}

export interface Step {
  stepContent: string;
  stepOrder: number;
}
