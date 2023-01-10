using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

namespace CodisConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Welcome to Codis Console Application");                
                showMenu();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Main Method", ex.ToString());
            }

        }
        public static void showMenu()
        {
            try
            {
                Console.WriteLine("Please select your operation");
                Console.WriteLine("Type \"add person\" to Add a person - you have to provide First Name, Last Name, Date of Birth and NickName(optional))");
                Console.WriteLine("Type \"edit person\" to Edit a person - you have to provide ID of the intended person to edit and new details");
                Console.WriteLine("Type \"view all\" to View all persons - no input required");
                Console.WriteLine("Type \"search person\" to Search a person - you have to provide ID or First Name or Last Name of the intended person");
                Console.WriteLine("Type \"delete person\" to Delete a person - you have to provide ID of the intended person");
                Console.WriteLine("Type \"add address\" to Add an address to a person - you have to provide ID of the intended person");
                Console.WriteLine("Type \"delete address\" to Delete an address to a person - you have to provide ID of the intended person");
                Console.WriteLine("Type \"edit address\" to Edit an address to a person - you have to provide ID of the intended person");
                Console.WriteLine("Type \"exit\" to terminate the application");
                switch (Console.ReadLine().ToLowerInvariant())
                {
                    case "add person":
                        Person.Create();
                        Console.WriteLine("Do you want to continue (Y/N)");
                        if (Console.ReadLine().ToLowerInvariant().Equals("y")) { showMenu(); }
                        break;
                    case "edit person":
                        Person.Edit();
                        Console.WriteLine("Do you want to continue (Y/N)");
                        if (Console.ReadLine().ToLowerInvariant().Equals("y")) { showMenu(); }
                        break;
                    case "view all":
                        Person.View();
                        Console.WriteLine("Do you want to continue (Y/N)");
                        if (Console.ReadLine().ToLowerInvariant().Equals("y")) { showMenu(); }
                        break;
                    case "search person":
                        Person.Read();
                        Console.WriteLine("Do you want to continue (Y/N)");
                        if (Console.ReadLine().ToLowerInvariant().Equals("y")) { showMenu(); }
                        break;
                    case "delete person":
                        Person.Delete();
                        Console.WriteLine("Do you want to continue (Y/N)");
                        if (Console.ReadLine().ToLowerInvariant().Equals("y")) { showMenu(); }
                        break;
                    case "add address":
                        Address.CreateAddress();
                        Console.WriteLine("Do you want to continue (Y/N)");
                        if (Console.ReadLine().ToLowerInvariant().Equals("y")) { showMenu(); }
                        break;
                    case "edit address":
                        Address.EditAddress();
                        Console.WriteLine("Do you want to continue (Y/N)");
                        if (Console.ReadLine().ToLowerInvariant().Equals("y")) { showMenu(); }
                        break;
                    case "delete address":
                        Address.DeleteAddress();
                        Console.WriteLine("Do you want to continue (Y/N)");
                        if (Console.ReadLine().ToLowerInvariant().Equals("y")) { showMenu(); }
                        break;
                    case "exit":
                        Console.WriteLine("Are you sure you want to exit (Y/N)");
                        if (Console.ReadLine().ToLowerInvariant().Equals("y")) { Environment.Exit(0); }
                        else { showMenu(); }
                        break;
                    default:
                        Console.WriteLine("Invalid operation command. Please try again");
                        showMenu();
                        break;


                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("ShowMenu Method", ex.ToString());
            }

        }

    }

    public class Person
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string NickName { get; set; }

        static string XMLDBPath = ConfigurationManager.AppSettings["XMLDBPath"];

        public static XDocument doc = XDocument.Load(XMLDBPath);

        public static void Create()
        {
            try
            {
                Person newPerson = new Person();
                var count = doc.Descendants("Person").Count();
                Console.WriteLine("Please enter Person First Name");
                newPerson.FirstName = Validation.isValidEntryPerson(Validation.stringIsnotNullNumber(Console.ReadLine()));
                Console.WriteLine("Please enter Person Last Name");
                newPerson.LastName = Validation.isValidEntryPerson(Validation.stringIsnotNullNumber(Console.ReadLine()));
                Console.WriteLine("Please enter Person DOB (DD/MM/YYYY)");
                newPerson.DOB = Validation.stringIsnotNullDate(Console.ReadLine());
                Console.WriteLine("Please enter Person nick name");
                newPerson.NickName = Validation.stringIsnotNumber(Console.ReadLine());

                doc.Root.Add(new XElement("Person", new XElement("ID", count + 1),
                                                   new XElement("FirstName", newPerson.FirstName),
                                                   new XElement("LastName", newPerson.LastName),
                                                   new XElement("DOB", newPerson.DOB),
                                                   new XElement("NickName", newPerson.NickName)));
                doc.Save(XMLDBPath);
                Console.WriteLine("Records saved successfully");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Create person method", ex.ToString());
            }

        }

        public static void View()
        {
            try
            {
                var person = doc.Root.Descendants("Person");
                foreach (var p in person)
                    //{
                    if (p != null)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("--------");
                        Console.WriteLine("Person Details :");
                        Console.WriteLine("ID : " + p.Element("ID").Value);
                        Console.WriteLine("FirstName : " + p.Element("FirstName").Value);
                        Console.WriteLine("LastName : " + p.Element("LastName").Value);
                        Console.WriteLine("DOB : " + p.Element("DOB").Value);
                        Console.WriteLine("NickName : " + p.Element("NickName").Value);
                        Console.WriteLine("--------");
                        var address = (from c in p.Elements("Address")
                                       select c);
                        if (address != null)
                        {
                            foreach (var a in address)
                            {
                                Console.WriteLine("Address Details :");
                                Console.WriteLine("ID : " + a.Element("ID").Value);// p.Element("ID").Value);
                                Console.WriteLine("Line1 : " + a.Element("Line1").Value);
                                Console.WriteLine("Line2 : " + a.Element("Line2").Value);
                                Console.WriteLine("Postal Code : " + a.Element("PostalCode").Value);
                                Console.WriteLine("Country : " + a.Element("Country").Value);
                                Console.WriteLine("--------");
                            }
                        }
                    }
                    else { Console.WriteLine("No Data found for person: "); }
                //}
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("View person method", ex.ToString());
            }


        }

        public static void Read()
        {
            try
            {
                Console.WriteLine("Please enter ID or First Name or Last Name of the person to search");
                string value = Console.ReadLine();
                var p = (from c in doc.Root.Descendants("Person")
                         where c.Element("ID").Value == value
                         select c).FirstOrDefault();
                if (p == null)
                {
                    p = (from c in doc.Root.Descendants("Person")
                         where c.Element("FirstName").Value.Contains(value) || c.Element("LastName").Value.Contains(value)
                         select c).FirstOrDefault();
                }
                if (p != null)
                {
                    Console.WriteLine("");
                    Console.WriteLine("--------");
                    Console.WriteLine("Person Details :");
                    Console.WriteLine("ID : " + p.Element("ID").Value);
                    Console.WriteLine("FirstName : " + p.Element("FirstName").Value);
                    Console.WriteLine("LastName : " + p.Element("LastName").Value);
                    Console.WriteLine("DOB : " + p.Element("DOB").Value);
                    Console.WriteLine("NickName : " + p.Element("NickName").Value);
                    Console.WriteLine("--------");
                    var address = (from c in p.Elements("Address")
                                   select c);
                    if (address != null)
                    {
                        foreach (var a in address)
                        {
                            Console.WriteLine("Address Details :");
                            Console.WriteLine("ID : " + a.Element("ID").Value);// p.Element("ID").Value);
                            Console.WriteLine("Line1 : " + a.Element("Line1").Value);
                            Console.WriteLine("Line2 : " + a.Element("Line2").Value);
                            Console.WriteLine("Postal Code : " + a.Element("PostalCode").Value);
                            Console.WriteLine("Country : " + a.Element("Country").Value);
                            Console.WriteLine("--------");
                        }
                    }
                }

                else { Console.WriteLine("No Data found for person: " + value); }

            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Read person method", ex.ToString());
            }


        }

        public static void Edit()
        {
            try
            {
                Console.WriteLine("Enter the ID of the Person you want to edit");
                int ID = Convert.ToInt32(Console.ReadLine());
                while (true)
                {
                    var p = doc.Root.Descendants("Person").
                                Where(x => x.Element("ID").Value == ID.ToString()).FirstOrDefault();

                    if (p == null)
                    {
                        Console.WriteLine("No record found for this ID!");
                        Console.WriteLine("Please enter the value again: ");
                    }
                    else break;
                    ID = Convert.ToInt32(Console.ReadLine());
                }
                Person newPerson = new Person();
                Console.WriteLine("Please enter Person First Name");
                newPerson.FirstName = Validation.isValidEntryPerson(Validation.stringIsnotNullNumber(Console.ReadLine()));
                Console.WriteLine("Please enter Person Last Name");
                newPerson.LastName = Validation.isValidEntryPerson(Validation.stringIsnotNullNumber(Console.ReadLine()));
                Console.WriteLine("Please enter Person DOB (DD/MM/YYYY)");
                newPerson.DOB = Validation.stringIsnotNullDate(Console.ReadLine());
                Console.WriteLine("Please enter Person nick name");
                newPerson.NickName = Validation.stringIsnotNumber(Console.ReadLine());

                XElement oldPerson = doc.Root.Descendants("Person").
                                        Where(x => x.Element("ID").Value == ID.ToString()).FirstOrDefault();
                oldPerson.Element("FirstName").Value = newPerson.FirstName;
                oldPerson.Element("LastName").Value = newPerson.LastName;
                oldPerson.Element("DOB").Value = newPerson.DOB;
                oldPerson.Element("NickName").Value = newPerson.NickName;
                doc.Save(XMLDBPath);
                Console.WriteLine("Record updated successfully");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Edit person method", ex.ToString());
            }
        }

        public static void Delete()
        {
            try
            {
                Console.WriteLine("Enter the ID of the Person you want to delete");
                int ID = Convert.ToInt32(Console.ReadLine());
                while (true)
                {
                    var p = doc.Root.Descendants("Person").
                                Where(x => x.Element("ID").Value == ID.ToString()).FirstOrDefault();

                    if (p == null)
                    {
                        Console.WriteLine("No record found for this ID!");
                        Console.WriteLine("Please enter the value again: ");
                    }
                    else break;
                    ID = Convert.ToInt32(Console.ReadLine());
                }
                XElement deletePerson = doc.Root.Descendants("Person").
                                        Where(x => x.Element("ID").Value == ID.ToString()).FirstOrDefault();
                deletePerson.Remove();
                doc.Save(XMLDBPath);
                Console.WriteLine("Record deleted successfully");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Delete person method", ex.ToString());
            }
        }

    }

    public class Validation
    {
        static string XMLDBPath = ConfigurationManager.AppSettings["XMLDBPath"];

        public static XDocument doc = XDocument.Load(XMLDBPath);

        public static string isValidEntryAddress(int ID, string input)
        {
            try
            {
                var p = doc.Root.Descendants("Person").
                            Where(x => x.Element("ID").Value == ID.ToString()).FirstOrDefault();
                var address = (from c in p.Elements("Address")
                               select c);
                while (true)
                {
                    if (address.Any(a => (a.Element("Line1").Value.ToString().ToLowerInvariant().Equals(input.ToLowerInvariant()))
                                   ||
                                   (a.Element("Line2").Value.ToString().ToLowerInvariant().Equals(input.ToLowerInvariant()))
                                   )
                                    )
                    {
                        Console.WriteLine("\r\nDuplicate address value cannot be inserted for Person ID: " + ID);
                        Console.WriteLine("Please enter the value again: ");
                    }
                    else break;
                    input = Console.ReadLine();

                }

            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("isValidEntryAddress method", ex.ToString());
            }
            return input;
        }

        public static string isValidEuropeanCounty(string input)
        {
            try
            {
                string[] EUContries = { "Austria", "Belgium", "Bulgaria", "Croatia", "Cyprus", "Czech Republic", "Denmark", 
                                    "Estonia", "Finland", "France", "Germany", "Greece", "Hungary", "Ireland", "Italy",
                                    "Latvia", "Lithuania", "Luxembourg", "Malta", "Netherlands", "Poland", "Portugal",
                                    "Romania", "Slovakia", "Slovenia", "Spain", "Sweden" };
                while (true)
                {
                    if (!EUContries.Any(x => x.ToString().ToLowerInvariant().Equals(input.ToLowerInvariant())))
                    {
                        Console.WriteLine("\r\nNot a valid country in Europe!");
                        Console.WriteLine("Please enter the country: ");
                    }
                    else break;
                    input = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("isValidEuropeanCounty method", ex.ToString());
            }

            return input;
        }

        public static string stringNoSpecialChar(string input)
        {
            try
            {
                while (true)
                {
                    if (String.IsNullOrEmpty(input))
                    {

                        Console.WriteLine("Value cannot be empty!");
                        Console.WriteLine("Please enter the value again: ");

                    }
                    else if (input.Any(ch => !char.IsLetterOrDigit(ch)))
                    {
                        Console.WriteLine("\r\nValue cannot have special characters!");
                        Console.WriteLine("Please enter the value again: ");
                    }
                    else break;
                    input = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("stringNoSpecialChar method", ex.ToString());
            }

            return input;
        }

        public static string isValidEntryPerson(string input)
        {
            try
            {
                while (true)
                {
                    var p = (from c in doc.Root.Descendants("Person")
                             where c.Element("FirstName").Value.ToLowerInvariant().Equals(input) || c.Element("LastName").Value.ToLowerInvariant().Equals(input)
                             select c).FirstOrDefault();

                    if (p != null)
                    {

                        Console.WriteLine("Value already exits, cannot enter duplicate value!");
                        Console.WriteLine("Please enter the value again: ");

                    }
                    else break;
                    input = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("isValidEntryPerson method", ex.ToString());
            }

            return input;
        }

        public static string stringIsnotNullNumber(string input)
        {
            try
            {
                while (true)
                {
                    if (String.IsNullOrEmpty(input))
                    {

                        Console.WriteLine("Value cannot be empty!");
                        Console.WriteLine("Please enter the value again: ");

                    }
                    else if (input.ToLowerInvariant().Any(char.IsDigit))
                    {
                        Console.WriteLine("\r\nValue cannot have number!");
                        Console.WriteLine("Please enter the value again: ");
                    }
                    else break;
                    input = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("stringIsnotNullNumber method", ex.ToString());
            }
            //var response = input.ToLowerInvariant().Any(char.IsDigit) ? false : true;
            
            return input;
        }

        public static string stringIsnotNullDate(string input)
        {
            try
            {
                DateTime dateResult;
                while (true)
                {
                    if (String.IsNullOrEmpty(input))
                    {

                        Console.WriteLine("Value cannot be empty!");
                        Console.WriteLine("Please enter the value again: ");

                    }
                    else if (!DateTime.TryParse(input, out dateResult))
                    {
                        Console.WriteLine("\r\nValue cannot have number!");
                        Console.WriteLine("Please enter the value again: ");
                    }
                    else break;
                    input = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("stringIsnotNullDate method", ex.ToString());
            }
            
            return input;
        }

        public static string stringIsnotNumber(string input)
        {
            try
            {
                while (true)
                {
                    if (input.ToLowerInvariant().Any(char.IsDigit))
                    {
                        Console.WriteLine("\r\nValue cannot have number!");
                        Console.WriteLine("Please enter the value again: ");
                    }
                    else break;
                    input = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("stringIsnotNumber method", ex.ToString());
            }
            
            return input;
        }

        public static string stringIsNotNull(string input)
        {
            try
            {
                while (true)
                {
                    if (String.IsNullOrEmpty(input))
                    {

                        Console.WriteLine("Value cannot be empty!");
                        Console.WriteLine("Please enter the value again: ");

                    }
                    else break;
                    input = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("stringIsNotNull method", ex.ToString());
            }
            
            return input;
        }
    }

    public class Address
    {
        public int ID { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }

        static string XMLDBPath = ConfigurationManager.AppSettings["XMLDBPath"];

        public static XDocument doc = XDocument.Load(XMLDBPath);

        public static void CreateAddress()
        {
            try
            {
                var count = doc.Descendants("Address").Count();
                Console.WriteLine("Enter the ID of the Person you want to add address for:");
                int ID = Convert.ToInt32(Console.ReadLine());
                while (true)
                {
                    var p = doc.Root.Descendants("Person").
                                Where(x => x.Element("ID").Value == ID.ToString()).FirstOrDefault();

                    if (p == null)
                    {
                        Console.WriteLine("No record found for this ID!");
                        Console.WriteLine("Please enter the value again: ");
                    }
                    else break;
                    ID = Convert.ToInt32(Console.ReadLine());
                }
                Address newAddress = new Address();

                Console.WriteLine("Please enter Address Line1");
                newAddress.Line1 = Validation.isValidEntryAddress(ID, Validation.stringIsNotNull(Console.ReadLine()));
                Console.WriteLine("Please enter Address Line2");
                newAddress.Line2 = Validation.isValidEntryAddress(ID, Validation.stringIsnotNumber(Console.ReadLine()));
                Console.WriteLine("Please enter postal code");
                newAddress.PostalCode = Validation.stringNoSpecialChar(Console.ReadLine());
                Console.WriteLine("Please enter country");
                newAddress.Country = Validation.isValidEuropeanCounty(Console.ReadLine());

                XmlDocument originalXml = new XmlDocument();
                originalXml.Load(XMLDBPath);

                XmlNode TechReport = originalXml.SelectSingleNode("Persons/Person[ID=" + ID + "]");

                if (TechReport != null)
                {
                    XmlNode Address = originalXml.CreateNode(XmlNodeType.Element, "Address", null);
                    XmlNode Aid = originalXml.CreateNode(XmlNodeType.Element, "ID", null); Aid.InnerXml = (count + 1).ToString();
                    XmlNode Line1 = originalXml.CreateNode(XmlNodeType.Element, "Line1", null); Line1.InnerXml = newAddress.Line1;
                    XmlNode Line2 = originalXml.CreateNode(XmlNodeType.Element, "Line2", null); Line2.InnerXml = newAddress.Line2;
                    XmlNode Country = originalXml.CreateNode(XmlNodeType.Element, "Country", null); Country.InnerXml = newAddress.Country;
                    XmlNode PostalCode = originalXml.CreateNode(XmlNodeType.Element, "PostalCode", null); PostalCode.InnerXml = newAddress.PostalCode;

                    Address.AppendChild(Aid);
                    Address.AppendChild(Line1);
                    Address.AppendChild(Line2);
                    Address.AppendChild(Country);
                    Address.AppendChild(PostalCode);
                    TechReport.AppendChild(Address);
                    originalXml.Save(XMLDBPath);
                }

                Console.WriteLine("Records saved successfully");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Create Address method", ex.ToString());
            }
            
        }

        public static void DeleteAddress()
        {
            try
            {
                Console.WriteLine("Enter the ID of the Address you want to delete");
                int ID = Convert.ToInt32(Console.ReadLine());
                while (true)
                {
                    XElement Address = doc.Root.Descendants("Address").
                                        Where(x => x.Element("ID").Value == ID.ToString()).FirstOrDefault();

                    if (Address == null)
                    {
                        Console.WriteLine("No record found for this ID!");
                        Console.WriteLine("Please enter the value again: ");
                    }
                    else break;
                    ID = Convert.ToInt32(Console.ReadLine());
                }

                XElement deleteAddress = doc.Root.Descendants("Address").
                                        Where(x => x.Element("ID").Value == ID.ToString()).FirstOrDefault();
                deleteAddress.Remove();
                doc.Save(XMLDBPath);
                Console.WriteLine("Record deleted successfully");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Delete Address method", ex.ToString());
            }
            
        }

        public static void EditAddress()
        {
            try
            {
                Console.WriteLine("Enter the ID of the Address you want to delete");
                int ID = Convert.ToInt32(Console.ReadLine());
                while (true)
                {
                    XElement Address = doc.Root.Descendants("Address").
                                        Where(x => x.Element("ID").Value == ID.ToString()).FirstOrDefault();

                    if (Address == null)
                    {
                        Console.WriteLine("No record found for this ID!");
                        Console.WriteLine("Please enter the value again: ");
                    }
                    else break;
                    ID = Convert.ToInt32(Console.ReadLine());
                }

                Address newAddress = new Address();

                Console.WriteLine("Please enter Address Line1");
                newAddress.Line1 = Validation.isValidEntryAddress(ID, Validation.stringIsNotNull(Console.ReadLine()));
                Console.WriteLine("Please enter Address Line2");
                newAddress.Line2 = Validation.isValidEntryAddress(ID, Validation.stringIsnotNumber(Console.ReadLine()));
                Console.WriteLine("Please enter postal code");
                newAddress.PostalCode = Validation.stringNoSpecialChar(Console.ReadLine());
                Console.WriteLine("Please enter country");
                newAddress.Country = Validation.isValidEuropeanCounty(Console.ReadLine());


                XElement oldAddress = doc.Root.Descendants("Address").
                                        Where(x => x.Element("ID").Value == ID.ToString()).FirstOrDefault();
                oldAddress.Element("Line1").Value = newAddress.Line1;
                oldAddress.Element("Line2").Value = newAddress.Line2;
                oldAddress.Element("PostalCode").Value = newAddress.PostalCode;
                oldAddress.Element("Country").Value = newAddress.Country;
                doc.Save(XMLDBPath);
                Console.WriteLine("Record updated successfully");
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("Edit Address method", ex.ToString());
            }
        }

    }

    public class ErrorLog
    {
        public static void WriteLog(string methodName, string error)
        {
            string logFile = ConfigurationManager.AppSettings["LogPath"].ToString();
            StreamWriter sw = new StreamWriter(logFile);
            sw.WriteLine(DateTime.Now + "||" + "Method Name: {0} || Error: {1}", methodName, error);
            sw.Close();
        }
    }
}
