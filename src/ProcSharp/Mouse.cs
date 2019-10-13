using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using static SDL2.SDL;

namespace ProcSharpCore
{
    class Mouse
    {

        private int x = 0;
        private int y = 0;
        private int previousX = 0;
        private int previousY = 0;


        private object gameObject;

        private MouseState mouseState = new MouseState();

        private MethodInfo mouseMoved;
        private MethodInfo mouseDragged;
        private MethodInfo mousePressed;
        private MethodInfo mouseReleased;
        private MethodInfo mouseClicked;
        private MethodInfo mouseWheel;

        private uint lastPressedButton = 0;


        public Mouse(Type gameType, object gameObject)
        {
            this.gameObject = gameObject;

            mouseMoved = gameType.GetMethod("MouseMoved");            
            mousePressed = gameType.GetMethod("MousePressed");
            mouseReleased = gameType.GetMethod("MouseReleased");
            mouseClicked = gameType.GetMethod("MouseClicked");
            mouseDragged = gameType.GetMethod("MouseDragged");
            mouseWheel = gameType.GetMethod("MouseWheel", new Type[] { typeof(MouseEvent)});

        }

        internal void Update()
        {
            previousX = x;
            previousY = y;

            SDL_GetMouseState(out x, out y);

            if (x != previousX || y != previousY)
            {
                if (AnyButtonDown())
                {
                    mouseDragged?.Invoke(gameObject, null);
                }
                else
                {
                    mouseMoved?.Invoke(gameObject, null);
                }
                
            }
        }

        internal int MouseX
        {
            get { return x; }
        }
        
        internal int MouseY
        {
            get { return y; }
        }

        internal int PMouseX
        {
            get { return previousX; }
        }
        
        internal int PMouseY
        {
            get { return previousY; }
        }

        internal void MouseButtonDown(SDL_Event e)
        {
            lastPressedButton = e.button.button;

            mouseState.ButtonPressed(e.button.button);
            mousePressed?.Invoke(gameObject, null);
            
        }

        internal void MouseButtonUp(SDL_Event e)
        {
            mouseState.ButtonReleased(e.button.button);
            if (!AnyButtonDown())
            {
                lastPressedButton = 0;
            }
            mouseReleased?.Invoke(gameObject, null);
            mouseClicked?.Invoke(gameObject, null);           

        }

        internal bool AnyButtonDown()
        {
            return mouseState.AnyButtonDown();
        }

        internal uint MouseButton
        {
            get { return lastPressedButton; }
        }

        internal void MouseWheel(SDL_Event e)
        {
            MouseEvent mouseEvent = new MouseEvent();
            mouseEvent.SetCount(e.wheel.y);
            mouseWheel?.Invoke(gameObject, new object[] { mouseEvent });
        }


    }
}
