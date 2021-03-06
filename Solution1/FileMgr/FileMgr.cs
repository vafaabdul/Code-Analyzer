﻿/////////////////////////////////////////////////////////////////////////
// FileMgr.cs   -  Class manages the files from the system             //
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
 * Filemanager class used to manage the files which is used to get from the path given by 
 * the user. This class will scroll through the list and give input to the next function 
 * one by one.
 * 
 * Public Interface
 * ================
 * public List<string> getFiles()                    //this function gets all files to be processed
 *  public void isRecurse()                          //this functions determines whether  to look in directories
 *  public void findFiles()                          // this function will get all the files pres for files sent at the path given by the user
 *  public void addPattern()                         // This function will add the patterns of files to be analyzed
 */
/*
 * Build Process
 * =============
 * Required Files:
 *   Executive.cs FileMgr.cs
 * 
 * Compiler Command:
 *   csc /target:exe /define:TEST_FILEMGR FileMgr.cs
 * 
 * Maintenance History
 * ===================
 * ver 1.0 : 05 October 2014
 * - first release
 * 
 */
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CodeAnalysis
{
    public class FileMgr
    {

        private List<string> files = new List<string>();
        private List<string> patterns = new List<string>();
        private bool recurse = false;

        public void isRecurse(bool value){
            recurse=value;
        }
        public void findFiles(string path)
        {
            if (patterns.Count == 0) {
                addPattern("*.*");
            
            }
            foreach(string pattern in patterns)
            {
                string[] newFiles = Directory.GetFiles(path, pattern);
                for (int i = 0; i < newFiles.Length; ++i)
                {
                    newFiles[i] = Path.GetFullPath(newFiles[i]);
               
                }
                files.AddRange(newFiles);
            }
            if(recurse)
            {
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                    findFiles(dir);
            }

        }

        public void addPattern(string pattern)
        {
            patterns.Add(pattern);
        }
        
        public List<string> getFiles()
        {
            return files;
        }

#if(TEST_FILEMGR)
        static void Main(string[] args)
        {
            Console.Write("\n  Testing FileMgr Class");
            Console.Write("\n =======================\n");

            FileMgr fm = new FileMgr();
            fm.addPattern("*.cs");
            fm.findFiles("../../");
            List<string> files = fm.getFiles();
            foreach (string file in files)
                Console.Write("\n  {0}", file);
            Console.Write("\n\n");
            
        }
#endif
    }
}
