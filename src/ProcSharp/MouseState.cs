using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;

namespace ProcSharpCore
{
    class MouseState
    {

        Dictionary<uint, bool> buttonsDown = new Dictionary<uint, bool>();

        
        internal MouseState()
        {
            buttonsDown.Add(SDL_BUTTON_LEFT, false);
            buttonsDown.Add(SDL_BUTTON_MIDDLE, false);
            buttonsDown.Add(SDL_BUTTON_RIGHT, false);
            buttonsDown.Add(SDL_BUTTON_X1, false);
            buttonsDown.Add(SDL_BUTTON_X2, false);
        }

        internal void ButtonPressed(uint button)
        {
            buttonsDown[button] = true;
        }

        internal void ButtonReleased(uint button)
        {
            buttonsDown[button] = false;
        }

        internal bool IsButtonDown(uint button)
        {
            return buttonsDown[button];
        }

        internal bool AnyButtonDown()
        {
            return buttonsDown.ContainsValue(true);
        }
        

    }
}
