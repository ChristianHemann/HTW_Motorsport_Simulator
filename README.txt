Softwareentwicklungsprojekt SS16 
Ingenieurinformatik 
HTW Berlin

Project: HTW Motorsport

Software:
Unity 5.3.2
.NET 3.5

Tutorial Unity:
-open MainMenu.unity from Assets/_Scene
-sometimes unity forgot the associated scripts. Then the following scripts need to be set (To set the script click the GameObject and drag the file in the inspector onto the the script):
--MainMenu.unity: MainCamera needs the script from Assets/Scripts/UnityInterface/Menu.cs
--Simulator.unity: Cube needs the script from Assets/Scripts/UnityInterface/Calculation