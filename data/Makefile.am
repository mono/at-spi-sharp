
EXTRA_DIST =  

net20dir   = $(prefix)/lib/mono/2.0
a11ydir    = $(prefix)/lib/mono/accessibility

SHELL = /bin/sh
MKINSTALLDIRS = $(SHELL) $(top_srcdir)/mkinstalldirs
INSTALL = $(SHELL) $(top_srcdir)/install-sh
INSTALL_D = $(INSTALL) -c -m 644

all: at-spi-sharp.pc

REWRITE=
at-spi-sharp.pc: at-spi-sharp.pc.in Makefile
	sed -e "s,@net20dir@,$(net20dir),g" < at-spi-sharp.pc.in > $@.tmp
	sed -e "s,@a11ydir@,$(a11ydir),g" < $@.tmp > $@.tmp2
	mv $@.tmp2 $@
	rm $@.tmp

install-data-local: at-spi-sharp.pc
	$(MKINSTALLDIRS) $(DESTDIR)$(libdir)/pkgconfig
	$(INSTALL_D) at-spi-sharp.pc $(DESTDIR)$(libdir)/pkgconfig

uninstall-local:
	rm -rf $(DESTDIR)$(libdir)/pkgconfig/at-spi-sharp.pc

clean-local:
	rm -rf at-spi-sharp.pc

test-local:

dist-local: dist-default

FILES =  \
	at-spi-sharp.pc.in
