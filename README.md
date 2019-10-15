# ProcSharp
An interpretation of the Processing framework in C#

## Try out ProcSharp
Create a new Console App (.NET Core) and add a reference to ProcSharp using NuGet. The following code shows how to set up a simple ProcSharp program.

### Program.cs
```csharp
using System;
using ProcSharpCore;

namespace ProcSharpUser
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

### Drawing images
The following example shows how to load and draw an image. The Program.cs is the same as in the previous example.

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

        PImage testImage;

        public void Setup()
        {
            Size(1280, 720);
            testImage = LoadImage("moose.png");
        }

        public void Draw()
        {
            Background(255, 255, 255);

            Image(testImage, MouseX, MouseY, 100, 200);

        }

    }
}
```


### Building ProcSharp from source
When building and using ProcSharp from the source code, you need to include the binaries of SDL2 for your platform and copy them to the output directory of your program. They can be found [here](https://www.libsdl.org/download-2.0.php).

