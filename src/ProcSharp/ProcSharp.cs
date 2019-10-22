using System;
using SDL2;
using static SDL2.SDL;

namespace ProcSharpCore
{
    public static class ProcSharp
    {
        private static IntPtr window;
        private static IntPtr renderer;

        private static Type gameType;
        private static object gameObject;

        private static bool quit = false;

        private static Initializer initializer;
        private static Mouse mouse;
        private static Keyboard keyboard;
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
            keyboard = new Keyboard(gameType, gameObject);

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

                        case SDL_EventType.SDL_KEYDOWN:
                            keyboard.KeyDown(e);
                            break;

                        case SDL_EventType.SDL_KEYUP:
                            keyboard.KeyUp(e);
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
        public static bool MouseIsPressed
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

        #region Keyboard

        /// <summary>
        /// The keycode of the latest key that was pressed
        /// </summary>
        public static string Key
        {
            get { return keyboard.LatestKey(); }
        }

        /// <summary>
        /// true if any key on the keyboard is currently pressed down
        /// </summary>
        public static bool KeyIsPressed
        {
            get { return keyboard.AnyKeyPressed(); }
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

        #region Random

        private static Random internalRandom;

        /// <summary>
        /// Returns a random float between 0 and a given upper bound.
        /// </summary>
        /// <param name="hi">The upper bound of the range.</param>
        /// <returns>A random float between 0 and a given upper bound.</returns>
        public static float Random(float hi)
        {
            if (hi == 0)
            {
                return 0;
            }

            if (internalRandom == null)
            {
                internalRandom = new Random();
            }

            return (float) internalRandom.NextDouble() * hi;
        }

        /// <summary>
        /// Returns a random float between two given boundaries.
        /// </summary>
        /// <param name="lo">The lower bound of the range.</param>
        /// <param name="hi">The upper bound of the range.</param>
        /// <returns>A random float between two given boundaries.</returns>
        public static float Random(float lo, float hi)
        {
            if (lo >= hi) return lo;

            float diff = hi - lo;

            return Random(diff) + lo;
        }

        /// <summary>
        /// Returns a float from a random series of numbers having a mean of 0 and standard deviation of 1.
        /// </summary>
        /// <returns>A random float from a normal distribution with a mean of 0 and a standard deviation of 1.</returns>
        public static float RandomGaussian()
        {
            if (internalRandom == null)
            {
                internalRandom = new Random();
            }

            // Use the Box-Muller transform to produce a random Gaussian sample.
            // Note that we assume mean = 0 and stDev = 1.
            double uniRand1 = 1.0 - internalRandom.NextDouble();
            double uniRand2 = 1.0 - internalRandom.NextDouble();
            double randGaussian = Math.Sqrt(-2.0 * Math.Log(uniRand1)) * Math.Sin(2.0 * Math.PI * uniRand2);

            return (float) randGaussian;
        }

        /// <summary>
        /// Sets the seed value for the random generator used in the Random methods.
        /// </summary>
        /// <param name="seed">The seed with which the RNG will be initialized.</param>
        public static void RandomSeed(int seed)
        {
            internalRandom = new Random(seed);
        }

        // Declare shared private constants for noise-related functions, which are implemented using Perlin noise.
        // Perlin constants
        private const int PerlinYWrapB = 4;
        private const int PerlinYWrap = 1 << PerlinYWrapB;
        private const int PerlinZWrapB = 8;
        private const int PerlinZWrap = 1 << PerlinZWrapB;
        private const int PerlinSize = 4095;

        // Perlin variables that affect the "smoothness" of the noise
        private static int perlinOctaves = 4; // PO = 4 results in "medium smooth" noise
        private static float perlinAmpFalloff = 0.5f;

        // Shared Perlin variables
        private static int perlinTwopi, perlinPi;
        private static float[] perlinCosTable;
        private static float[] perlin;

        private static Random perlinRandom;

        // Pre-calculate the cosine lookup table once in Noise(x, y, z) and reuse it afterwards to improve performance
        private static float[] cosLookupTable;
        private const float SinCosPrecision = 0.5f;
        private const int SinCosLength= (int) (360f / SinCosPrecision);

        /// <summary>
        /// Returns the Perlin noise value at specified coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate in noise space.</param>
        /// <param name="y">The y-coordinate in noise space.</param>
        /// <param name="z">The z-coordinate in noise space.</param>
        /// <returns>A float equal to the Perlin noise value at the specified x, y, and z coordinates.</returns>
        public static float Noise(float x, float y, float z)
        {
            if (cosLookupTable == null)
            {
                float degToRad = (float)Math.PI / 180.0f;

                cosLookupTable = new float[SinCosLength];
                for (int i = 0; i < SinCosLength; i++)
                {
                    cosLookupTable[i] = (float) Math.Cos(i * degToRad * SinCosPrecision);
                }
            }

            if (perlin == null)
            {
                if (perlinRandom == null) perlinRandom = new Random();

                perlin = new float[PerlinSize + 1];
                for (int i = 0; i < PerlinSize + 1; i++)
                {
                    perlin[i] = (float) perlinRandom.NextDouble();
                }

                perlinCosTable = cosLookupTable;
                perlinTwopi = perlinPi = SinCosLength;
                perlinPi >>= 1;
            }

            // Declare variables used in the Perlin noise algorithm
            if (x < 0) x = -x;
            if (y < 0) y = -y;
            if (z < 0) z = -z;

            int xi = (int)x, yi = (int)y, zi = (int)z;
            float xf = x - xi;
            float yf = y - yi;
            float zf = z - zi;
            float rxf, ryf;

            float r = 0;
            float ampl = 0.5f;

            float n1, n2, n3;

            // Repeat the Perlin noise algorithm as many times as specified by the level of detail (perlinOctaves)
            for (int i = 0; i < perlinOctaves; i++)
            {
                int of = xi + (yi << PerlinYWrapB) + (zi << PerlinZWrapB);

                rxf = NoiseFsc(xf);
                ryf = NoiseFsc(yf);

                n1 = perlin[of & PerlinSize];
                n1 += rxf * (perlin[(of + 1) & PerlinSize] - n1);
                n2 = perlin[(of + PerlinYWrap) & PerlinSize];
                n2 += rxf * (perlin[(of + PerlinYWrap + 1) & PerlinSize] - n2);
                n1 += ryf * (n2 - n1);

                of += PerlinZWrap;
                n2 = perlin[of & PerlinSize];
                n2 += rxf * (perlin[(of + 1) & PerlinSize] - n2);
                n3 = perlin[(of + PerlinYWrap) & PerlinSize];
                n3 += rxf * (perlin[(of + PerlinYWrap + 1) & PerlinSize] - n3);
                n2 += ryf * (n3 - n2);

                n1 += NoiseFsc(zf) * (n2 - n1);

                r += n1 * ampl;
                ampl *= perlinAmpFalloff;
                xi <<= 1; xf *= 2;
                yi <<= 1; yf *= 2;
                zi <<= 1; zf *= 2;

                if (xf >= 1.0f) { xi++; xf--; }
                if (yf >= 1.0f) { yi++; yf--; }
                if (zf >= 1.0f) { zi++; zf--; }
            }

            return r;
        }

        /// <summary>
        /// Returns the Perlin noise value at specified x and y coordinates with z = 0.
        /// </summary>
        /// <param name="x">The x-coordinate in noise space.</param>
        /// <param name="y">The y-coordinate in noise space.</param>
        /// <returns>A float equal to the Perlin noise value at the specified x and y coordinates with z = 0.</returns>
        public static float Noise(float x, float y)
        {
            return Noise(x, y, 0f);
        }

        /// <summary>
        /// Returns the Perlin noise value at the specified x-coordinate with y = 0 and z = 0..
        /// </summary>
        /// <param name="x">The x-coordinate in noise space.</param>
        /// <returns>A float equal to the Perlin noise value at the specified x, y, and z coordinates.</returns>
        public static float Noise(float x)
        {
            return Noise(x, 0f, 0f);
        }

        /// <summary>
        /// Returns a transformed version of cos(i) for a given float i for usage in the Perlin noise algorithm.
        /// </summary>
        /// <param name="i">The scalar by which pi will be multiplied in the transformation.</param>
        /// <returns></returns>
        private static float NoiseFsc(float i)
        {
            return 0.5f * (1.0f - perlinCosTable[(int) (i * perlinPi) % perlinTwopi]);
        }

        /// <summary>
        /// Adjusts the level of detail produced by the Perlin noise function.
        /// </summary>
        /// <param name="levelOfDetail">The desired level of detail for the Perlin noise function.</param>
        public static void NoiseDetail(int levelOfDetail)
        {
            if (levelOfDetail > 0) perlinOctaves = levelOfDetail;
        }

        /// <summary>
        /// Adjusts the level of detail and character produced by the Perlin noise function.
        /// </summary>
        /// <param name="levelOfDetail">The desired level of detail for the Perlin noise function.</param>
        /// <param name="falloff">The desired falloff factor for each octave of the level of detail.</param>
        public static void NoiseDetail(int levelOfDetail, float falloff)
        {
            if (levelOfDetail > 0) perlinOctaves = levelOfDetail;
            if (falloff > 0) perlinAmpFalloff = falloff;
        }

        /// <summary>
        /// Sets the seed value for the random generator used in the Noise methods.
        /// </summary>
        /// <param name="seed">The seed with which the random generator will be initialized.</param>
        public static void NoiseSeed(int seed)
        {
            perlinRandom = new Random(seed);
            perlin = null; // Reset the Perlin table when changing the seed
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
