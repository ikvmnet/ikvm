#include <stddef.h>
#include <jni.h>

JNIEXPORT jstring JNICALL Java_ikvm_tests_jni_JniTests_echoImpl(JNIEnv* env, jclass cls, jstring value)
{
	jstring ret;
	const char* tmp;

	tmp = (*env)->GetStringUTFChars(env, value, 0);
	ret = (*env)->NewStringUTF(env, tmp);

	return ret;
}
