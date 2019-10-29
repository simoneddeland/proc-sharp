using System;
using System.Collections.Generic;
using System.Text;
using static SDL2.SDL;
using static SDL2.SDL_mixer;

namespace ProcSharpCore
{
    public class SoundFile
    {

        internal static string contentFolderPath = "content/";

        internal static List<SoundFile> soundFiles = new List<SoundFile>();

        private IntPtr mixChunk;

        private object parent;
        private string fullPath;

        /// <summary>
        /// Creates and loads a SoundFile into memory
        /// </summary>
        /// <param name="parent">The ProcSharp game using this SoundFile</param>
        /// <param name="path">Path to the file to be loaded</param>
        public SoundFile(object parent, string path)
        {
            this.parent = parent;
            fullPath = contentFolderPath + path;
            soundFiles.Add(this);
            mixChunk = Mix_LoadWAV(fullPath);
            if (mixChunk == IntPtr.Zero)
            {
                throw new Exception($"Could not load the file {path}");
            }
        }

        /// <summary>
        /// Plays the soundfile
        /// </summary>
        public void Play()
        {
            Mix_PlayChannel(-1, mixChunk, 0);
        }

        internal void Destroy()
        {
            Mix_FreeChunk(mixChunk);

        }

    }
}
