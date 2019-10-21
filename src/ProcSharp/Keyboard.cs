using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using static SDL2.SDL;

namespace ProcSharpCore
{
    class Keyboard
    {

        private MethodInfo keyPressed;
        private MethodInfo keyReleased;
        private MethodInfo keyTyped;
        string latestKey;

        private object gameObject;
        private List<string> pressedKeys = new List<string>();

        internal Keyboard(Type gameType, object gameObject)
        {
            this.gameObject = gameObject;

            keyPressed = gameType.GetMethod("KeyPressed");
            keyReleased = gameType.GetMethod("KeyReleased");
            keyTyped = gameType.GetMethod("KeyTyped");
        }

        internal void KeyDown(SDL_Event e)
        {
            pressedKeys.Add(latestKey);
            latestKey = SDL_GetKeyName(e.key.keysym.sym);            
            keyPressed?.Invoke(gameObject, null);
            keyTyped?.Invoke(gameObject, null);
        }

        internal void KeyUp(SDL_Event e)
        {
            pressedKeys.Remove(SDL_GetKeyName(e.key.keysym.sym));
            keyReleased?.Invoke(gameObject, null);
            
        }

        internal string LatestKey()
        {
            return latestKey;
        }

        internal bool AnyKeyPressed()
        {
            return pressedKeys.Count != 0;
        }



    }
}
