/*
    Copyright (c) 2016 Kevin Norman.

    Permission is hereby granted, free of charge, to any person obtaining
    a copy of this software and associated documentation files (the "Software"),
    to deal in the Software without restriction, including without limitation
    the rights to use, copy, modify, merge, publish, distribute, sublicense,
    and/or sell copies of the Software, and to permit persons to whom the Software
    is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
    OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
    CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
    TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE
    OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using UnityEngine;
using System.Collections;
using System;

//Simple class that counts the number of seconds without locking the thread.
/// <summary>
/// A class that can time how many milliseconds since it was constructed.
/// </summary>
public class SecondsTimer
    {

        float timerStarted;
        float timerDuration;

        /// <summary>
        /// Constructor, that starts the timer with the duration set by the user
        /// </summary>
        /// <param name="duration">The duration the timer will return false for finished()</param>
        public SecondsTimer(float duration)
        {
            timerDuration = duration;
            resetTimer();
        }
        public void setDelay(float duration)
        {
            timerDuration = duration;
        }
        /// <summary>
        /// Returns number of MS since the timer was started
        /// </summary>
        /// <returns>float representing number of milliseconds since timer started</returns>
        public float timeSinceStarted()
        {
            return Time.time * 1000 - timerStarted;
        }

        /// <summary>
        /// Resets timer so that timeSinceStarted() will be zero immediately after
        /// </summary>
        public void resetTimer()
        {
            timerStarted = Time.time * 1000;
        }

        //Returns a boolean as to whether the timer is finished or not.
        /// <summary>
        /// Has the duration been exceeded yet?
        /// </summary>
        /// <returns>True if the timer is finished, false if not</returns>
        public bool finished()
        {
            if (timeSinceStarted() > timerDuration) {
                return true;
            }
            else {
                return false;
            }
        }
    }