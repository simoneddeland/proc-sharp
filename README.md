# ProcSharp
An implementation of the Processing framework in C#

## Try out ProcSharp
Create a new Console App (.NET Core) and add a reference to ProcSharp using NuGet. The following code shows how to set up a simple ProcSharp program.

### Program.cs
```csharp
using System;
using ProcSharpCore;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcSharp.Initialize(typeof(Game), new Game());
        }
    }
}
```

### Game.cs
```csharp
using System;
using System.Collections.Generic;
using System.Text;
using static ProcSharpCore.ProcSharp;

namespace ConsoleApp1
{
    class Game
    {
        public void Setup()
        {
            Color(255, 255, 255, 255);
        }

        public void Draw()
        {
            Background(0, 100, 100);
            Color(255, 0, 0, 0);
            Square(MouseX, MouseY, 50);
        }
    }
}
```
