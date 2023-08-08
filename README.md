# Quiz Master's Apprentice

An application that is used to help manage practices for quizzing or scholastic bowls.

##Building

- Make sure a valid path to 'signtool.exe' is in the 'PATH' environment variable on the development machine.
	- *Windows SDK* may need to be installed for 'signtool.exe' to be present on the development machine.
- Make sure to update the certificate thumbnail in 'signfile.bat'.
- Batch file must be run 'as admin' (otherwise certificate signing will fail)
- Run NSIS 'as admin' (otherwise certificate signing will fail)