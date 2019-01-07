using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

public class walking : MonoBehaviour {

    public class Sandwich
    {
        public string ingredient1;
        public string ingredient2;
        public string ingredient3;
        public string ingredient4;

        public bool isSandwich;
        public bool isEdible;
    }

	public GameObject outsideArm;
	public GameObject insideArm;
	public GameObject outsideLeg;
	public GameObject insideLeg;

    public SpriteRenderer box;

    public bool incomingItem;

	private bool outsideArmMovingForward = true;
	private bool insideArmMovingForward = false;
	private bool outsideLegMovingForward = false;
	private bool insideLegMovingForward = true;

    public float rotationLimit = .1f;
    public float rotationShiftSize = .000005f;
    public float neutralRotationLimit = .01f;

    public Vector3 boxOrigin;
    public float boxMovementSpeed = .75f;

    private bool waitingForAnswer = false;

    public GameObject backgroundSymbol;

    public Canvas choiceCanvas;
    public Canvas gameOverCanvas;

    public UnityEngine.UI.Text choice1;
    public UnityEngine.UI.Text choice2;
    public UnityEngine.UI.Text choice3;
    public UnityEngine.UI.Text choice4;

    public UnityEngine.UI.Text healthText;
    public UnityEngine.UI.Text distanceText;
    private float distance = 0f;
    private float walkDistance = .1f;
    public UnityEngine.UI.Text smugnessText;
    private float smugness = 0f;

    public UnityEngine.UI.Text choiceText;

    private string choice = "";

    private StreamReader reader;

    public List<Sandwich> ingredientList = new List<Sandwich>();

    private float maxHealth = 100f;
    private float currentHealth = 100f;
    private float lostHealthPerStep = .25f;

    /*private float eatEdibleSandwich = 20f;
    private float eatNotEdibleSandwich = 5f;
    private float eatEdibleNotSandwich = -5f;
    private float eatNotEdibleNotSandwich = -20f;

    private float ignoreEdibleSandwich = -20f;
    private float ignoreNotEdibleSandwich = -5f;
    private float ignoreEdibleNotSandwich = 0f;
    private float ignoreNotEdibleNotSandwich = 0f;*/

    private bool gameOver = false;

    private bool currentSandwichStatus = false;
    private bool currentEdibleStatus = false;
    private bool singleIngredient = false;

    private string currentIngredient1 = "";
    private string currentIngredient2 = "";
    private string currentIngredient3 = "";
    private string currentIngredient4 = "";

    private float ateSandwich = 5f;
    private float ateNotSandwich = -5f;

    private float ateEdible = 15f;
    private float ateNotEdible = -5f;

    public void backMenu()
    {
       SceneManager.LoadSceneAsync(0);
    }

    public void eatTheSandwich()
    {
        choiceCanvas.gameObject.SetActive(false);
        incomingItem = false;
        backgroundSymbol.transform.SetPositionAndRotation(backgroundSymbol.transform.position, new Quaternion(backgroundSymbol.transform.rotation.x, 90, backgroundSymbol.transform.rotation.z, backgroundSymbol.transform.rotation.w));
        box.enabled = false;
        waitingForAnswer = false;
        choice = "do";

        // Train Jam 2018 says _ is _ and _ (Status)
        if (currentEdibleStatus && currentSandwichStatus) // edible and sandwich
        {
            currentHealth += ateEdible + 10f;
            smugness += ateSandwich;
            if (singleIngredient)
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + " is a sandwich they would eat. (+" + ateEdible + " health, + " + ateSandwich + " smugness)";
            }
            else // multi ingredients
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + ", " + currentIngredient2 + ", " + currentIngredient3 + ", and " + currentIngredient4 + " is a sandwich they would eat. (+" + ateEdible + " health, +" + ateSandwich + " smugness)";
            }
        }
        else if (!currentEdibleStatus && currentSandwichStatus) // not edible but sandwich
        {
            currentHealth += ateNotEdible;
            smugness += ateSandwich;

            if (singleIngredient)
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + " is a sandwich they would not eat. (" + ateNotEdible + " health, +" + ateSandwich + " smugness)";
            }
            else // multi ingredients
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + ", " + currentIngredient2 + ", " + currentIngredient3 + ", and " + currentIngredient4 + " is a sandwich they would not eat. (" + ateNotEdible + " health, +" + ateSandwich + " smugness)";
            }

        }
        else if (currentEdibleStatus && !currentSandwichStatus) // edible but not sandwich
        {
            currentHealth += ateEdible;
            smugness += ateNotSandwich;

            if (singleIngredient)
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + " is not a sandwich, but they would eat it. (+" + ateEdible + " health, " + ateNotSandwich + " smugness)";
            }
            else // multi ingredients
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + ", " + currentIngredient2 + ", " + currentIngredient3 + ", and " + currentIngredient4 + " is not a sandwich, but they would eat it. (+" + ateEdible + " health, " + ateNotSandwich + " smugness)";
            }
        }
        else  // not edible and not sandwich
        {
            currentHealth += ateNotEdible;
            smugness += ateNotSandwich;

            if (singleIngredient)
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + " is not a sandwich, and they would not eat it. (" + ateNotEdible + " health, " + ateNotSandwich + " smugness)";
            }
            else // multi ingredients
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + ", " + currentIngredient2 + ", " + currentIngredient3 + ", and " + currentIngredient4 + " is not a sandwich, and they would not eat it. (" + ateNotEdible + " health, " + ateNotSandwich + " smugness)";
            }
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (currentHealth <= 0)
        {
            gameOver = true;
        }

        // if health falls out game over
    }

    public void doNotEatTheSandwich()
    {
        choiceCanvas.gameObject.SetActive(false);
        incomingItem = false;
        backgroundSymbol.transform.SetPositionAndRotation(backgroundSymbol.transform.position, new Quaternion(backgroundSymbol.transform.rotation.x, 90, backgroundSymbol.transform.rotation.z, backgroundSymbol.transform.rotation.w));
        waitingForAnswer = false;
        choice = "not";

        if (currentEdibleStatus && currentSandwichStatus) // edible and sandwich
        {
            smugness += ateNotSandwich;

            if (singleIngredient)
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + " is a sandwich they would eat. (+ 0 health, " + ateNotSandwich + " smugness)";
            }
            else // multi ingredients
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + ", " + currentIngredient2 + ", " + currentIngredient3 + ", and " + currentIngredient4 + " is a sandwich they would eat. (+ 0 health, " + ateNotSandwich + " smugness)";
            }
        }
        else if (!currentEdibleStatus && currentSandwichStatus) // not edible but sandwich
        {
            smugness += ateNotSandwich;

            if (singleIngredient)
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + " is a sandwich they would not eat. (+ 0 health, " + ateNotSandwich + " smugness)";
            }
            else // multi ingredients
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + ", " + currentIngredient2 + ", " + currentIngredient3 + ", and " + currentIngredient4 + " is a sandwich they would not eat. (+ 0 health, " + ateNotSandwich + " smugness)";
            }
        }
        else if (currentEdibleStatus && !currentSandwichStatus) // edible but not sandwich
        {
            

            if (singleIngredient)
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + " is not a sandwich, but they would eat it. (+ 0 health, + 0 smugness)";
            }
            else // multi ingredients
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + ", " + currentIngredient2 + ", " + currentIngredient3 + ", and " + currentIngredient4 + " is not a sandwich, but they would eat it. (+ 0 health, + 0 smugness)";
            }
        }
        else  // not edible and not sandwich
        {
            

            if (singleIngredient)
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + " is not a sandwich, and they would not eat it. (+ 0 health, + 0 smugness)";
            }
            else // multi ingredients
            {
                choiceText.text = "Train Jam 2018 says " + currentIngredient1 + ", " + currentIngredient2 + ", " + currentIngredient3 + ", and " + currentIngredient4 + " is not a sandwich, and they would not eat it. (+ 0 health, + 0 smugness)";
            }
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (currentHealth <= 0)
        {
            gameOver = true;
        }

        // if health falls out game over
    }

    public void restartGame()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void readInSandwiches()
    {

		string result = "\r\nS31\r\nCheeseburger\r\nCheeseburger\r\nCheeseburger\r\nCheeseburger\r\nYES\r\nYES\r\nS16\r\nGround Beef\r\nBell Pepper\r\nLasagna\r\nPancake\r\nYES\r\nNO\r\nS24\r\nMashed Potatoes\r\nFlour Tortilla\r\nOlives\r\nShredded Cheese\r\nYES\r\nYES\r\nS99\r\nCherry Pie\r\nRed Snapper\r\nRefried Beans\r\nAlmond Butter\r\nNO\r\nNO\r\nS32\r\nCherry Pie\r\nLettuce\r\nSnap Peas\r\nSalt\r\nYES\r\nNO\r\nS40\r\nSliced Cheese\r\nCoconut Oil\r\nRice Krispies Bar\r\nCorn on the Cob\r\nNO\r\nNO\r\nS17\r\nBurrito\r\nBurrito\r\nBurrito\r\nBurrito\r\nYES\r\nYES\r\nS25\r\nSliced Bread\r\nBagel\r\nTomato\r\nKale\r\nYES\r\nNO\r\nS33\r\nMushroom\r\nBagel\r\nMuscles\r\nSliced Beets\r\nYES\r\nNO\r\nS18\r\nCheese Omelette\r\nCheese Omelette\r\nCheese Omelette\r\nCheese Omelette\r\nYES\r\nYES\r\nS41\r\nOreo Cookies\r\nLettuce\r\nBleached Flour\r\nCoffee Cake\r\nNO\r\nNO\r\nS26\r\nSliced Deli Meat\r\nPancake\r\nAlmonds\r\nCupcake\r\nYES\r\nNO\r\nS34\r\nCake\r\nKale\r\nButter\r\nOreo Cookies\r\nNO\r\nYES\r\nS19\r\nCheetos\r\nCheetos\r\nEggplant\r\nLettuce\r\nNO\r\nNO\r\nS42\r\nTomato\r\nSliced Bread\r\nBaked Potato\r\nSpaghetti Noodles\r\nYES\r\nNO\r\nS27\r\nMashed Potatoes\r\nBell Pepper\r\nEggplant\r\nVegemite\r\nNO\r\nNO\r\nS50\r\nMuscles\r\nCoconut Oil\r\nZucchini\r\nRaw Egg\r\nNO\r\nNO\r\nS35\r\nBrussels Sprout\r\nBlack Beans\r\nOlive Oil\r\nPaprika\r\nNO\r\nNO\r\nS100\r\nClams\r\nBell Pepper\r\nFrench Fries\r\nCookies\r\nNO\r\nNO\r\nS43\r\nCrackers\r\nAlmond Milk\r\nCheeseburger\r\nTahini\r\nNO\r\nNO\r\nS28\r\nCake\r\nFrench Toast\r\nPulled Pork\r\nMelted Cheese\r\nYES\r\nNO\r\nS51\r\nSliced Cheese\r\nSliced Bread\r\nChili Powder\r\nArtichoke Heart\r\nYES\r\nYES\r\nS36\r\nMashed Potatoes\r\nPancake\r\nGoat Cheese\r\nChicken Nuggets\r\nYES\r\nYES\r\nS44\r\nBowtie Pasta\r\nAlmond Butter\r\nOnions\r\nRefried Beans\r\nNO\r\nNO\r\nS29\r\nBagel\r\nBagel\r\nBagel\r\nBagel\r\nYES\r\nYES\r\nS52\r\nBagel\r\nBagel\r\nBagel\r\nBagel\r\nYES\r\nYES\r\nS37\r\nShredded Chicken\r\nLettuce\r\nOreo Cookies\r\nTaco\r\nNO\r\nNO\r\nS60\r\nPinto Beans\r\nCoconut Oil\r\nCorn on the Cob\r\nRoasted Potatoes\r\nNO\r\nYES\r\nS101\r\nPeanut Butter\r\nChocolate Bar\r\nKale\r\nSoy Sauce\r\nYES\r\nNO\r\nS45\r\nSliced Deli Meat\r\nIce Cream\r\nHam and Cheese Omelette\r\nMelted Cheese\r\nNO\r\nNO\r\nS53\r\nHam and Cheese Omelette\r\nHam and Cheese Omelette\r\nHam and Cheese Omelette\r\nHam and Cheese Omelette\r\nYES\r\nYES\r\nS38\r\nPotato Chips\r\nPeanut Butter\r\nGrilled Sausage\r\nBaguette\r\nYES\r\nYES\r\nS61\r\nBagel\r\nMushroom\r\nGarlic Powder\r\nTaco\r\nYES\r\nYES\r\nS46\r\nSalt\r\nBell Pepper\r\nBanana\r\nS'Mores\r\nNO\r\nYES\r\nS54\r\nKale\r\nChicken Wing\r\nSalmon\r\nCanned Tuna\r\nNO\r\nNO\r\nS102\r\nMustard\r\nFlour Tortilla\r\nClams\r\nChocolate Chip Cookies\r\nYES\r\nNO\r\nS39\r\nSliced Deli Meat\r\nRoasted Potatoes\r\nEgg Whites\r\nDuck Breast\r\nNO\r\nYES\r\nS62\r\nKale\r\nBrussels Sprout\r\nOysters\r\nMashed Potatoes\r\nNO\r\nNO\r\nS47\r\nSoy Sauce\r\nBlack Beans\r\nAlmond Milk\r\nClams\r\nNO\r\nYES\r\nS70\r\nSnap Peas\r\nWaffle\r\nCroissant\r\nCorn on the Cob\r\nNO\r\nYES\r\nS55\r\nEgg Whites\r\nBagel\r\nSliced Cheese\r\nChili Powder\r\nYES\r\nYES\r\nS63\r\nChicken Salad\r\nSliced Cheese\r\nEggplant\r\nClams\r\nYES\r\nYES\r\nS48\r\nOlives\r\nPepper\r\nRed Snapper\r\nRefried Beans\r\nNO\r\nYES\r\nS71\r\nPinto Beans\r\nCorn Tortilla\r\nRaw Egg\r\nCarrots\r\nNO\r\nNO\r\nS56\r\nBell Pepper\r\nLettuce\r\nAnchovies\r\nPinto Beans\r\nYES\r\nNO\r\nS103\r\nAnchovies\r\nPie Crust\r\nMushroom\r\nSliced Cheese\r\nYES\r\nNO\r\nS64\r\nKale\r\nSliced Bread\r\nBalsamic Vinegar\r\nPumpkin Pie\r\nYES\r\nNO\r\nS49\r\nSriracha\r\nCanned Tuna\r\nBanana\r\nRaw Onion\r\nNO\r\nNO\r\nS72\r\nKidney Beans\r\nPeaches\r\nPumpkin\r\nCashews\r\nNO\r\nNO\r\nS80\r\nFrench Toast\r\nEnglish Muffin\r\nZucchini\r\nPoached Egg\r\nYES\r\nYES\r\nS57\r\nPie Crust\r\nToast\r\nSliced Deli Meat\r\nPancake\r\nYES\r\nYES\r\nS65\r\nSliced Cheese\r\nEnglish Muffin\r\nJalepeno Peppers\r\nAlmonds\r\nYES\r\nYES\r\nS73\r\nSpinach\r\nSliced Bread\r\nOreo Cookies\r\nChocolate Bar\r\nYES\r\nYES\r\nS81\r\nBanana\r\nEnglish Muffin\r\nHoney Mustard\r\nCream Cheese\r\nYES\r\nNO\r\nS58\r\nDoritos\r\nBagel\r\nCrepe with Butter and Jam\r\nAlmond Milk\r\nYES\r\nNO\r\nS104\r\nRaw Egg\r\nSalt\r\nPeanut Butter\r\nSummer Sausage\r\nNO\r\nNO\r\nS66\r\nApple Pie\r\nGround Beef\r\nPie Crust\r\nCrepe with Butter and Jam\r\nNO\r\nNO\r\nS74\r\nTahini\r\nDates\r\nBaked Potato\r\nAlmond Butter\r\nNO\r\nNO\r\nS82\r\nBlack Beans\r\nRavioli\r\nCookies\r\nGoat Cheese\r\nNO\r\nYES\r\nS59\r\nPie Crust\r\nPie Crust\r\nPie Crust\r\nPie Crust\r\nYES\r\nYES\r\nS67\r\nFeta Cheese\r\nChicken Breast\r\nCrackers\r\nFried Potatoes\r\nNO\r\nYES\r\nS90\r\nShredded Chicken\r\nCream Cheese\r\nPinto Beans\r\nZucchini\r\nNO\r\nNO\r\nS75\r\nCheeseburger\r\nCheeseburger\r\nCheeseburger\r\nCheeseburger\r\nYES\r\nYES\r\nS105\r\nChicken Wings\r\nBacon Wrapped Dates\r\nShredded Chicken\r\nFrench Toast\r\nYES\r\nYES\r\nS83\r\nPeanut Butter\r\nFlour Tortilla\r\nMayonaise\r\nCorn Tortilla\r\nNO\r\nNO\r\nS68\r\nPinto Beans\r\nChocolate Bar\r\nAlmonds\r\nGrilled Sausage\r\nNO\r\nNO\r\nS91\r\nCoconut Oil\r\nFeta Cheese\r\nChicken Salad\r\nS'Mores\r\nNO\r\nNO\r\nS76\r\nSpinach\r\nArtichoke Heart\r\nCayenne\r\nCorn Meal\r\nNO\r\nNO\r\nS84\r\nRefried Beans\r\nRed Snapper\r\nOreo Cookies\r\nMuscles\r\nNO\r\nNO\r\nS69\r\nShredded Lettuce\r\nGreen Beans\r\nCabbage\r\nAnchovies\r\nNO\r\nYES\r\nS92\r\nSliced Deli Meat\r\nTomato\r\nOlives\r\nOrange\r\nYES\r\nYES\r\nS77\r\nBanana\r\nFlour Tortilla\r\nLettuce\r\nSoy Sauce\r\nYES\r\nNO\r\nS10\r\nRibs\r\nPepper\r\nBagel\r\nOrange\r\nYES\r\nYES\r\nS85\r\nSnap Peas\r\nCorn Tortilla\r\nRavioli\r\nRoast Beef\r\nNO\r\nYES\r\nS1\r\nMustard\r\nLettuce\r\nRoast Beef\r\nOnion\r\nYES\r\nYES\r\nS93\r\nFoie Gras\r\nBiscuits and Gravy\r\nChocolate Chip Cookies\r\nStrawberry Jam\r\nYES\r\nNO\r\nS78\r\nMushroom\r\nHoney\r\nSummer Sausage\r\nMarshmellows\r\nNO\r\nYES\r\nS2\r\nAioli\r\nLettuce\r\nCrackers\r\nCorn Meal\r\nYES\r\nNO\r\nS11\r\nReese's Peanut Butter Cups\r\nPinto Beans\r\nMustard\r\nTahini\r\nNO\r\nNO\r\nS86\r\nTahini\r\nCorn Tortilla\r\nPotato Chips\r\nCake\r\nNO\r\nNO\r\nS3\r\nSalt\r\nSliced Cheese\r\nSoy Sauce\r\nPinto Beans\r\nNO\r\nNO\r\nS94\r\nMaple Syrup\r\nWaffle\r\nChicken Salad\r\nRefried Beans\r\nYES\r\nNO\r\nS79\r\nEggplant\r\nBaguette\r\nShredded Pork\r\nRavioli\r\nYES\r\nYES\r\nS4\r\nSliced Beets\r\nPeaches\r\nPie Crust\r\nDoritos\r\nNO\r\nNO\r\nS12\r\nSnap Peas\r\nRoasted Potatoes\r\nSliced Beets\r\nCookies\r\nNO\r\nNO\r\nS87\r\nHot Pocket\r\nCaviar\r\nKidney Beans\r\nAlmond Milk\r\nNO\r\nNO\r\nS20\r\nChicken Wing\r\nCabbage\r\nCanned Tuna\r\nSnap Peas\r\nNO\r\nNO\r\nS5\r\nRaw Egg\r\nLettuce\r\nHam\r\nBiscuits and Gravy\r\nNO\r\nNO\r\nS95\r\nCoffee Cake\r\nS'Mores\r\nBagel\r\nSriracha\r\nNO\r\nNO\r\nS6\r\nPepper\r\nSliced Bread\r\nStrawberry Jam\r\nCauliflower\r\nYES\r\nNO\r\nS13\r\nEgg Whites\r\nRelish\r\nCupcake\r\nSliced Cheese\r\nNO\r\nNO\r\nS88\r\nRibs\r\nCanned Chicken\r\nCrackers\r\nMushroom\r\nNO\r\nYES\r\nS21\r\nPumpkin Pie\r\nCorn Tortilla\r\nTuna Salad\r\nOnions\r\nYES\r\nNO\r\nS96\r\nPumpkin\r\nBagel\r\nHard Boiled Egg\r\nBanana\r\nYES\r\nNO\r\nS7\r\nSliced Deli Meat\r\nDates\r\nOreo Cookies\r\nOrange\r\nYES\r\nNO\r\nS14\r\nKetchup\r\nPancake\r\nPinto Beans\r\nCorn Chips\r\nYES\r\nNO\r\nS8\r\nKale\r\nLettuce\r\nCooked Onion\r\nAioli\r\nYES\r\nYES\r\nS89\r\nPinto Beans\r\nPeaches\r\nSmoked Salmon\r\nOatmeal\r\nNO\r\nNO\r\nS22\r\nStrawberry Jam\r\nStrawberry Jam\r\nCream Cheese\r\nChocolate Chip Cookies\r\nYES\r\nYES\r\nS97\r\nRed Snapper\r\nRefried Beans\r\nChocolate Chip Cookies\r\nBrussels Sprout\r\nNO\r\nNO\r\nS9\r\nCream Cheese\r\nPoached Egg\r\nArtichoke Heart\r\nBurrito\r\nNO\r\nNO\r\nS30\r\nRavioli\r\nRavioli\r\nRavioli\r\nRavioli\r\nYES\r\nYES\r\nS15\r\nTomato\r\nChicken Wing\r\nGreen Beans\r\nCrustless Quiche\r\nNO\r\nNO\r\nS23\r\nOysters\r\nBagel\r\nCheeseburger\r\nMarinara Sauce\r\nYES\r\nNO\r\nS98\r\nOreo Cookies\r\nRavioli\r\nRibs\r\nCaviar\r\nNO\r\nNO\r\nS22\r\nBagel\r\nStrawberry Jam\r\nCream Cheese\r\nChocolate Chip Cookies\r\nYES\r\nYES\r\nS41\r\nOreo Cookies\r\nLettuce\r\nBleached Flour\r\nCoffee Cake\r\nNO\r\nNO\r\nS49\r\nSriracha\r\nCanned Tuna\r\nBanana\r\nRaw Onion\r\nNO\r\nNO\r\nS17\r\nBurrito\r\nBurrito\r\nBurrito\r\nBurrito\r\nYES\r\nYES\r\nS36\r\nMashed Potatoes\r\nPancake\r\nGoat Cheese\r\nChicken Nuggets\r\nYES\r\nYES\r\nS23\r\nOysters\r\nBagel\r\nCheeseburger\r\nMarinara Sauce\r\nYES\r\nNO\r\nS42\r\nTomato\r\nSliced Bread\r\nBaked Potato\r\nSpaghetti Noodles\r\nYES\r\nNO\r\nS10\r\nRibs\r\nPepper\r\nBagel\r\nOrange\r\nYES\r\nYES\r\nS18\r\nCheese Omelette\r\nCheese Omelette\r\nCheese Omelette\r\nCheese Omelette\r\nYES\r\nYES\r\nS37\r\nShredded Chicken\r\nLettuce\r\nOreo Cookies\r\nTaco\r\nNO\r\nNO\r\nS24\r\nMashed Potatoes\r\nFlour Tortilla\r\nOlives\r\nShredded Cheese\r\nYES\r\nYES\r\nS1\r\nMustard\r\nLettuce\r\nRoast Beef\r\nOnion\r\nYES\r\nYES\r\nS11\r\nReese's Peanut Butter Cups\r\nPinto Beans\r\nMustard\r\nTahini\r\nNO\r\nNO\r\nS19\r\nCheetos\r\nCheetos\r\nEggplant\r\nLettuce\r\nNO\r\nNO\r\nS30\r\nRavioli\r\nRavioli\r\nRavioli\r\nRavioli\r\nYES\r\nYES\r\nS38\r\nPotato Chips\r\nPeanut Butter\r\nGrilled Sausage\r\nBaguette\r\nYES\r\nYES\r\nS2\r\nAioli\r\nLettuce\r\nCrackers\r\nCorn Meal\r\nYES\r\nNO\r\nS43\r\nCrackers\r\nAlmond Milk\r\nCheeseburger\r\nTahini\r\nNO\r\nNO\r\nS25\r\nSliced Bread\r\nBagel\r\nTomato\r\nKale\r\nYES\r\nNO\r\nS44\r\nBowtie Pasta\r\nAlmond Butter\r\nOnions\r\nRefried Beans\r\nNO\r\nNO\r\nS3\r\nSalt\r\nSliced Cheese\r\nSoy Sauce\r\nPinto Beans\r\nNO\r\nNO\r\nS12\r\nSnap Peas\r\nRoasted Potatoes\r\nSliced Beets\r\nCookies\r\nNO\r\nNO\r\nS31\r\nCheeseburger\r\nCheeseburger\r\nCheeseburger\r\nCheeseburger\r\nYES\r\nYES\r\nS39\r\nSliced Deli Meat\r\nRoasted Potatoes\r\nEgg Whites\r\nDuck Breast\r\nNO\r\nYES\r\nS50\r\nMuscles\r\nCoconut Oil\r\nZucchini\r\nRaw Egg\r\nNO\r\nNO\r\nS4\r\nSliced Beets\r\nPeaches\r\nPie Crust\r\nDoritos\r\nNO\r\nNO\r\nS26\r\nSliced Deli Meat\r\nPancake\r\nAlmonds\r\nCupcake\r\nYES\r\nNO\r\nS45\r\nSliced Deli Meat\r\nIce Cream\r\nHam and Cheese Omelette\r\nMelted Cheese\r\nNO\r\nNO\r\nS13\r\nEgg Whites\r\nRelish\r\nCupcake\r\nSliced Cheese\r\nNO\r\nNO\r\nS5\r\nRaw Egg\r\nLettuce\r\nHam\r\nBiscuits and Gravy\r\nNO\r\nNO\r\nS32\r\nCherry Pie\r\nLettuce\r\nSnap Peas\r\nSalt\r\nYES\r\nNO\r\nS51\r\nSliced Cheese\r\nSliced Bread\r\nChili Powder\r\nArtichoke Heart\r\nYES\r\nYES\r\nS6\r\nPepper\r\nSliced Bread\r\nStrawberry Jam\r\nCauliflower\r\nYES\r\nNO\r\nS27\r\nMashed Potatoes\r\nBell Pepper\r\nEggplant\r\nVegemite\r\nNO\r\nNO\r\nS46\r\nSalt\r\nBell Pepper\r\nBanana\r\nS'Mores\r\nNO\r\nYES\r\nS14\r\nKetchup\r\nPancake\r\nPinto Beans\r\nCorn Chips\r\nYES\r\nNO\r\nS7\r\nSliced Deli Meat\r\nDates\r\nOreo Cookies\r\nOrange\r\nYES\r\nNO\r\nS33\r\nMushroom\r\nBagel\r\nMuscles\r\nSliced Beets\r\nYES\r\nNO\r\nS20\r\nChicken Wing\r\nCabbage\r\nCanned Tuna\r\nSnap Peas\r\nNO\r\nNO\r\nS28\r\nCake\r\nFrench Toast\r\nPulled Pork\r\nMelted Cheese\r\nYES\r\nNO\r\nS8\r\nKale\r\nLettuce\r\nCooked Onion\r\nAioli\r\nYES\r\nYES\r\nS47\r\nSoy Sauce\r\nBlack Beans\r\nAlmond Milk\r\nClams\r\nNO\r\nYES\r\nS15\r\nTomato\r\nChicken Wing\r\nGreen Beans\r\nCrustless Quiche\r\nNO\r\nNO\r\nS34\r\nCake\r\nKale\r\nButter\r\nOreo Cookies\r\nNO\r\nYES\r\nS9\r\nCream Cheese\r\nPoached Egg\r\nArtichoke Heart\r\nBurrito\r\nNO\r\nNO\r\nS21\r\nPumpkin Pie\r\nCorn Tortilla\r\nTuna Salad\r\nOnions\r\nYES\r\nNO\r\nS29\r\nBagel\r\nBagel\r\nBagel\r\nBagel\r\nYES\r\nYES\r\nS40\r\nSliced Cheese\r\nCoconut Oil\r\nRice Krispies Bar\r\nCorn on the Cob\r\nNO\r\nNO\r\nS48\r\nOlives\r\nPepper\r\nRed Snapper\r\nRefried Beans\r\nNO\r\nYES\r\nS16\r\nGround Beef\r\nBell Pepper\r\nLasagna\r\nPancake\r\nYES\r\nNO\r\nS35\r\nBrussels Sprout\r\nBlack Beans\r\nOlive Oil\r\nPaprika\r\nNO\r\nNO\n\r\nS31\r\nCheeseburger\r\nCheeseburger\r\nCheeseburger\r\nCheeseburger\r\nYES\r\nYES\r\nS16\r\nGround Beef\r\nBell Pepper\r\nLasagna\r\nPancake\r\nYES\r\nNO\r\nS24\r\nMashed Potatoes\r\nFlour Tortilla\r\nOlives\r\nShredded Cheese\r\nYES\r\nYES\r\nS99\r\nCherry Pie\r\nRed Snapper\r\nRefried Beans\r\nAlmond Butter\r\nNO\r\nNO\r\nS32\r\nCherry Pie\r\nLettuce\r\nSnap Peas\r\nSalt\r\nYES\r\nNO\r\nS40\r\nSliced Cheese\r\nCoconut Oil\r\nRice Krispies Bar\r\nCorn on the Cob\r\nNO\r\nNO\r\nS17\r\nBurrito\r\nBurrito\r\nBurrito\r\nBurrito\r\nYES\r\nYES\r\nS25\r\nSliced Bread\r\nBagel\r\nTomato\r\nKale\r\nYES\r\nNO\r\nS33\r\nMushroom\r\nBagel\r\nMuscles\r\nSliced Beets\r\nYES\r\nNO\r\nS18\r\nCheese Omelette\r\nCheese Omelette\r\nCheese Omelette\r\nCheese Omelette\r\nYES\r\nYES\r\nS41\r\nOreo Cookies\r\nLettuce\r\nBleached Flour\r\nCoffee Cake\r\nNO\r\nNO\r\nS26\r\nSliced Deli Meat\r\nPancake\r\nAlmonds\r\nCupcake\r\nYES\r\nNO\r\nS34\r\nCake\r\nKale\r\nButter\r\nOreo Cookies\r\nNO\r\nYES\r\nS19\r\nCheetos\r\nCheetos\r\nEggplant\r\nLettuce\r\nNO\r\nNO\r\nS42\r\nTomato\r\nSliced Bread\r\nBaked Potato\r\nSpaghetti Noodles\r\nYES\r\nNO\r\nS27\r\nMashed Potatoes\r\nBell Pepper\r\nEggplant\r\nVegemite\r\nNO\r\nNO\r\nS50\r\nMuscles\r\nCoconut Oil\r\nZucchini\r\nRaw Egg\r\nNO\r\nNO\r\nS35\r\nBrussels Sprout\r\nBlack Beans\r\nOlive Oil\r\nPaprika\r\nNO\r\nNO\r\nS100\r\nClams\r\nBell Pepper\r\nFrench Fries\r\nCookies\r\nNO\r\nNO\r\nS43\r\nCrackers\r\nAlmond Milk\r\nCheeseburger\r\nTahini\r\nNO\r\nNO\r\nS28\r\nCake\r\nFrench Toast\r\nPulled Pork\r\nMelted Cheese\r\nYES\r\nNO\r\nS51\r\nSliced Cheese\r\nSliced Bread\r\nChili Powder\r\nArtichoke Heart\r\nYES\r\nYES\r\nS36\r\nMashed Potatoes\r\nPancake\r\nGoat Cheese\r\nChicken Nuggets\r\nYES\r\nYES\r\nS44\r\nBowtie Pasta\r\nAlmond Butter\r\nOnions\r\nRefried Beans\r\nNO\r\nNO\r\nS29\r\nBagel\r\nBagel\r\nBagel\r\nBagel\r\nYES\r\nYES\r\nS52\r\nBagel\r\nBagel\r\nBagel\r\nBagel\r\nYES\r\nYES\r\nS37\r\nShredded Chicken\r\nLettuce\r\nOreo Cookies\r\nTaco\r\nNO\r\nNO\r\nS60\r\nPinto Beans\r\nCoconut Oil\r\nCorn on the Cob\r\nRoasted Potatoes\r\nNO\r\nYES\r\nS101\r\nPeanut Butter\r\nChocolate Bar\r\nKale\r\nSoy Sauce\r\nYES\r\nNO\r\nS45\r\nSliced Deli Meat\r\nIce Cream\r\nHam and Cheese Omelette\r\nMelted Cheese\r\nNO\r\nNO\r\nS53\r\nHam and Cheese Omelette\r\nHam and Cheese Omelette\r\nHam and Cheese Omelette\r\nHam and Cheese Omelette\r\nYES\r\nYES\r\nS38\r\nPotato Chips\r\nPeanut Butter\r\nGrilled Sausage\r\nBaguette\r\nYES\r\nYES\r\nS61\r\nBagel\r\nMushroom\r\nGarlic Powder\r\nTaco\r\nYES\r\nYES\r\nS46\r\nSalt\r\nBell Pepper\r\nBanana\r\nS'Mores\r\nNO\r\nYES\r\nS54\r\nKale\r\nChicken Wing\r\nSalmon\r\nCanned Tuna\r\nNO\r\nNO\r\nS102\r\nMustard\r\nFlour Tortilla\r\nClams\r\nChocolate Chip Cookies\r\nYES\r\nNO\r\nS39\r\nSliced Deli Meat\r\nRoasted Potatoes\r\nEgg Whites\r\nDuck Breast\r\nNO\r\nYES\r\nS62\r\nKale\r\nBrussels Sprout\r\nOysters\r\nMashed Potatoes\r\nNO\r\nNO\r\nS47\r\nSoy Sauce\r\nBlack Beans\r\nAlmond Milk\r\nClams\r\nNO\r\nYES\r\nS70\r\nSnap Peas\r\nWaffle\r\nCroissant\r\nCorn on the Cob\r\nNO\r\nYES\r\nS55\r\nEgg Whites\r\nBagel\r\nSliced Cheese\r\nChili Powder\r\nYES\r\nYES\r\nS63\r\nChicken Salad\r\nSliced Cheese\r\nEggplant\r\nClams\r\nYES\r\nYES\r\nS48\r\nOlives\r\nPepper\r\nRed Snapper\r\nRefried Beans\r\nNO\r\nYES\r\nS71\r\nPinto Beans\r\nCorn Tortilla\r\nRaw Egg\r\nCarrots\r\nNO\r\nNO\r\nS56\r\nBell Pepper\r\nLettuce\r\nAnchovies\r\nPinto Beans\r\nYES\r\nNO\r\nS103\r\nAnchovies\r\nPie Crust\r\nMushroom\r\nSliced Cheese\r\nYES\r\nNO\r\nS64\r\nKale\r\nSliced Bread\r\nBalsamic Vinegar\r\nPumpkin Pie\r\nYES\r\nNO\r\nS49\r\nSriracha\r\nCanned Tuna\r\nBanana\r\nRaw Onion\r\nNO\r\nNO\r\nS72\r\nKidney Beans\r\nPeaches\r\nPumpkin\r\nCashews\r\nNO\r\nNO\r\nS80\r\nFrench Toast\r\nEnglish Muffin\r\nZucchini\r\nPoached Egg\r\nYES\r\nYES\r\nS57\r\nPie Crust\r\nToast\r\nSliced Deli Meat\r\nPancake\r\nYES\r\nYES\r\nS65\r\nSliced Cheese\r\nEnglish Muffin\r\nJalepeno Peppers\r\nAlmonds\r\nYES\r\nYES\r\nS73\r\nSpinach\r\nSliced Bread\r\nOreo Cookies\r\nChocolate Bar\r\nYES\r\nYES\r\nS81\r\nBanana\r\nEnglish Muffin\r\nHoney Mustard\r\nCream Cheese\r\nYES\r\nNO\r\nS58\r\nDoritos\r\nBagel\r\nCrepe with Butter and Jam\r\nAlmond Milk\r\nYES\r\nNO\r\nS104\r\nRaw Egg\r\nSalt\r\nPeanut Butter\r\nSummer Sausage\r\nNO\r\nNO\r\nS66\r\nApple Pie\r\nGround Beef\r\nPie Crust\r\nCrepe with Butter and Jam\r\nNO\r\nNO\r\nS74\r\nTahini\r\nDates\r\nBaked Potato\r\nAlmond Butter\r\nNO\r\nNO\r\nS82\r\nBlack Beans\r\nRavioli\r\nCookies\r\nGoat Cheese\r\nNO\r\nYES\r\nS59\r\nPie Crust\r\nPie Crust\r\nPie Crust\r\nPie Crust\r\nYES\r\nYES\r\nS67\r\nFeta Cheese\r\nChicken Breast\r\nCrackers\r\nFried Potatoes\r\nNO\r\nYES\r\nS90\r\nShredded Chicken\r\nCream Cheese\r\nPinto Beans\r\nZucchini\r\nNO\r\nNO\r\nS75\r\nCheeseburger\r\nCheeseburger\r\nCheeseburger\r\nCheeseburger\r\nYES\r\nYES\r\nS105\r\nChicken Wings\r\nBacon Wrapped Dates\r\nShredded Chicken\r\nFrench Toast\r\nYES\r\nYES\r\nS83\r\nPeanut Butter\r\nFlour Tortilla\r\nMayonaise\r\nCorn Tortilla\r\nNO\r\nNO\r\nS68\r\nPinto Beans\r\nChocolate Bar\r\nAlmonds\r\nGrilled Sausage\r\nNO\r\nNO\r\nS91\r\nCoconut Oil\r\nFeta Cheese\r\nChicken Salad\r\nS'Mores\r\nNO\r\nNO\r\nS76\r\nSpinach\r\nArtichoke Heart\r\nCayenne\r\nCorn Meal\r\nNO\r\nNO\r\nS84\r\nRefried Beans\r\nRed Snapper\r\nOreo Cookies\r\nMuscles\r\nNO\r\nNO\r\nS69\r\nShredded Lettuce\r\nGreen Beans\r\nCabbage\r\nAnchovies\r\nNO\r\nYES\r\nS92\r\nSliced Deli Meat\r\nTomato\r\nOlives\r\nOrange\r\nYES\r\nYES\r\nS77\r\nBanana\r\nFlour Tortilla\r\nLettuce\r\nSoy Sauce\r\nYES\r\nNO\r\nS10\r\nRibs\r\nPepper\r\nBagel\r\nOrange\r\nYES\r\nYES\r\nS85\r\nSnap Peas\r\nCorn Tortilla\r\nRavioli\r\nRoast Beef\r\nNO\r\nYES\r\nS1\r\nMustard\r\nLettuce\r\nRoast Beef\r\nOnion\r\nYES\r\nYES\r\nS93\r\nFoie Gras\r\nBiscuits and Gravy\r\nChocolate Chip Cookies\r\nStrawberry Jam\r\nYES\r\nNO\r\nS78\r\nMushroom\r\nHoney\r\nSummer Sausage\r\nMarshmellows\r\nNO\r\nYES\r\nS2\r\nAioli\r\nLettuce\r\nCrackers\r\nCorn Meal\r\nYES\r\nNO\r\nS11\r\nReese's Peanut Butter Cups\r\nPinto Beans\r\nMustard\r\nTahini\r\nNO\r\nNO\r\nS86\r\nTahini\r\nCorn Tortilla\r\nPotato Chips\r\nCake\r\nNO\r\nNO\r\nS3\r\nSalt\r\nSliced Cheese\r\nSoy Sauce\r\nPinto Beans\r\nNO\r\nNO\r\nS94\r\nMaple Syrup\r\nWaffle\r\nChicken Salad\r\nRefried Beans\r\nYES\r\nNO\r\nS79\r\nEggplant\r\nBaguette\r\nShredded Pork\r\nRavioli\r\nYES\r\nYES\r\nS4\r\nSliced Beets\r\nPeaches\r\nPie Crust\r\nDoritos\r\nNO\r\nNO\r\nS12\r\nSnap Peas\r\nRoasted Potatoes\r\nSliced Beets\r\nCookies\r\nNO\r\nNO\r\nS87\r\nHot Pocket\r\nCaviar\r\nKidney Beans\r\nAlmond Milk\r\nNO\r\nNO\r\nS20\r\nChicken Wing\r\nCabbage\r\nCanned Tuna\r\nSnap Peas\r\nNO\r\nNO\r\nS5\r\nRaw Egg\r\nLettuce\r\nHam\r\nBiscuits and Gravy\r\nNO\r\nNO\r\nS95\r\nCoffee Cake\r\nS'Mores\r\nBagel\r\nSriracha\r\nNO\r\nNO\r\nS6\r\nPepper\r\nSliced Bread\r\nStrawberry Jam\r\nCauliflower\r\nYES\r\nNO\r\nS13\r\nEgg Whites\r\nRelish\r\nCupcake\r\nSliced Cheese\r\nNO\r\nNO\r\nS88\r\nRibs\r\nCanned Chicken\r\nCrackers\r\nMushroom\r\nNO\r\nYES\r\nS21\r\nPumpkin Pie\r\nCorn Tortilla\r\nTuna Salad\r\nOnions\r\nYES\r\nNO\r\nS96\r\nPumpkin\r\nBagel\r\nHard Boiled Egg\r\nBanana\r\nYES\r\nNO\r\nS7\r\nSliced Deli Meat\r\nDates\r\nOreo Cookies\r\nOrange\r\nYES\r\nNO\r\nS14\r\nKetchup\r\nPancake\r\nPinto Beans\r\nCorn Chips\r\nYES\r\nNO\r\nS8\r\nKale\r\nLettuce\r\nCooked Onion\r\nAioli\r\nYES\r\nYES\r\nS89\r\nPinto Beans\r\nPeaches\r\nSmoked Salmon\r\nOatmeal\r\nNO\r\nNO\r\nS22\r\nStrawberry Jam\r\nStrawberry Jam\r\nCream Cheese\r\nChocolate Chip Cookies\r\nYES\r\nYES\r\nS97\r\nRed Snapper\r\nRefried Beans\r\nChocolate Chip Cookies\r\nBrussels Sprout\r\nNO\r\nNO\r\nS9\r\nCream Cheese\r\nPoached Egg\r\nArtichoke Heart\r\nBurrito\r\nNO\r\nNO\r\nS30\r\nRavioli\r\nRavioli\r\nRavioli\r\nRavioli\r\nYES\r\nYES\r\nS15\r\nTomato\r\nChicken Wing\r\nGreen Beans\r\nCrustless Quiche\r\nNO\r\nNO\r\nS23\r\nOysters\r\nBagel\r\nCheeseburger\r\nMarinara Sauce\r\nYES\r\nNO\r\nS98\r\nOreo Cookies\r\nRavioli\r\nRibs\r\nCaviar\r\nNO\r\nNO\n\r\nS2\r\nCheeseburger\r\nCheeseburger\r\nCheeseburger\r\nCheeseburger\r\nYES\r\nYES\r\nS12\r\nCherry Pie\r\nBagel\r\nCheeseburger\r\nLasagna\r\nYES\r\nNO\r\nS7\r\nFoie Gras\r\nCake\r\nArtichoke Heart\r\nOnions\r\nNO\r\nNO\r\nS3\r\nCream Cheese\r\nShrimp\r\nGraham Crackers\r\nLasagna\r\nNO\r\nNO\r\nS10\r\nSteak\r\nBiscuits\r\nSmoked Salmon\r\nBiscuits and Gravy\r\nYES\r\nNO\r\nS8\r\nBanana\r\nTaco\r\nS'Mores\r\nIce Cream\r\nYES\r\nNO\r\nS4\r\nOnions\r\nCorn Tortilla\r\nPulled Pork\r\nPeanut Butter\r\nNO\r\nNO\r\nS9\r\nBlack Beans\r\nBell Pepper\r\nGarlic\r\nFeta Cheese\r\nYES\r\nYES\r\nS11\r\nRoasted Potatoes\r\nGoat Cheese\r\nCherry Pie\r\nSliced Cheese\r\nNO\r\nNO\r\nS5\r\nPeaches\r\nBiscuits\r\nGraham Crackers\r\nCanned Tuna\r\nNO\r\nNO\r\nS1\r\nMustard\r\nLettuce\r\nRoast Beef\r\nOnion\r\nYES\r\nYES\r\nS6\r\nAvocado\r\nCanned Tuna\r\nBagel\r\nBaguette\r\nYES\r\nNO";
		byte[] byteArray = Encoding.UTF8.GetBytes(result);
		MemoryStream stream = new MemoryStream(byteArray);

		reader = new StreamReader(stream);


        int i = 0;
        choiceText.text = "";
        while (!reader.EndOfStream)
        {
            string readLine = reader.ReadLine(); // The ID, we don't do anything with it but have it
            if (readLine.Trim() == "")
            {
                // Do nothing
            }
            else
            {
                string ingredient1 = reader.ReadLine().Trim();
                string ingredient2 = reader.ReadLine().Trim();
                string ingredient3 = reader.ReadLine().Trim();
                string ingredient4 = reader.ReadLine().Trim();

                Sandwich currentSandwich = new Sandwich();

                if (ingredient1 == ingredient2 && ingredient2 == ingredient3 && ingredient3 == ingredient4)
                { // the ingredients are treated as a single "Sandwich" object

                    currentSandwich.ingredient1 = ingredient1;
                    currentSandwich.ingredient2 = "";
                    currentSandwich.ingredient3 = "";
                    currentSandwich.ingredient4 = "";
                }
                else // The ingredients are meant to be taken independently
                {
                    currentSandwich.ingredient1 = ingredient1;
                    currentSandwich.ingredient2 = ingredient2;
                    currentSandwich.ingredient3 = ingredient3;
                    currentSandwich.ingredient4 = ingredient4;
                }

                string sandwichClassification = reader.ReadLine().Trim();

                if (sandwichClassification == "YES")
                {
                    currentSandwich.isSandwich = true;
                }
                else
                {
                    currentSandwich.isSandwich = false;
                }

                string edibleClassification = reader.ReadLine().Trim();

                if (edibleClassification == "YES")
                {
                    currentSandwich.isEdible = true;
                }
                else
                {
                    currentSandwich.isEdible = false;
                }

                ingredientList.Add(currentSandwich);

            }
        }
    }

    // Use this for initialization
    void Start () {
		incomingItem = false;
        boxOrigin = box.transform.position;
        backgroundSymbol.transform.SetPositionAndRotation(backgroundSymbol.transform.position, new Quaternion(backgroundSymbol.transform.rotation.x, 90, backgroundSymbol.transform.rotation.z, backgroundSymbol.transform.rotation.w));
        readInSandwiches();
	}
	
	// Update is called once per frame
	void Update () {

        //  box enabled = false if it's yes. remains true if it's no.
        // reset the position rotation of the backgroundSymbolto 90 when we get an answer
        if (!gameOver)
        {
            if (!waitingForAnswer)
            {
                if (Mathf.Abs(box.transform.position.x + 3 - this.transform.position.x) < 5 && box.enabled && choice == "")
                {
                    incomingItem = true;
                    choiceText.text = "";
                }
                else if (Mathf.Abs(box.transform.position.x - 6.3f - this.transform.position.x) < 4)
                {
                    box.transform.position = boxOrigin;
                    box.enabled = true;
                    choice = "";
                    
                }
                else
                {
                    Vector3 newBoxLocation = box.transform.position;
                    newBoxLocation.x += boxMovementSpeed;
                    box.transform.position = newBoxLocation;

                    //box.transform.SetPositionAndRotation(new Vector3(box.transform.position.x + boxMovementSpeed, box.transform.position.y, box.transform.position.z), box.transform.rotation);
                }
            }

            if (waitingForAnswer)
            {
                // do nothing
            }

            else if (incomingItem)
            {
                distance += walkDistance;

                outsideArm.transform.rotation = Quaternion.Lerp(outsideArm.transform.rotation, Quaternion.identity, 0.25f);
                insideArm.transform.rotation = Quaternion.Lerp(insideArm.transform.rotation, Quaternion.identity, 0.25f);
                outsideLeg.transform.rotation = Quaternion.Lerp(outsideLeg.transform.rotation, Quaternion.identity, 0.25f);
                insideLeg.transform.rotation = Quaternion.Lerp(insideLeg.transform.rotation, Quaternion.identity, 0.25f);

                bool outsideArmDone = false;
                bool insideArmDone = false;
                bool outsideLegDone = false;
                bool insideLegDone = false;

                if (insideArm.transform.rotation == Quaternion.identity)
                {
                    insideArmDone = true;
                }
                if (outsideArm.transform.rotation == Quaternion.identity)
                {
                    outsideArmDone = true;
                }
                if (insideLeg.transform.rotation == Quaternion.identity)
                {
                    insideLegDone = true;
                }
                if (outsideLeg.transform.rotation == Quaternion.identity)
                {
                    outsideLegDone = true;
                }

                /*Quaternion outsideArmRotation = outsideArm.transform.rotation;
                Quaternion insideArmRotation = insideArm.transform.rotation;
                Quaternion outsideLegRotation = outsideLeg.transform.rotation;
                Quaternion insideLegRotation = insideLeg.transform.rotation;*/



                /*if (Mathf.Abs(outsideArmRotation.z) < neutralRotationLimit)
                {
                    outsideArmDone = true;
                }
                else if (Mathf.Abs(outsideArmRotation.z) > 0)
                {
                    outsideArmRotation.z = outsideArmRotation.z - rotationShiftSize;
                }
                else // negative value
                {
                    outsideArmRotation.z = outsideArmRotation.z + rotationShiftSize;
                }

                if (Mathf.Abs(insideArmRotation.z) < neutralRotationLimit)
                {
                    insideArmDone = true;
                }
                else if (Mathf.Abs(insideArmRotation.z) > 0)
                {
                    insideArmRotation.z = insideArmRotation.z - rotationShiftSize;
                }
                else // negative value
                {
                    insideArmRotation.z = insideArmRotation.z + rotationShiftSize;
                }

                if (Mathf.Abs(outsideLegRotation.z) < neutralRotationLimit)
                {
                    outsideLegDone = true;
                }
                else if (Mathf.Abs(outsideLegRotation.z) > 0)
                {
                    outsideLegRotation.z = outsideLegRotation.z - rotationShiftSize;
                }
                else // negative value
                {
                    outsideLegRotation.z = outsideLegRotation.z + rotationShiftSize;
                }

                if (Mathf.Abs(insideLegRotation.z) < neutralRotationLimit)
                {
                    insideLegDone = true;
                }
                else if (Mathf.Abs(insideLegRotation.z) > 0)
                {
                    insideLegRotation.z = insideLegRotation.z - rotationShiftSize;
                }
                else // negative value
                {
                    insideLegRotation.z = insideLegRotation.z + rotationShiftSize;
                }*/

                if (outsideArmDone && insideArmDone && outsideLegDone && insideLegDone)
                {
                    waitingForAnswer = true;
                    incomingItem = false;

                    /*insideArmRotation.z = 0;
                    outsideArmRotation.z = 0;
                    insideLegRotation.z = 0;
                    outsideLegRotation.z = 0;

                    insideArm.transform.rotation = insideArmRotation;
                    outsideArm.transform.rotation = outsideArmRotation;
                    insideLeg.transform.rotation = insideLegRotation;
                    outsideLeg.transform.rotation = outsideLegRotation;*/

                    backgroundSymbol.transform.SetPositionAndRotation(backgroundSymbol.transform.position, new Quaternion(backgroundSymbol.transform.rotation.x, 0, backgroundSymbol.transform.rotation.z, backgroundSymbol.transform.rotation.w));

                    choiceCanvas.gameObject.SetActive(true);

                    while(ingredientList.Count == 0)
                    {
                        readInSandwiches();
                    }

                    int slotSelected = Random.Range(0, ingredientList.Count);

                    Sandwich currentSandwich = ingredientList[slotSelected];

                    choice1.text = currentSandwich.ingredient1;
                    currentIngredient1 = currentSandwich.ingredient1;

                    choice2.text = currentSandwich.ingredient2;
                    currentIngredient2 = currentSandwich.ingredient2;

                    choice3.text = currentSandwich.ingredient3;
                    currentIngredient3 = currentSandwich.ingredient3;

                    choice4.text = currentSandwich.ingredient4;
                    currentIngredient4 = currentSandwich.ingredient4;

                    currentSandwichStatus = currentSandwich.isSandwich;
                    currentEdibleStatus = currentSandwich.isEdible;

                    if (currentSandwich.ingredient2 == "")
                    {
                        singleIngredient = true;
                    }
                    else
                    {
                        singleIngredient = false;
                    }

                }
                /*else
                {
                    insideArm.transform.rotation = insideArmRotation;
                    outsideArm.transform.rotation = outsideArmRotation;
                    insideLeg.transform.rotation = insideLegRotation;
                    outsideLeg.transform.rotation = outsideLegRotation;
                }*/

                // if it's greater than 0 subtract if it's less than 0 add and if it's absolute value is less than .2 do nothing once all are less than .2 set waiting for answer

            }

            else
            {
                distance += walkDistance;
                

                // 30 goes backwards, -30 goes forward

                Quaternion outsideArmRotation = outsideArm.transform.rotation;


                if (outsideArmMovingForward)
                {
                    if (outsideArmRotation.z >= rotationLimit)
                    { //reset movingForward and move back
                        outsideArmMovingForward = false;
                        outsideArmRotation.z = outsideArmRotation.z - rotationShiftSize;
                    }
                    else
                    { // continue moving forward
                        outsideArmRotation.z = outsideArmRotation.z + rotationShiftSize;
                    }
                }
                else
                { // Moving backwards
                    if (outsideArmRotation.z <= -rotationLimit)
                    { // reset to moving forward and move forward
                        outsideArmMovingForward = true;
                        outsideArmRotation.z = outsideArmRotation.z + rotationShiftSize;
                    }
                    else
                    { // continue moving back
                        outsideArmRotation.z = outsideArmRotation.z - rotationShiftSize;
                    }
                }

                Quaternion insideArmRotation = insideArm.transform.rotation;

                if (insideArmMovingForward)
                {
                    if (insideArmRotation.z >= rotationLimit)
                    { //reset movingForward and move back
                        insideArmMovingForward = false;
                        insideArmRotation.z = insideArmRotation.z - rotationShiftSize;
                    }
                    else
                    { // continue moving forward
                        insideArmRotation.z = insideArmRotation.z + rotationShiftSize;
                    }
                }
                else
                { // Moving backwards
                    if (insideArmRotation.z <= -rotationLimit)
                    { // reset to moving forward and move forward
                        insideArmMovingForward = true;
                        insideArmRotation.z = insideArmRotation.z + rotationShiftSize;
                    }
                    else
                    { // continue moving back
                        insideArmRotation.z = insideArmRotation.z - rotationShiftSize;
                    }
                }

                Quaternion outsideLegRotation = outsideLeg.transform.rotation;

                if (outsideLegMovingForward)
                {
                    if (outsideLegRotation.z >= rotationLimit)
                    { //reset movingForward and move back
                        outsideLegMovingForward = false;
                        outsideLegRotation.z = outsideLegRotation.z - rotationShiftSize;
                    }
                    else
                    { // continue moving forward
                        outsideLegRotation.z = outsideLegRotation.z + rotationShiftSize;
                    }
                }
                else
                { // Moving backwards
                    if (outsideLegRotation.z <= -rotationLimit)
                    { // reset to moving forward and move forward
                        outsideLegMovingForward = true;
                        outsideLegRotation.z = outsideLegRotation.z + rotationShiftSize;
                    }
                    else
                    { // continue moving back
                        outsideLegRotation.z = outsideLegRotation.z - rotationShiftSize;
                    }
                }

                Quaternion insideLegRotation = insideLeg.transform.rotation;

                if (insideLegMovingForward)
                {
                    if (insideLegRotation.z >= rotationLimit)
                    { //reset movingForward and move back
                        insideLegMovingForward = false;
                        insideLegRotation.z = insideLegRotation.z - rotationShiftSize;
                    }
                    else
                    { // continue moving forward
                        insideLegRotation.z = insideLegRotation.z + rotationShiftSize;
                    }
                }
                else
                { // Moving backwards
                    if (insideLegRotation.z <= -rotationLimit)
                    { // reset to moving forward and move forward
                        insideLegMovingForward = true;
                        insideLegRotation.z = insideLegRotation.z + rotationShiftSize;
                    }
                    else
                    { // continue moving back
                        insideLegRotation.z = insideLegRotation.z - rotationShiftSize;
                    }
                }

                // if it was positive but now is negative or vice versa remove health

                if (outsideArm.transform.rotation.z > 0 && outsideArmRotation.z < 0)
                {
                    currentHealth -= lostHealthPerStep;
                }
                else if (outsideArm.transform.rotation.z < 0 && outsideArmRotation.z > 0)
                {
                    currentHealth -= lostHealthPerStep;
                }
                else { } // do nothing

                if (currentHealth <= 0f)
                {
                    gameOver = true;
                }


                outsideArm.transform.rotation = outsideArmRotation;
                insideArm.transform.rotation = insideArmRotation;
                outsideLeg.transform.rotation = outsideLegRotation;
                insideLeg.transform.rotation = insideLegRotation;

            }
        }
        else
        {
            backgroundSymbol.transform.SetPositionAndRotation(backgroundSymbol.transform.position, new Quaternion(backgroundSymbol.transform.rotation.x, 0, backgroundSymbol.transform.rotation.z, backgroundSymbol.transform.rotation.w));
            gameOverCanvas.gameObject.SetActive(true);
            choiceCanvas.gameObject.SetActive(false);

        }

        distanceText.text = "Distance: " + distance.ToString("F2");
        healthText.text = "Health: " + currentHealth;
        if(currentHealth <= (maxHealth * .2f) )
        {
            healthText.color = new Color(1f, 0f, 0f);
            if (currentHealth <= (maxHealth* .05))
            {
                healthText.fontSize = 25;
            }
            else if (currentHealth <= (maxHealth * .1)) {
                healthText.fontSize = 20;
            }
            else if (currentHealth <= (maxHealth * .15)) {
                healthText.fontSize = 18;
            }
            else
            {
                healthText.fontSize = 16;
            }
            
        }
        else
        {
            healthText.color = new Color(0f, 0f, 0f);
            healthText.fontSize = 14;
        }
        smugnessText.text = "Smugness: " + smugness;

    }
}
