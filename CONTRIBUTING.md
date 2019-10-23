Contributing to ProcSharp
--------------------

1. Find a function from the Processing library not yet implemented in ProcSharp, see the [Processing reference](https://processing.org/reference/).
2. Fork ProcSharp.
3. Implement the selected function as a static function in ProcSharp.cs. If you need more than a few lines of code, write the implementation in another and call a method from that class from inside the static method in ProcSharp.cs. For an example of this, this is how the MouseButton function is written in ProcSharp.cs

```csharp
/// <summary>
/// The last pressed mouse button. Is either LEFT, MIDDLE or RIGHT
/// </summary>
public static uint MouseButton
{
    get { return mouse.MouseButton; }
}
```

The code that manages which mouse button was last pressed is contained inside the Mouse class.

4. Create a pull request with your contribution.

## Building and testing ProcSharp using the source
The easiest way to get all dependencies set up without getting them from external sources is to use the project found in the *playground* folder.
1. Build the ProcSharp solution found in the src folder.
2. Open the ProcSharpPlayground solution found in the *playground* folder. It should automatically be using the version of ProcSharp that you build in step 1.
3. Make any changes you want in the ProcSharp source code and test them in the ProcSharpPlayground solution. Don't forget to rebuild ProcSharp after you made changes to the source code.
