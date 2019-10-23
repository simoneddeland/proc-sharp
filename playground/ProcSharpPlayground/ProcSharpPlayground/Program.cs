using System;
using ProcSharpCore;

namespace ProcSharpGame
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcSharp.Initialize(typeof(Game), new Game());
        }
    }
}
