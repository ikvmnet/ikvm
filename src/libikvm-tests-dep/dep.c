#ifdef _WIN32
#define EXPORT __declspec(dllexport)
#else
#define EXPORT __attribute__((visibility("default")))
#endif

// Imported by libikvm-tests-dependent. Used to test that the dependencies of a JNI library are found.
EXPORT int ikvm_tests_dep_value(void) {
    return 42;
}
