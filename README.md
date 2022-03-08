# Matrix multiplication application
## Desktop application created in MVC model 
Used technologies and programming languages:
- WPF
- C#
- C++
- Asm x64
- Dynamically linked libraries(.dll) with C++ and Asm languages multiplication functions

Main purpose of this application is to compare performance between low level(Asm) and high level(C++) proggraming languages.
<br/>
For interested - screens of the performance charts are below.

# How Does the program work
User interface screenshot(Application language is polish)
![image](https://user-images.githubusercontent.com/93645494/157262685-9eca8f06-9b8d-4025-b99b-5f0eaea93402.png)

There are to options of passing matrixes to the program:
- Load matrixes from .txt file(Left side of the user interface with 2 text fields for two file paths).
- Generate random matrixes(Middle side of the user interface with 4 text fields for each dimension of the both matrixes).
Regardless of the ways of passing matrixes, program checks if passed matrixes are multiplicable from a mathematical point of view.
<br/>
Also we need to pass number of threads that program will use during the multiplication(Right side of the user interface, by default there is used one thread).
<br/>
At each stage, the program is protected against giving incorrect data and the program will not perform multiplication with incorrect input data.
<br/>
In case of problems user can click "instrukcja" button. Program will show short instruction what to do i correct way.
# Screens of the applications work

.txt file with correct matrix(First there are dimensions rows and columns)
![image](https://user-images.githubusercontent.com/93645494/157268256-53734990-1ada-4d9e-93bd-ed34c72470a2.png)

Information about result(Number of used threads, Multiplication duration in milliseconds)
<br/>
![image](https://user-images.githubusercontent.com/93645494/157268564-d31750b0-41f8-4437-9d41-8ae626c73af1.png)






 
