// TEST.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <stdio.h>
#include "dirent.h"

using namespace std;


void travel(string path)
{
	DIR *dir = opendir(path.c_str());
	if (dir)
	{
		struct dirent *ent;
		while ((ent = readdir(dir)) != NULL)
		{
			if (string(".").compare(ent->d_name) == 0 || string("..").compare(ent->d_name) == 0)
			{
				continue;
			}
			std::string currentPath = path + "\\" + ent->d_name;
			if (ent->d_type == DT_DIR)
			{
				travel(currentPath);
				if (RemoveDirectory(currentPath.c_str()))
				{
					cout << ("removed: " + currentPath).c_str() << endl;
				}
			}
			else if (ent->d_type == DT_REG)
			{
				if (remove(currentPath.c_str()))
				{
					cout << ("removed: " + currentPath).c_str() << endl;
				}
			}
		}
	}
	else
	{
		cout << "Error opening directory" << endl;
	}
}

std::string current_working_directory()
{
	char working_directory[MAX_PATH + 1];
	GetCurrentDirectoryA(sizeof(working_directory), working_directory); // **** win32 specific ****
	return working_directory;
}

int _tmain(int argc, _TCHAR* argv[])
{
	//string path = "D:\\QtTest\\build-QtTest-Desktop_Qt_5_8_0_MSVC2013_32bit-Release\\release";
	travel(current_working_directory());

	getchar();
	return 0;
}

