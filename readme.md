C# Code Refactoring Tool
Purpose
This Python script automatically refactors large C# methods by moving them into separate classes while maintaining functionality through delegation.

How to Use
Place your C# file in the same directory as the Python script
Set the desired line threshold for method size
Run the script with your input file:
python refactor_tool.py
Example Input/Output
Input file structure:

namespace MyApp {
  public class MyClass {
    public void LargeMethod() {
      // More than threshold lines of code
    }
  }
}
Generated output:

// Original file (refactored):
public void LargeMethod() {
  return new LargeMethodClass().LargeMethod();
}

// New file (LargeMethodClass.cs):
public class LargeMethodClass {
  // Original method implementation
}
Features
Automatically identifies methods exceeding size threshold
Creates separate class files for large methods
Maintains original method signatures and access modifiers
Handles method parameters and return types
Preserves namespace structure
Limitations
May not handle all C# syntax variations
Doesn't handle complex dependencies between methods
Limited support for class fields and properties
Basic parsing of C# syntax (no full C# parser)
File Output
The script generates:

A refactored version of your original file (originalname_refactored.cs)
Separate class files for each extracted method (MethodNameClass.cs)