using System;
using System.Collections.Generic;
using System.Text;

namespace RockPaperScissors.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRunnable
    {
        /// <summary>
        /// All IApplication instances must implement the Run() method. All Applications are runnable.
        /// </summary>
        void Run();
    }
}
