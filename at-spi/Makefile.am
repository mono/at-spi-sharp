EXTRA_DIST =

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
#ASSEMBLY_COMPILER_FLAGS = -noconfig -codepage:utf8 -warn:4 -optimize+ -debug "-define:DEBUG" -keyfile:../mono-uia.snk -warnaserror
ASSEMBLY_COMPILER_FLAGS = -noconfig -codepage:utf8 -warn:4 -optimize+ -debug "-define:DEBUG" -keyfile:../mono-uia.snk

ASSEMBLY = bin/Debug/at-spi-sharp.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug

ATSPISHARP_DLL_MDB_SOURCE=bin/Debug/at-spi-sharp.dll.mdb
ATSPISHARP_DLL_MDB=$(BUILD_DIR)/at-spi-sharp.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS = -noconfig -codepage:utf8 -warn:4 -optimize+ -keyfile:../mono-uia.snk -warnaserror
ASSEMBLY = bin/Release/at-spi-sharp.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release

ATSPI_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=.resources.dll

PROGRAMFILES = \
	$(ATSPI_DLL_MDB)  


RESGEN=resgen2
	
#all: $(ASSEMBLY) $(PROGRAMFILES) $(ASSEMBLY).config
all: $(ASSEMBLY) $(PROGRAMFILES)

FILES =  \
	Accessible.cs \
	Action.cs \
	Application.cs \
	AssemblyInfo.cs \
	Component.cs \
	Desktop.cs \
	EditableText.cs \
	Helper.cs \
	Image.cs \
	Registry.cs \
	Role.cs \
	StateSet.cs \
	StateType.cs \
	Text.cs \
	Value.cs

DATA_FILES = 

RESOURCES = 

EXTRAS = 

REFERENCES =  \
	$(NDESK_DBUS_LIBS)

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES)

install-data-local: gac-install

uninstall-local: gac-uninstall

include $(top_srcdir)/Makefile.include


$(ASSEMBLY).config: $(srcdir)/at-spi-sharp.dll.config.in Makefile
	sed -e "s|\@expanded_libdir\@|@expanded_libdir@|" \
	    -e "s|\@GTK_MODULES_DIR\@|@GTK_MODULES_DIR@|" \
	    < $< > $@


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
