/////////////////////////////////////////////////////////////////////////
// Display.cs   -  Class for displaying types, functions and          //
//                 Relationships in a format to the console           //
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
 * Display module displays all the display all the information 
 * on the console. All the display takes place within this class
 * 
 * Public Interface
 * ================
 * public void displayResults()                        // functions which displays the function compexity
 * public void displayRel()                            // this function displays the relationship between types
 * public void createXML()                            //  this function sotres relationship in XML
 * public void createFuncXML()                        // this function stores function complexity and size in XML
 * public void displaySummary()                       //this function displays the summary of analysis
 */
//
/*
 * Build Process
 * =============
 * Required Files:
 *   Display.cs
 * 
 * Compiler Command:
 *   csc /target:exe /define:TEST_DISPLAY Display.cs
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
using System.Xml;
using System.Xml.Linq;

namespace CodeAnalysis
{
    //Handles all the display to the console output
    class Display
    {

        public static int numberOfFiles
        {
            get;
            set;
        }
        public static int numberOffunctions
        {
            get;
            set;
        }
        /////////////////////////////////////////////////////////
        // This function displays the results of function 
        // complexity and size analysis
        public void displayResults(List<Elem> table, Object file)
        {
            int size = 0;


            Console.WriteLine("*****************************************************************************");
            Console.WriteLine("{0}", file as string);
            numberOfFiles++;
            Console.WriteLine("*****************************************************************************");


            foreach (Elem e in table)
            {

                if (e.type.Equals("function"))
                {

                    Console.WriteLine("Function Name: {0}", e.name);
                    size = e.end - e.begin + 1;
                    Console.WriteLine("Function Size: {0}", size);
                    Console.WriteLine("Function Complexity: {0}", e.cc);
                    Console.WriteLine();
                    numberOffunctions++;
                }
                else if (e.type == "class")
                {

                    Console.Write(e.type.ToUpper());
                    Console.WriteLine(" : {0}", e.name);

                }
            }
        }
        /////////////////////////////////////////////////////////
        // This function displays the results of relationship 
        // between types analysis

        public void displayRel(List<Elem> table, Object file)
        {

            Console.WriteLine("*****************************************************************************");
            Console.WriteLine("{0}", file as string);
            Console.WriteLine("*****************************************************************************");

            foreach (Elem e in table)
            {
                if (e.type == "class")
                {

                    Console.WriteLine();
                    Console.WriteLine("{0}  {1}", e.type.ToUpper(), e.name);
                    Console.WriteLine("=========================================================");
                }
                else if (!(e.type == "struct" || e.type == "enum" || e.type=="interface"))
                {
                    Console.WriteLine("{0,10} {1,25}", e.type, e.name);
                }
            }

            Console.WriteLine();
        }
    

        /////////////////////////////////////////////////////////////
        // This function displays the total number of files tested
        //for function analysis
        public void displaySummary()
        {
            Console.WriteLine("\n***********************************************");
            Console.WriteLine("================ Summary ======================");
            Console.WriteLine("Number of files tested = {0}", numberOfFiles);
            Console.WriteLine("Number of functions found = {0}", numberOffunctions);
            Console.WriteLine();
        }

#if(TEST_DISPLAY)

        static void Main(string[] args)
        {
            Console.Write("\n  Demonstrating Display");
            Console.Write("\n ======================\n");
            String fileName = "C:\\test\\TestFile.cs";
            List<Elem> table=new List<Elem>();  
            
            Elem typeElement = new Elem();
            typeElement.type = "class";
            typeElement.name = "Test";

            Elem typeElement1 = new Elem();
            typeElement1.type = "namespace";
            typeElement1.name = "Test";

            Elem typeElement2 = new Elem();
            typeElement2.type = "function";
            typeElement2.name = "Test";
            typeElement2.begin = 20;
            typeElement2.end = 30;
            typeElement2.cc = 2;
                        
                        
            table.Add(typeElement);
            table.Add(typeElement1);
            table.Add(typeElement2);

            Display display=new Display();

            Console.WriteLine("Testing displayResults function");
            display. displayResults(table,fileName);

            Console.WriteLine("Testing displayResults function");
            display.displayRel(table,fileName);

            Console.WriteLine("Testing displayResults function");
            display.createXML(table);

            Console.WriteLine("Testing displayResults function");
            display.createFuncXML(table);

            Console.WriteLine("Testing displayResults function");
            display.displaySummary();
            Console.Write("\n\n");
        }
#endif

    }
}
