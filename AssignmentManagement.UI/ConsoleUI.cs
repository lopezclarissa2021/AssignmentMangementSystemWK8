using AssignmentManagement.Core;

using System;

namespace AssignmentManagement.UI
{
    public class ConsoleUI
    {
        private readonly IAssignmentService _assignmentService;

        public ConsoleUI(IAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        public void Run()
        {
            while (true)
            {
                DisplayMenu();
                var input = Console.ReadLine();
                ProcessUserChoice(input);
            }
        }

        private void DisplayMenu()
        {
            Console.WriteLine("\nAssignment Manager Menu:");
            Console.WriteLine("1. Add Assignment");
            Console.WriteLine("2. List All Assignments");
            Console.WriteLine("3. List Incomplete Assignments");
            Console.WriteLine("4. Mark Assignment as Complete");
            Console.WriteLine("5. Search Assignment by Title");
            Console.WriteLine("6. Update Assignment");
            Console.WriteLine("7. Delete Assignment");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");
        }

        private void ProcessUserChoice(string input)
        {
            var actions = new Dictionary<string, Action>
    {
        { "1", AddAssignment },
        { "2", ListAllAssignments },
        { "3", ListIncompleteAssignments },
        { "4", MarkAssignmentComplete },
        { "5", SearchAssignmentByTitle },
        { "6", UpdateAssignment }, 
        { "7", DeleteAssignment },
        { "0", () => { Console.WriteLine("Goodbye!"); Environment.Exit(0); } }
    };

            if (actions.TryGetValue(input, out var action))
            {
                action.Invoke();
            }
            else
            {
                Console.WriteLine("Invalid choice. Try again.");
            }
        }


        private void AddAssignment()
        {
            Console.WriteLine("Enter assignment title: ");
            var title = Console.ReadLine();
            Console.WriteLine("Enter assignment description: ");
            var description = Console.ReadLine();

            try
            {
                var assignment = new Assignment(title, description);
                if (_assignmentService.AddAssignment(assignment))
                {
                    Console.WriteLine("Assignment added successfully.");
                }
                else
                {
                    Console.WriteLine("An assignment with this title already exists.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void ListAllAssignments()
        {
            var assignments = _assignmentService.ListAll();
            if (assignments.Count == 0)
            {
                Console.WriteLine("No assignments found.");
                return;
            }

            foreach (var assignment in assignments)
            {
                Console.WriteLine($"- {assignment.Title}: {assignment.Description} (Completed: {assignment.IsCompleted})");
            }
        }

        private void ListIncompleteAssignments()
        {
            var assignments = _assignmentService.ListIncomplete();
            if (assignments.Count == 0)
            {
                Console.WriteLine("No incomplete assignments found.");
                return;
            }

            foreach (var assignment in assignments)
            {
                Console.WriteLine($"- {assignment.Title}: {assignment.Description} (Completed: {assignment.IsCompleted})");
            }
        }

        private void MarkAssignmentComplete()
        {
            Console.WriteLine("Enter the title of the assignment to mark as complete:");
            var title = Console.ReadLine();
            if (_assignmentService.MarkAssignmentComplete(title))
            {
                Console.WriteLine("Assignment marked as complete.");
            }
            else
            {
                Console.WriteLine("Assignment not found or already completed.");
            }
        }

        private void SearchAssignmentByTitle()
        {
            Console.WriteLine("Enter the title of the assignment to search:");
            var title = Console.ReadLine();
            var assignment = _assignmentService.FindAssignmentByTitle(title);
            if (assignment != null)
            {
                Console.WriteLine($"Found Assignment: {assignment.Title} - {assignment.Description} (Completed: {assignment.IsCompleted})");
            }
            else
            {
                Console.WriteLine("Assignment not found.");
            }
        }

        private void UpdateAssignment()
        {
            Console.WriteLine("Enter the title of the assignment to update:");
            var oldTitle = Console.ReadLine();
            Console.Write("Enter new title: ");
            var newTitle = Console.ReadLine();
            Console.Write("Enter new description: ");
            var newDescription = Console.ReadLine();
            if (_assignmentService.UpdateAssignment(oldTitle, newTitle, newDescription))
            {
                Console.WriteLine("Assignment updated successfully.");
            }
            else
            {
                Console.WriteLine("Assignment not found or update failed.");
            }
        }

        private void DeleteAssignment()
        {
            // TODO: Implement UI for deleting assignment
        }
    }
}
