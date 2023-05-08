#!/bin/bash -ex

export cwd=$(dirname $(readlink -f $0))

# build crosstool-ng if not already built
if [ ! -d $cwd/sysroot.crosstool-ng/bin ]
then
	cd $cwd/crosstool-ng
	./bootstrap
	./configure --prefix=$cwd/sysroot.crosstool-ng
	make install
fi
export PATH=$cwd/sysroot.crosstool-ng/bin:$PATH

# if not passed a target, recurse into available targets
target=$1
if [ -z "$target" ]
then
	for i in $(ls sysroot.*/.config | cut -d'.' -f2);
	do
		$0 $i
	done
	exit
fi

# common directories
dir=$cwd/sysroot.$target
root=$dir/root
dist=$dir/dist

# build cross compiler
if [ ! -f root.stamp ]
then
	mkdir -p /tmp/ctngsrc
	cd $dir
	ct-ng upgradeconfig
	ct-ng build
	touch root.stamp
fi

# use tools from root
export PATH=$root/bin:$PATH

# install dist kernel headers
# translate target into kernel arch name
case "${target%%-*}" in
	x86_64) kernel_arch="x86";;
	i686) kernel_arch="x86";;
	arm*) kernel_arch="arm";;
	aarch64) kernel_arch="arm64";;
esac

# copy Linux headers for distribution
if [ ! -f $dist.linux.stamp ]
then
	pushd $cwd/linux
	make headers_install ARCH=$kernel_arch INSTALL_HDR_PATH=$dist.linux
	mkdir -p $dist/include
	cp -rv $dist.linux/include/* $dist/include
	popd
	touch $dist.linux.stamp
fi

# build GLIBC for distribution
if [ ! -f $dist.glibc.stamp ]
then
	mkdir -p $dist.glibc
	pushd $dist.glibc
	$cwd/glibc/configure \
		--host=$target \
		--target=$target \
		--prefix="" \
		--with-sysroot=$root \
		--with-headers=$dist/include \
		--disable-nls --disable-multilib --disable-selinux --disable-profile --disable-tunables
	make
	make DESTDIR=$dist install
	popd
	touch $dist.glibc.stamp
fi

# build GCC for distribution
if [ ! -f $dist.gcc.stamp ]
then
	# rely on built in versions of libraries
	pushd $cwd/gcc
	./contrib/download_prerequisites
	popd

	mkdir -p $dist.gcc
	pushd $dist.gcc
	$cwd/gcc/configure \
		--host=$target \
		--target=$target \
		--prefix="" \
		--with-sysroot=$dist \
		--disable-bootstrap --disable-nls --disable-multilib --enable-languages=c,c++
	make all-gcc all-target-libgcc all-target-libstdc++
	make DESTDIR=$dist install-target-libgcc install-target-libstdc++
	popd
	touch $dist.gcc.stamp
fi
