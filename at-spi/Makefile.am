EXTRA_DIST = at-spi-sharp.dll.config.in

ASSEMBLY_COMPILER_COMMAND = gmcs
#ASSEMBLY_COMPILER_FLAGS = -noconfig -unsafe -codepage:utf8 -warn:4 -optimize+ -debug "-define:DEBUG" -keyfile:$(top_srcdir)/mono-uia.snk -warnaserror
ASSEMBLY_COMPILER_FLAGS = -noconfig -unsafe -codepage:utf8 -warn:4 -optimize+ -debug "-define:DEBUG" -keyfile:$(top_srcdir)/mono-uia.snk

ASSEMBLY = bin/at-spi-sharp.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = bin

ATSPISHARP_DLL_MDB_SOURCE=bin/at-spi-sharp.dll.mdb
ATSPISHARP_DLL_MDB=$(BUILD_DIR)/at-spi-sharp.dll.mdb


PACKAGE_FLAG = /package accessibility

AL=al2
SATELLITE_ASSEMBLY_NAME=.resources.dll

PROGRAMFILES = \
	$(ATSPI_DLL_MDB)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(ASSEMBLY).config

if ENABLE_EXTERNAL_NDESK_DBUS
NDESK_DBUS_FILES =
else
NDESK_DBUS_FILES = $(top_srcdir)/ndesk-dbus/*.cs
endif

SOURCE_FILES =  \
	Accessible.cs \
	Action.cs \
	Application.cs \
	AssemblyInfo.cs \
	Component.cs \
	Desktop.cs \
	DeviceEventController.cs \
	Document.cs \
	EditableText.cs \
	EventBase.cs \
	EventDocument.cs \
	EventFocus.cs \
	EventKeyboard.cs \
	EventMouse.cs \
	EventObject.cs \
	EventTerminal.cs \
	EventWindow.cs \
	Hyperlink.cs \
	Hypertext.cs \
	Image.cs \
	Registry.cs \
	RelationSet.cs \
	Role.cs \
	Selection.cs \
	StateSet.cs \
	StateType.cs \
	Table.cs \
	Text.cs \
	UncachedChildren.cs \
	Value.cs

FILES =  \
	$(SOURCE_FILES) \
	$(NDESK_DBUS_FILES)

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
	    < $< > $@


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
