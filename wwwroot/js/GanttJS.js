
        document.addEventListener("DOMContentLoaded", function () {
            // Prepare the task data from the model
            var tasks = {
                data: [
        @foreach (var task in Model.Tasks)
        {
            <text>
                            {
                                id: @task.Id,
                                text: "@task.Title", // Properly escape quotes for strings
                                start_date: "@task.StartDate.ToString("dd-MM-yyyy")", // Ensure valid format
                                end_date: "@task.Deadline.ToString("dd-MM-yyyy")",   // Ensure valid format
                                duration: @((task.Deadline.DayNumber - task.StartDate.DayNumber)), // Calculate duration using DayNumber
                                open: true
                            },
            </text>
        }
                ]
            };

            // Log tasks for debugging
            console.log(tasks);

            // Configure Gantt chart columns
            gantt.config.columns = [
                { name: "text", label: "Task", width: "200",align: "center", tree: true },
               
            ];

            // Add custom styling for tasks
            gantt.templates.task_class = function (start, end, task) {
                if (task.duration <= 5) return "short-task";
                else if (task.duration > 5 && task.duration <= 10) return "medium-task";
                else return "long-task";
            };

            // Set scale and grid appearance
            gantt.config.scale_unit = "month";
            gantt.config.date_scale = "%F %Y"; // Display full month name and year
            gantt.config.subscales = [{ unit: "day", step: 1, date: "%D, %j" }]; // Display weekdays and days

        // Adjust the scale height
        gantt.config.scale_height = 75; // Increase the height for better visibility

        // Adjust the width of scale cells
        gantt.templates.scale_cell_class = function(date) {
            return "gantt_scale_cell"; // Optionally, apply custom classes for further styling
        };


            // Styling for gridlines and taskbars
            gantt.templates.grid_header_class = function () {
                return "grid-header";
            };
            gantt.templates.task_text = function (start, end, task) {
                return task.text;
            };

            gantt.init("gantt-chart");
            gantt.parse(tasks);
        });
