# Flowish

Flowish is a prototype embedded Data-in-Motion interpreted language loosely modeled after Visual Basic. The idea is that a language should be used as an alternative to traditional "Alarm" and "Correlation Engine" approaches commonly used by Complex Event Processors. Correlation Engines tend to occupy large quantities of memory because of their limited logical branches.

Features of a Data-In-Motion language need to be:

- Low overhead cost to the Data-In-Motion
- Ability to modify, improve, delete, store, redirect, branch, alarm, etc. based on the data received
- Multiple methods, recursion, and otherwise behave like a language rather than a filter
- Simplification of metadata CRUD operations for incoming messages
