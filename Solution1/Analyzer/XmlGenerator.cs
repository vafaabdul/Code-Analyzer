/////////////////////////////////////////////////////////////////////////
// XMLGenerator.cs   -  Conversion of types, functions and             //
//                      relationships to XML file.                     //
//                                                                     //
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
 * XMLGenerator module stores the function and relationship 
 * analysis  in an XML.
 * 
 * Public Interface
 * ================
 * public void createRelationshipXML                  //  this function sotres relationship in XML
 * public void createFuncXML()                        // this function stores function complexity and size in XML
 */
//
/*
 * Build Process
 * =============
 * Required Files:
 *   XMLGenerator.cs
 * 
 * Compiler Command:
 *   csc /target:exe /define:TEST_XML XMLGenerator.cs.cs
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
    //Creates XML files and stores the output data
    class XmlGenerator
    {

        /////////////////////////////////////////////////////////////
        // This function creates XML for  the results of function 
        // complexity and size analysis
        public void createRelationshipXML(List<Elem> table)
        {

            XDocument xml = new XDocument();
            xml.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            XElement rel = new XElement("TypeRelationships");
            xml.Add(rel);
            XElement newClass = null;
            foreach (Elem e in table)
            {
                if (e.type == "class")
                {
                    if (newClass != null)
                        rel.Add(newClass);
                    newClass = new XElement(e.type, new XAttribute("Name", e.name));

                }
                else if (!(e.type == "struct" || e.type == "enum" || e.type == "interface"))
                {
                    XElement child = new XElement(e.type, e.name);
                    newClass.Add(child);

                }
            }

            if (newClass != null)
                rel.Add(newClass);


            xml.Save("../../../XMLOutput/Relationship.xml");

            Console.WriteLine("\n- XML File Created as Relationship.xml");
        }
        ////////////////////////////////////////////////////////////////
        // This function creates XML for  the results of relationship 
        // between types analysis

        public void createFuncXml(List<Elem> table)
        {

            XDocument xml = new XDocument();
            xml.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            XElement root = new XElement("Analysis");
            xml.Add(root);


            XElement newClass = null;
            XElement nspc = null;
            foreach (Elem e in table)
            {
                if (e.type == "namespace")
                {
                    if (nspc != null)
                        root.Add(nspc);
                    nspc = new XElement(e.type, new XAttribute("Name", e.name));
                }
                if (e.type == "class")
                {
                    newClass = new XElement(e.type, new XAttribute("Name", e.name));
                    nspc.Add(newClass);

                }
                else if (e.type == "function")
                {
                    int size = e.end - e.begin + 1;
                    XElement child = new XElement(e.type);
                    XElement funcName = new XElement("Name", e.name);
                    XElement funcsize = new XElement("Size", size);
                    XElement funcComp = new XElement("Complexity", e.cc);
                    child.Add(funcName);
                    child.Add(funcsize);
                    child.Add(funcComp);
                    newClass.Add(child);

                }
            }


            if (nspc != null)
                root.Add(nspc);

            xml.Save("../../../XMLOutput/CodeAnalysis.xml");

            Console.WriteLine("\n- XML File Created as CodeAnalysis.xml");
        }

#if(TEST_XML)

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

            XMLGenerator xmlGen=new Display();

            Console.WriteLine("Testing displayResults function");
            xmlGen.createRelationshipXML(table);

            Console.WriteLine("Testing displayResults function");
            xmlGen.createFuncXML(table);

           
            Console.Write("\n\n");
        }
#endif
    }
}
