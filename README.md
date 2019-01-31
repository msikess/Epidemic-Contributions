The work that I contributed to the Epidemic game

*Note:

1) The Unity project in this repository is the last version of the game that I got to work on around January 2018. After that, the game has been transferred to a different team and the current version can be viewed at the following website:

https://stat2games.sites.grinnell.edu/

(The name of the game has also been changed to Statsville. the Epidemic game on the website is a revamped version of this project that I worked on in Summer 2018)

2) You can also just play the demo, "Mariam's ParticleEpi 8 levels", is available in the "Mariam's ParticleEpi 8 levels" folder.

3) The Following are the scripts I worked on/contributed to (script files not mentioned here were written by my teammates (Mike Zou, Ritika Agarwal, Jemuel Santos, Jimin Tan, and Hoang Cao) without my help):
	Json.cs
	CSVC.cs
	SubmitData.cs
	GameOver.cs
	LevelScreen.cs
	StartMenu.cs
	StopRule.cs
	Day.cs
	GameBuilder1.cs
	Initializer.cs
	Randomize.cs
	WorldModel.cs	
	FaceScript.cs
	Filler.cs
	PercentageSlider.cs
	SetText.cs
	Syringe.cs
	GameController.cs
	
________________________________________________________________

Epidemic

Overview:
This document is supposed to help understand how scripts that the Grinnell MAP team wrote imitate the Statistical model given to us by Shonda Kuiper and written by Rod Sturdivan. It is also supposed to help understand how the scripts communicate with each other and relevant Game Objects behind the scenes to pass the data and build the game (hence the script GameBuilder1.cs). The scripts and other objects this document will discuss are:
·         Initializer.cs
·         WorldModel.cs
·         GameBuilder1.cs
·         Day.cs
·         Randomize.cs
·         StopRule.cs
·         Syringe.cs (will touch upon this, but not discuss it);
·         WorldModel (GameObject)
o   Initializer (Script Component)
o   WorldModel (Script Component)
 
Implementation Considerations:
	The decision to make the game more object oriented (to later make the game-data transfer from the game to the servers easier) was made at the very beginning of the MAP. Following that decision, the initial GameBuilder.cs was split into and modeled through Initializer.cs, WorldModel.cs, GameBuilder1.cs, Day.cs, StopRule.cs, and Randomize.cs (the successful version of Binomial Distribution calculations to make the game more random);
 
Initializer.cs – the reason for creating Initializer.cs was to have a separate script that would have serialized fields of variables (visible in the Unity Editor UI, under the Inspector tab as a Script Component of the WorldModel Game Object) necessary for setting up the parameters and the stopping rules of every level.
	The non-serialized fields in the script are variables that are dynamically determined in the game (percentage assignments of the treatments) with the help of the Syringe.cs script that is used to determine what percent does the player set the syringe sliders at and translate the slider percentage to a treatment percentage (a float to a double). The reason we’re doing this is to allow the player to comfortably set the percentages on the game screen.
 
Day – the reason for creating Day.cs was to give the game a more OO structure, the most basic unit of which would be a Day object with all the daily statistics as its instances. This decision made sense since the game is played/treatments are assigned on an in-game daily basis.
 
GameBuilder1 – the reason for creating GameBuilder1.cs was the same as for Day.cs. However, GameBuilder.cs is the script that does all the math calculations (cure calculations are done with the help of Randomize.cs). It is the script that makes sure the game follows the Statistical Model (although, WorldModel.cs is also partially responsible for that, mainly because of when it calls the two methods and the constructor of a Day object/Day.cs).
     The only instance that the GameBuilder1 script has is a list of Day objects called Calendar. It is initialized in the constructor of the class GameObject only with the initial statistics of the first day stored in the first Day object in the list.
     The method calculate() is used once the player determines the treatments percentages that they want to assign and it calculates the rest of the statistics for the last Day in the list (the first day is also the last day when the list has only 1 Day object in it).
     The method nextDay() is used after the method calculate() in order to generate the next Day object with the statistics and parameters based on what happened (the calculations) on the previous Day.
The reason for structuring the class in this way was to make the player-dependent dynamic treatment assignment calculations easier to handle while giving the player all the time they need in real world to decide what combination they want to assign.
 
WorldModel.cs – the reason for creating this script was to have a single script that would handle the game progression and the communication/information-exchange from the GameBuilder1 and Day objects to any other script (UI related or the StopRule.cs) that needed it.
     The script will be discussed in more detail later since it is somewhat complex. Besides that, it also has serialized instances of certain scripts that take in parameters from the Unity Editor under the Inspector tab (e.g. Initializer and UI related scripts: Filler.cs, GameOver.cs, SetText.cs, SpawnM1.cs, SpawnPopulation.cs (also, an AudioSource object for the pump’s sounds));
 
Randomize – the reason for creating this script was to enable the Binomial Inverse randomization of the cure rates of the treatments. The script on its own is heavy on parsing a CSV file with all the Binomial and Normal distribution values (the file is InverseNorm.csv in Assets>Resources). It also has a function responsible for the randomized generation of an Inverse Binomial (hence the randomized calculation of the treatment’s effectiveness for that day) and the calculation of the number of people cured.
 
StopRule – the reason for creating this script was to have a separate script that would track the game-state. The script takes in the daily stats information from the GameBuilder1 instance of the WorldModel script (which is also the component of the WorldModel Game object), and compares it to the stop rule variables from Initializer (also accessed from/through WorldModel script). This way, the StopRule.cs script monitors if the game was won, lost, or none on the last day available in the WorldModel’s GameBuilder1 instance.
 
WorldModel (Game Object) – the reason for creating this Game Object was to have one Game Object that would have all the Unity Editor intractable scripts as its components (all the scripts that can have their instances/parameters set through the Unity Editor under the Inspector tab). Technically, it exists for the convenience of the developers so that they don’t have to go through multiple scripts to set up all the parameters every time the level initial stats change.
     Two notable (for this document) script components of the WorldModel Game Object are the Initializer and the WolrdModel scripts (same as described above, setting them as script components of the Game Object just enables us to set up their serialized private instances/parameters through the Unity Editor under the Inspector tab).
 
Code Discussion:
 
Initializer.cs (Script and Script Component)
 
	As a script that’s also a script component, all the variables of the Initializer script that are topped with Serialize Field command are easily declarable/set-able from the Unity Editor UI (under the Inspector tab) once it’s attached to an object (in our case the WorldModel game object).
All the variables are needed at least for the game calculations and might also be necessary for certain UI related scripts and StopRule script to monitor the game state (all of which is handled through WorldModel script). However, since the variables are all private, there’s a function getParam that takes a String argument and returns the variables in Double form (most strings just repeat the variable names, although some are shortened for the sake of convenience so it’s better to look at strings in the if statements and try to remember them than the variable names as they are not used anywhere else, but the strings are). All string input are case insensitive, so the function works as long as the correct string is inputted.
     Although, it’s arguable whether returning some of the Integer variables as Doubles makes sense or not, the decision to squeeze all variable returns in single function was made for the sake of convenience (especially considering that another function get(String s) was also created for the Day objects to retrieve daily stats).
	Therefore, the only role of this script is to have variables that are mostly fixed (except for the percent treatment ones) and pass those variables to other scripts through the getParam() function.

Update:

·         Added the serialized private double recoveryRate instance variable.
·         Added an if statement check case in the getParam method to return the instance as a double (needed input to method: “recovery”).


 
 
 
 
 
Day.cs
 
     Although this script has serialized variables too, those fields are serialized in order to enable the object to be stored as a JSON file (since Json serialization is in-built in Unity, once you serialize a class and serialize the fields of the class that you would want to include in Json, later on it’s possible to store the object along with serialized fields as a json string with the call of a single function (more on this in the Data Transfer Document)).
     All of the variables in a Day object are fixed (initialized/declared only once) and reflect the variables from the Statistical model (with the addition of a few more for the sake of convenience since some UI and stop rules consider cumulative calculation but not daily ones and the Statistical model didn’t have these cumulative calculations, so we had to improvise). However, due to the game play structure (since treatment assignment percentages are not known until the player hits the Next Day button), the constructor of a Day object initializes/declares only the variables that are possible to calculate with the given initializer parameters (and only those if it’s the first day) and the previous day’s after-treatment calculated variables (if the game is at least on day 2).
     Therefore, there’s the constructor Day that declares all these variables, and then there’s also the method restOfDay that declares all the other variables. Both the constructor and the method take in quite a number of arguments, and it can get quite messy to input them all correctly. However, one plus of Unity’s own text-editor MonoDevelopment is that as you’re inputting variables to a function/method, it displays what you should be inputting at the moment, so that helps (especially since the names of the calculated variables in GameBuilder1 mimic the input variable names for the method and the constructor).
     The class also has a get method that takes in a string argument and returns the corresponding variable. Unlike Initializer’s getParam, since all the variables in a Day object are integers, all of them can be returned through this function. Moreover, the strings are exactly the same as variable names (case insensitive).

Update:

·         Added a serialized private int recovered as an instance variable of a Day object.
·         Added the instance variable (recovered) initializer/setter in the restOfDay method;
·         Added an if statement check case in the get method to return the instance as an integer (needed input to method: “recovered”).
 
GameBuilder1.cs
 
     The reason for serializing this script’s class was to reach the serialized Day objects in the GameBuilder1’s List <of Days> object called Calendar (the reasoning behind all this is once again connected to the convenience of Json file creation). As for the Randomize, the object is initiated through the constructor only once within the GameBuilder1 constructor itself (more on why we have to initiate a Randomize object later). Otherwise, the GameBuilder1 constructor also initiates the Day object List calendar and adds to it the first part of the first Day of the game by calling the initDay method of the its own class. This method checks if all of the initializer parameters are logical (could work potentially) and creates the first part of stats and initialized them in the Day object so that the player can make the treatment assignment decision based on that information.
     The constructor also calls the SpawnM1 and SpawnPopulation methods, both called, onStart (these functions are supposed to take in the information from the GameBuilder1 object and the initializer and set up the population (healthy and sick) and the clinic (sick) arrays correctly.
     Besides these, the script also has two more methods connected to Day initialization/generation.  First, calculate (takes initializer as its parameter) is the method that is called in world model once the player decides the treatment assignments and hits the Next Day button. The method calculates all the remaining (non-initialized, treatment assignment dependent) parameters and initializes them in the last/current day of the calendar (List<of days>). After that world model calls the other method from GameBuilder1, nextDay (takes initializer as a parameter) which calculates the initial stats for the new day of the game based on the calculations just made for the last/current day and adds a new day to the list with these initial stats parameters.
     GameBuilder1 also has two more methods. One is retDay(integer has to be greater than 0), which returns a day object from the calendar but in reverse order (1 will return today, 2 will return yesterday, 3 the day before, etc.). The method comes in handy in most if not all of the other scripts that might need daily information from specific days and not just the last/current one. The other method, totalDays(), simply returns the count of the items in calendar.

Update:

·         Added the int recovered calculation to the calculate method;
·         Updated total?uredDay method to include recovered number.
 
WorldModel.cs (Script and SC)
 
	The most complex of the scripts, WorldModel was envisioned and created as a hub through which all the scripts are controlled. Although, this script does a lot more than just control. Utmost of all, it instantiates a GameBuilder1 object based on initializer parameters and enables the player to choose treatment assignments before hitting the next day button, after which the script calculates the remaining variables for that day, creates a new day and prompts that up on the screen for the player. Meanwhile, the script also regularly (after every next day generation) checks the state of the game. It enables and disables the syringe sliders and next day button while animations are transitioning so that the player doesn’t break the game. It also makes filler and setText scripts to prompt the current information on the screen (the tubes and text boxes). And, finally it calls the animation coroutines within its own coroutine and the pumping sound during the animation transition.
     Before I get into more detail on why we have coroutines nested in a coroutine, I’m going to explain that the coroutine in general acts like an Update function as in, technically, it’s called every frame. However, calling certain methods, such as GameBuilder1’s calculate() and nextDay(), every frame would crash the game. One way to get around such problem is to have the call of the methods under if-statements (once in a if-statement check is done everything under it will be called only once and not per frame). Therefore, the script has two Boolean variables that are declared true once the button is clicked and false after the coroutine checks them. Clicked and Checker might not really make sense as the names of these Booleans (so changing their names later on might be a good idea), but the fact is that they’re important for making sure the game doesn’t crash.
     As for nested coroutines… When we first wrote the WorldModel coroutine we thought that we would call the animation functions with a yield return of a function that would make the WorldModel coroutine wait for x milliseconds real-time in between the treatment assignment effects (the if statement checking if clicked is true) and the new day prompt (the if statement checking if checker is true). However, that proved to be problematic as animations (depending on how many objects they had to spawn/retract) would not always take the same amount of time. So, once again to make sure the game would not crash we had to get around this problem and we figured that the best quickest fix was to yield return those functions as coroutines (which also entails turning functions into IEEnumerators so that they can be yielded as coroutines). This way, they are called only once (since they’re located within the if statements) and each coroutine takes as long as it needs to before the next one is called. Hence, problem solved.
     Moreover, WorldModel also has four more methods towards the end of the script. All four of them are needed for other scripts (e.g.: Json).
 
*! Side Note: I don’t quite remember why we have both Start() and Awake(). It would be nice to see whether the coroutines in Start would also work in Awake and move them if so.
 
Randomize.cs
 
*! Notes: the print and test functions are to be disregarded as they were needed when Jimin was testing the script outside of Unity. Also, the commented out chunk of code at the end of the script is the previous way of parsing a csv file which works when the game is built for a computer/phone/etc, but doesn’t work when it’s built for webGL. Therefore we developed another way of reading csv files that works universally for all builds available in Unity.
 
     First, the reason why a Randomize object needs to be instantiated in GameBuilder1 is for the object/script to create a dictionary/dic (List <of strings and doubles>) of the binomial inverses/normal distribution only once. It’s also needed to set the variance (integer) which matters for randomization of the cure rates (if variance = 0, there is no randomization).
     The instantiation of the dictionary happens through the method called readInverseBinomialNumbers(), which parses a csv file containing these numbers and adds the first column to listA (the string representation list) and the second column to listB (the doubles/actual numbers). Once it’s done parsing, it integrates the two lists into dic thus creating a dictionary of inverse binomials which later on is used to randomize the cure calculations through another Randomize method called curedPopulationRnd(int of people to be cured, double of effectiveness of the treatment).
     Therefore, in GameBuilder1 for the curedAnum and curedBnum, we call this method with the corresponding inputs from the current day and initializer (day contains the number of people to be treated with treatment a/b, and initializer contains the effectiveness of both treatments).
 
 
StopRule.cs
 
	One of the simplest scripts in the game, has only one function called check (takes a GameBuilder1 object and an initializer as parameters), has the stop rules of the game hardcoded, which is why depending if the stop rule in real life changes from making sure you get x number of people sick for y days consecutively to making sure you have only z number of people in your clinic on any day, the script will need to be changed (more day objects will be needed for comparison to the goalPeople variable from initializer).
	Otherwise, the function checks if the current stats are making the player lose, win or none and returns a string based on the state. The string, is needed for two scripts: gameOver and Json. The first one needs it to disable the gameUI and prompt a win/lose message along with submit and menu buttons. The latter one needs the script to decide when to separate the json strings with a coma and when not to (it doesn’t need a coma separation for the last day of the game (the win/lose state)).
 
WorldModel (GO)
 
	A game object the components of which are all scripts. Just like WorldModel.cs (script), the game object too is envisioned as a script hub where all the code needed to run the game is located. WorlModel (GO) has the following components:
·         Initializer – to make the declaration of the initial game stats more convenient (plus, the WorldModel(Script) takes it in as a variable too).
·         WorldModel – the script-hub that pretty much runs the whole game. Plus, this way it’s more convenient to assign the serialized private script instances (Initializer, SetText, Filler, etc.).
·         Filler, GameOver, SetText, SubmitData, and SoundControl scripts are attached either because WorldModel (script) needs them as variables and/or because they contain function needed for the UI objects.
·         SpawnM1 and SpawnPopulation scripts are attached for the convenience of declaring their variables needed for spawning the population and clinic game objects correctly. They also need WorldModel (script) as their variable.