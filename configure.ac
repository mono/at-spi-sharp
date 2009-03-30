AC_PREREQ([2.54])
AC_INIT([at-spi-sharp], [1.0])
AM_INIT_AUTOMAKE([foreign])

# pkg-config
AC_PATH_PROG(PKG_CONFIG, pkg-config, no)
if test "x$PKG_CONFIG" = "xno"; then
        AC_MSG_ERROR([You need to install pkg-config])
fi

SHAMROCK_EXPAND_LIBDIR
SHAMROCK_EXPAND_BINDIR
SHAMROCK_EXPAND_DATADIR

AC_CHECK_TOOL(CC, gcc, gcc)
AC_PROG_CC
AC_PROG_INSTALL
AM_PROG_LIBTOOL

AC_PATH_PROG(MONO, mono, no)
if test "x$MONO" = "xno"; then
        AC_MSG_ERROR([mono Not found])
fi
AC_SUBST(MONO)

AC_PATH_PROG(GMCS, gmcs, no)
if test "x$GMCS" = "xno"; then
        AC_MSG_ERROR([gmcs Not found])
fi

AC_PATH_PROG(CC, gcc, no)
if test "x$CC" = "xno"; then
        AC_MSG_ERROR([gcc Not found])
fi

AC_PATH_PROG(GACUTIL, gacutil, no)
if test "x$GACUTIL" = "xno" ; then
	AC_MSG_ERROR([No gacutil tool found])
fi

AC_ARG_ENABLE(tests,
	AC_HELP_STRING([--disable-tests],
		[Do not build unit tests [default=no]]),
		enable_tests=no, enable_tests=yes)

NUNIT_PATH=

PKG_CHECK_MODULES(NUNIT, mono-nunit >= 2.4, nunit_24=yes, nunit_24=no)
if test "x$nunit_24" = "xno"; then
	AC_MSG_WARN([Could not find mono-nunit >= 2.4; unit tests will not be available.])
	enable_tests=no
else
	NUNIT_PATH=`pkg-config --variable=libdir mono-nunit`/mono/2.0/
fi

AC_SUBST(NUNIT_PATH)

# Require Gtk+ devel package so that we can get access at the pc file to
# construct a path to the modules directory
PKG_CHECK_MODULES([NDESK_DBUS], [ndesk-dbus-1.0 ndesk-dbus-glib-1.0])

AM_CONDITIONAL(ENABLE_TESTS, test "x$enable_tests" = "xyes")

dnl Following modules used for tests
PKG_CHECK_MODULES(DBUS_GLIB, [dbus-glib-1 >= 0.7.0])
AC_SUBST(DBUS_GLIB_LIBS)
AC_SUBST(DBUS_GLIB_CFLAGS)

PKG_CHECK_MODULES(LIB_XML, [libxml-2.0 >= 2.0.0])
AC_SUBST(LIB_XML_LIBS)
AC_SUBST(LIB_XML_CFLAGS)

PKG_CHECK_MODULES(GOBJ, [gobject-2.0 >= 2.0.0])
AC_SUBST(GOBJ_LIBS)
AC_SUBST(GOBJ_CFLAGS)

PKG_CHECK_MODULES(GMODULE, [gmodule-2.0 >= 2.0.0])	
AC_SUBST(GMODULE_LIBS)
AC_SUBST(GMODULE_CFLAGS)

PKG_CHECK_MODULES(ATK, [atk >= 1.17.0])
AC_SUBST(ATK_LIBS)
AC_SUBST(ATK_CFLAGS)
AC_ARG_ENABLE(debug,
	AC_HELP_STRING([--enable-debug],
		[Use 'DEBUG' Configuration [default=YES]]),
		enable_debug=yes, enable_debug=no)
AM_CONDITIONAL(ENABLE_DEBUG, test x$enable_debug = xyes)
if test "x$enable_debug" = "xyes" ; then
	CONFIG_REQUESTED="yes"
fi
AC_ARG_ENABLE(release,
	AC_HELP_STRING([--enable-release],
		[Use 'RELEASE' Configuration [default=NO]]),
		enable_release=yes, enable_release=no)
AM_CONDITIONAL(ENABLE_RELEASE, test x$enable_release = xyes)
if test "x$enable_release" = "xyes" ; then
	CONFIG_REQUESTED="yes"
fi
if test -z "$CONFIG_REQUESTED" ; then
	AM_CONDITIONAL(ENABLE_DEBUG, true)
	enable_debug=yes
fi


TOPDIR=$PWD
AC_SUBST(TOPDIR)

AC_CONFIG_FILES([
Makefile
at-spi/Makefile
tests/Makefile
tests/apps/Makefile
tests/at-spi-sharp/Makefile
tests/at-spi-sharp/atspitest.sh
])

AC_OUTPUT
