#include <stdlib.h>
#include <stdio.h>
#include <strings.h>
#include <errno.h>
#include <string.h>

char *findJavaTZ_md(const char *java_home_dir) {
  char *tz;
  char *javatz = NULL;
  char *freetz = NULL;

  tz = getenv("TZ");

  if (tz == NULL || *tz == '\0') {
    tz = "UTC";
  }

  if (tz != NULL) {
    /* Ignore preceding ':' */
    if (*tz == ':') {
      tz++;
    }

    if (freetz == NULL) {
      /* strdup if we are still working on getenv result. */
      javatz = strdup(tz);
    } else if (freetz != tz) {
      /* strdup and free the old buffer, if we moved the pointer. */
      javatz = strdup(tz);
      free((void *)freetz);
    } else {
      /* we are good if we already work on a freshly allocated buffer. */
      javatz = tz;
    }
  }

  return javatz;
}
