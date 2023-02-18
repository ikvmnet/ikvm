using System;
using System.IO;

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Attempts to lock an unlocked file to capture teh error message, since IOException provides no way to know
    /// whether the reason corresponds to not being locked.
    /// </summary>
    static class NotLockedHack
    {

        static readonly string message;

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static NotLockedHack()
        {
            try
            {
                var tmp = Path.GetTempFileName();

                using (var fs = new FileStream(tmp, FileMode.Create))
                {
                    try
                    {
                        fs.Unlock(0, 1);
                    }
                    catch (System.IO.IOException e)
                    {
                        message = e.Message;
                    }
                }

                File.Delete(tmp);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Returns <c>true</c> if the exception represents the file not being locked.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsErrorNotLocked(IOException e)
        {
            return e.Message == message;
        }

    }

}