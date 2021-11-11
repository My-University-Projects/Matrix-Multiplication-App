// pch.cpp: plik źródłowy odpowiadający wstępnie skompilowanemu nagłówkowi

#include "pch.h"
#include <cstdio>
#ifdef _DEBUG
#pragma comment(lib, "msvcrtd.lib")
#else
#pragma comment(lib, "msvcrt.lib")
#endif

extern"C"
int mainCRTStartup();

extern"C"
int entrypoint()
{
	//call the CRT startup code
	mainCRTStartup();
	return 0;
}

//you need the function with this name
//since the startup code will call it
int main()
{
	//printf("Hello World!");

	return 0;
}
// W przypadku korzystania ze wstępnie skompilowanych nagłówków ten plik źródłowy jest niezbędny, aby kompilacja powiodła się.
