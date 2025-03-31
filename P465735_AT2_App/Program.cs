/* Program:     Task Manager
 * Description: A console application that allows the user to keep track of tasks wth the data saved to a text file
 * 
 * 
 * Author:      Samuel Dunlop
 * Created:     27/02/2025
 * Modified:    9/03/2025
 * Version:     1.1
 */

namespace P465735_AT2_App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // declare file to store data
            string filePath = "tasks.txt";

            // load existing tasks from file
            List<string> tasks = LoadTasksFromFile(filePath);

            // To enable us to exit the loop
            bool exit = false;

            while (!exit)
            {
                // clear all
                Console.Clear();
                // display the menu options
                Console.WriteLine("==== Task Manager ====");
                Console.WriteLine("\n Menu:");
                Console.WriteLine("1. View Current Tasks");
                Console.WriteLine("2. Add a Task");
                Console.WriteLine("3. Complete a Task");
                Console.WriteLine("4. Delete Completed Tasks");
                Console.WriteLine("5. Exit");
                Console.Write("\nSelect an option: ");
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        DisplayTasks(tasks);
                        Pause();
                        break;
                    case "2":
                        Console.Clear();
                        AddTask(tasks, filePath);
                        break;
                    case "3":
                        Console.Clear();
                        CompleteTask(tasks, filePath);
                        break;
                    case "4":
                        Console.Clear();
                        DeleteCompletedTasks(tasks, filePath);
                        break;
                    case "5":
                        Console.Clear();
                        exit = true;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid option. Please try again.");
                        Pause();
                        break;
                } // end switch
            } // end while
        } // end Main

        // reads a stored task list from a file
        static List<string> LoadTasksFromFile(string filePath)
        {
            List<string> tasks = new List<string>();

            // if the file already exists
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    // keep going until we reach the end of the file
                    while (!reader.EndOfStream)
                    {
                        // read each line
                        string? line = reader.ReadLine();
                        // if there is data on the line
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            // add it as an item to our list
                            tasks.Add(line);
                        } // end if
                    } // end while
                } // end using
            } // end if

            return tasks;
        } // end LoadTasksFromFile

        // writes the current task list file, creating a new file or overwriting a previous one
        static void SaveTasksToFile(List<string> tasks, string filePath)
        {
            // create a StreamWriter object
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string task in tasks)
                {
                    writer.WriteLine(task);
                } // end foreach
            } // end using
        } // end SaveTasksToFile

        // display the list of tasks if they exist
        static void DisplayTasks(List<string> tasks)
        {
            // if the list is empty
            if (tasks.Count == 0)
            {
                Console.WriteLine("No recorded tasks.\n");
            }
            else
            {
                // header
                Console.WriteLine("=== Current Tasks ===");

                // loop through the list
                for (int i = 0; i < tasks.Count; i++)
                {
                    // each task is stored as "name|priority|date|status"
                    string[] parts = tasks[i].Split('|'); // split on the pipe (|)
                    if (parts.Length == 4)
                    {
                        Console.WriteLine($"{i + 1}. {parts[0]}, " +
                            $"Priority: {parts[1]}, Date Added: {parts[2]}, Status: {parts[3]}");
                    } // end if
                } // end for

                Console.WriteLine(); // display formatting
            } // end if
        } // end DisplayTasks

        // adds a new task
        static void AddTask(List<string> tasks, string filePath)
        {
            // get the task name from the user
            Console.Write("Enter task name: ");
            string? taskName = Console.ReadLine();

            // validate task name isn't blank
            if (string.IsNullOrWhiteSpace(taskName))
            {
                Console.WriteLine("\nA task name is required.");
                Pause();
                return;
            } // end if

            Console.Write("Enter the task priority (Low, Medium, or High): ");
            string? taskPriority = Console.ReadLine();

            // Convert to uppercase for validation and formatting
            taskPriority = taskPriority.ToUpper();

            // validate taskPriority is "Low", "Medium", or "High"
            if (taskPriority != "LOW" && taskPriority != "MEDIUM" && taskPriority != "HIGH")
            {
                Console.WriteLine("\nPriority must be set to Low, Medium, or High.");
                Pause();
                return;
            }

            // gets current date
            string taskDate = DateTime.Now.ToString("dd-MM-yyyy");

            // Task starts as not completed
            string? taskStatus = "To-Do";

            // format the new task entry and add it to the list
            string newTask = $"{taskName}|{taskPriority}|{taskDate}|{taskStatus}";
            tasks.Add(newTask);

            // write the updated expense list back to the file
            SaveTasksToFile(tasks, filePath);

            Console.WriteLine("\nTask added!\n");
            DisplayTasks(tasks);
            Pause();
        } // end AddTask

        // marks a single task complete
        static void CompleteTask(List<string> tasks, string filePath)
        {
            // if the list is empty
            if (tasks.Count == 0)
            {
                Console.WriteLine("No recorded tasks.\n");
            }
            else
            {
                DisplayTasks(tasks);

                Console.Write("Enter the number of the task to mark complete: ");

                // checks if the task number is in the correct range
                if (int.TryParse(Console.ReadLine(), out int taskNumber)
                    && taskNumber > 0 && (taskNumber <= tasks.Count))
                {
                    // each task is stored as "name|priority|date|status"
                    string[] parts = tasks[taskNumber - 1].Split('|'); // split on the pipe (|)
                    if (parts.Length == 4)
                    {
                        parts[3] = "Completed";
                        tasks[taskNumber - 1] = $"{parts[0]}|{parts[1]}|{parts[2]}|{parts[3]}";
                    } // end if

                    SaveTasksToFile(tasks, filePath);

                    Console.Clear();
                    DisplayTasks(tasks);
                    Console.WriteLine("Task completed!\n");
                }
                else
                {
                    Console.WriteLine("\nInvalid task number.");
                }
            }

            Pause();
        }

        // deletes all completed tasks
        static void DeleteCompletedTasks(List<string> tasks, string filePath)
        {
            // if the list is empty
            if (tasks.Count == 0)
            {
                Console.WriteLine("No recorded tasks.\n");
            }
            else
            {
                DisplayTasks(tasks);
                Console.Write("Are you sure you want to delete all completed tasks? (Enter 'y' to continue): ");
                string? input = Console.ReadLine();

                // convert to lowercase for validation
                input = input.ToLower();

                if (input == "y")
                {
                    for (int i = tasks.Count - 1; i >= 0; i--)
                    {
                        // each task is stored as "name|priority|date|status"
                        string[] parts = tasks[i].Split('|'); // split on the pipe (|)
                        if (parts.Length == 4 && parts[3] == "Completed")
                        {
                            tasks.RemoveAt(i);
                        } // end if
                    }

                    SaveTasksToFile(tasks, filePath);

                    Console.Clear();
                    DisplayTasks(tasks);
                    Console.WriteLine("All completed tasks deleted!\n");
                }
                else
                {
                    Console.WriteLine("\nAborting... No tasks deleted.");
                }
            }

            Pause();
        }

        // wait for user keypress to continue
        static void Pause()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        } // end Pause
    } // end class
} // end namespace
