/////////////////////////////////////////////////////////////////////////
// Analyzer.cs   -  Analysis the files for finding function complexity //
//                   size and elationship btween types                 //
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
 * Analyzer class processes the file one after the other and calls the class
 * Parser to find relationship between types and calculate complexity of functions.
 * It also calls the display class to display the results.
 * 
 * Public Interface
 * ================
 * public string[] getFiles()                           \\ calls the Filemanager to get the list of files to process.
 *                                                      .
 * public void doAnalysis()                              \\process the list of iles to calculate the functions size 
 *                                                        and complexity.
 * 
 * public List<Elem> findRelationShipPass1()              \\finds the types present in the files for furthe processing
 * 
 * public void findRelationShipPass2()                    \\finds the relationship between types
 * 
 * public void startProcessing()                          \\This method processes the command line arguments and does
 *                                                        the necessary analysis
 */
//
/*
 * Build Process
 * =============
 * Required Files:
 *   Analyzer.cs Display.cs FileMgr.cs Parser.cs
 * 
 * Compiler Command:
 *   csc /target:exe /define:TEST_ANALYZER Analyzer.cs Display.cs FileMgr.cs Parser.cs
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

namespace CodeAnalysis
{

    class Analyzer
    {
        /////////////////////////////////////////////////////////
        // This function gets the list of files matching the 
        //pattern given by the user

        public string[] getFiles(string path, List<string> patterns, bool recur)
        {
            FileMgr fm = new FileMgr();
            fm.isRecurse(recur);
            foreach (string pattern in patterns)
                fm.addPattern(pattern);
            fm.findFiles(path);
            return fm.getFiles().ToArray();
        }
        /////////////////////////////////////////////////////////////
        // This method calculates the function size and complexity

        public void doAnalysis(string[] files, bool writeXML)
        {
            Display display = new Display();
            List<Elem> rsltTable = new List<Elem>();

            foreach (object file in files)
            {
                CSsemi.CSemiExp semi = new CSsemi.CSemiExp();
                semi.displayNewLines = false;
                if (!semi.open(file as string))
                {
                    Console.Write("\n  Can't open {0}\n\n", file);
                    return;
                }

                BuildCodeAnalyzer builder = new BuildCodeAnalyzer(semi);
                Parser parser = builder.build();
                try
                {
                    while (semi.getSemi())
                        parser.parse(semi);
                }
                catch (Exception ex)
                {
                    Console.Write("\n\n  {0}\n", ex.Message);
                }
                Repository rep = Repository.getInstance();
                List<Elem> table = rep.locations;

                foreach (Elem e in table)
                {
                    if (!(e.type == "struct" || e.type == "enum"))
                    {
                        Elem typeElement = new Elem();
                        typeElement.type = e.type;
                        typeElement.name = e.name;
                        typeElement.begin = e.begin;
                        typeElement.end = e.end;
                        typeElement.cc = e.cc;
                        rsltTable.Add(typeElement);

                    }
                }

                //display results
                display.displayResults(table, file);

                semi.close();
            }
            display.displaySummary();

            XmlGenerator generateXML = new XmlGenerator();
            if (writeXML)
                generateXML.createFuncXml(rsltTable);
        }
        /////////////////////////////////////////////////////////
        // This method detcet types defined in the files and
        // stores them for finding relationship between files

        public List<Elem> findRelationShipPass1(string[] files)
        {
            List<Elem> typesTable = new List<Elem>();
            foreach (object file in files)
            {
               
                CSsemi.CSemiExp semi = new CSsemi.CSemiExp();
                semi.displayNewLines = false;
                if (!semi.open(file as string))
                {
                    Console.Write("\n  Can't open {0}\n\n", file);
                    return null;
                }

                BuildCodeAnalyzer builder = new BuildCodeAnalyzer(semi);
                Parser parser = builder.build();
                try
                {
                    while (semi.getSemi())
                        parser.parse(semi);
                }
                catch (Exception ex)
                {
                    Console.Write("\n\n  {0}\n", ex.Message);
                }
                Repository rep = Repository.getInstance();
                List<Elem> table = rep.locations;

                foreach (Elem e in table)
                {
                    if (e.type == "class" || e.type == "struct" || e.type == "enum")
                    {
                        Elem typeElement = new Elem();
                        typeElement.type = e.type;
                        typeElement.name = e.name;
                        typesTable.Add(typeElement);

                    }
                }
                semi.close();
            }

            return typesTable;
        }
        /////////////////////////////////////////////////////////
        // This method find reltionship between types
        public void findRelationShipPass2(string[] files, List<Elem> typesList, bool writeXML)
        {
            Display disp = new Display();
            List<Elem> typesTable = new List<Elem>();

            foreach (object file in files)
            {


                CSsemi.CSemiExp semi = new CSsemi.CSemiExp();
                semi.displayNewLines = false;
                if (!semi.open(file as string))
                {
                    Console.Write("\n  Can't open {0}\n\n", file);
                    return;
                }

                BuildCodeAnalyzer builder = new BuildCodeAnalyzer(semi);
                Parser parser = builder.build(typesList);
                try
                {
                    while (semi.getSemi())
                        parser.parse(semi);
                }
                catch (Exception ex)
                {
                    Console.Write("\n\n  {0}\n", ex.Message);
                }
                Repository rep = Repository.getInstance();
                List<Elem> tableRel = rep.locations;

                foreach (Elem e in tableRel)
                {
                    if (!(e.type == "struct" || e.type == "enum"))
                    {
                        Elem typeElement = new Elem();
                        typeElement.type = e.type;
                        typeElement.name = e.name;
                        typesTable.Add(typeElement);

                    }
                }
                disp.displayRel(tableRel, file);


                semi.close();
            }

            XmlGenerator generateXML = new XmlGenerator();
            if (writeXML)
                generateXML.createRelationshipXML(typesTable);
        }

        /////////////////////////////////////////////////////////
        // This method processes the command line arguments and does
        // the necessary analysis as per the arguments
        public void startProcessing(string[] args)
        {
            string path = "../..";
            List<string> patterns = new List<string>();
            List<Elem> typesTable = new List<Elem>();
            int noOfArgs = args.Length;
            bool recursive = false;
            bool findRel = false;
            bool writeXML = false;

            if (args.Length == 0)
            {

                path = "../../";
                patterns.Add("*.cs");
            }
            else
            {
                String[] pattern = args[0].Split(',');

                foreach (String p in pattern)
                {
                    patterns.Add(p);
                }

                for (int i = 0; i < noOfArgs; i++)
                {
                    if (args[i].Contains("/S"))
                        recursive = true;

                    if (args[i].Contains("/R"))
                        findRel = true;

                    if (args[i].Contains("/X"))
                        writeXML = true;

                }
                path = args[1];
            }

            string[] files = getFiles(path, patterns, recursive);

            if (findRel)
            {
                typesTable = findRelationShipPass1(files);
                findRelationShipPass2(files, typesTable, writeXML);
                
            }
            else
               doAnalysis(files, writeXML);

               Console.ReadLine();

        }
    }

#if(TEST_ANALYZER)
        static void Main(string[] args)
        {
            Console.Write("\n  Testing Analyzer Class");
            Console.Write("\n =======================\n");
            Analyzer analyzer = new Analyzer();
            analyzer.startProcessing(args);
            Console.Write("\n\n");
            
        }
#endif
}
