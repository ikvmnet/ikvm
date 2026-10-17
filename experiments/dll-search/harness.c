// harness.exe <mode> <dllpath> [setdefault]
// mode: plain | current | altered | retry
#include <windows.h>
#include <stdio.h>
#include <string.h>
#include <wchar.h>

static int is_absolute(const wchar_t* p)
{
    return (p[0] && p[1] == L':' && (p[2] == L'\\' || p[2] == L'/')) || (p[0] == L'\\' && p[1] == L'\\');
}

int wmain(int argc, wchar_t** argv)
{
    if (argc < 3) return 2;
    if (argc > 3 && wcscmp(argv[3], L"setdefault") == 0)
        if (!SetDefaultDllDirectories(LOAD_LIBRARY_SEARCH_DEFAULT_DIRS)) { printf("SetDefaultDllDirectories failed %lu\n", GetLastError()); return 2; }

    const wchar_t* mode = argv[1];
    const wchar_t* path = argv[2];
    HMODULE h = NULL;
    DWORD err = 0;
    if (wcscmp(mode, L"plain") == 0) h = LoadLibraryW(path);
    else if (wcscmp(mode, L"current") == 0) h = LoadLibraryExW(path, NULL, LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR);
    else if (wcscmp(mode, L"altered") == 0) h = LoadLibraryExW(path, NULL, LOAD_WITH_ALTERED_SEARCH_PATH);
    else if (wcscmp(mode, L"retry") == 0) {
        h = LoadLibraryExW(path, NULL, LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR);
        if (h == NULL && GetLastError() == ERROR_MOD_NOT_FOUND && is_absolute(path))
            h = LoadLibraryExW(path, NULL, LOAD_WITH_ALTERED_SEARCH_PATH);
    }
    else return 2;
    err = GetLastError();
    if (h == NULL) { printf("FAIL(%lu)\n", err); return 1; }
    printf("OK\n");
    return 0;
}
