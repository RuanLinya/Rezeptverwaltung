
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { CategoriesComponent } from './components/categories/categories.component';
import { CategoryComponent } from './components/category/category.component';
import { RecipeComponent } from './components/recipe/recipe.component';
import { AddRecipeComponent } from './components/add-recipe/add-recipe.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ProfileComponent } from './components/profile/profile.component';
import { FollowingComponent } from './components/following/following.component';
import { IngredientsComponent } from './components/ingredients/ingredients.component';
import { DirectionsComponent } from './components/directions/directions.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'categories', component: CategoriesComponent },
  { path: 'category', component: CategoryComponent },
  { path: 'recipe', component: RecipeComponent },
  { path: 'add-recipe', component: AddRecipeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'following', component: FollowingComponent },
  { path: 'ingredients', component: IngredientsComponent },
  { path: 'directions', component: DirectionsComponent },
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
