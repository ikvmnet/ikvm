#!/bin/bash -ex

export sdk=$(dirname $(readlink -f $0))
export ext=$(dirname $sdk)/../ext

# if not passed a target, recurse into available targets
target=$1
if [ -z "$target" ]
then
	pushd $sdk
	for i in $(ls */sdk.config | cut -d'/' -f1);
	do
		$sdk/build.sh $i
	done
	popd
	exit
fi

# common directories
home=$sdk/$target
dist=$home/dist
root=$home/root

# include ct-nt variable
libc=`cat $home/sdk.config`

# install dist kernel headers
# translate target into kernel arch name
case "${target%%-*}" in
	x86_64) kernel_arch="x86";;
	i686) kernel_arch="x86";;
	arm*) kernel_arch="arm";;
	aarch64) kernel_arch="arm64";;
esac

# copy Linux headers for distribution
if [ ! -f $home/linux/stamp ]
then
	pushd $ext/linux
	make headers_install ARCH=$kernel_arch INSTALL_HDR_PATH=$home/linux
	popd
	pushd $home/linux
	mkdir -p $dist/include
	cp -rv include/* $dist/include
	touch stamp
	popd
fi

# build crosstool-ng if not already built
if [ ! -f $sdk/crosstool-ng/bin/ct-ng ]
then
	pushd $ext/crosstool-ng
	./bootstrap
	./configure --prefix=$sdk/crosstool-ng
	make install
	popd
fi
export PATH=$sdk/crosstool-ng/bin:$PATH

# build cross compiler
if [ ! -f $home/ct-ng-stamp ]
then
	mkdir -p /tmp/ctngsrc
	pushd $home
	ct-ng upgradeconfig
	ct-ng build
	touch ct-ng-stamp
	popd
fi

# use tools from root
export PATH=$root/bin:$PATH
echo $PATH

# build GLIBC for distribution
if [ $libc == "glibc" ]
then
	if [ ! -f $home/glibc/stamp ]
	then
		mkdir -p $home/glibc
		pushd $home/glibc
		$ext/glibc/configure \
			CFLAGS="-O2" \
			--host=$target \
			--target=$target \
			--prefix="" \
			--with-sysroot=$root \
			--with-headers=$dist/include \
			--disable-nls --disable-multilib --disable-selinux --disable-profile --disable-tunables
		make
		make DESTDIR=$dist install
		touch stamp
		popd
	fi

	# build GCC for distribution
	if [ ! -f $home/gcc/stamp ]
	then
		# rely on built in versions of libraries
		pushd $ext/gcc
		./contrib/download_prerequisites
		popd

		mkdir -p $home/gcc
		pushd $home/gcc
		$ext/gcc/configure \
			CFLAGS="-O2" \
			--host=$target \
			--target=$target \
			--prefix="" \
			--with-sysroot=$dist \
			--with-native-system-header-dir=/include \
			--disable-bootstrap --disable-nls --disable-multilib --enable-languages=c,c++
		make all-target-libgcc all-target-libstdc++-v3
		make DESTDIR=$dist install-target-libgcc install-target-libstdc++-v3
		touch stamp
		popd
	fi
elif [ $libc == "musl" ]
then
	# build musl for distribution
	if [ ! -f $home/musl/stamp ]
	then
		mkdir -p $home/musl
		pushd $home/musl
		$ext/musl/configure \
			CROSS_COMPILE=$target \
			CFLAGS="-O2" \
			--host=$target \
			--target=$target \
			--prefix="" \
			--with-sysroot=$dist
		make
		make DESTDIR=$dist install
		touch stamp
		popd
	fi
fi

# remove unused directories
rm -rf $dist/bin $dist/etc $dist/libexec $dist/sbin $dist/share $dist/var
