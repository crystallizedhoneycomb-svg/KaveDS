KaveDS stands for Key And Value Element Data Storage and is an alternative to Json.
It is simpler and doesn't offer as complex storage. 
It has similar but different syntax. 
It works like this: In the file you have objects with keys and elements inside. You define objects by typing "object", pressing space and typing the objects name. Then you put {} like in an if statement: For example:
object monthlySalaries
{
  "John" : 40000;
  "Emma" : 55000;
  "Jared" : 35000;
  "Companies" : ["Software Engineering", "Linguistics", "Economics"];
}

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
keyValuePairs: A string, string dictionary in which the keys are all the files keys and the values all the files values.

Constructor:
When creating an object of the KaveDS class, you need to enter the directory. It needs to end with .kaveds

Other information:
When you make an array in the file you have to make it like this: [1, 2, 3, "hello"] (the values are just examples).
When you use an array in SetValueByKey you need to input it as such: new dynamic[] {1, 2, 3} (the values are just examples).

Oh, and please leave feedback and/or report bugs!
