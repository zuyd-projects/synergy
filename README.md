# Exotisch Nederland - Pilot Midden-Limburg

## Project Overview

This repository is dedicated to the development of a mobile application for Exotisch Nederland, focusing on the **Pilot Midden-Limburg** project. The goal of this project is to enhance the experience of nature monitoring and observation, and to promote engagement with the environment by making the collection and management of wildlife and plant data more efficient.

### Inhoud

1. [Casus - Exotisch Nederland](#casus---exotisch-nederland)
2. [Missie en Visie](#missie-en-visie)
3. [Geschiedenis](#geschiedenis)
4. [Opdracht](#opdracht)
   - [Opdrachtomschrijving](#opdrachtomschrijving)
   - [Op te leveren](#op-te-leveren)
5. [Project Management Tools](#project-management-tools)
6. [Development Guidelines](#development-guidelines)

## Casus - Exotisch Nederland

### Missie en Visie

Exotisch Nederland aims to **protect, enhance, and sustainably manage** the green heritage of the country. We work towards a future where nature is both preserved and experienced in harmony with sustainable practices. Our motto is *“Dat zit in onze natuur”* (It's in our nature). By 2025, our mission is aligned with three core pillars:

1. **Better protection** of nature.
2. **More engagement** with the public.
3. **Sustainable utilization** of our resources.

### Geschiedenis

Founded in 1899 and restructured in 1998, Exotisch Nederland operates as an independent administrative body (RWT) under the Dutch Ministry of Economic Affairs, with a focus on managing natural terrains and integrating sustainable development into society.

## Opdracht

We are focusing on improving our engagement strategy under the **"More Engagement"** pillar. Currently, volunteers are key to gathering data about native and exotic species, but the manual process is slow and inefficient. We aim to digitize this process with a **mobile app** that allows volunteers and the general public to submit wildlife and plant observations directly from the field.

### Opdrachtomschrijving

The pilot will take place in the **Midden-Limburg** area. The mobile app must:

1. Allow users to submit observations with:
   - **Name** of the animal/plant (required)
   - **Category** (required) – e.g., tree species, animal type
   - **Location** (automatically filled in if possible)
   - **Description** (optional)
   - **Photo** (optional)
2. Include a **navigation** feature for volunteers to locate specific areas.
3. Provide a platform for the public, including children, to engage through a **serious game** that involves nature quests and informative games.
4. Enable planning of routes through natural areas with an option to visit the nearest restaurant at lunchtime.

### Op te leveren

1. **Application Code**: The application will be built using the graph-library discussed in week 4. It will provide navigation and allow users to plan routes between natural areas and nearby facilities.
2. **Day Program Generator**: The app will generate a day plan based on selected locations, incorporating nearby restaurants for lunch.
3. **Test Report**: A report showcasing the functionality and performance of the app.
4. **Research Report**: Justification of the chosen data structures and algorithms, with special attention to recursive algorithms and complexity analysis, including average-case and worst-case scenarios.

## Project Management Tools

We use **Linear** for project management and task tracking. To join our project board, click the link below:

- [Linear Board](https://linear.app/bent-synergy/join/cdcf41bfc8ef89e3b65496d88954a0f1?s=4)

For team communication and collaboration, join us on **Slack**:

- [Slack Channel](https://join.slack.com/t/bent-synergy/shared_invite/zt-2q0pxsuwy-kq6eUiZi3lbDIc8dhthYNg)

## Development Guidelines

### Branch Structure

We follow a branching structure that ensures code stability and organized development:

- **Main Branch**: Production-ready code.
- **Staging Branch**: Pre-production, used for testing.
- **Working Branch**: Feature or task-specific branches, named after the Linear ticket (e.g., `BEN-1`).

### Pull Requests (PRs)

All pull requests must meet the following criteria:

1. **At least one reviewer** is required to review and approve the PR before merging into the main branch.
2. Ensure that PRs are well-documented and follow our [commenting standards](https://conventionalcomments.org/).

### Commit Messages

We adhere to **conventional commit** guidelines for all commit messages. This ensures clarity and traceability throughout the development process. Please refer to this guide for detailed examples:

- [Conventional Commit](https://www.google.com/search?client=safari&rls=en&q=conventional+commit&ie=UTF-8&oe=UTF-8)

## Development documentation

### Helper functions

Helpers.cs contains:

```cs
public static string MenuSelect(Dictionary<string, string> _items, bool _indexes = false, List<string> _text = null)
```

Create a selection menu

- _items is a dictionary of key/value items for the menu, the key will be returned after selection
- _indexes = true will add numbers to the start of the line
- _text is a List of strings that will be printed above the menu (because on every key press the screen is cleared first)

```cs
public static string HashPassword(string _password)
```

Take in a password and return the SHA256 hash

```cs
public static Dictionary<string, string> LoadSettings()
```

Loads the settings in the .env file located in the ExotischNederland folder. This function is needed for the DB connection string.

```cs
public static string ReadInputWithEsc(bool _hidden = false)
```

Read an input from the user or return null if ESC is pressed (used primarily in the Form class). If hidden is set to true then the input will show as asterisks to the user

```cs
public static bool ConfirmPrompt()
```

Returns true or false depending on keyboard J or N key press (waits for either one)
Example:

```cs
Console.Write($"Weet u zeker dat u gebruiker {_user.Id} wilt verwijderen? [J/N]");
if (Helpers.ConfirmPrompt())
{
    _user.Delete(this.authenticatedUser);
    Console.WriteLine("Gebruiker verwijderd!");
    Console.ReadKey();
    return;
}
```

```cs
public static string GetSafeFilePath(string _name, string _extension, string _subFolder = null)
```

Returns a path to the AppData/Roaming/ExotischNederland folder with an optional subFolder and filename + extension included. Will make sure the file ends in this extension and will block path traversal and illegal characters.

### Menus

Menus should be created in the Menus folder and inherit from the IMenu interface. They should have a constructor taking in the User object of the currently authenticated user and store this in a private variable. They should implement:

```cs
public Dictionary<string, string> GetMenuItems()
```

To dynamically create a list of menu items based on conditions (e.g. user roles). This method is public so that unit tests are possible.

```cs
public void Show()
```

To render the menu on the screen
