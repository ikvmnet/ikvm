/*
  Copyright (C) 2004, 2005 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
	 claim that you wrote the original software. If you use this software
	 in a product, an acknowledgment in the product documentation would be
	 appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
	 misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net

*/
#ifdef _WIN32

#include <windows.h>
#include "jni.h"

#else

#include <sys/types.h>
#include <sys/mman.h>
#include <dlfcn.h>
#include "jni.h"

JNIEXPORT void* JNICALL ikvm_mmap(int fd, jboolean writeable, jboolean copy_on_write, jlong position, jint size)
{
	return mmap(0, size, writeable ? PROT_WRITE | PROT_READ : PROT_READ, copy_on_write ? MAP_PRIVATE : MAP_SHARED, fd, position);
}

JNIEXPORT int JNICALL ikvm_munmap(void* address, jint size)
{
	return munmap(address, size);
}

JNIEXPORT int JNICALL ikvm_msync(void* address, jint size)
{
	return msync(address, size, MS_SYNC);
}

#endif