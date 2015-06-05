/////////////////////////////////////////////////////////////////////////
// Executive.cs   -  Entry Point for executing Project.             //
// ver 1.0                                                             //
// Language:    C#, Visual Studio 12.0, .Net Framework 4.5             //
// Platform:    Dell Inspiron , Win 7, SP 3                            //
// Application: Project Number 2 Demonstration, CSE681, Fall 2014      //
// Author:      Abdulvafa Choudhary, Syracuse University               //
//              (315) 289-3444, aachoudh@syr.edu                      //
/////////////////////////////////////////////////////////////////////////
/*
 * Module Operations
 * =================
 * It's the main class, it basically starts the program instantiates the
 * first method of the Analyzer class
 * 
 * Public Interface
 * ================
 * public static main(String[])             // defines the main method
 *
 */
/*
 * Build Process
 * =============
 * Required Files:
 * FileMgr.cs Analyzer.cs
 * 
 * Compiler Command:
 *   csc /target:exe /define:TEST_SEMI Analyzer.cs
 * 
 * Maintenance History
 * ===================
 * ver 1.0 : 05 October 2014
 * - first release
 */
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
    class Executive
    {
        //Main function instantiates the Analyzer class to start the program.
        public static void Main(string[] args)
        {
            Analyzer analyzer = new Analyzer();
            analyzer.startProcessing(args);
           
        }
    }
}
