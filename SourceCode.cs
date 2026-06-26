global using System.IO;
global using System.Collections.Generic;
global using System.Linq;
global using System.Globalization;
global using System.ComponentModel;
public class KaveDS
{
    public string dir { get; set; }
    public string fileText { get; set; }
    public List<string> fileObjects { get; set;}
    public List<string> fileObjectsNames { get; set; }
    public Dictionary<string, object> fileObjectsDict {get; set;}

    
    public KaveDS(string dir)
    {
        this.dir = dir;
        if(!dir.EndsWith(".kaveds"))
        {
            throw new Exception(".kaveds file not found");
        }
        this.fileText = File.ReadAllText(dir).Replace("\r", "");
        this.fileObjects = fileText.Split("object").ToList();
        fileObjects.RemoveAt(0);
        this.fileObjectsNames = new List<string>();
        this.fileObjectsDict = new Dictionary<string, object>();
        for(int i = 0; i < fileObjects.Count(); i++)
            {
               string obj = fileObjects[i];
               string firstLine = obj.TrimStart().Split("\n")[0].Trim();
               
               this.fileObjectsNames.Add(firstLine);
               this.fileObjectsDict.Add(firstLine, obj);
                
            }

        
        
    }

    public dynamic GetValueByKey(string objectName, string key)
    {
        string[] lines = fileObjectsDict[objectName].ToString().Split("\n");
        foreach(string line in lines)
        {
            if(!line.Contains(':'))
            {
                continue;
            }
            if(line.Split(':')[0].Trim(' ').Trim('"') == key)
            {
                if(line.Contains('['))
                {
                   
                  
                    string arrayPart = line.Split(':')[1].Trim().TrimEnd(';');
                    
                  
                    arrayPart = arrayPart.Replace("[", "").Replace("]", "");
                    
                   
                    string[] elements = arrayPart.Split(',');
                    dynamic[] arrayReturn = new dynamic[elements.Length];
                    
                    for(int x = 0; x < elements.Length; x++)
                    {
                        string cleanItem = elements[x].Trim().Trim('"');
                        
                        // Spara som en int om det är en siffra, annars som en sträng
                        if (int.TryParse(cleanItem, out int intVal))
                        {
                            arrayReturn[x] = intVal;
                        }
                        else
                        {
                            arrayReturn[x] = cleanItem;
                        }
                    }
                    return arrayReturn;
                }
                string returnValue = line.Split(':')[1].Trim().TrimEnd(';');
                
                if(returnValue.StartsWith("\"") && returnValue.EndsWith("\""))
                {
                    string cleanText = returnValue.Trim('"');

                    if(char.TryParse(cleanText, out char charResult))
                    {
                        return charResult;
                    }

                    return cleanText;
                }
                if(int.TryParse(returnValue, out int intValue))
                {
                    if(!returnValue.Contains('.') && !returnValue.Contains(','))
                    {
                        return intValue;
                    }
                }

                if(bool.TryParse(returnValue, out bool boolResult))
                {
                    return boolResult;
                }

                if(returnValue.Length > 0)
                {
                    char lastChar = returnValue[returnValue.Length - 1];
                    if(lastChar == 'f' || lastChar == 'F')
                    {
                        return float.Parse(returnValue.TrimEnd('f', 'F'), CultureInfo.InvariantCulture);
                    }else if(lastChar == 'm' || lastChar == 'M')
                    {
                        return decimal.Parse(returnValue.TrimEnd('m', 'M'), CultureInfo.InvariantCulture);
                    }else
                    {
                        return double.Parse(returnValue, CultureInfo.InvariantCulture);
                    }
                }

                return returnValue;
              
                
            
            
            }
        }
        return null;
    }
    public string GetObjectsTextByName(string objectName)
    {
        if(fileObjectsDict[objectName] == null)
        {
            throw new Exception($"Object {objectName} not found");
        }else
        {
            return fileObjectsDict[objectName].ToString();
        }
    }

    public void SetValueByKey(string objectName, string key,  dynamic value, int arrayIndex = -1)
    {
        key = key.Trim('"');
        
        if(arrayIndex >= 0)
        {
            dynamic currentArray = GetValueByKey(objectName, key);

            if(currentArray is dynamic[])
            {
                currentArray[arrayIndex] = value;

                value = currentArray;
            }else
            {
                throw new Exception($"Key {key} is not an array, cannot update index {arrayIndex}");
            }
        }

        List<string> fileLines = File.ReadAllLines(dir).ToList().Select(line => line.Replace("\r", "")).ToList();;
        int objectLine = -1;
        for(int i = 0; i < fileLines.Count; i++)
        {
            if(fileLines[i].Contains($"object {objectName}"))
            {
                objectLine = i;
                break;
            }
        }
        if(objectLine == -1) 
        {
            throw new Exception($"Object '{objectName}' not found in the file.");
        }

        string[] objectLines = fileObjectsDict[objectName].ToString().Split("\n");
        bool updated = false;
        for(int i = 0; i < objectLines.Length; i++)
        {
           string currentLine = objectLines[i].Replace("\r", "");

           if(currentLine.Contains(':') && currentLine.Split(':')[0].Trim().Trim('"') == key)
            {
                
                int targetFileLine = objectLine + i;
                if(targetFileLine < fileLines.Count)
                {
                    
                    string originalKey = currentLine.Split(':')[0].Trim();
                    if(value is dynamic[])
                    {
                        string arrayValue = "[]";
                        int position = 1;
                        
                        for(int x = 0; x < value.Length; x++)
                        {
                            string itemText = "";
                            if (value[x] is string)
                            {
                                itemText = $"\"{value[x]}\"";
                            }else
                            {
                                itemText = value[x].ToString();
                            }
                            if(x < value.Length - 1)
                            {
                                itemText += ", ";
                            }
                            
                            arrayValue = arrayValue.Insert(position, itemText);
                            position += itemText.Length;
                        }
                        fileLines[targetFileLine] = $"    {originalKey} : {arrayValue};";
                        updated = true;
                        break;
                    }else
                    {
                         fileLines[targetFileLine] = $"    {originalKey} : {value};";
                         updated = true;
                         break;
                    }
                     
                   
                }
                else
                {
                    throw new Exception("Key not found");
                }
                
            }
        }

        if(updated)
        {
            File.WriteAllLines(dir, fileLines);

            //Update cached data
            this.fileText = File.ReadAllText(dir).Replace("\r", "");;
            this.fileObjects.Clear();
            this.fileObjectsNames.Clear();
            this.fileObjectsDict.Clear();

            this.fileObjects = fileText.Split("object").ToList();
            fileObjects.RemoveAt(0);

            for(int i = 0; i < fileObjects.Count(); i++)
            {
               string obj = fileObjects[i];
               string firstLine = obj.TrimStart().Split("\n")[0].Trim();
               
               this.fileObjectsNames.Add(firstLine);
               this.fileObjectsDict.Add(firstLine, obj);
                
            }
        }else
        {
            throw new Exception($"Key {key} not found in object {objectName}");
        }
        
    }
}
