#!/bin/bash -ex

export sdk=$(dirname $(readlink -f $0))
export ext=$(dirname $sdk)/../ext

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

# if not passed a target, recurse into available targets
target=$1
if [ -z "$target" ]
then
	pushd $sdk
	for i in $(ls *.dir/.config | cut -d'.' -f1);
	do
		$sdk/build.sh $i
	done
	popd
	exit
fi

# common directories
dist=$sdk/dist/$target
home=$sdk/$target.dir
root=$home/root

# build cross compiler
if [ ! -f $home/stamp ]
then
	mkdir -p /tmp/ctngsrc
	pushd $home
	ct-ng upgradeconfig
	ct-ng build
	touch stamp
	popd
fi

# use tools from root
export PATH=$root/bin:$PATH
echo $PATH

# install dist kernel headers
# translate target into kernel arch name
case "${target%%-*}" in
	x86_64) kernel_arch="x86";;
	i686) kernel_arch="x86";;
	arm*) kernel_arch="arm";;
	aarch64) kernel_arch="arm64";;
esac

# copy Linux headers for distribution
if [ ! -f $dist.linux/stamp ]
then
	pushd $ext/linux
	make headers_install ARCH=$kernel_arch INSTALL_HDR_PATH=$dist.linux
	popd
	pushd $dist.linux
	mkdir -p $dist/include
	cp -rv include/* $dist/include
	touch stamp
	popd
fi

# build GLIBC for distribution
if [ ! -f $dist.glibc/stamp ]
then
	mkdir -p $dist.glibc
	pushd $dist.glibc
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
if [ ! -f $dist.gcc/stamp ]
then
	# rely on built in versions of libraries
	pushd $ext/gcc
	./contrib/download_prerequisites
	popd

	mkdir -p $dist.gcc
	pushd $dist.gcc
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

# remove unused directories
rm -rf $dist/bin $dist/etc $dist/libexec $dist/sbin $dist/share $dist/var
