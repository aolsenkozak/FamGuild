This is a project for a suite of apps aiming to solve the various challenges a family faces day-to-day.  The following features are envisioned for this suite:

1. Create and track the family budget - development in progress
2. Pantry/Garage Inventory Tracker
3. Family Calendar
4. ToDoList

The main idea for this is to be a one-stop shop for various family activities in an attempt to reduce the number of apps needed on a person's smartphone, 
as well as leveraging the relationships of various items (eg, bill payments, calendar appointments or pantry notifications can pop up in the ToDoList as "Here's what you need to do today.")
The FamGuild name is meant to evoke a sense of fantasy and whimsy, adding a layer of fun to what is usually boring fmaily administration stuff

Technologies:
Backend: Minimal APIs written in C# that hook into Entity Framework pointing to a Postgres database
Frontend: Options still being explored

Project Methodology:
I'm implementing Domain Driven Design, as I really like the concepts of classes/aggregates managing their own internal state and ensuring that itself is always valid. 
The name of the game here is to keep things as simple as I possibly can, making the app force me to add complexities like abstractions where appropriate.  In the spirit of this, 
I'm not religiously following all of the strategic design recommendations of DDD, I will introduce these if/when they become necessary.
