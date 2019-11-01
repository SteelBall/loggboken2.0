using System;
//using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace loggboken2
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //En lista som kan innehålla listor, för att kunna spara multipla värden för varje index i detta fall titel och inlägg
            List<string[]> Post = new List<string[]>();

            //Mina funktioner(metoder)
            //Read and check användarinputer. All användarinput går via denna funktion.
            //på det viset har vi alltid koll på vart data bör saneras
            //Funktionen är byggt för att förhindra körtidsfel som kan inträffa vid felinmatning
            //Length används för att meddela maxlängd
            string ReadAndCheck(string type, int length = -1)
            {
                //Check Type int, string, bool och kommer meddela användaren om vad som är tillåtet
                bool loop = true;
                string read = "";
                while (loop)
                {
                    //Läs in användarinmatning. Spara i var read
                    read = Console.ReadLine();
                    //Checka sedan read beroende på type
                    if (type == "string")
                    {
                        if (length == -1 || read.Length <= length)
                        {
                            loop = false;
                        }
                        else
                        {
                            Console.WriteLine("Max " + length.ToString() + " tecken är tillåtet.");
                            Console.Write("Väj: ");
                        }
                    }

                    else if (type == "int")
                    {
                        int test;

                        //Om anvädarn vill avsluta
                        if (read == "E")
                        {
                            loop = false;
                        }
                        //Testa om strängen går att konvertera till en lista
                        else if (int.TryParse(read, out test))
                        {
                            if (length == -1 || test <= length)
                            {
                                loop = false;
                            }
                            //Meddela användaren att "Endast siffor mellan 0-X" + " är tillåtet (E för att avsluta)"
                            else
                            {
                                Console.WriteLine("Endast siffor mellan 0-" + length.ToString() + " är tillåtet (E för att avsluta)");
                                Console.Write("Välj [0-" + length.ToString() + "]: ");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Endast siffor är tillåtet");
                            Console.Write("Välj [0-" + length.ToString() + "]: ");
                        }
                    }

                    else if (type == "bool")
                    {
                        if (read == "true" || read == "false")
                        {
                            loop = false;
                        }
                        else
                        {
                            Console.WriteLine("Endast true / false är tillåtet");
                            Console.Write("Välj [true/false]: ");
                        }
                    }

                    else
                    {
                        Console.WriteLine("Okänd inmatning");
                        Console.Write("Välj: ");
                    }

                }
                return read;
            }

            //Submenyn (metod)
            bool submeny(string val1)
            {
                if (Post.Count() == 0){
                    Console.Clear();
                    return false;
                }
                //Submeny som frågar användaren om hen vill skriva fler blad till loggboken eller återgå till huvudmenyn
                //Submenyn skapades för att få en konsekvent gui
                Console.WriteLine("________________________________");
                Console.WriteLine("[0] " + val1);
                Console.WriteLine("[E] Gå tillbaka till huvudmenyn");
                Console.Write("Välj: ");
                string menyval = ReadAndCheck("int", 0);
                //string menyval = Console.ReadLine();

                //int val = Convert.ToInt32(menyval);
                if (menyval == "E")
                {
                    return false;
                }
                return true;
            }

            string mainmeny()
            {
                //Mainmeny som frågar användaren vad han/hon vill göra, menyvalen 2 och 3 dyker upp efter att ett inlägg har skrivits 
                //Jag har valt att använda console.clear för att rensa det tidigare skrifterna, för enkelthetens skull.
                Console.Clear();
                Console.WriteLine("\tVälkommen till Loggboken välj nr i menyn nedan");
                Console.WriteLine("________________________________");
                Console.WriteLine("[0] Skriv inlägg");
                if (Post.Count() != 0)
                {
                    Console.WriteLine("[1] Hantera inlägg");
                    Console.WriteLine("[2] Sök inlägg");
                }

                Console.WriteLine("[E] Avsluta");
                Console.WriteLine("________________________________");
                Console.Write("välj: ");
                string str = ReadAndCheck("int", 2);
                Console.Clear();
                return str;   
            }

            //Sökfunktion (metod) som gör wildcardsökningar, i det här fallet går det att söka på allt i loggboken och får användaren en match 
            //skrivs svaret ut vart den ligger, om sökningen inte hade någon match skrivs "Fann inga resultat som matchade din sökning."
            //Needle är det man söker efter. key är index i vektorn som sak checkas.
            bool linearsearch (string needle, int key)
            {
                bool found = false;

                if (Post.Count() == 0){
                    Console.WriteLine("Finns inga inlägg.\n________________________________");
                    return found;
                }

                Console.WriteLine("Söker efter " + needle + "\n________________________________");
                //Gå igenom loggbokens inlägg
                for (int i = 0; i < Post.Count; i++)
                {
                    //Kolla om needle finns i strängen
                    if (Post[i][key].Contains(needle))
                    {
                        //Vi fann en matchning 
                        Console.WriteLine("[" + i + "] - " + Post[i][key]);
                        //Rapporterar att vi funnit en matchning
                        found = true;
                    }

                }


                if (!found){
                    Console.WriteLine("Fann inga resultat som matchade din sökning.\n________________________________");    
                } else {
                    Console.WriteLine("________________________________"); 
                }

                return found;
            }

            //Funktion (metod) funnen på Stackoverflow, som jag valt att använda för min editeringsfunktion
            //funktionen ser till att förinmata text till readline, använde mig först av SendKeys.SendWait(Post[num][2] + "");
            //Men den fungerade inte på Mac:en, detta var då en unviversiel lösning.
            string ReadLine(string PrePendText)
            {
                int pos = Console.CursorLeft;
                Console.Write(PrePendText);
                ConsoleKeyInfo info;
                List<char> chars = new List<char>();
                if (string.IsNullOrEmpty(PrePendText) == false)
                {
                    chars.AddRange(PrePendText.ToCharArray());
                }

                while (true)
                {
                    info = Console.ReadKey(true);
                    if (info.Key == ConsoleKey.Backspace && Console.CursorLeft > pos)
                    {
                        chars.RemoveAt(chars.Count - 1);
                        Console.CursorLeft -= 1;
                        Console.Write(' ');
                        Console.CursorLeft -= 1;

                    }
                    else if (info.Key == ConsoleKey.Enter) { Console.Write(Environment.NewLine); break; }
                    //Här behöver du skapa en egen checkning för symboler
                    else if (char.IsLetterOrDigit(info.KeyChar))
                    {
                        Console.Write(info.KeyChar);
                        chars.Add(info.KeyChar);
                    }
                }
                return new string(chars.ToArray());
            }


            //////////////Slut på metoder///////////y



            //Huvudloop för programmet
            bool isRunning = true;
            while (isRunning)
                
            {
                //loop för submeny
                bool loop2 = true;
                //Vi skriver ut huvudmenyn med hjälp av metoden mainmeny() som returnernar användarens val till Swtich'n
                switch (mainmeny())
                {
                    case "0":
                        //Menyval 1 omber användaren att skriva ett inlägg och en titel.
                        while (loop2)
                        {   
                            Console.WriteLine("Lägg till nytt inlägg\n________________________________");
                            //Vi sparar användarens inlägg i en vektor
                            string[] myPost = new string[3];
                            myPost[2] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                            //Submenyn ihop med loopen, tillåtna tecken vid skift
                            Console.Write("Skriv en titel: ");
                            myPost[0] = ReadAndCheck("string", 30);
                            Console.Write("Skriv ett inlägg: ");
                            myPost[1] = ReadAndCheck("string", 1000);
                            loop2 = submeny("Skriv en ny Titel och ett nytt inlägg");
                            //Lägg in vektorn i Listan Post
                            Post.Add(myPost);
                        }
                        break;

                    case "1":
                        //Denna funktion är deaktiverad om inga inlägg finns
                        if (Post.Count() == 0)
                        {
                            break;
                        }

                        while (loop2)
                        {
                            //skriver ut hantera inlägg, här kan användaren redigera/radera och öppna nytt inlägg.
                            //En for loop för att söka genom loggboken samt en vektor med valen för användaren.
                            Console.WriteLine("Skriver ut titlar från loggboken\n________________________________");
                            for (int i = 0; i < Post.Count; i++)
                            {
                                Console.WriteLine("[" + i + "] " + Post[i][0] + "("+Post[i][2]+")");
                            }

                            int length = Post.Count - 1;
                            Console.WriteLine("--------Slut på utskrift--------");
                            Console.Write("[0-" + length + "] öppna inlägg: ");
                            string st = ReadAndCheck("int", length);
                            Console.Clear();
                            int num = Convert.ToInt32(st);
                            Console.WriteLine(" Titel: " +Post[num][0]+ "\n Inlägg: " +Post[num][1]+ "\n Datum & Tid: " +Post[num][2]);
                            Console.WriteLine("________________________________");
                            Console.WriteLine("[0] Editera inlägg\n[1] Radera inlägg\n[2] Öppna nytt inlägg\n[E] Gå tillbaka till huvudmenyn");
                            string editmenyval = ReadAndCheck("int", 2);

                            if (editmenyval == "E"){
                                loop2 = false;
                            } else if (editmenyval == "0"){
                                //Redigera inlägg
                                Console.Write("Editera inlägg: ");
                                //kalla på funktionen ReadLine som prependar texten som ska editeras.
                                //Sparar över befintlig edition med den editerade utgåvan.
                                //Har skulle man kunna lägga till en editerat tidsstämpel
                                Post[num][1] = ReadLine(Post[num][1]);
                            } else if (editmenyval == "1"){
                                //Radera inlägg
                                Post.RemoveAt(num);
                                //skriv ut submenyn och kolla om användaren vill hantera ett nytt inlägg eller återgå
                                loop2 = submeny("Hantera ett nytt inlägg?");
                            } else if (editmenyval == "2"){

                                //Öppna nytt inlägg. Vi Loopar igen.
                                loop2 = true;
                            }
                            //loop2 = submeny("Hantera ett nytt inlägg?");

                        }    
                        break;

                    case "2":
                        //Sök efter inlägg i loggboken, användaren kan söka på titel, inlägg samt datum.
                        //I detta fall har jag valt att ha en if sats som gör valen enklare och snyggare i menyn för användaren.
                        //Console.clear kommer rensa tidigare skrift som bara ser kladdigt och för mycket för öga att se igen.

                        //Funktionen är inaktiverad om inga inlägg finns
                        if (Post.Count() == 0)
                        {
                            break;
                        }

                        while (loop2)
                        {
                            
                            Console.WriteLine("[0] Sök på titel \n[1] sök på inlägg\n[2] Sök på datum\n[E] Gå tillbaka till huvudmenyn");
                            Console.Write("Välj: ");
                            string sokmenyval = ReadAndCheck("int", 2);
                            Console.Clear();

                            if (sokmenyval == "E")
                            {
                                loop2 = false;
                            }
                            else if (sokmenyval == "0")
                            {
                                //Sök på titel, med max antal tecken tillåtna
                                Console.Write("Sök på titel: ");
                                linearsearch(ReadAndCheck("string", 50), 0);
                            }
                            else if (sokmenyval == "1")
                            {
                                //Sök på inlägg, med max antal tecken tillåtna
                                Console.Write("Sök på inlägg: ");
                                linearsearch(ReadAndCheck("string", 50), 1);

                            }
                            else if (sokmenyval == "2")
                            {
                                //Sök på datum, med max antal tecken tillåtna
                                Console.Write("Sök på datum: ");
                                linearsearch(ReadAndCheck("string", 50), 2);

                            }
                        }
                        break;

                    case "E":
                        // Avslutar programmet
                        Console.Write("Avsluta");
                        isRunning = false;
                        break;
                }

            }
            //Stänga ner programmet helt
            System.Console.Write("Exit");
            Environment.Exit(0);
        }
    }
}
