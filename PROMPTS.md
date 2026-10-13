# Gradebook: the live class prompt script

A 90-minute live demo. You paste each prompt into a free AI chat in the browser
(ChatGPT, Claude.ai, Gemini, and so on), copy the files it returns into the
project, run them, and finish by deploying the app to Render for free.

**The lesson of the day:** *AI writes the code. You decide what is correct. Tests prove it.*

Every prompt follows the same four-part pattern, and that pattern is worth a slide:

| Part | What it does | Example from Prompt 1 |
|---|---|---|
| **Context** | Says what already exists | "My project is a Blazor app called Gradebook" (Prompt 0) |
| **Task** | Says exactly what to build | "Create GradeCalculator.cs with three methods" |
| **Rules** | Gives the facts the AI must not invent | "A = 90 or more, B = 80 or more" |
| **Format** | Says how the answer must look | "Complete files, path above each file" (Prompt 0) |

---

## Before class (students do this at home, about 20 minutes)

1. Install the **.NET 10 SDK** from https://dotnet.microsoft.com/download.
   Afterwards, `dotnet --version` should print `10.something`.
2. Install **VS Code** with the **C# Dev Kit** extension, and install **Git**.
3. Create a **GitHub** account at https://github.com.
4. Create a **Render** account at https://render.com by choosing *Sign in with GitHub*.
   No credit card is needed.
5. Create a free account on any AI chat site.

> **If `dotnet --version` shows a different major version** (for example 11),
> replace `10` with that number in Prompt 0 and in Prompt 5. Nothing else changes.

---

## Timing plan

| Time | Step |
|---|---|
| 0:00–0:10 | Slides: what we build, how the AI fits in, the four-part prompt pattern |
| 0:10–0:15 | **Setup**: create the projects and run the empty app |
| 0:15–0:22 | **Prompt 0** (context) and **Prompt 1** (grading rules) |
| 0:22–0:35 | **Prompt 2** (tests), run them, then **break it on purpose** |
| 0:35–0:43 | **Prompt 3** (database) |
| 0:43–0:58 | **Prompt 4** (the page), run it, add and delete a student |
| 0:58–1:05 | **Prompt 5** (Dockerfile) |
| 1:05–1:20 | Push to GitHub and deploy on Render (the build takes about 5 minutes, so use the wait for questions) |
| 1:20–1:30 | Push a change and watch it go live, then the finale: **a failing test blocks a bad deploy** |

If something breaks at any point, use the **Fix-it prompt** at the end of this file.

---

## Setup (terminal, about 5 minutes)

Run these commands one at a time from the folder where you keep your projects:

```bash
mkdir Gradebook
```

```bash
cd Gradebook
```

```bash
dotnet new blazor -n Gradebook
```

```bash
dotnet new xunit -n Gradebook.Tests
```

```bash
dotnet new sln -n Gradebook
```

```bash
dotnet sln add Gradebook Gradebook.Tests
```

```bash
dotnet add Gradebook.Tests reference Gradebook
```

```bash
dotnet add Gradebook package Microsoft.EntityFrameworkCore.Sqlite
```

```bash
dotnet new gitignore
```

```bash
echo "*.db" >> .gitignore
```

Delete the sample test file, because Prompt 2 replaces it:

```bash
rm Gradebook.Tests/UnitTest1.cs
```

Remove the template's sample pages, which have nothing to do with a gradebook:

```bash
rm Gradebook/Components/Pages/Counter.razor
```

```bash
rm Gradebook/Components/Pages/Weather.razor
```

Then remove the links to them. These are the only hand edits in the class:
1. In `Gradebook/Components/Layout/NavMenu.razor`, delete the two `<div class="nav-item px-3">` blocks
   for **Counter** and **Weather**. Keep the **Home** block.
2. In `Gradebook/Components/Layout/MainLayout.razor`, delete the line with the **About** link.

Run the empty app to prove the setup works:

```bash
dotnet run --project Gradebook
```

**✅ Check:** open the `http://localhost:...` address printed in the terminal.
You should see "Hello, world!" and a sidebar with only **Home**. Press **Ctrl+C** to stop the app.

**Point out:** we have two projects. One is the app, the other tests it.
The `.sln` (or `.slnx`) file ties them together.

---

## Prompt 0: Context (start a NEW chat)

Use **one chat for the whole class** so the AI remembers the earlier files.

```text
You are helping me, a beginner C# student, build a small web app called Gradebook during a live class.

My setup (already done, do not repeat it):
- .NET 10 SDK
- A Blazor Web App created with: dotnet new blazor -n Gradebook
  (render mode Interactive Server, Bootstrap is included, pages are static unless they say @rendermode InteractiveServer)
- An xUnit test project called Gradebook.Tests that already references Gradebook
- The NuGet package Microsoft.EntityFrameworkCore.Sqlite is already installed in Gradebook
- C# files in the app use the namespace Gradebook; test files use the namespace Gradebook.Tests

Rules for EVERY answer in this chat:
1. Give me COMPLETE files, never partial snippets and never "..." - I will copy-paste them as they are.
2. Write the file path above each file, like this: File: Gradebook/Student.cs
3. Only create the files I ask for. Do not add NuGet packages.
4. Keep the code short and simple, with short comments. I only know C# basics: variables, if, loops, classes and lists.
5. After the files, explain what the code does in at most 3 bullet points.

For now, reply only with the word: Ready
```

**✅ Check:** the AI answers "Ready".

**Point out:** the AI knows nothing about your computer. Context is the first thing you give it.

---

## Prompt 1: The grading rules

```text
Create two files.

1) Gradebook/Student.cs - a class Student with these properties:
   - int Id
   - string Name (default value: empty string)
   - int Homework, int Midterm, int Final (each one is a score from 0 to 100)

2) Gradebook/GradeCalculator.cs - a static class GradeCalculator with three static methods:

   - double Average(int homework, int midterm, int final)
     Weighted average: homework counts 30%, midterm 30%, final 40%.
     Round the result to 1 decimal place.

   - string Letter(double average)
     90 or more = "A", 80 or more = "B", 70 or more = "C", 60 or more = "D", below 60 = "F".

   - bool IsValidScore(int score)
     Returns true only if the score is between 0 and 100 (0 and 100 are both allowed).

GradeCalculator must not use the database or the web page. It is pure logic, so we can test it on its own.
```

Create the two files and paste in the code.

```bash
dotnet build
```

**✅ Check:** `Build succeeded` with 0 errors.

**Point out:** the *rules* (30/30/40, the grade lines) came from **us**, not from the AI.
The AI only turned them into C#.

---

## Prompt 2: The tests

```text
Create the file Gradebook.Tests/GradeCalculatorTests.cs with xUnit tests for GradeCalculator.
Use [Fact] for a single check and [Theory] with [InlineData] for a list of examples.

Test these exact examples. I worked them out by hand, so do NOT change any expected value.

Average(homework, midterm, final) -> expected
- 85, 72, 91 -> 83.5
- 100, 100, 100 -> 100
- 0, 0, 0 -> 0
- 88, 88, 93 -> 90

Letter(average) -> expected
- 95 -> "A"
- 90 -> "A"   (exactly on the line counts as the higher grade)
- 89.9 -> "B"
- 80 -> "B"
- 79.9 -> "C"
- 70 -> "C"
- 60 -> "D"
- 59.9 -> "F"
- 0 -> "F"

IsValidScore(score) -> expected
- -1 -> false
- 0 -> true
- 100 -> true
- 101 -> false

Also add one [Fact] that combines them: a student with homework 88, midterm 88 and final 93 must get the letter "A".
```

```bash
dotnet test
```

**✅ Check:** `Passed!` with **0 failed** (about 18 tests).

**Point out:** the AI typed the tests, but **we** chose every expected value.
If the AI picked the expected values too, it would only be checking itself.

### Break it on purpose (the key moment, about 3 minutes)

1. Open `Gradebook/GradeCalculator.cs`.
2. In `Letter`, change `>= 90` to `> 90`. This is a realistic, tiny mistake.
3. Run `dotnet test`.

**✅ Check:** tests now **fail**: "Expected: A, Actual: B" for an average of exactly 90.
The code still compiles and looks fine. Only the test caught the bug.

4. Change it back to `>= 90` and run `dotnet test` again. Everything passes.

> Keep this broken line in mind, because the finale uses it again.

---

## Prompt 3: The database

```text
Create Gradebook/GradebookDb.cs - an Entity Framework Core DbContext that uses SQLite.

- A class GradebookDb that inherits from DbContext, with a DbSet<Student> called Students.
- In OnConfiguring, use the SQLite connection string "Data Source=gradebook.db".
- Add a static method Setup() that:
  1. creates the database if it does not exist, using Database.EnsureCreated() (no migrations),
  2. if the Students table is empty, adds these 5 sample students and saves them:
     Omar 88/88/93, Sara 85/72/91, Lina 70/65/80, Yousef 60/75/58, Mona 50/45/55
     (the numbers are Homework/Midterm/Final)
- Do not use dependency injection. The page will create the context like this: using var db = new GradebookDb();

Then tell me exactly which lines to add to Program.cs and where, including any using line it needs.
(Program.cs currently has only "using Gradebook.Components;" at the top.)
```

Create the file, then add **two lines** to `Program.cs`. Don't let the AI rewrite the whole file:
- `using Gradebook;` as the very first line
- `GradebookDb.Setup();` on the line just above `app.Run();`

> If the AI forgets the `using` line, the build fails with
> *"The name 'GradebookDb' does not exist in the current context"*.
> That's a good moment to show the **Fix-it prompt**.

```bash
dotnet build
```

**✅ Check:** `Build succeeded`.

**Point out:** the database is just one file, `gradebook.db`. The `Student` class
*is* the table: each property becomes a column.
The sample students cover every grade, A to F. Omar lands exactly on 90.0.

---

## Prompt 4: The page

```text
Replace Gradebook/Components/Pages/Home.razor with a complete new version:

- Keep @page "/" and add @rendermode InteractiveServer on the next line.
- Page title and heading: "Class Gradebook".
- Under the heading, a small grey note: "Demo data - it resets when the app restarts."
- A form in one row with plain HTML inputs using @bind:
  Name (text), Homework, Midterm, Final (type="number"), and an "Add student" button using @onclick.
  Do NOT use EditForm.
- When "Add student" is clicked:
  - if the name is empty, or any score fails GradeCalculator.IsValidScore, show a red error message and do not save;
  - otherwise save the new student to the database, clear the inputs, hide the error and reload the list.
- A table of all students ordered by name with the columns:
  Name, Homework, Midterm, Final, Average (1 decimal), Letter, and a "Delete" button on each row.
- Use GradeCalculator.Average and GradeCalculator.Letter - do not repeat the formula in the page.
- Inside each method, use the database like this: using var db = new GradebookDb();
- Style it only with Bootstrap classes (table, form-control, btn btn-primary, btn btn-sm btn-danger, alert alert-danger). No CSS file.
- Put all the C# code in an @code block at the bottom of the same file.
```

```bash
dotnet run --project Gradebook
```

**✅ Check:** the page lists the 5 sample students. Omar shows 90.0 / A, and Mona shows 50.5 / F.
Add a student, delete one, and try a score of 150, which should show the red error.
Stop the app and start it again: your changes are still there, because the database is a file on your disk.
Show students that file: `Gradebook/gradebook.db`. That one file is the whole database. Git ignores it because of the `*.db` line in `.gitignore`.

**Point out:** the page does **no** maths itself. It asks `GradeCalculator`, the
same code the tests already checked.

---

## Prompt 5: Get ready for the internet (Dockerfile)

```text
I want to deploy this app to Render.com on the free plan, using Docker.
My repository root contains: the folder Gradebook (the web app), the folder Gradebook.Tests (the tests), and the solution file.

Create two files in the repository root:

1) Dockerfile - a multi-stage build:
   - Build stage: image mcr.microsoft.com/dotnet/sdk:10.0. Copy everything, run the tests with
     "dotnet test Gradebook.Tests" (if any test fails, the build must fail), then publish
     Gradebook/Gradebook.csproj in Release mode to /app/publish.
   - Final stage: image mcr.microsoft.com/dotnet/aspnet:10.0. Copy the published files into /app and start Gradebook.dll.
   - Render sends traffic to port 10000, so add: ENV ASPNETCORE_HTTP_PORTS=10000 and EXPOSE 10000.
   - Do NOT add a USER line, because the app must be allowed to write its SQLite file.

2) .dockerignore - ignore bin and obj folders at any depth (**/bin, **/obj), the .git folder, and *.db files at any depth.
```

Create both files in the **root** `Gradebook` folder, next to the `.sln` file, not inside `Gradebook/Gradebook`.

**Point out:** students don't need Docker installed. Render reads this recipe and
builds the app on its own computers. Because the recipe runs the tests first,
**a failing test stops a broken app from going live.**

---

## Deploy (no AI needed, about 15 minutes)

### 1. Put the code on GitHub

The easiest way is in VS Code. Open the root `Gradebook` folder, go to **Source Control**,
click **Publish to GitHub**, and choose a **public** repository.

Or use the terminal (create an empty repository on github.com first):

```bash
git init
```

```bash
git add .
```

```bash
git commit -m "Gradebook app"
```

```bash
git branch -M main
```

```bash
git remote add origin https://github.com/YOUR-NAME/gradebook.git
```

```bash
git push -u origin main
```

### 2. Create the web service on Render

1. On https://dashboard.render.com, click **New → Web Service**.
2. Pick your `gradebook` repository from GitHub.
3. Render detects the Dockerfile and sets **Language: Docker**. Leave it.
4. **Instance Type: Free**.
5. Click **Deploy Web Service**.

The first build takes about 3–5 minutes. Watch the log scroll by: you'll see the tests run.

**✅ Check:** the page opens at `https://gradebook-xxxx.onrender.com`. Share the link!

**Point out:**
- On the free plan the app **sleeps after 15 minutes** with no visitors. The next
  visit takes about a minute to wake it up.
- When it wakes, the database starts fresh with the 5 sample students. That's why
  the page says "Demo data". Real apps keep their data in a separate hosted database.

### 3. Push a change and watch it go live

Change the heading in `Home.razor` (for example to "Class Gradebook - Section B"), then:

```bash
git commit -am "Change heading"
```

```bash
git push
```

Render rebuilds automatically, and the new heading is live in a few minutes.

### 4. Finale: tests guard the internet (if time allows)

1. Make the bug again: in `GradeCalculator.Letter`, change `>= 90` to `> 90`.
2. Commit and push it.
3. On Render, the build **fails** at the `dotnet test` step, and the **old, correct
   version stays online**. Nobody ever sees the bug.
4. Fix it, then commit and push. The site updates normally.

> This is the whole lesson in one picture: AI wrote the code, we wrote the
> rules, the tests caught the mistake, and users never saw it.

---

## Fix-it prompt (use whenever something breaks)

```text
I got this error when I ran: <the command, for example dotnet build>

<paste the FULL error message>

Here is the complete file the error mentions:

<paste the whole file>

Explain the cause in one sentence, then give me the complete corrected file.
```

**Tip:** if the chat gets confused, start a **new** chat, paste **Prompt 0** again,
then paste your current files and say what you want.

---

## Safety net: checkpoints (teacher only)

This folder is the finished reference project, tested end to end. Each step is saved as a git tag:

| Tag | The project after... |
|---|---|
| `step-0` | the setup commands, with the sample pages removed |
| `step-1` | Prompt 1: `Student.cs`, `GradeCalculator.cs` |
| `step-2` | Prompt 2: `GradeCalculatorTests.cs` (18 passing tests) |
| `step-3` | Prompt 3: `GradebookDb.cs` and the two `Program.cs` lines |
| `step-4` | Prompt 4: the `Home.razor` page |
| `step-5` | Prompt 5: `Dockerfile`, `.dockerignore` (the finished app, the same as `main`) |

If the live code breaks and two tries with the Fix-it prompt don't solve it, don't debug
in front of the class. Open a terminal in **this** folder and jump to the step you just finished:

```bash
git checkout step-3
```

Then do that step's ✅ Check here and carry on with the next prompt.
Before deploying, go back to the finished version:

```bash
git checkout main
```

To rehearse the whole class, run `git checkout step-0` and work through the prompts again.

---

## Homework prompts (students try these on their own)

Each is one prompt. Students write a test for it first, with expected values they work out by hand.

1. **Class average:** "Add a line under the table showing the class average of all students' averages (1 decimal). Put the calculation in GradeCalculator as a new method ClassAverage(List<double> averages) and tell me what tests to add."
2. **Colour the grades:** "Show the Letter column as a Bootstrap badge: A and B green, C and D yellow, F red."
3. **Edit a student:** "Add an Edit button on each row that loads the student into the form, and change Add into Save while editing."
4. **Search:** "Add a text box above the table that filters the students by name as I type."
