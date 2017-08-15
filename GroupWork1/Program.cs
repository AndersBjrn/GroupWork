using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projektarbete
{

    class Program
    {
        static List<int> membersPerGroup = new List<int>();
        static List<Member> InputNames() //Frågar efter gruppmedlemmar
        {
            Console.Write("Vill du läsa in gruppmedlemmarna från en fil? (Ja/Nej): ");
            string input = Console.ReadLine();
            List<Member> namesList = new List<Member>();

			//ADDED COMMENT: Saknade textfil ;D (kunde ingått i repot från början)
			//TODO: Lägg till klassnamn.txt i repot ?
			if (input.Equals("ja", StringComparison.InvariantCultureIgnoreCase))
            {
				/*TODO: Går att lyfta ut till egen metod? 
				ADDED COMMENT: (kan vara praktiskt ifall man ska läsa in fler filer att då bara ha en metod att anropa 
				istället för att kopiera koden igen.*/
                string[] text = System.IO.File.ReadAllLines("klassnamn.txt");
                foreach (string name in text)
                {
                    namesList.Add(new Member(name));
                }
                return namesList;
            }
            else
                Console.Write("Ange gruppmedlemmar här, separera namn med komma (exempel Lisa,Pelle,Lotta): ");
            string names = Console.ReadLine();
			
			//ADDED COMMENT: Kanske också kan göras i en egen metod?
            List<string> memberNames = NameList(names);
            foreach (string name in memberNames)
            {
                namesList.Add(new Member(name));
            }
            return namesList;
        }

        static List<string> NameList(string names)  //Gör om vår string till en lista
        {
            List<string> nameArray = names.Split(',').ToList<string>();
            return nameArray;

        }

        static List<string> GroupNames(List<Member> members) //Skapa grupper
        {
            int groupNumbers = AskForNumbersOfGroups();
            while (members.Count < groupNumbers)
            {
                Console.WriteLine("Antal grupper var större än antalet medlemmar.");
                groupNumbers = AskForNumbersOfGroups();
            }
            List<string> groups = new List<string>();

            for (int i = 0; i < groupNumbers; i++)
            {
				//ADDED COMMENTS: Nu är jag ju inte helt klart i huvudet så här i slutet på dagen,
				//men det var inte så helt tydligt att det va "gruppnamn" som efterfrågades. =)
                Console.Write("Ange namn för grupp nummer " + (i + 1) + ": ");
                string groupName = Console.ReadLine();
                groups.Add(groupName);
            }
            return groups;
        }

        static bool AskForCustomMadeGroups(List<Member> membersList, int numberOfGroups)
        {
            Console.Write("Vill du ange specifikt antal medlemmar per grupp?: ");
            string input = Console.ReadLine();
            while (!input.Equals("Ja", StringComparison.InvariantCultureIgnoreCase) && !input.Equals("Nej", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Du angav ej rätt");
                input = Console.ReadLine();
            }
            bool b = false;
            if (input.Equals("ja", StringComparison.InvariantCultureIgnoreCase))
            {
                while (!b)
                {
                    b = DecideNumberOfMembers(membersList, numberOfGroups);
                }
                return true;
            }
            return false;
        }

        static bool DecideNumberOfMembers(List<Member> membersList, int numberOfGroups)
        {
            int sumOfGroupsMembers = 0;
            List<int> numbersPerGroup = new List<int>();
            for (int i = 0; i < numberOfGroups; i++)
            {
                Console.Write("Ange antal för grupp nummer " + (i + 1) + ": ");
                string inputInt = Console.ReadLine();
                while (!CheckIfInputInt(inputInt))
                {
                    Console.Write("Du angav ej ett tal, ange tal på nytt: ");
                    inputInt = Console.ReadLine();
                }
                sumOfGroupsMembers = sumOfGroupsMembers + int.Parse(inputInt);
                if (sumOfGroupsMembers > membersList.Count - (numberOfGroups - (i + 1)))
                {
                    Console.WriteLine("Antalet går inte upp");
                    return false;
                }
                if (membersList.Count - sumOfGroupsMembers > 0)
                {
                    Console.WriteLine("Antal medlemmar kvar att fördela är " + (membersList.Count - sumOfGroupsMembers) + " på " + (numberOfGroups - (i + 1)) + " grupper");
                }
                numbersPerGroup.Add(int.Parse(inputInt));
            }
            if (membersList.Count - sumOfGroupsMembers == 0)
            {
                membersPerGroup = numbersPerGroup;
                return true;
            }
            return false;
        }

        static int AskForNumbersOfGroups()
        {
            Console.Write("Ange antal grupper: ");
            string input = Console.ReadLine();
            while (!CheckIfInputInt(input))
            {
                Console.Write("Du angav ej ett tal, ange antal grupper på nytt: ");
                input = Console.ReadLine();
            }
            int groupNumbers = int.Parse(input);
            return groupNumbers;
        }

        static bool CheckIfInputInt(string input)
        {
            int number;
            bool b = int.TryParse(input, out number);
            return b;
        }

        static List<Group> AddMembers(List<Member> members, List<string> groups)
        {
            List<Group> groupsWithMembers = new List<Group>();
            List<Member> shuffledMembers = Shuffle(members);
            int minimumMembersPerGroup = (shuffledMembers.Count / groups.Count);
            int rest = shuffledMembers.Count % groups.Count;

            for (int i = 0; i < groups.Count; i++)
            {
                List<Member> membersList = new List<Member>();
                string groupname = groups[i];
                for (int t = 0; t < minimumMembersPerGroup; t++)
                {
                    membersList.Add(shuffledMembers[0]);
                    shuffledMembers.RemoveAt(0);
                }
                if (rest > i)
                {
                    membersList.Add(shuffledMembers[0]);
                    shuffledMembers.RemoveAt(0);
                }
                Group g = new Group(groupname, membersList, membersList[0]);
                groupsWithMembers.Add(g);
            }
            return groupsWithMembers;
        }

        static List<Group> AddMembersSpecificNumbers(List<Member> members, List<string> groupsNames, List<int> groupNumbers)
        {
            List<Group> groupsWithMembers = new List<Group>();
            List<Member> shuffledMembers = Shuffle(members);
            for (int i = 0; i < groupsNames.Count; i++)
            {
                int number = groupNumbers[i];
                string groupname;
                groupname = groupsNames[i];
                List<Member> membersList = new List<Member>();
                for (int t = 0; t < number; t++)
                {
                    membersList.Add(shuffledMembers[0]);
                    shuffledMembers.RemoveAt(0);
                }
                Group g = new Group(groupname, membersList, membersList[0]);
                groupsWithMembers.Add(g);
            }
            return groupsWithMembers;
        }

        static List<Member> Shuffle(List<Member> array) // Blanda lista
        {
            Random random = new Random();
            int n = array.Count;
            for (int i = 0; i < array.Count; i++)
            {
                int r = i + random.Next(n - i);
                Member t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
            return array;
        }

        static void PrintGroups(List<Group> groups)
        {
            Console.WriteLine();
			Console.WriteLine("(Gruppledare markeras med grön text)");
			Console.WriteLine();
			foreach (Group group in groups)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(group.name);
                Console.ResetColor();
				Console.ForegroundColor = ConsoleColor.Green;
				string leaderName = group.members[0].name;
                Console.WriteLine(leaderName + " <-- BANANAPA ;D");
				Console.ResetColor();
				for (int i = 1; i < group.members.Count; i++)
                {
                    Console.WriteLine(group.members[i].name);
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            List<Member> members = InputNames();
            List<string> groups = GroupNames(members);
            if (AskForCustomMadeGroups(members, groups.Count))
            {
                List<Group> groupsWithMembers = AddMembersSpecificNumbers(members, groups, membersPerGroup);
                PrintGroups(groupsWithMembers);
            }
            else
            {
                List<Group> groupsWithMembers = AddMembers(members, groups);
                PrintGroups(groupsWithMembers);
            }
        }
    }
}