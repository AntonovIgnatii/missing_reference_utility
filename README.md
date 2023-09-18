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

The solution is to sort through assets either in the root of the project, the current scene, or in all scenes specified in the build settings. Using Component checks components for validity, and using SerializedPropertyType checks serialized properties.
The window itself is similar to the address asset duplicate analyzer window; in my opinion, this is an elegant and informative option.