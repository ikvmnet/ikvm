#ifdef _WIN32
#define EXPORT __declspec(dllexport)
#define IMPORT __declspec(dllimport)
#else
#define EXPORT __attribute__((visibility("default")))
#define IMPORT
#endif

IMPORT int ikvm_tests_dep_value(void);

// Imports a function from libikvm-tests-dep, so that loading this library requires its dependency to be found.
EXPORT int ikvm_tests_dependent_value(void) {
    return ikvm_tests_dep_value();
}
