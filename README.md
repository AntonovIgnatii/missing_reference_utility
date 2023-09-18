# missing_reference_utility
Test task to find lost links in a Unity project

To install the utility, use the Window -> Package Manager -> + button -> Add package from git URL -> set https://github.com/AntonovIgnatii/missing_reference_utility.git

![Add package via Package Manager](https://i.ibb.co/BcbB1Qj/image.png)
![Add package via Package Manager](https://i.ibb.co/rsVJsph/0-1.png)

To use, you need to open the analyzer window through the menu item "Tools/Find Missing Reference/Analyze Window", as shown in the screenshot below

![Open analyze window](https://i.ibb.co/PjHtq47/1.png)

In the analyzer window we see two buttons “Analyze Selected Rules” and “Clear” at the top, as well as the same list of analyzed rules.
When a rule is selected (you can select several rules), you can use the “Analyze Selected Rules” button to activate analysis rules, the result of which will be updated in the same window in the rules section. Using the "Clear" button we will clear the analysis history and open the window to the default position.

![Analyze window in default](https://i.ibb.co/bNQzRkV/2.png)
![Analyze window by result](https://i.ibb.co/TMRXZ6d/3.png)

You can find all lost links using the following approach:
1) Go through all the content in run-time and collect warnings in the console
2) Use ready-made solutions, like https://odininspector.com/odin-validator
3) Write your own utility

When writing your own utility, you can be guided by one of the existing approaches, which are described below

There are several ways in which you can find all missing references throughout the entire project in one iteration using a homemade script in Unity:

1) Locate all scenes and objects in a project: You can use functions like SceneManager.GetAllScenes and GameObject.FindObjectsOfType to get all the scenes and objects in a project. Then go through each object and check for missing links.

2) Using recursion to traverse a tree of objects: You can write a recursive function that will traverse all child objects and check for missing references. This will allow you to efficiently go through all the objects in the project in one iteration.

3) Using AssetDatabase to traverse all prefabs: You can use the AssetDatabase.FindAssets function to find all prefabs in a project. Then go through each prefab and check for missing links.

4) Using AssetDatabase to traverse all scripts and prefabs containing components: You can use the AssetDatabase.FindAssets function to find all scripts and prefabs in a project. Then go through each script and prefab, and for each component, check for missing references.

The option of using recursion to traverse the object tree is the most efficient way to find all missing references throughout the entire project in a single iteration.

Solution - Use recursion to traverse the object tree: across the entire project, the current scene, or across all scenes specified in the build settings. Using Component tests components for validity, and using SerializedPropertyType tests serialized properties of components.
The window itself is similar to the Address Asset Duplicate Analyzer window; I think this is an elegant and informative option.