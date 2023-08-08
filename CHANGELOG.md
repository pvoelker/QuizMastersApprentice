# Quiz Master's Apprentice change log:

## 0.1.0
- First alpha version of application
- Base functionality is present and working
- Question 'assignment' to quizzers not yet implemented

## 0.1.1
- Add CHANGELOG.md
- Clean up install
- Update text for selecting a local database file

## 0.1.2
- Add help for importing Bible Fact-Pak™ generated practice question sets

## 0.2.0
- Changed encoding on README and CHANGELOG files
- Fix how data grids with unpersisted data handle drill downs (like *Questions Sets* → *Questions*)
- Minor data grid layout fixes

## 0.2.1
- Implement visual styles for list boxes/views
- Add copyright on main assembly and main menu window
- Add 'not for general distribution' on main menu
- Clean up terminology on select database window
- Clean up saving and updating code for grid views

## 0.2.2
- Cleanup practice window select questions page
- Fix Bible Fact-Pak™ help documentation
- Minor fix to list box and list view UI style
- Enhance the run practice window to include point values and question number

## 0.2.3
- Create data grid buttons control to centralized code

## 0.3.0
- Fix problem with Bible Fact Pak™ import where existing items will be re-imported a second time
- Refactor IQuestionImporter interface so default constructors can now be used
- Refactor Bible Fact-Pak™ import user interface into a generic 'direct text import' user control

## 0.4.0
- Refactor question import check logic from 'direct text import' user control into a testable library
- Move Bible Fact Pak™ import help into the user control
- Complete implementation of question import on question edit window
- Fixed binding bug on direct text import user control

## 0.4.1
- Properly handle max question point values in practices
- Minor adjustments to practice configuration user interface
- Clean up dead code in practice configuration
- Clean up code in practice run

## 0.5.0
- Implement assignment of questions to quizzers on a team
- Fix bug with generating primary keys on imported questions
- Fix bugs in GetByKey methods in some of the repositories

## 0.5.1
- Clean up of repository objects

## 0.5.2
- Add assigned questions to practice report
- Clean up closing of practice and practice report windows when practice reports are successfully sent
- Add progress bar to sending of reports
- Adjust practice run user interface on how questions are assigned

## 0.6.0
- Refactor QMA.DataAccess.JsonFile to use DataStore object from a singleton

## 0.7.0
- Implement async pattern in repository patterns
- Implement AsyncRelayCommands to handle user interface commands
- Fix CallMethodAction bindings
- Implement handling of unhandled exceptions (for implementation of async relay commands)
- Implement async methods in practice report sending

## 0.7.1
- Minor refinement of reloading of data for JSON data store
- Fix issue when quizzer is deleted that is part of a team when a practice is being configured
- Refine error message for bad email addresses

## 0.8.0
- Major code refactor to create ItemsEditorObservable viewmodel base class in order to reduce duplicated code
- Implement 'busy' property and wait cursor during async operations

## 0.8.1
- Switch from deprecated Microsoft.Toolkit.Mvvm to CommunityToolkit.Mvvm
- Fix issues with primary key values on the JSON database file
- Show busy during data load and initialization of windows
- Fix issue with imports where items do not show up in the edit questions window properly
- Improve how rows are initially committed on data grids
- Fix problem with editing of assigned questions to team members

## 1.0.0
- First production version
- Update to .NET 7 with associated library updates
- Change installer build to sign installer and uninstaller
- Add ability to select ranges of questions for practice
- When importing Bible Fact Pak™ questions, duplicates are filtered out
- Add more error checking to import functionality