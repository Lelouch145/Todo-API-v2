using System.Security.Cryptography.X509Certificates;
using System.Xml.XPath;

var builder = WebApplication.CreateBuilder(args); // Skapar en builder som används för att konfigurera webbservern

// Registrerar services som applikationen ska använda
builder.Services.AddEndpointsApiExplorer(); // Låter ASP.NET hitta alla endpoints (GET, POST osv.) så Swagger kan dokumentera dem
builder.Services.AddSwaggerGen(); // Skapar Swagger-dokumentationen (OpenAPI JSON)

var app = builder.Build(); // Bygger den färdiga webbapplikationen

// Konfigurerar HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // Genererar Swagger-dokumentationen (JSON som beskriver API:t)
    app.UseSwaggerUI(); // Visar ett grafiskt gränssnitt där man kan testa API-endpoints
}

app.UseHttpsRedirection(); // Tvingar API:t att använda HTTPS

// Skapar en lista i minnet som lagrar alla tasks
List<TaskItem> tasks = new List<TaskItem>
{
    new TaskItem { Id = 1, Title = "Learn c#", IsDone = false },       // Första tasken
    new TaskItem { Id = 2, Title = "Build API", IsDone = false },      // Andra tasken
    new TaskItem { Id = 3, Title = "Practice Backend", IsDone = true } // Tredje taske
};
// Om någon öppnar /tasks, skicka tillbaka hela listan
app.MapGet("/tasks", () => tasks); // GET endpoint som returnerar alla tasks i listan

app.MapPost("/tasks", (TaskItem task) => // POST endpoint som tar emot en task från request body (JSON)
{
    // Om titeln är tom så får användaren en statuskod 400
    if (string.IsNullOrWhiteSpace(task.Title))
    {
        return Results.BadRequest("Title cannot be empty! ");
    }
    // Skapar en start värde
    int newId = 1;
    // Om listan inte är tom
    if (tasks.Count > 0)
    {
        // Om det finns tasks, ta sista taskens id och lägg till 1
        newId = tasks[tasks.Count - 1].Id + 1;
    }
    // sätter in det nya värdet till task.Id
    task.Id = newId;
    // Varje ny task börjar som inte klar
    task.IsDone = false;

    tasks.Add(task); // Lägger till tasken i listan
    return Results.Ok(task);     // Returnerar objektet som lades till
});

app.MapGet("/", () => // Root endpoint för att snabbt testa att API:t fungerar
{
    return "My first API is working!";
});

app.MapGet("/tasks/{id}", (int id) => // GET endpoint för att hämta en specifik task via id (t.ex. /tasks/2)
{
    TaskItem? foundTask = null; // Variabel för att lagra tasken vi letar efter

    foreach (TaskItem task in tasks) // Loopar igenom alla tasks i listan
    {
        if (task.Id == id) // Om taskens Id matchar id från URL:en
        {
            foundTask = task; // Sparar tasken som hittades
            break;            // Stoppar loopen när rätt task hittats
        }
    }

    if (foundTask == null) // Om ingen task hittades
    {
        return Results.NotFound("Task not found"); // Returnerar HTTP 404
    }

    return Results.Ok(foundTask); // Returnerar tasken som hittades
});

app.MapDelete("/tasks/{id}", (int id) => // DELETE endpoint för att ta bort en task via id
{
    TaskItem? taskDelete = null; // Variabel för tasken som ska tas bort

    foreach (TaskItem task in tasks) // Loopar igenom listan för att hitta rätt task
    {
        if (task.Id == id)
        {
            taskDelete = task; // Sparar tasken som ska tas bort
            break;             // Stoppar loopen när den hittats
        }
    }

    if (taskDelete == null) // Om tasken inte finns
    {
        return Results.NotFound("Task not found"); // Returnerar HTTP 404
    }
    else
    {
        tasks.Remove(taskDelete); // Tar bort tasken från listan
        return Results.Ok("Task deleted"); // Returnerar ett OK-svar
    }
});

// Här ändrar vi IsDone
// Skapar en endpoint, tar emot ett id från url
app.MapPatch("/tasks/{id}/done", (int id) =>
{
    // Skapar en variabel för att hitta rätt task
    TaskItem? foundTask = null;
    // Loopar igenom listan
    foreach (TaskItem task in tasks)
    {
        // Kollar om det är rätt task
        if (task.Id == id)
        {
            //Sparar den och slutar leta
            foundTask = task;
            break;
        }
    }
    // Om inget hittades skickar vi http 404
    if (foundTask == null)
    {
        return Results.NotFound("Task not found!");
    }
    // Ändrar värdet till true
    foundTask.IsDone = true;
    // Returnerar den uppdaterade tasken
    return Results.Ok(foundTask);
});
// Skapar en PATCH-endpoint
app.MapPatch("/tasks/{id}/undone", (int id) =>
{
    TaskItem? foundTask = null;
    // Loopar igenom listan
    foreach (TaskItem task in tasks)
    {
        // kolalr om id:en matchar
        if (task.Id == id)
        {
            // Spara tasken i foundTask
            foundTask = task;
            break;
        }
    }
    // Om tasken inte finns
    if (foundTask == null)
    {
        return Results.NotFound("Task not found!");
    }
    // Här sker själva förändrigen
    foundTask.IsDone = false;
    // Returnerar den uppdaterade tasken
    return Results.Ok(foundTask);
});

app.MapPut("/tasks/{id}", (int id, TaskItem updatedTask) =>
{
    // Om titeln är tom så får användaren en statuskod 400
    if (string.IsNullOrWhiteSpace(updatedTask.Title))
    {
        return Results.BadRequest("Title cannot be empty! ");
    }
    TaskItem? foundTask = null;

    foreach (TaskItem task in tasks)
    {
        if (task.Id == id)
        {
            foundTask = task;
            break;
        }
    }

    if (foundTask == null)
    {
        return Results.NotFound("Task not found!");
    }

    foundTask.Title = updatedTask.Title;
    foundTask.IsDone = updatedTask.IsDone;

    return Results.Ok(foundTask);
});
// Skapar en Get-endpint, Url är /task/done
app.MapGet("/tasks/done", () =>
{
    // Skapar en ny lista
    List<TaskItem> doneTasks = new List<TaskItem>();
    // Loopar igenom alla tasks 
    foreach (TaskItem task in tasks)
    {
        //kollar statusen
        if (task.IsDone == true)
        {
            // Om det är true så sparar vi i task
            doneTasks.Add(task);
        }
    }
    // returnerar listan
    return Results.Ok(doneTasks);
});
// skapar en Get-endpoint, url är /tasks/notdone
// Samma princip som i "/tasks/done"
app.MapGet("/tasks/notdone", () =>
{
    List<TaskItem> notDoneTasks = new List<TaskItem>();

    foreach (TaskItem task in tasks)
    {
        if (task.IsDone == false)
        {
            notDoneTasks.Add(task);
        }
    }
    return Results.Ok(notDoneTasks);
});


app.Run(); // Startar webbservern

class TaskItem // Modellklass som representerar en task
{
    public int Id { get; set; }      // Unikt id för tasken
    public string Title { get; set; } // Namnet eller beskrivningen av tasken
    public bool IsDone { get; set; }
}