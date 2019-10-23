using System;
using System.Collections.Generic;
using System.Text;
using ProcSharpCore;
using static ProcSharpCore.ProcSharp;

namespace ProcSharpGame
{
    class Game
    {
        public void Setup()
        {
            Size(800, 600);
        }

        public void Draw()
        {
            Background(255, 255, 255);
            Line(100, 100, MouseX, MouseY);
        }

    }
}
