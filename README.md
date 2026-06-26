KaveDS stands for Key And Value Element Data Storage and is an alternative to Json.
It uses key and value elements (Important: Each element must be on one line, not multiple)
It is simpler and doesn't offer as complex storage. 
It has similar but different syntax. 
It works like this: In the file you have objects with keys and elements inside. You define objects by typing "object", pressing space and typing the objects name. Then you put {} like in an if statement, see tue Example.kaveds file

How to use:
Download the latest .nupkg file from the **Releases** folder.
If you want to add it to one project:
In the command prompt, paste dotnet add package KaveDS --source "[The directory to the folder that the .nupkg file is in]" in your project.
If you want to add it to your whole computer:
Create a folder where you store your libraries or use one if you already have (or just use a permanent folder).
Paste this into the command prompt **one time** dotnet nuget add source "[The directory of the folder that the .nupkg file is in]" --name "[KaveDS name or a nickname]".
Then when you want to add it to a project and you have added it globally just type dotnet add package kaveDS in the command prompt in you project.
Then you just need to add "using kaveDS" in the top of you .cs file.

Here are explanations of all the class members of KaveDS in the .cs file:
Methods (recomended to use):
GetValueByKey: Returns the value of a key by using two string parameters, the first one is the object that the key is nestled within's name, the second one is the key's name.
GetTextByName: Returns the text of an object by using a string parameter that is the object's name.
SetValueByKey: Sets the value of a key with 2 string parameters, the object's name and the key of the value's name, 1 dynamic parameter, the new value and one optional int parameter, if you want a specific element from the value if it is an array you use it's index as the last parameter.
If the value is an array and you don't specify what index it will return a dynamic array.
Fields (not recommended to use):
fileText: The entire file's text
fileObjects: A string list with all the objects of the file
fileObjectNames: A string list with all the names of the objects of the file
fileObjectDict: A string, string dictionary in which the keys are the objects names and the values are the objects text


Constructor:
When creating an object of the KaveDS class, you need to enter the directory. It needs to end with .kaveds

Other information:
When you make an array in the file you have to make it like this: [1, 2, 3, "hello"] (the values are just examples).
When you use an array in SetValueByKey you need to input it as such: new dynamic[] {1, 2, 3} (the values are just examples).
You also have to end every element in a .kaveds file with a ;


Oh, and please leave feedback and/or report bugs!
