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
using ProcSharpCore;
using static ProcSharpCore.ProcSharp;

namespace ProcSharpUser
{
    class Game
    {
        public void Setup()
        {
            Size(480, 120);
        }

        public void Draw()
        {
            if (MousePressed)
            {
                Fill(0);
            }
            else
            {
                Fill(255);
            }
            Square(MouseX, MouseY, 80);
        }

    }
}
```
This is a screenshot from the example program.

![Screenshot from example program](procsharp_screenshot.png "Screenshot from example program")
