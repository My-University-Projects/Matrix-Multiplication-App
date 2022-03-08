# Matrix multiplication desktop application created in MVC model 
Used technologies and programming languages:
- WPF
- C#
- C++
- Asm x64
- Dynamically linked libraries(.dll) with C++ and Asm languages multiplication functions

Main purpose of this application is to compare performance between low level(Asm) and high level(C++) proggraming languages.
For interested - screens of the performance charts are below.

# How Does the program works
User interface screenshot(Application language is polish)
![image](https://user-images.githubusercontent.com/93645494/157262685-9eca8f06-9b8d-4025-b99b-5f0eaea93402.png)

We can load matrixes from .txt file or generate random matrixes.
Regardless of the ways of passing matrixes, program checks if passed matrixes are multiplicable from a mathematical point of view
Also we need to pass number of threads that program will use during the multiplication 
In program we can choose in which language we want matrixes be multiplied and how many threads application has to use during multiplication. 
We can generate matrix with passed dimensions fill with random integers from 0 to 99 or we can pass a .txt file with correctly passed matrix(screen of the correct .txt input file below)
