window.onload = () => {
    allIngredients.forEach(createIngredient);
    allRecipes.forEach(prepareRecipe);
    prepareUnlocks();
    addUnlockButton();

    window.onload = undefined;
}

function createIngredient(ingredient) {
    var spanQuantity = document.createElement('span');
    spanQuantity.classList.add('ingredient');
    spanQuantity.onclick = ev => ingredientOnClick(ingredient, -1, ev.button);

    var img = document.createElement('img');
    img.classList.add('ingredient');
    img.src = ingredient.src;
    img.onclick = ev => ingredientOnClick(ingredient, +1, ev.button);

    var div = document.createElement('div');
    div.classList.add('ingredient');
    div.appendChild(spanQuantity);
    div.appendChild(img);

    ingredient.getQuantity = () => ingredient.quantity;

    ingredient.setQuantity = value => {
        const nonzeroClass = 'nonzero';
        ingredient.quantity = value;
        spanQuantity.innerHTML = value;
        if (value === 0)
        {
            if (div.classList.contains(nonzeroClass)) {
                div.classList.remove(nonzeroClass);
            }
        } else {
            if (!div.classList.contains(nonzeroClass)) {
                div.classList.add(nonzeroClass);
            }
        }
    }

    ingredient.reset = () => ingredient.remaining = ingredient.quantity;

    ingredient.setQuantity(0);
    ingredient.reset();

    document.getElementById('allIngredientsDiv').appendChild(div);
}

function ingredientOnClick(ingredient, delta, button) {
    switch (button)
    {
        case 0: // left
            var quantity = ingredient.getQuantity() + delta;
            if (quantity >= 0 && quantity <= 9) {
                ingredient.setQuantity(quantity);
                calculateRecipes();
            }
            break;

        case 2: // right
            break;
    }
}

function prepareRecipe(recipe) {
    recipe.imgs = [];

    recipe.ingredients = recipe.ingredients.map(name =>
        allIngredients.find(ingredient => ingredient.name === name)
    );

    recipe.createImg = () => {
        var img = document.createElement('img');
        img.classList.add('unlock');
        img.src = recipe.isUnlocked ? 'soup.png' : 'blank.png';
        img.onclick = ev => {
            recipe.setIsUnlocked(!recipe.isUnlocked);
        };
        recipe.imgs.push(img);
        return img;
    };
    
    recipe.getAbundance = () => recipe.ingredients
        .map(ingredient => ingredient.getQuantity())
        .map(quantity => quantity * quantity)
        .reduce((a, b) => a + b);
    
    recipe.canConsume = () => {
        if (recipe.ingredients[0] === recipe.ingredients[1]) {
            return recipe.ingredients[0].remaining >= 2;
        } else {
            return recipe.ingredients[0].remaining >= 1
                && recipe.ingredients[1].remaining >= 1;
        }
    };
    
    recipe.consume = () => { recipe.ingredients.forEach(ingredient => { ingredient.remaining--; }); };

    recipe.revert = () => { recipe.ingredients.forEach(ingredient => { ingredient.remaining++; }); };

    recipe.setIsUnlocked = (value) => {
        recipe.isUnlocked = value;
        window.localStorage.setItem(recipe.name, value ? 1 : 0);
        recipe.imgs.forEach(img => {
            img.src = value ? 'soup.png' : 'blank.png';
        });
        calculateRecipes();
    };

    recipe.lock = () => { recipe.setIsUnlocked(false); }

    recipe.unlock = () => { recipe.setIsUnlocked(true); }

    recipe.setIsUnlocked(window.localStorage.getItem(recipe.name) == 1);
}

function prepareUnlocks() {
    var span = document.createElement('span');
    span.innerHTML = 'X';
    span.onclick = hideUnlocks;

    var td = document.createElement('td');
    td.classList.add('button');
    td.classList.add('unlock');
    td.onclick = hideUnlocks;
    td.appendChild(span);

    var tr = document.createElement('tr');
    tr.classList.add('tableHeader');
    tr.classList.add('unlock');
    tr.appendChild(td);
    allIngredients.forEach(ingredient => tr.appendChild(createUnlockHeaderCell(ingredient)));

    var table = document.getElementById('unlockTable');
    table.appendChild(tr);

    allIngredients.forEach(ingredientY => {
        var tr = document.createElement('tr');
        tr.classList.add('unlock');
        tr.appendChild(createUnlockHeaderCell(ingredientY));

        allIngredients.forEach(ingredientX => {
            var recipe = allRecipes.find(r => 
                (r.ingredients[0] === ingredientY && r.ingredients[1] === ingredientX) ||
                (r.ingredients[1] === ingredientY && r.ingredients[0] === ingredientX)
            );
            tr.appendChild(createUnlockRecipeCell(recipe));
        });
        table.appendChild(tr);
    });
}

function createUnlockHeaderCell(ingredient) {
    var img = document.createElement('img');
    img.classList.add('unlock');
    img.src = ingredient.src;
    var td = document.createElement('td');
    td.classList.add('unlock');
    td.appendChild(img);
    return td;
}

function createUnlockRecipeCell(recipe) {
    var td = document.createElement('td');
    td.classList.add('unlock');
    td.appendChild(recipe.createImg());
    td.onmouseenter = ev => enterUnlockRecipe(td);
    td.onmouseleave = ev => leaveUnlockRecipe(td);

    return td;
}

function enterUnlockRecipe(td) {
    [td, getColumnHeader(td), getRowHeader(td)]
        .map(node => node.classList)
        .forEach(classList => classList.add('bright'));
}

function leaveUnlockRecipe(td) {
    [td, getColumnHeader(td), getRowHeader(td)]
        .map(node => node.classList)
        .forEach(classList => classList.remove('bright'));
}

function getRowHeader(td) {
    return td.parentNode.getElementsByTagName('td')[0];
}

function getColumnHeader(td) {
    var tr = td.parentNode;
    var index = Array.from(tr.children).indexOf(td);
    return tr.parentNode.getElementsByTagName('tr')[0].children[index];
}

function addUnlockButton() {
    var spanUnlock = document.createElement('span');
    spanUnlock.innerHTML = 'Recipe Book';
    spanUnlock.onclick = showUnlocks;

    var div = document.createElement('div');
    div.classList.add('button');
    div.classList.add('ingredient');
    div.onclick = showUnlocks;
    div.appendChild(spanUnlock);

    document.getElementById('allIngredientsDiv').appendChild(div);
}

function showUnlocks() {
    document.getElementById('calculatorDiv').classList.add('hide');
    document.getElementById('unlockDiv').classList.remove('hide');
}

function hideUnlocks() {
    document.getElementById('calculatorDiv').classList.remove('hide');
    document.getElementById('unlockDiv').classList.add('hide');
}

function calculateRecipes() {
    if (window.onload) { return; }

    allIngredients.forEach(ingredient => ingredient.reset());

    var div = document.getElementById('newRecipesDiv');
    div.parentNode.replaceChild(div.cloneNode(false), div);

    var div = document.getElementById('bestRecipesDiv');
    div.parentNode.replaceChild(div.cloneNode(false), div);
    
    var possibleRecipes = allRecipes
        .filter(recipe => recipe.canConsume());

    calculateNewRecipes(possibleRecipes);
    calculateBestRecipes(possibleRecipes);
}

function calculateNewRecipes(possibleRecipes) {
    possibleRecipes
        .filter(recipe => !recipe.isUnlocked)
        .sort(byAbundance)
        .slice(0, 7)
        .forEach(createNewRecipe);
}

function byAbundance(recipeA, recipeB) {
    return recipeB.getAbundance() - recipeA.getAbundance()
}

function createNewRecipe(recipe) {
    var img1 = document.createElement('img');
    img1.classList.add('ingredient');
    img1.src = recipe.ingredients[0].name + '.png';
    img1.onclick = recipe.unlock;

    var img2 = document.createElement('img');
    img2.classList.add('ingredient');
    img2.src = recipe.ingredients[1].name + '.png';
    img2.onclick = recipe.unlock;

    var div = document.createElement('div');
    div.classList.add('newRecipe');
    div.appendChild(img1);
    div.appendChild(img2);

    document.getElementById('newRecipesDiv').appendChild(div);
}

function calculateBestRecipes(possibleRecipes) {
    possibleRecipes = possibleRecipes
        .filter(recipe => recipe.isUnlocked)
        .sort(byValueAndAbundance);

    getBestRecipes(possibleRecipes, 0)
        .forEach(createBestRecipe);
}

function byValueAndAbundance(recipeA, recipeB) {
    var result = recipeB.value - recipeA.value;
    if (result !== 0) return result;

    return byAbundance(recipeA, recipeB);
}

function getBestRecipes(recipes, index) {
    var bestRecipes = [];
    var bestValue = 0;
    while (index < recipes.length) {
        var recipe = recipes[index];
        if (recipe.canConsume()) {
            recipe.consume();
            var thisRecipes = getBestRecipes(recipes, index);
            thisRecipes.unshift(recipe);
            var thisValue = thisRecipes.map(r => r.value).reduce((a, b) => a + b);
            if (bestValue < thisValue) {
                bestValue = thisValue;
                bestRecipes = thisRecipes;
            }
            recipe.revert();
        }
        index++;
    }
    return bestRecipes;
}

function createBestRecipe(recipe) {
    var img1 = document.createElement('img');
    img1.classList.add('ingredient');
    img1.src = recipe.ingredients[0].name + '.png';

    var img2 = document.createElement('img');
    img2.classList.add('ingredient');
    img2.src = recipe.ingredients[1].name + '.png';

    var spanValue = document.createElement('span');
    spanValue.classList.add('recipeValue');
    spanValue.innerHTML = recipe.value;

    var spanName = document.createElement('span');
    spanName.classList.add('recipeName');
    spanName.innerHTML = recipe.name

    var div = document.createElement('div');
    div.classList.add('bestRecipe');
    div.appendChild(img1);
    div.appendChild(img2);
    div.appendChild(spanValue);
    div.appendChild(spanName);

    document.getElementById('bestRecipesDiv').appendChild(div);
}
