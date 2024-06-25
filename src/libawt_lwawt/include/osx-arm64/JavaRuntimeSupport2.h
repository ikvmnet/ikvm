#include <stdlib.h> 
#include <string.h>

static inline char* JRSCopyOSVersion2() {
    char *returnString = malloc(16);
    strcpy(returnString, "10.15");
    return returnString;
}
