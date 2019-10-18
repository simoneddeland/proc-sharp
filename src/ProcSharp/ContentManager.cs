using System;
using System.Collections.Generic;
using System.Text;
using SDL2;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace ProcSharpCore
{
    class ContentManager
    {
        Dictionary<string, IntPtr> images = new Dictionary<string, IntPtr>();

        IntPtr renderer;
        string contentFolderPath;

        public ContentManager(IntPtr renderer, string contentFolderPath)
        {
            this.renderer = renderer;
            this.contentFolderPath = contentFolderPath;
        }

        public PImage LoadImage(string url)
        {
            PImage pimage = new PImage(LoadTexture(url));   

            return pimage;
        }

        IntPtr LoadTexture(string url)
        {

            url = contentFolderPath + url;

            if (images.ContainsKey(url))
            {
                return images[url];
            }

            IntPtr texture;

            IntPtr loadedSurface = IMG_Load(url);
            if (loadedSurface == null)
            {
                throw new Exception("Could not load image " + url);
            }

            texture = SDL_CreateTextureFromSurface(renderer, loadedSurface);

            if (texture == null)
            {
                throw new Exception("Unable to create texture from image " + url);
            }

            images.Add(url, texture);
            return texture;
        }

        internal void Destroy()
        {
            foreach (var kvp in images)
            {
                SDL_DestroyTexture(kvp.Value);
            }

            images.Clear();
        }
    }
}
