# AcContextualTabRules 

This repo consists of three C# projects.
- MM19CustomTab
	- This project create a CUI Tab,panel and ribbon command button.
	- The goal is to Contextually Highlight the tab when user selects a Line.
- MM19Helper
	- This project consists a custom function rule to trigger, we just check if the selection is Line or not, if it Line we return true, which eventually contextually highling the tab.
- Rules
	- This project consists a custom function rule to trigger up selection of two circles, In this exercise we highlight an existing tab.


### To Build
```
git clone https://github.com/MadhukarMoogala/AcContextualTabRules.git
cd AcContextualTabRules
deven MM19CustomTab.sln
msbuild /t:Build MM19CustomTab.sln
```
### GIF Demonstration
![](MM19Context.gif)
