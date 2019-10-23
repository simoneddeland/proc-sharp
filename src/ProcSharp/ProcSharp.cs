using System;
using SDL2;
using static SDL2.SDL;

namespace ProcSharpCore
{
    public class ProcSharp
    {
        private static IntPtr window;
        private static IntPtr renderer;

        private static Type gameType;
        private static object gameObject;

        private static bool quit = false;

        private static Initializer initializer;
        private static Mouse mouse;
        private static ContentManager contentManager;
        
        private static SDL_Color fillColor;
        private static SDL_Color strokeColor;

        #region PUBLIC_CONSTANTS

        /// <summary>
        /// The left mouse button
        /// </summary>
        public const uint LEFT = 1;

        /// <summary>
        /// The middle mouse button
        /// </summary>
        public const uint MIDDLE = 2;

        /// <summary>
        /// The right mouse button
        /// </summary>
        public const uint RIGHT = 3;

        #endregion

        public static void Initialize(Type type, object game)
        {
            gameType = type;
            gameObject = game;

            // Default fill white
            fillColor.r = 255;
            fillColor.g = 255;
            fillColor.b = 255;
            fillColor.a = 255;

            // Default stroke gray
            strokeColor.r = 100;
            strokeColor.g = 100;
            strokeColor.b = 100;
            strokeColor.a = 255;

            initializer = new Initializer();
            initializer.Initialize(ref window, ref renderer);

            // Set up all services
            mouse = new Mouse(gameType, gameObject);
            contentManager = new ContentManager(renderer, "content/");            

            SDL_RenderClear(renderer);

            // Call setup from the user
            var userSetup = gameType.GetMethod("Setup");
            userSetup?.Invoke(gameObject, null);
            
            SDL_RenderPresent(renderer);
            
            // Start the main loop
            MainLoop();
        }

        private static void MainLoop()
        {

            var userDraw = gameType.GetMethod("Draw");
            SDL_Event e;

            while (!quit)
            {
                // Update services
                mouse.Update();

                while (SDL_PollEvent(out e) != 0)
                {
                    switch (e.type)
                    {
                        case SDL_EventType.SDL_QUIT:
                            quit = true;
                            break;

                        case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                            mouse.MouseButtonDown(e);
                            break;

                        case SDL_EventType.SDL_MOUSEBUTTONUP:
                            mouse.MouseButtonUp(e);
                            break;

                        case SDL_EventType.SDL_MOUSEWHEEL:
                            mouse.MouseWheel(e);
                            break;

                    }
                }

                // Call the users draw function
                userDraw?.Invoke(gameObject, null);

                // Render to the screen
                SDL_RenderPresent(renderer);
            }

            InternalExit();
        }

        private static void InternalExit()
        {
            // CLEANUP

            contentManager.Destroy();

            SDL_DestroyRenderer(renderer);
            SDL_DestroyWindow(window);
            SDL_Quit();
        }

        private static void SetColor(SDL_Color color)
        {
            SDL_SetRenderDrawColor(renderer, color.r, color.b, color.g, color.a);
        }



        #region Public methods

        /// <summary>
        /// Sets the stroke color
        /// </summary>
        /// <param name="rgb">The RGB part of the color (all get the same value)</param>
        public static void Fill(float rgb)
        {
            fillColor.r = Convert.ToByte(rgb);
            fillColor.g = Convert.ToByte(rgb);
            fillColor.b = Convert.ToByte(rgb);
            fillColor.a = 255;
        }

        /// <summary>
        /// Sets the fill color
        /// </summary>
        /// <param name="r">Red part of the color, 0 - 255</param>
        /// <param name="g">Green part of the color, 0 - 255</param>
        /// <param name="b">Blue part of the color, 0 - 255</param>
        /// <param name="a">Alpha part of the color, 0 - 255</param>
        public static void Fill(float r, float g, float b, float a)
        {
            fillColor.r = Convert.ToByte(r);
            fillColor.g = Convert.ToByte(g);
            fillColor.b = Convert.ToByte(b);
            fillColor.a = Convert.ToByte(a);
        }

        /// <summary>
        /// Sets the stroke color
        /// </summary>
        /// <param name="rgb">The RGB part of the color (all get the same value)</param>
        public static void Stroke(float rgb)
        {
            strokeColor.r = Convert.ToByte(rgb);
            strokeColor.g = Convert.ToByte(rgb);
            strokeColor.b = Convert.ToByte(rgb);
            strokeColor.a = 255;
        }

        /// <summary>
        /// Sets the stroke color
        /// </summary>
        /// <param name="r">Red part of the color, 0 - 255</param>
        /// <param name="g">Green part of the color, 0 - 255</param>
        /// <param name="b">Blue part of the color, 0 - 255</param>
        /// <param name="a">Alpha part of the color, 0 - 255</param>
        public static void Stroke(float r, float g, float b, float a)
        {
            strokeColor.r = Convert.ToByte(r);
            strokeColor.g = Convert.ToByte(g);
            strokeColor.b = Convert.ToByte(b);
            strokeColor.a = Convert.ToByte(a);            
        }

        /// <summary>
        /// Sets the window size
        /// </summary>
        /// <param name="width">Width of the window in pixels</param>
        /// <param name="height">Height of the window in pixels</param>
        public static void Size(int width, int height)
        {
            SDL_SetWindowSize(window, width, height);
            SDL_SetWindowPosition(window, SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED);
        }

        /// <summary>
        /// Exits the program after the current Draw has finished
        /// </summary>
        public static void Exit()
        {
            quit = true;
        }

        #region Mouse
        /// <summary>
        /// The X-coordinate of the mouse
        /// </summary>
        public static int MouseX
        {
            get
            { return mouse.MouseX; }
        }

        /// <summary>
        /// The Y-coordinate of the mouse
        /// </summary>
        public static int MouseY
        {
            get
            { return mouse.MouseY; }
        }

        /// <summary>
        /// The X-coordinate of the mouse during the previous Draw
        /// </summary>
        public static int PMouseX
        {
            get { return mouse.PMouseX; }
        }

        /// <summary>
        /// The Y-coordinate of the mouse during the previous Draw
        /// </summary>
        public static int PMouseY
        {
            get { return mouse.PMouseY; }
        }

        /// <summary>
        /// true if any mouse button is currently being pressed, otherwise false
        /// </summary>
        public static bool MousePressed
        {
            get { return mouse.AnyButtonDown(); }
        }

        /// <summary>
        /// The last pressed mouse button. Is either LEFT, MIDDLE or RIGHT
        /// </summary>
        public static uint MouseButton
        {
            get { return mouse.MouseButton; }
        }

        #endregion

        #region Images

        /// <summary>
        /// Loads the image with the specified filename
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static PImage LoadImage(string file)
        {
            return contentManager.LoadImage(file);
        }

        /// <summary>
        /// Draws an image onto the screen
        /// </summary>
        /// <param name="pimage">The PImage to draw</param>
        /// <param name="x">X-coordinate of the top left corner</param>
        /// <param name="y">Y-coordinate of the top left corner</param>
        public static void Image(PImage pimage, float x, float y)
        {
            SDL_Rect destRect;
            destRect.x = (int)x;
            destRect.y = (int)y;
            destRect.w = pimage.width;
            destRect.h = pimage.height;
            SDL_RenderCopy(renderer, pimage.texture, IntPtr.Zero, ref destRect);
        }

        /// <summary>
        /// Draws an image onto the screen
        /// </summary>
        /// <param name="pimage"></param>
        /// <param name="x">X-coordinate of the top left corner</param>
        /// <param name="y">Y-coordinate of the top left corner</param>
        /// <param name="width">The width that the image will be drawn in</param>
        /// <param name="height">The height that the image will be drawn in</param>
        public static void Image(PImage pimage, float x, float y, float width, float height)
        {
            SDL_Rect destRect;
            destRect.x = (int)x;
            destRect.y = (int)y;
            destRect.w = (int)width;
            destRect.h = (int)height;
            SDL_RenderCopy(renderer, pimage.texture, IntPtr.Zero, ref destRect);
        }

        #endregion


        /// <summary>
        /// Draws a square in the currently selected color
        /// </summary>
        /// <param name="x">X-coordinate of the top left corner of the square</param>
        /// <param name="y">Y-coordinate of the top left corner of the square</param>
        /// <param name="extent">The length of the squares side</param>
        public static void Square(float x, float y, float extent)
        {
            SDL_Rect drawRect;
            drawRect.x = (int)x;
            drawRect.y = (int)y;
            drawRect.h = (int)extent;
            drawRect.w = (int)extent;

            // Draw the filling
            SetColor(fillColor);
            SDL_RenderFillRect(renderer, ref drawRect);
            // Draw the stroke
            SetColor(strokeColor);
            SDL_RenderDrawRect(renderer, ref drawRect);

        }
                
        /// <summary>
        /// Draws a point, a coordinate in space at the dimension of one pixel
        /// </summary>
        /// <param name="x">x-coordinate of the point</param>
        /// <param name="y">y-coordinate of the point</param>
        public static void Point(float x, float y)
        {
            SDL_Point drawPoint;
            drawPoint.x = (int)x;
            drawPoint.y = (int)y;
            
            // Draw the stroke
            SetColor(strokeColor);
            SDL_RenderDrawPoint(renderer, drawPoint.x, drawPoint.y);

        }

        /// <summary>
        /// Draws a rectangle to the screen
        /// </summary>
        /// <param name="a">x-coordinate of the rectangle by default</param>
        /// <param name="b">y-coordinate of the rectangle by default</param>
        /// <param name="c">width of the rectangle by default</param>
        /// <param name="d">height of the rectangle by default</param>
        public static void Rectangle(float a, float b, float c, float d)
        {
            SDL_Rect drawRect;
            drawRect.x = (int)a;
            drawRect.y = (int)b;
            drawRect.h = (int)c;
            drawRect.w = (int)d;

            // Draw the filling
            SetColor(fillColor);
            SDL_RenderFillRect(renderer, ref drawRect);
            // Draw the stroke
            SetColor(strokeColor);
            SDL_RenderDrawRect(renderer, ref drawRect);

        }
        
        /// <summary>
        /// Clears the background with the specified color
        /// </summary>
        /// <param name="v1">Red part of the color, 0 - 255</param>
        /// <param name="v2">Green part of the color, 0 - 255</param>
        /// <param name="v3">Blue part of the color, 0 - 255</param>
        public static void Background(float v1, float v2, float v3)
        {
            byte currentRed;
            byte currentGreen;
            byte currentBlue;
            byte currentAlpha;
            SDL_GetRenderDrawColor(renderer, out currentRed, out currentGreen, out currentBlue, out currentAlpha);
            SDL_SetRenderDrawColor(renderer, Convert.ToByte(v1), Convert.ToByte(v2), Convert.ToByte(v3), 255);
            SDL_RenderClear(renderer);
            SDL_SetRenderDrawColor(renderer, currentRed, currentGreen, currentBlue, currentAlpha);

        }

        #endregion
    }
}
