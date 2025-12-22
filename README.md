This is a project for a suite of apps aiming to solve the various challenges a family faces day-to-day.  The following features are envisioned for this suite:

1. Create and track the family budget - development in progress
2. Pantry/Garage Inventory Tracker
3. Family Calendar
4. To-Do List

The main idea for this is to be a one-stop shop for various family activities in an attempt to reduce the number of apps needed on a person's smartphone, 
as well as leveraging the relationships of various items (eg, bill payments, calendar appointments or pantry notifications can pop up in the To-Do List as "Here's what you need to do today.")
The FamGuild name is meant to evoke a sense of fantasy and whimsy, adding a layer of fun to what is usually boring family administration stuff

Technologies:
Backend: Minimal APIs written in C# that hook into Entity Framework pointing to a PostgreSQL database
Frontend: Blazor WebAssembly for the Web.  Mobile UI still to be determined...

Blazor is chosen here to keep the language consistent across the back and front ends, as well as offering strong typing of objects/classes right out of the box.  In addition, validation objects and API contracts (commands and queries) can be easily shared across the front and back end so that a change need to be made in only one file when modifying the desired payload of an API call.

Project Methodology:
I'm implementing Domain Driven Design (DDD), as I really like the concepts of classes/aggregates managing their own internal state and each aggregate ensuring that itself is always valid. 
The name of the game here is to keep things as simple as I possibly can, making the app force me to add complexities like abstractions where appropriate.  In the spirit of this, 
I'm not religiously following all of the strategic design recommendations of DDD, I will introduce these if/when they become necessary.

Also, Vertical Slice Architecture is being followed here, to allow for heavy cohesion of code files.
